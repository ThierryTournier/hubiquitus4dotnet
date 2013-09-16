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




namespace HubiquitusDotNetPortable.hapi.util
{
    public static class ErrorMessage
    {
        public readonly static string noConnectivity = "No connectivity! Please check your network connection.";
        public readonly static string alreadyDisconn = "Already disconnected!";
        public readonly static string alreadyConn = "Already connected!";
        public readonly static string disconnWhileDisconnecting = "Can not disconnect while a disconnection is in progress!";
        public readonly static string disconnWhileConnecting = "Can not disconnect while a connection is in progress!";
        public readonly static string connWhileConnecting = "Can not connect while a connection is in progress!";
        public readonly static string reconnIn5s = "Try to reconnect in 5s";
        public readonly static string notConn = "Not connected.";
        public readonly static string nullMessage = "Provided message is null";
        public readonly static string missingActor = "Actor is missing.";
        public readonly static string missingConvid = "Convid is missing.";
        public readonly static string missingStatus = "Status is missing.";
        public readonly static string timeout = "The response of message is time out!";

    }
}
