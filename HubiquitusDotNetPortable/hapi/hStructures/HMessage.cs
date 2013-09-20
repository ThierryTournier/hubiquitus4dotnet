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


using HubiquitusDotNet.hapi.util;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace HubiquitusDotNet.hapi.hStructures
{
    /// <summary>
    /// Version 0.5
    /// hAPI Message. For more info, see Hubiquitus reference
    /// </summary>
    public class HMessage : JObject
    {

        public HMessage()
        {
        }

        public HMessage(JObject jsonObj)
            : base(jsonObj)
        {
        }

        /* Getters & Setters */

        /// <summary>
        /// Mandatory. Filled by the hApi. message id. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetMsgid()
        {
            string msgid = null;
            try
            {
                msgid = this["msgid"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the msgid attribute", e.ToString());
            }
            return msgid;
        }

        public void SetMsgid(string msgid)
        {
            try
            {
                if (msgid == null)
                {
                    this.Remove("msgid");
                }
                else
                {
                    this["msgid"] = msgid;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the msgid attribute", e.ToString());
            }
        }

        /// <summary>
        /// Mandatory. channel id. NULL if undefined 
        /// </summary>
        /// <returns></returns>
        public string GetActor()
        {
            string actor = null;
            try
            {
                actor = this["actor"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the actor attribute", e.ToString());
            }
            return actor;
        }

        public void SetActor(string actor)
        {
            try
            {
                if (actor == null)
                {
                    this.Remove("actor");
                }
                else
                {
                    this["actor"] = actor;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the actor attribute", e.ToString());
            }
        }

        /// <summary>
        /// Mandatory. Filled by the hApi if empty. conversation id. NULL if undefined 
        /// </summary>
        /// <returns></returns>
        public string GetConvid()
        {
            string convid = null;
            try
            {
                convid = this["convid"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the convid attribute", e.ToString());
            }
            return convid;
        }

        public void SetConvid(string convid)
        {
            try
            {
                if (convid == null)
                {
                    this.Remove("convid");
                }
                else
                {
                    this["convid"] = convid;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the convid attribute", e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>reference to another hMessage msgid. NULL if undefined.</returns>
        public string GetRef()
        {
            string @ref = null;
            try
            {
                @ref = this["ref"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the ref attribute", e.ToString());
            }
            return @ref;
        }

        public void SetRef(string @ref)
        {
            try
            {
                if (@ref == null)
                {
                    this.Remove("ref");
                }
                else
                {
                    this["ref"] = @ref;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the ref attribute", e.ToString());
            }
        }



        /// <summary>
        /// Get type of the message payload. NULL if undefined 
        /// </summary>
        /// <returns></returns>
        public string GetType()
        {
            string type = null;
            try
            {
                type = this["type"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the type attribute", e.ToString());
            }
            return type;
        }

        public void SetType(string type)
        {
            try
            {
                if (type == null)
                {
                    this.Remove("type");
                }
                else
                {
                    this["type"] = type;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the type attribute", e.ToString());
            }
        }

        /// <summary>
        /// If UNDEFINED, priority lower to 0. 
        /// </summary>
        /// <returns></returns>
        public HMessagePriority? GetPriority()
        {
            HMessagePriority? priority = null;
            try
            {
                priority = this["priority"].ToObject<HMessagePriority>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the priority attribute", e.ToString());
            }
            return priority;
        }

        public void SetPriority(HMessagePriority? priority)
        {
            try
            {
                if (priority == null)
                    this.Remove("priority");
                else
                    this["priority"] = (int)priority;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the priority attribute", e.ToString());
            }
        }


        /// <summary>
        /// Timestamp until which the message is considered as relevant. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public long GetRelevance()
        {
            long relevance = 0;
            try
            {
                relevance = this["relevance"].ToObject<long>();

            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the relevance attribute", e.ToString());
            }
            return relevance;
        }

        public DateTime? GetRelevanceAsDate()
        {
            DateTime? relevance = null;
            try
            {
                relevance = HUtil.Timestamps2Datetime(this["relevance"].ToObject<long>());
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the relevance attribute", e.ToString());
            }
            return relevance;
        }

        public void SetRelevance(DateTime? relevance)
        {
            try
            {
                if (relevance == null)
                {
                    this.Remove("relevance");
                }
                else
                {
                    this["relevance"] = HUtil.DateTime2Timestamps(relevance.Value);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the relevance attribute", e.ToString());
            }
        }

        public void SetRelevance(long relevance)
        {
            try
            {
                if (relevance == 0)
                {
                    this.Remove("relevance");
                }
                else
                {
                    this["relevance"] = relevance;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the relevance attribute", e.ToString());
            }
        }


        /// <summary>
        /// If true, the message is not persistent. persist message or not. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public bool? GetPersistent()
        {
            bool? persistent = null;
            try
            {
                persistent = this["persistent"].ToObject<bool>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the persistent attribute", e.ToString());
            }
            return persistent;
        }

        public void SetPersistent(bool? persistent)
        {
            try
            {
                if (persistent == null)
                    this.Remove("persistent");
                else
                    this["persistent"] = persistent;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the persistent attribute", e.ToString());
            }
        }

        /// <summary>
        /// The geographical location to which the message refer. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public HLocation GetLocation()
        {
            HLocation location = null;
            try
            {
                location = new HLocation(JObject.Parse(this["location"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the location attribute", e.ToString());
            }
            return location;
        }

        public void SetLocation(HLocation location)
        {
            try
            {
                if (location == null)
                {
                    this.Remove("location");
                }
                else
                {
                    this["location"] = location;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the location attribute", e.ToString()); ;
            }
        }

        /// <summary>
        /// author of this message. NULL if undefined 
        /// </summary>
        /// <returns></returns>
        public string GetAuthor()
        {
            string author = null;
            try
            {
                author = this["author"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the author attribute", e.ToString());
            }
            return author;
        }

        public void SetAuthor(string author)
        {
            try
            {
                if (author == null)
                {
                    this.Remove("author");
                }
                else
                {
                    this["author"] = author;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the author attribute", e.ToString());
            }
        }

        /// <summary>
        /// Mandatory. publisher of this message. NULL if undefined 
        /// </summary>
        /// <returns></returns>
        public string GetPublisher()
        {
            string publisher = null;
            try
            {
                publisher = this["publisher"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the publisher attribute", e.ToString());
            }
            return publisher;
        }

        public void SetPublisher(string publisher)
        {
            try
            {
                if (publisher == null)
                {
                    this.Remove("publisher");
                }
                else
                {
                    this["publisher"] = publisher;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the publisher attribute", e.ToString());
            }
        }

        /// <summary>
        /// Mandatory. The date and time at which the message has been published. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public DateTime? GetPublishedAsDate()
        {
            DateTime? published = null;
            try
            {
                published = HUtil.Timestamps2Datetime(this["published"].ToObject<long>());
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the published attribute", e.ToString());
            }
            return published;
        }

        public long GetPublished()
        {
            long published = 0;
            try
            {
                published = this["published"].ToObject<long>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the published attribute", e.ToString());
            }
            return published;
        }

        public void SetPublished(DateTime? published)
        {
            try
            {
                if (published == null)
                {
                    this.Remove("published");
                }
                else
                {
                    this["published"] = HUtil.DateTime2Timestamps(published.Value);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the published attribute", e.ToString());
            }
        }

        public void SetPublished(long published)
        {
            try
            {
                if (published == 0)
                {
                    this.Remove("published");
                }
                else
                {
                    this["published"] = published;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the published attribute", e.ToString());
            }
        }

        /// <summary>
        /// The list of headers attached to this message. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public JObject GetHeaders()
        {
            JObject headers = null;
            try
            {
                headers = this["headers"].ToObject<JObject>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the headers attribute", e.ToString());
            }
            return headers;
        }

        public void SetHeaders(JObject headers)
        {
            try
            {
                if (headers == null)
                {
                    this.Remove("headers");
                }
                else
                {
                    this["headers"] = headers;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the headers attribute", e.ToString());
            }
        }

        /// <summary>
        /// if payload type is JObject
        /// </summary>
        /// <returns>The payload of JObject type.</returns>
        public JObject GetPayloadAsJObject()
        {
            JObject payload = null;
            try
            {
                payload = this["payload"].ToObject<JObject>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is JArray.
        /// </summary>
        /// <returns>The paylaod of JArray type.</returns>
        public JArray GetPayloadAsJArray()
        {
            JArray payload = null;
            try
            {
                payload = this["payload"].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is string.
        /// </summary>
        /// <returns>The payload of string type.</returns>
        public string GetPayloadAsString()
        {
            string payload = null;
            try
            {
                payload = this["payload"].ToObject<string>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is boolean.
        /// </summary>
        /// <returns>The payload of boolean type.</returns>
        public bool? GetPayloadAsBoolean()
        {
            bool? payload = null;
            try
            {
                payload = this["payload"].ToObject<bool>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is int
        /// </summary>
        /// <returns>The payload of int type.</returns>
        public int? GetPayloadAsInt()
        {
            int? payload = null;
            try
            {
                payload = this["payload"].ToObject<int>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is double.
        /// </summary>
        /// <returns>The payload of double type.</returns>
        public double? GetPayloadAsDouble()
        {
            double? payload = null;
            try
            {
                payload = this["payload"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is HResult.
        /// </summary>
        /// <returns>The payload of HResult type.</returns>
        public HResult GetPayloadAsHResult()
        {
            HResult payload = null;
            try
            {
                payload = new HResult(JObject.Parse(this["payload"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }

        /// <summary>
        /// If payload type is HCommand.
        /// </summary>
        /// <returns>The payload of HCommand type.</returns>
        public HCommand GetPayloadAsHCommand()
        {
            HCommand payload = null;
            try
            {
                payload = new HCommand(JObject.Parse(this["payload"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the payload attribute", e.ToString());
            }
            return payload;
        }


        /// <summary>
        /// Payload type could be instance of JSONObject(HAlert, HAck ...), JSONArray, String, Boolean, Number.
        /// </summary>
        /// <param name="payload"></param>
        public void SetPayload(JToken payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(JObject payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(JArray payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(string payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(bool payload)
        {
            try
            {
                this["payload"] = payload;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(int payload)
        {
            try
            {
                this["payload"] = payload;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(double payload)
        {
            try
            {
                this["payload"] = payload;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(HResult payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        public void SetPayload(HCommand payload)
        {
            try
            {
                if (payload == null)
                {
                    this.Remove("payload");
                }
                else
                {
                    this["payload"] = payload;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the payload attribute", e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>timeout. 0 if undefined.</returns>
        public int GetTimeout()
        {
            if (this["timeout"] == null)
                return 0;
            else
                return 
                    this["timeout"].ToObject<int>();
            
        }
    
        public void SetTimeout(int timeout)
        {
            try
            {
                if (timeout == 0)
                    this.Remove("timeout");
                else
                    this["timeout"] = timeout;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the timeout attribute", e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>sent. Null if undefined.</returns>
        public DateTime? GetSentAsDate()
        {
            DateTime? sent = null;
            try
            {
                sent = HUtil.Timestamps2Datetime(this["sent"].ToObject<long>());
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the sent attribute", e.ToString());
            }
            return sent;
        }

        public long GetSent()
        {
            long sent = 0;
            try
            {
                sent = this["sent"].ToObject<long>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the sent attribute", e.ToString());
            }
            return sent;
        }

        public void SetSent(DateTime? sent)
        {
            try
            {
                if (sent == null)
                    this.Remove("sent");
                else
                    this["sent"] = HUtil.DateTime2Timestamps(sent.Value);
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the sent attribute", e.ToString());
            }
        }

        public void SetSent(long sent)
        {
            try
            {
                if (sent == 0)
                    this.Remove("sent");
                else
                    this["sent"] = sent;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the sent attribute", e.ToString());
            }
        }
    }
}
