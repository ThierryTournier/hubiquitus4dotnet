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
    public class HArrayOfValue : JObject
    {
        private string name;

        public HArrayOfValue()
            : base()
        { 
        }

        public HArrayOfValue(JObject jsonObj)
            : base(jsonObj)
        { 
        }

        public HArrayOfValue(string name, JArray value)
            : base()
        {
            this.name = name;
            SetValues(value);
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetValues(JArray value)
        {
            try
            {
                this[name] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the value attribute", e.ToString());
            }
        }

        public JArray GetValues()
        {
            JArray value = null;
            try
            {
                value = this[name].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the value attribute", e.ToString());
            }
            return value;
        }
    }
}
