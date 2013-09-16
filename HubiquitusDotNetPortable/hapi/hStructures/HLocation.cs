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
    /// This structure describe the location
    /// </summary>
    public class HLocation : JObject
    {
        public HLocation()
        { 
        }

        public HLocation(JObject jsonObj)
            : base(jsonObj)
        {
        }

        
        //Getters & setters

        /// <summary>
        /// Get the pos. null if undefined
        /// </summary>
        /// <returns></returns>
        public HGeo GetPos()
        {
            HGeo pos = null;
            try
            {
                pos = new HGeo(JObject.Parse(this["pos"].ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the pos attribute", e.ToString());
            }
            return pos;
        }

        /// <summary>
        /// Set the pos.
        /// </summary>
        /// <param name="pos"></param>

        public void SetPos(HGeo pos)
        {
            try
            {
                if (pos == null)
                    Debug.WriteLine("{0} : The pos attribute can not be null!");
                else
                    this["pos"] = pos;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the pos attribute: {0}", e.ToString());
            }
        }


        /// <summary>
        /// Get the zip code of the location. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetZip()
        {
            string zip = null;
            try
            {
                zip = this["zip"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the zip attribute", e.ToString());
            }
            return zip;
        }

        public void SetZip(string zip)
        {
            try
            {
                if (zip == null)
                    this.Remove("zip");
                else
                    this["zip"] = zip;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the zip attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the way number of the location. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetNum()
        {
            string num = null;
            try
            {
                num = this["num"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the num attribute", e.ToString());
            }
            return num;
        }

        public void SetNum(string num)
        {
            try
            {
                if (num == null)
                    this.Remove("num");
                else
                    this["num"] = num;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the num attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the type of the way of the location. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetWaytype()
        {
            string waytype = null;
            try
            {
                waytype = this["waytype"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the waytype attribute", e.ToString());
            }
            return waytype;
        }

        public void SetWaytype(string waytype)
        {
            try
            {
                if (waytype == null)
                    this.Remove("waytype");
                else
                    this["waytype"] = waytype;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the waytype attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the name of the street/way of the location. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetWay()
        {
            string way = null;
            try
            {
                way = this["way"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the way attribute", e.ToString());
            }
            return way;
        }

        public void SetWay(string way)
        {
            try
            {
                if (way == null)
                    this.Remove("way");
                else
                    this["way"] = way;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the way attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the address complement of the location. NULL if undefined.
        /// </summary>
        /// <returns></returns>
        public string GetAddr()
        {
            string addr = null;
            try
            {
                addr = this["addr"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the addr attribute", e.ToString());
            }
            return addr;
        }

        public void SetAddr(string addr)
        {
            try
            {
                if (addr == null)
                    this.Remove("addr");
                else
                    this["addr"] = addr;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the addr attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the floor number of the location. NULL if undefined.
        /// </summary>
        /// <returns></returns>
        public string GetFloor()
        {
            string floor = null;
            try
            {
                floor = this["floor"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the floor attribute", e.ToString());
            }
            return floor;
        }

        public void SetFloor(string floor)
        {
            try
            {
                if (floor == null)
                    this.Remove("floor");
                else
                    this["floor"] = floor;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the floor the floor attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get the building’s identifier of the location. NULL if undefined.
        /// </summary>
        /// <returns></returns>
        public string GetBuilding()
        {
            string building = null;
            try
            {
                building = this["building"].ToString() ;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the building attribute", e.ToString());
            }
            return building;
        }

        public void SetBuilding(string building)
        {
            try
            {
                if (building == null)
                    this.Remove("building");
                else
                    this["building"] = building;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the building attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get city of the location. NULL if undefined
        /// </summary>
        /// <returns></returns>
        public string GetCity()
        {
            string city = null;
            try
            {
                city = this["city"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the city attribute", e.ToString());
            }
            return city;
        }

        public void SetCity(string city)
        {
            try
            {
                if (city == null)
                    this.Remove("city");
                else
                    this["city"] = city;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the city attribute", e.ToString());
            }
        }

        /// <summary>
        /// Get countryCode of the location. NULL if undefined.
        /// </summary>
        /// <returns></returns>
        public string GetCountryCode()
        {
            string countryCode = null;
            try
            {
                countryCode = this["countryCode"].ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not fetch the countryCode attribute", e.ToString());
            }
            return countryCode;
        }

        public void SetCountryCode(string countryCode)
        {
            try
            {
                if (countryCode == null)
                    this.Remove("countryCode");
                else
                    this["countryCode"] = countryCode;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the countyCode attribute", e.ToString());
            }
        }
    }
}
