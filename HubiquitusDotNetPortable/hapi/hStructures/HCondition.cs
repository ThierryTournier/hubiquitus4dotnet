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
    public class HCondition : JObject
    {

        public HCondition()
            : base()
        { 
        }

        public HCondition(JObject jsobObj)
            : base(jsobObj)
        { 
        }

        public void SetEqValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("eq");
                else
                    this["eq"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the eq value attribute", e.ToString());
            }
        }

        public HValue GetEqValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["eq"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the eq value attribute", e.ToString());
            }
            return value;
        }

        public void SetNeValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("ne");
                else
                    this["ne"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the ne value attribute", e.ToString());
            }
        }

        public HValue GetNeValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["ne"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the ne value attribute", e.ToString());
            }
            return value;
        }

        public void SetGtValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("gt");
                else
                    this["gt"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the gt value attribute", e.ToString());
            }
        }

        public HValue GetGtValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["gt"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the gt value attribute", e.ToString());
            }
            return value;
        }

        public void SetGteValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("gte");
                else
                    this["gte"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the gte value attribute", e.ToString());
            }
        }

        public HValue GetGteValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["gte"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the gte value attribute", e.ToString());
            }
            return value;
        }

        public void SetLtValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("lt");
                else
                    this["lt"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lt value attribute", e.ToString());
            }
        }

        public HValue GetLtValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["lt"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the lt value attribute", e.ToString());
            }
            return value;
        }

        public void SetLteValue(HValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("lte");
                else
                    this["lte"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lte value attribute", e.ToString());
            }
        }

        public HValue GetLteValue()
        {
            HValue value = null;
            try
            {
                value = new HValue(JObject.Parse(this["lte"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the lte value attribute", e.ToString());
            }
            return value;
        }

        public void SetInValue(HArrayOfValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("in");
                else
                    this["in"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the in value attribute", e.ToString());
            }
        }

        public HArrayOfValue GetInValue()
        {
            HArrayOfValue value = null;
            try
            {
                value = new HArrayOfValue(JObject.Parse(this["in"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the in value attribute", e.ToString());
            }
            return value;
        }

        public void SetNinValue(HArrayOfValue value)
        {
            try
            {
                if (value == null)
                    this.Remove("nin");
                else
                    this["nin"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the nin value attribute", e.ToString());
            }
        }

        public HArrayOfValue GetNinValue()
        {
            HArrayOfValue value = null;
            try
            {
                value = new HArrayOfValue(JObject.Parse(this["nin"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the nin value attribute", e.ToString());
            }
            return value;
        }

        public void SetAndValue(JArray value)
        {
            try
            {
                if (value == null)
                    this.Remove("and");
                else
                    this["and"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the and value attribute", e.ToString());
            }
        }

        public JArray GetAndValue()
        {
            JArray value = null;
            try
            {
                value = this["and"].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the and value attribute", e.ToString());
            }
            return value;
        }

        public void SetOrValue(JArray value)
        {
            try
            {
                if (value == null)
                    this.Remove("or");
                else
                    this["or"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the or value attribute", e.ToString());
            }
        }

        public JArray GetOrValue()
        {
            JArray value = null;
            try
            {
                value = this["or"].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the or value attribute", e.ToString());
            }
            return value;
        }

        public void SetNorValue(JArray value)
        {
            try
            {
                if (value == null)
                    this.Remove("nor");
                else
                    this["nor"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the nor value attribute", e.ToString());
            }
        }

        public JArray GetNorValue()
        {
            JArray value = null;
            try
            {
                value = this["nor"].ToObject<JArray>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the nor value attribute", e.ToString());
            }
            return value;
        }

        public void SetNotValue(HCondition value)
        {
            try
            {
                if (value == null)
                    this.Remove("not");
                else
                    this["not"] = value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the not value attribute", e.ToString());
            }
        }

        public HCondition GetNotValue()
        {
            HCondition value = null;
            try
            {
                value = new HCondition(JObject.Parse(this["not"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the not value attribute", e.ToString());
            }
            return value;
        }

        public bool? GetRelevant()
        {
            bool? relevant;
            try
            {
                relevant = this["relevant"].ToObject<bool>();
            }
            catch (Exception)
            {
                relevant = null;
            }
            return relevant;
        }

        public void SetRelevant(bool? relevant)
        {
            try
            {
                if (relevant == null)
                    this.Remove("relevant");
                else
                    this["relevant"] = relevant;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the relevant attribute", e.ToString());
            }
        }

        public HPos GetGeo()
        {
            HPos geo;
            try
            {
                geo = new HPos(JObject.Parse(this["geo"].ToString()));
            }
            catch (Exception e)
            {
                geo = null;
                Debug.WriteLine("{0} : Can not fetch the geo attribute", e.ToString());
            }
            return geo;
        }

        public void SetGeo(HPos geo)
        {
            try
            {
                if (geo == null)
                    this.Remove("geo");
                else
                    this["geo"] = geo;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the geo attribute", e.ToString());
            }
        }
    }
}
