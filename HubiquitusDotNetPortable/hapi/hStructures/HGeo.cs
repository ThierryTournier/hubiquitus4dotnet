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
    public class HGeo : JObject
    {

        public HGeo()
        {
        }

        public HGeo(JObject jsonObj)
            : base(jsonObj)
        {
        }

        public HGeo(double lat, double lng)
            : base()
        {
            SetLat(lat);
            SetLng(lng);
        }

        //Getter & Setter

        public double GetLat()
        {
            double lat = 0;
            try
            {
                lat = this["lat"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the lat attribute", e.ToString());
            }
            return lat;
        }

        public void SetLat(double lat)
        {
            try
            {
                this["lat"] = lat;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lat attribute", e.ToString());
            }
        }

        public double GetLng()
        {
            double lng = 0;
            try
            {
                lng = this["lng"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the lng attribute", e.ToString());
            }
            return lng;
        }

        public void SetLng(double lng)
        {
            try
            {
                this["lng"] = lng;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lng attribute", e.ToString());
            }
        }
        
    }
}
