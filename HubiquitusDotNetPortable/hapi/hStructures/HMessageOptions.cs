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

namespace HubiquitusDotNetPortable.hapi.hStructures
{
    /// <summary>
    /// Version 0.5
    /// hAPI MessageOption. For more info, see Hubiquitus reference
    /// </summary>
    public class HMessageOptions
    {
        private string @ref = null;
        private string convid = null;
        private HMessagePriority? priority = null;
        private long relevance = 0;
        private int? relevanceOffset = null;
        private bool? persistent = null;
        private HLocation location = null;
        private string author = null;
        private JObject headers = null;
        private long published = 0;
        private int timeout = 0;

        public string Ref
        {
            get { return @ref; }
            set { @ref = value; }
        }


        public string Convid 
        {
            get { return convid; }
            set { convid = value; }
        }

        public HMessagePriority? Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public long Relevance
        {
            get { return relevance; }
            set { relevance = value; }
        }


        public int? RelevanceOffset
        {
            get { return relevanceOffset; }
            set { relevanceOffset = value; }
        }

        public bool? Persistent
        {
            get { return persistent; }
            set { persistent = value; }
        }

        public HLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public JObject Headers
        {
            get { return headers; }
            set { headers = value; }
        }

        public long Published
        {
            get { return published; }
            set { published = value; }
        }

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
    }
}
