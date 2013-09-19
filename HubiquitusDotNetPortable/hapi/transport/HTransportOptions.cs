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

namespace HubiquitusDotNet.hapi.transport
{
    public class HTransportOptions
    {
        private string login = null;
        private string fullUrn = null;
        private string domain = null;
        private string resource = null;
        private string password = null;
        private string endpointHost = null;
        private int endpointPort = 0;
        private string endpointPath = null;
        private string hserver = "hnode";
        private int timeout = 0;
        private JObject context = null;
        public AuthenticationCallback AuthCb { get; set; }

        public HTransportOptions()
        {
        }

       

        


        //getter & setter 

        public string Login 
        {
            get { return login; }
            set
            {
                login = value;
            }
        }

        public string FullUrn
        {
            get { return fullUrn; }
            set 
            {
                fullUrn = value;
                Domain = fullUrn.Split(":".ToCharArray())[1];
                Resource = fullUrn.Split(":".ToCharArray())[2].Split("/".ToCharArray())[1];
            }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        
        public string Resource
        {
            get { return resource; }
            set { resource = value; }
        }


        public string Password
        {
            get { return password; }
            set { password = value; }
        }

       

        public string EndpointHost
        {
            get { return endpointHost; }
            set { endpointHost = value; }
        }

        public int EndpointPort
        {
            get { return endpointPort; }
            set { endpointPort = value; }
        }

        public string EndpointPath
        {
            get { return endpointPath; }
            set { endpointPath = value; }
        }

        public string Hserver 
        {
            get { return hserver; }
            set { hserver = value; }
        }

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public JObject Context
        {
            get { return context; }
            set { context = value; }
        }
    }
}
