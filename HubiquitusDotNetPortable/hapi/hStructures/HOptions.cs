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


using HubiquitusDotNet.hapi.transport;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace HubiquitusDotNet.hapi.hStructures
{

    public class HOptions : JObject
    {

        public HOptions()
        {
        }

        public HOptions(JObject jsonObj)
            : base(jsonObj)
        {
        }

        public HOptions(HOptions options)
        {
            SetEndpoints(options.GetEndpoints());
            SetTransport(options.GetTransport());
            SetTimeout(options.GetTimeout());
            SetMsgTimeout(options.GetMsgTimeout());
        }

        public AuthenticationCallback AuthCb { get; set; }

        public string GetTransport()
        {
            if (this["transport"] == null)
                return "socketio";
            return this["transport"].ToString();
        }

        public void SetTransport(string transport)
        {
            try
            {
                if (transport == null || transport.Length <= 0)
                    this["transport"] = "socketio";
                else
                    this["transport"] = transport;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the transport attribute", e.ToString());
            }
        }


        public JArray GetEndpoints()
        {
            if (this["endpoints"] == null)
            {
                JArray endpoints = new JArray();
                endpoints.Add("http://localhost:8080");
                return endpoints;
            }
            else 
                return  this["endpoints"].ToObject<JArray>();
        
        }

        public void SetEndpoints(JArray endpoints)
        {
            try
            {
                if (endpoints != null && endpoints.Count > 0)
                    this["endpoints"] = endpoints;
                else
                    Debug.WriteLine("{0} : The endpoints attribute can not be null or empty.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the endpoints attribute", e.ToString());
            }
        }

        /// <summary>
        /// default timeout value used by the hAPI before rise a connection timeout error during connection attempt
        /// Defaut value is 15000 ms.
        /// </summary>
        /// <returns></returns>
        public int GetTimeout()
        {
            if (this["timeout"] == null)
                return 10000;
            else
                return this["timeout"].ToObject<int>();
        }

        public void SetTimeout(int timeout)
        {
            try
            {
                if (timeout >= 0)
                    this["timeout"] = timeout;
                else
                    this["timeout"] = 15000; // 15000ms by default.
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the timeout attribute", e.ToString());
            }
        }

        /// <summary>
        /// default timeout value used by the hAPI for all the services except the send() one
        /// Defaut value is 30000 ms.
        /// </summary>
        /// <returns></returns>
        public int GetMsgTimeout()
        {
            if (this["msgTimeout"] == null)
                return 30000;
            else 
                return this["msgTimeout"].ToObject<int>();
        }

        public void SetMsgTimeout(int timeout)
        {
            try
            {
                if (timeout >= 0)
                    this["msgTimeout"] = timeout;
                else
                    this["msgTimeout"] = 30000; //30000ms by default
            }

            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the msgTimerout attribute", e.ToString());
            }
        }

    }
}
