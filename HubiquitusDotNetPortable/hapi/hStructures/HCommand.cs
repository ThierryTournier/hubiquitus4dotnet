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
    /// hAPI Command. For more info, see Hubiquitus reference
    /// </summary>
    public class HCommand : JObject
    {
        public HCommand()
        { 
        }

        public HCommand(JObject jsonObj)
            : base(jsonObj)
        {
        }

        public HCommand(string cmd, JToken @params, HCondition filter)
        {
            SetCmd(cmd);
            SetParams(@params);
            SetFilter(filter);
        }

     
        //Getters &setters
       
        /// <summary>
        /// Get the command name. Mandatory.
        /// </summary>
        /// <returns>Null if undefined.</returns>
        public string GetCmd()
        {
            string cmd = null;
            try
            {
                cmd = this["cmd"].ToString();
            }
            catch (Exception e)
            {  
                Debug.WriteLine("{0} : Can not fetch the cmd attribute", e.ToString());
            }
            return cmd;
        }

        /// <summary>
        /// Set the command name. Mandatory.
        /// </summary>
        /// <param name="cmd"></param>
        public void SetCmd(string cmd)
        {
            try
            {
                if (cmd == null)
                    this.Remove("cmd");
                else
                    this["cmd"] = cmd;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the cmd attribute", e.ToString());
            }
        }
        /// <summary>
        /// Get params thrown to the hserver. 
        /// </summary>
        /// <returns>Null if undefined.</returns>
        public JToken GetParams()
        {
            JToken @params = null;
            try
            {
                @params = this["params"].ToObject<JToken>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the params attribute", e.ToString());
            }
            return @params;
        }

        /// <summary>
        /// Set params thrown to the hserver.
        /// </summary>
        /// <param name="params"></param>
        public void SetParams(JToken @params)
        {
            try
            {
                if (@params == null)
                    this.Remove("params");
                else
                    this["params"] = @params;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the params attribute", e.ToString());
            }
        }

        public HCondition GetFilter() 
        {
            HCondition filter = null;
            try
            {
                filter =  new HCondition(this["filter"].ToObject<JObject>());
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the params attribute", e.ToString());
            }
            return filter;
        }


        public void SetFilter(HCondition filter)
        {
            try
            {
                if (filter == null)
                    this.Remove("filter");
                else
                    this["filter"] = filter;
            }
            catch (Exception e) 
            {
                Debug.WriteLine("{0} : Can not update the filter attribute", e.ToString());
            }
        }
    }
}
