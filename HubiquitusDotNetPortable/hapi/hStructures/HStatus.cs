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

namespace HubiquitusDotNet.hapi.hStructures
{
    /// <summary>
    /// Version 0.5
    /// This structure describe the connection status
    /// </summary>
    public class HStatus : JObject
    {

        public HStatus()
        { 
        }

        public HStatus(JObject jsonObj)
            : base(jsonObj)
        {
        }


        //Getters & setters
        /// <summary>
        /// Get the status of connection. Null if not defined.
        /// </summary>
        /// <returns></returns>
        public ConnectionStatus? GetStatus()
        {
            ConnectionStatus? status = null;
            try
            {
                status = (ConnectionStatus)this["status"].ToObject<int>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the status attribute", e.ToString());
            }
            return status;
        }

        public void SetStatus(ConnectionStatus? status)
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
        /// Get the error of connction. Null if undefined.
        /// </summary>
        /// <returns></returns>
        public ConnectionErrors? GetErrorCode()
        {
            ConnectionErrors? errorCode = null;
            try
            {
                errorCode = (ConnectionErrors)this["errorCode"].ToObject<int>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the errorCode attribute", e.ToString());
            }
            return errorCode;
        }

        public void SetErrorCode(ConnectionErrors? errorCode)
        {
            try
            {
                if (errorCode == null)
                    this.Remove("errorCode");
                else
                    this["errorCode"] = (int)errorCode;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the errorCode attribute : ",e);
            }
        }

        /// <summary>
        /// Get error message of connection. Null if undefined.
        /// </summary>
        /// <returns></returns>
        public string GetErrorMsg()
        {
            string errorMsg = null;
            try
            {
                errorMsg = this["errorMsg"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : {0} : Can not fetch the errorMsg attribute", e.ToString());
            }
            return errorMsg;
        }

        public void SetErrorMsg(string errorMsg)
        {
            try
            {
                if (errorMsg == null)
                    this.Remove("errorMsg");
                else
                    this["errorMsg"] = errorMsg;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : {0} : Can not update the errorMsg attribute", e.ToString());
            }
        }


    }
}
