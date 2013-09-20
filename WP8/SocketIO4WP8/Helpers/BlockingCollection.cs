using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketIO4WP8.Helpers
{
    public class BlockingCollection<T> : IEnumerable<T>, ICollection, IEnumerable, IDisposable
    {
        const int spinCount = 5;

        readonly IProducerConsumerCollection<T> underlyingColl;
        readonly int upperBound;

        AtomicBoolean isComplete;
        long completeId;

        /* The whole idea of the collection is to use these two long values in a transactional
         * way to track and manage the actual data inside the underlying lock-free collection
         * instead of directly working with it or using external locking.
         *
         * They are manipulated with CAS and are guaranteed to increase over time and use
         * of the instance thus preventing ABA problems.
         */
        long addId = long.MinValue;
        long removeId = long.MinValue;

        /* These events are used solely for the purpose of having an optimized sleep cycle when
         * the BlockingCollection have to wait on an external event (Add or Remove for instance)
         */
        ManualResetEventSlim mreAdd = new ManualResetEventSlim(true);
        ManualResetEventSlim mreRemove = new ManualResetEventSlim(true);

        /* For time based operations, we share this instance of Stopwatch and base calculation
           on a time offset at each of these method call */
        static Stopwatch watch = new Stopwatch();
     

        #region ctors
        public BlockingCollection()
            : this(new SocketIO4WP8.Helpers.ConcurrentQueue<T>(), -1)
        {
        }

        public BlockingCollection(int upperBound)
            : this(new SocketIO4WP8.Helpers.ConcurrentQueue<T>(), upperBound)
        {
        }

        public BlockingCollection(IProducerConsumerCollection<T> underlyingColl)
            : this(underlyingColl, -1)
        {
        }

        public BlockingCollection(IProducerConsumerCollection<T> underlyingColl, int upperBound)
        {
            this.underlyingColl = underlyingColl;
            this.upperBound = upperBound;
            this.isComplete = new AtomicBoolean();
        }

        
        #endregion

        #region Add & Remove (+ Try)
        public void Add(T item)
        {
            Add(item, CancellationToken.None);
        }

        public void Add(T item, CancellationToken token)
        {
            TryAdd(item, -1, token);
        }

        public bool TryAdd(T item)
        {
            return TryAdd(item, 0, CancellationToken.None);
        }

        public bool TryAdd(T item, int milliseconds, CancellationToken token)
        {
            if (milliseconds < -1)
                throw new ArgumentOutOfRangeException("milliseconds");

            long start = milliseconds == -1 ? 0 : watch.ElapsedMilliseconds;
            SpinWait sw = new SpinWait();

            do
            {
                token.ThrowIfCancellationRequested();

                long cachedAddId = addId;
                long cachedRemoveId = removeId;

                // If needed, we check and wait that the collection isn't full
                if (upperBound != -1 && cachedAddId - cachedRemoveId > upperBound)
                {
                    if (milliseconds == 0)
                        return false;

                    if (sw.Count <= spinCount)
                    {
                        sw.SpinOnce();
                    }
                    else
                    {
                        mreRemove.Reset();
                        if (cachedRemoveId != removeId || cachedAddId != addId)
                        {
                            mreRemove.Set();
                            continue;
                        }

                        mreRemove.Wait(ComputeTimeout(milliseconds, start), token);
                    }

                    continue;
                }

                // Check our transaction id against completed stored one
                if (isComplete.Value && cachedAddId >= completeId)
                    ThrowCompleteException();

                // Validate the steps we have been doing until now
                if (Interlocked.CompareExchange(ref addId, cachedAddId + 1, cachedAddId) != cachedAddId)
                    continue;

                // We have a slot reserved in the underlying collection, try to take it
                if (!underlyingColl.TryAdd(item))
                    throw new InvalidOperationException("The underlying collection didn't accept the item.");

                // Wake up process that may have been sleeping
                mreAdd.Set();

                return true;
            } while (milliseconds == -1 || (watch.ElapsedMilliseconds - start) < milliseconds);

            return false;
        }

        public bool TryAdd(T item, TimeSpan ts)
        {
            return TryAdd(item, (int)ts.TotalMilliseconds);
        }

        public bool TryAdd(T item, int millisecondsTimeout)
        {
            return TryAdd(item, millisecondsTimeout, CancellationToken.None);
        }

        public T Take()
        {
            return Take(CancellationToken.None);
        }

        public T Take(CancellationToken token)
        {
            T item;
            TryTake(out item, -1, token, true);

            return item;
        }

        public bool TryTake(out T item)
        {
            return TryTake(out item, 0, CancellationToken.None);
        }

        public bool TryTake(out T item, int millisecondsTimeout, CancellationToken token)
        {
            return TryTake(out item, millisecondsTimeout, token, false);
        }

        bool TryTake(out T item, int milliseconds, CancellationToken token, bool throwComplete)
        {
            if (milliseconds < -1)
                throw new ArgumentOutOfRangeException("milliseconds");

            item = default(T);
            SpinWait sw = new SpinWait();
            long start = milliseconds == -1 ? 0 : watch.ElapsedMilliseconds;

            do
            {
                token.ThrowIfCancellationRequested();

                long cachedRemoveId = removeId;
                long cachedAddId = addId;

                // Empty case
                if (cachedRemoveId == cachedAddId)
                {
                    if (milliseconds == 0)
                        return false;

                    if (IsCompleted)
                    {
                        if (throwComplete)
                            ThrowCompleteException();
                        else
                            return false;
                    }

                    if (sw.Count <= spinCount)
                    {
                        sw.SpinOnce();
                    }
                    else
                    {
                        mreAdd.Reset();
                        if (cachedRemoveId != removeId || cachedAddId != addId)
                        {
                            mreAdd.Set();
                            continue;
                        }

                        mreAdd.Wait(ComputeTimeout(milliseconds, start), token);
                    }

                    continue;
                }

                if (Interlocked.CompareExchange(ref removeId, cachedRemoveId + 1, cachedRemoveId) != cachedRemoveId)
                    continue;

                while (!underlyingColl.TryTake(out item)) ;

                mreRemove.Set();

                return true;

            } while (milliseconds == -1 || (watch.ElapsedMilliseconds - start) < milliseconds);

            return false;
        }

        public bool TryTake(out T item, TimeSpan ts)
        {
            return TryTake(out item, (int)ts.TotalMilliseconds);
        }

        public bool TryTake(out T item, int millisecondsTimeout)
        {
            item = default(T);

            return TryTake(out item, millisecondsTimeout, CancellationToken.None, false);
        }

        static int ComputeTimeout(int millisecondsTimeout, long start)
        {
            return millisecondsTimeout == -1 ? 500 : (int)Math.Max(watch.ElapsedMilliseconds - start - millisecondsTimeout, 1);
        }
        #endregion

        #region static methods
        static void CheckArray(BlockingCollection<T>[] collections)
        {
            if (collections == null)
                throw new ArgumentNullException("collections");
            if (collections.Length == 0 || IsThereANullElement(collections))
                throw new ArgumentException("The collections argument is a 0-length array or contains a null element.", "collections");
        }

        static bool IsThereANullElement(BlockingCollection<T>[] collections)
        {
            foreach (BlockingCollection<T> e in collections)
                if (e == null)
                    return true;
            return false;
        }

        public static int AddToAny(BlockingCollection<T>[] collections, T item)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                try
                {
                    coll.Add(item);
                    return index;
                }
                catch { }
                index++;
            }
            return -1;
        }

        public static int AddToAny(BlockingCollection<T>[] collections, T item, CancellationToken token)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                try
                {
                    coll.Add(item, token);
                    return index;
                }
                catch { }
                index++;
            }
            return -1;
        }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryAdd(item))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, TimeSpan ts)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryAdd(item, ts))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryAdd(item, millisecondsTimeout))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout,
                                       CancellationToken token)
        {
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryAdd(item, millisecondsTimeout, token))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TakeFromAny(BlockingCollection<T>[] collections, out T item)
        {
            item = default(T);
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                try
                {
                    item = coll.Take();
                    return index;
                }
                catch { }
                index++;
            }
            return -1;
        }

        public static int TakeFromAny(BlockingCollection<T>[] collections, out T item, CancellationToken token)
        {
            item = default(T);
            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                try
                {
                    item = coll.Take(token);
                    return index;
                }
                catch { }
                index++;
            }
            return -1;
        }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item)
        {
            item = default(T);

            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryTake(out item))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, TimeSpan ts)
        {
            item = default(T);

            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryTake(out item, ts))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout)
        {
            item = default(T);

            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryTake(out item, millisecondsTimeout))
                    return index;
                index++;
            }
            return -1;
        }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout,
                                          CancellationToken token)
        {
            item = default(T);

            CheckArray(collections);
            int index = 0;
            foreach (var coll in collections)
            {
                if (coll.TryTake(out item, millisecondsTimeout, token))
                    return index;
                index++;
            }
            return -1;
        }
        #endregion

        public void CompleteAdding()
        {
            // No further add beside that point
            completeId = addId;
            isComplete.Value = true;
            // Wakeup some operation in case this has an impact
            mreAdd.Set();
            mreRemove.Set();
        }

        void ThrowCompleteException()
        {
            throw new InvalidOperationException("The BlockingCollection<T> has"
                                                 + " been marked as complete with regards to additions.");
        }

        void ICollection.CopyTo(Array array, int index)
        {
            underlyingColl.CopyTo(array, index);
        }

        public void CopyTo(T[] array, int index)
        {
            underlyingColl.CopyTo(array, index);
        }

        public IEnumerable<T> GetConsumingEnumerable()
        {
            return GetConsumingEnumerable(CancellationToken.None);
        }

        public IEnumerable<T> GetConsumingEnumerable(CancellationToken token)
        {
            while (true)
            {
                T item = default(T);

                try
                {
                    item = Take(token);
                }
                catch
                {
                    // Then the exception is perfectly normal
                    if (IsCompleted)
                        break;
                    // otherwise rethrow
                    throw;
                }

                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)underlyingColl).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)underlyingColl).GetEnumerator();
        }

        public void Dispose()
        {

        }

        protected virtual void Dispose(bool managedRes)
        {

        }

        public T[] ToArray()
        {
            return underlyingColl.ToArray();
        }

        public int BoundedCapacity
        {
            get
            {
                return upperBound;
            }
        }

        public int Count
        {
            get
            {
                return underlyingColl.Count;
            }
        }

        public bool IsAddingCompleted
        {
            get
            {
                return isComplete.Value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return isComplete.Value && addId == removeId;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return underlyingColl.SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return underlyingColl.IsSynchronized;
            }
        }
    }


    public interface IProducerConsumerCollection<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        bool TryAdd(T item);
        bool TryTake(out T item);
        T[] ToArray();
        void CopyTo(T[] array, int index);
    }

    internal struct AtomicBooleanValue
    {
        int flag;
        const int UnSet = 0;
        const int Set = 1;

        public bool CompareAndExchange(bool expected, bool newVal)
        {
            int newTemp = newVal ? Set : UnSet;
            int expectedTemp = expected ? Set : UnSet;

            return Interlocked.CompareExchange(ref flag, newTemp, expectedTemp) == expectedTemp;
        }

        public static AtomicBooleanValue FromValue(bool value)
        {
            AtomicBooleanValue temp = new AtomicBooleanValue();
            temp.Value = value;

            return temp;
        }

        public bool TrySet()
        {
            return !Exchange(true);
        }

        public bool TryRelaxedSet()
        {
            return flag == UnSet && !Exchange(true);
        }

        public bool Exchange(bool newVal)
        {
            int newTemp = newVal ? Set : UnSet;
            return Interlocked.Exchange(ref flag, newTemp) == Set;
        }

        public bool Value
        {
            get
            {
                return flag == Set;
            }
            set
            {
                Exchange(value);
            }
        }

        public bool Equals(AtomicBooleanValue rhs)
        {
            return this.flag == rhs.flag;
        }

        public override bool Equals(object rhs)
        {
            return rhs is AtomicBooleanValue ? Equals((AtomicBooleanValue)rhs) : false;
        }

        public override int GetHashCode()
        {
            return flag.GetHashCode();
        }

        public static explicit operator bool(AtomicBooleanValue rhs)
        {
            return rhs.Value;
        }

        public static implicit operator AtomicBooleanValue(bool rhs)
        {
            return AtomicBooleanValue.FromValue(rhs);
        }
    }

    internal class AtomicBoolean
    {
        int flag;
        const int UnSet = 0;
        const int Set = 1;

        public bool CompareAndExchange(bool expected, bool newVal)
        {
            int newTemp = newVal ? Set : UnSet;
            int expectedTemp = expected ? Set : UnSet;

            return Interlocked.CompareExchange(ref flag, newTemp, expectedTemp) == expectedTemp;
        }

        public static AtomicBoolean FromValue(bool value)
        {
            AtomicBoolean temp = new AtomicBoolean();
            temp.Value = value;

            return temp;
        }

        public bool TrySet()
        {
            return !Exchange(true);
        }

        public bool TryRelaxedSet()
        {
            return flag == UnSet && !Exchange(true);
        }

        public bool Exchange(bool newVal)
        {
            int newTemp = newVal ? Set : UnSet;
            return Interlocked.Exchange(ref flag, newTemp) == Set;
        }

        public bool Value
        {
            get
            {
                return flag == Set;
            }
            set
            {
                Exchange(value);
            }
        }

        public bool Equals(AtomicBoolean rhs)
        {
            return this.flag == rhs.flag;
        }

        public override bool Equals(object rhs)
        {
            return rhs is AtomicBoolean ? Equals((AtomicBoolean)rhs) : false;
        }

        public override int GetHashCode()
        {
            return flag.GetHashCode();
        }

        public static explicit operator bool(AtomicBoolean rhs)
        {
            return rhs.Value;
        }

        public static implicit operator AtomicBoolean(bool rhs)
        {
            return AtomicBoolean.FromValue(rhs);
        }
    }
}