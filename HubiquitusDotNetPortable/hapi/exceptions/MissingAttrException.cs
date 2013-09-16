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

using System;

namespace HubiquitusDotNetPortable.hapi.exceptions
{
    /// <summary>
    /// Exception to notify a missing attribute (ONLY used in builders)
    /// </summary>
    public class MissingAttrException : Exception
    {
        private static readonly long serialVersionUID = 1L;
        private string attrName = null;

        public string AttrName { get { return attrName; } set { attrName = value; } }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="attrName">
        /// Name of the missing attribute
        /// </param>
        public MissingAttrException(string attrName) 
        {
            this.attrName = attrName;
        }

        public string GetMessage() 
        {
            return "Attribute " + this.attrName + " is required but missing.";
        }

        public string GetLocalizedMessage()
        {
            return GetMessage();
        }

        public override string ToString()
        {
            return GetMessage();
        }

    }
}
