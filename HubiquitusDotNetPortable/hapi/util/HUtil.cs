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
using System.Collections.Generic;
using System.Linq;

namespace HubiquitusDotNetPortable.hapi.util
{
    public class HUtil
    {
        public static string DateISO8601Format = "yyyy-MM-ddTHH:mm:sszz00";

        public static int PickIndex(JArray jArray)
        {
            return (int)(jArray.Count() * (new Random()).NextDouble());
        }

        public static string GetHost(string endpoint)
        {
            string host = null;
            try
            {
                Uri uri = new Uri(endpoint);
                host = uri.Host;
            }
            catch (Exception)
            {
                host = null;
                throw;
            }
            return host;
        }

        public static int GetPort(string endpoint)
        {
            int port = 0;
            try
            {
                Uri uri = new Uri(endpoint);
                port = uri.Port;
            }
            catch (Exception)
            {
                port = 0;
                throw;
            }
            return port;
        }

        public static string GetPath(string endpoint)
        {
            string path = null;
            try
            {
                Uri uri = new Uri(endpoint);
                path = uri.PathAndQuery;
            }
            catch (Exception)
            {
                path = null;
                throw;
            }
            return path;
        }

        public static bool CheckAck(string ack)
        {
            List<string> AckValues = new List<string>();
            AckValues.Add("read");
            AckValues.Add("recv");
            if (AckValues.Contains(ack))
                return true;
            else
                return false;
        }

        public static string GetApiRef(string @ref)
        {
            if (@ref != null)
                return @ref.Split("#".ToCharArray())[0];
            else
                return null;
        }


        public static long DateTime2Timestamps(DateTime d)
        {
            return (d.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000;
        }

        public static DateTime Timestamps2Datetime(long t)
        {
            return new DateTime(t * 10000 + DateTime.Parse("01/01/1970 00:00:00").Ticks);
        }
    }
}
