/*
 * Copyright (c) Novedia Group 2012.
 *
 *    This file is part of Hubiquitus
 *
 *    Permission is hereby granted, free of charge, to any person obtaining a copy
 *    of this software and associated documentation files (the "Software"), to deal
 *    in the Software without restriction, including without limitation the rights
 *    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 *    of the Software, and to permit persons to whom the Software is furnished to do so,
 *    subject to the following conditions:
 *
 *    The above copyright notice and this permission notice shall be included in all copies
 *    or substantial portions of the Software.
 *
 *    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 *    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 *    PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 *    FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 *    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 *    You should have received a copy of the MIT License along with Hubiquitus.
 *    If not, see <http://opensource.org/licenses/mit-license.php>.
 */


using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace HubiquitusDotNetPortable.hapi.hStructures
{
    /// <summary>
    /// Version 0.5
    /// hAPI result. For more info, see Hubiquitus reference
    /// </summary>
    public class HResult : JObject
    {

        public HResult()
        {
        }

        public HResult(JObject jsonObj)
            : base(jsonObj)
        {
        }


        //Getters & setters

        /// <summary>
        /// Get the status of the result. Null if undefined.
        /// </summary>
        /// <returns></returns>
        public ResultStatus? GetStatus()
        {
            ResultStatus? status = null;
            try
            {
                status = this["status"].ToObject<ResultStatus>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the status attritbute : {0}", e.ToString());
            }
            return status;
        }

        public void SetStatus(ResultStatus? status)
        {
            try
            {
                if (status == null)
                    this.Remove("status");
                else
                    this["status"] = (int)status;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the status attribute", e.ToString());
            }
        }

        /// <summary>
        /// If we don't know the type of result.
        /// </summary>
        /// <returns></returns>
        public object GetResult()
        {
            object result = null;
            try
            {
                result = this["result"];
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }

        /// <summary>
        /// If result type is JObject.
        /// </summary>
        /// <returns></returns>
        public JObject GetResultAsJObject()
        {
            JObject result = null;
            try
            {
                result = this["result"].ToObject<JObject>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }
        /// <summary>
        /// If result type is JArray.
        /// </summary>
        /// <returns></returns>
        public JArray GetResultASJArray()
        {
            JArray result = null;
            try
            {
                result = this["result"].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }

        /// <summary>
        /// If result type is string.
        /// </summary>
        /// <returns></returns>
        public string GetResultAsString()
        {
            string result = null;
            try
            {
                result = this["result"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }

        /// <summary>
        /// If the result type is bool.
        /// </summary>
        /// <returns></returns>
        public bool? GetResultAsBoolean()
        {
            bool? result = null;
            try
            {
                result = this["result"].ToObject<bool>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }

        /// <summary>
        /// If the result type is int.
        /// </summary>
        /// <returns></returns>
        public int? GetResultAsInt()
        {
            int? result = null;
            try
            {
                result = this["result"].ToObject<int>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }

        /// <summary>
        /// If the result type is double.
        /// </summary>
        /// <returns></returns>
        public double? GetResultAsDouble()
        {
            double? result = null;
            try
            {
                result = this["result"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the result attribute", e.ToString());
            }
            return result;
        }


        /// <summary>
        /// The result type could be JObject, JArray, String, Boolean, Number.
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(JToken result)
        {
            try
            {
                if (result == null)
                    this.Remove("result");
                else
                    this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }


        public void SetResult(JObject result)
        {
            try
            {
                if (result == null)
                    this.Remove("result");
                else
                    this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

        public void SetResult(JArray result)
        {
            try
            {
                if (result == null)
                    this.Remove("result");
                else
                    this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

        public void SetResult(string result)
        {
            try
            {
                if (result == null)
                    this.Remove("result");
                else
                    this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

        public void SetResult(bool result)
        {
            try
            {
                this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

        public void SetResult(int result)
        {
            try
            {
                this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

        public void SetResult(double result)
        {
            try
            {
                this["result"] = result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the result attribute", e.ToString());
            }
        }

    }
}
