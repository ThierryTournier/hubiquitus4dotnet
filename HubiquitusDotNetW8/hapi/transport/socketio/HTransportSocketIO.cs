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


using HubiquitusDotNetPortable.hapi.hStructures;
using HubiquitusDotNetPortable.hapi.transport;
using HubiquitusDotNetPortable.hapi.util;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Diagnostics;
using Windows.System.Threading;

namespace HubiquitusDotNetW8.hapi.transport.socketio
{
    public class HTransportSocketIO : HTransport
    {
        private ConnectionStatus connStatus = ConnectionStatus.DISCONNECTED;
        private HTransportOptions options = null;
        Client socketIO;
        private ThreadPoolTimer connTimeoutTimer = null;
        private TimeSpan connTimeout; // max by default.
        private bool isFullJidSet = false;
        public event DataEventHandler onData;
        public event StatusEventHandler onStatus;
        

        public void Connect(HTransportOptions options)
        {
            this.connStatus = ConnectionStatus.CONNECTING;
            this.options = options;

            //TODO init the connection timeout value!!
            connTimeout = new TimeSpan(0, 0, 0, 0, options.Timeout);

            string endpointHost = options.EndpointHost;
            int endpointPort = options.EndpointPort;
            string endpointPath = options.EndpointPath;

            string endpointAdress = ToEndpointAdress(endpointHost, endpointPort, endpointPath);

            connTimeoutTimer = ThreadPoolTimer.CreateTimer(timeout_Elapsed, connTimeout);
          
            socketIO = new Client(endpointAdress);

            socketIO.Message += socketIO_Message;
            socketIO.SocketConnectionClosed += socketIO_SocketConnectionClosed;
            socketIO.Error += socketIO_Error;
            socketIO.On("connect", (message) =>
                {
                    if (this.options.AuthCb != null)
                        this.options.AuthCb(options.Login, Login);
                    else
                        Login(options.Login, options.Password);
                });
            socketIO.ConnectAsync();

        }

        private void Login(string username, string password)
        {
            JObject data = new JObject();
            try
            {
                data.Add("login", username);
                data.Add("password", password);
                data.Add("sent",HUtil.DateTime2Timestamps(DateTime.UtcNow));
                if (options.Context != null)
                {
                    data.Add("context", options.Context);
                }

                socketIO.Emit("hConnect", data);

            }
            catch (Exception e)
            {
                if (socketIO != null)
                    Disconnect();
                if (connTimeoutTimer != null)
                {
                    connTimeoutTimer.Cancel();
                    connTimeoutTimer = null;
                }
                updateStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.TECH_ERROR, e.Message);
            }
        }

        void socketIO_Message(object sender, MessageEventArgs e)
        {

            if ("hStatus".Equals(e.Message.Event, StringComparison.OrdinalIgnoreCase))
            {
                if (e.Message.Json.Args != null)
                {
                    JObject data = (JObject)e.Message.Json.Args[0];
                    try
                    {
                        HStatus status = new HStatus(data);
                        if (connTimeoutTimer != null)
                        {
                            connTimeoutTimer.Cancel();
                            connTimeoutTimer = null;
                        }
                        if (status.GetStatus().Value == ConnectionStatus.CONNECTED)
                        {
                            if (isFullJidSet)
                                updateStatus((ConnectionStatus)status.GetStatus(), (ConnectionErrors)status.GetErrorCode(), status.GetErrorMsg());
                        }
                        else
                            updateStatus((ConnectionStatus)status.GetStatus(), (ConnectionErrors)status.GetErrorCode(), status.GetErrorMsg());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("{0} : {0} exception caught.", ex);
                        if (connTimeoutTimer != null)
                        {
                            connTimeoutTimer.Cancel();
                            connTimeoutTimer = null;
                        }
                        socketIO.Close();
                        updateStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.TECH_ERROR, ex.Message);
                    }
                }

            }
            else if("hMessage".Equals(e.Message.Event, StringComparison.OrdinalIgnoreCase))
            {
                if (e.Message.Json.Args != null)
                {
                    JObject data = (JObject)e.Message.Json.Args[0];
                    try
                    {
                        if (connTimeoutTimer != null)
                        {
                            connTimeoutTimer.Cancel();
                            connTimeoutTimer = null;
                        }
                        onData(e.Message.Json.Name, data);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("{0} : {0} exception caught.", ex);
                        if (connTimeoutTimer != null)
                        {
                            connTimeoutTimer.Cancel();
                            connTimeoutTimer = null;
                        }
                    }
                }
            }
            else if ("attrs".Equals(e.Message.Event, StringComparison.OrdinalIgnoreCase))
            {
                if (e.Message.Json.Args != null)
                {
                    JObject data = (JObject)e.Message.Json.Args[0];
                    try
                    {
                        options.FullUrn = data["publisher"].ToString();
                        isFullJidSet = true;
                        if (connStatus != ConnectionStatus.CONNECTED)
                            updateStatus(ConnectionStatus.CONNECTED, ConnectionErrors.NO_ERROR, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("{0} : {0} exception caught.", ex);
                    }
                    
                }
            }
        }


        void socketIO_Error(object sender, ErrorEventArgs e)
        {
            if (socketIO != null && socketIO.IsConnected)
            {
                socketIO.Close();
                this.Close();
            }
            socketIO = null;
            string errorMsg = null;
            if (e != null)
            {
                errorMsg = e.Exception.ToString();
            }
            if (connTimeoutTimer != null)
            {
                connTimeoutTimer.Cancel();
                connTimeoutTimer = null;
            }
            Debug.WriteLine("[SOCKET_IO]: socketIO Error ");
            updateStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.TECH_ERROR, errorMsg);
        }

        void socketIO_SocketConnectionClosed(object sender, EventArgs e)
        {
            if (connTimeoutTimer != null)
            {
                connTimeoutTimer.Cancel();
                connTimeoutTimer = null;
            }
            if (this.connStatus != ConnectionStatus.DISCONNECTED)
            {
                updateStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.NO_ERROR, null);
            }
            this.Close();
            Debug.WriteLine("[SOCKET_IO]: socketIO closed ");
        }

        public void updateStatus(ConnectionStatus status, ConnectionErrors error, string errorMsg)
        {
            this.connStatus = status;
            if (onStatus != null)
                onStatus(status, error, errorMsg);
            else
                throw new ArgumentNullException("Error: " + this.GetType() + " require a StatusEventHandler onStatus");
        }

        void timeout_Elapsed(object state)
        {
            if (connTimeoutTimer != null)
            {
                connTimeoutTimer.Cancel();
                connTimeoutTimer = null;
            }
            updateStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.CONN_TIMEOUT, null);
            if (socketIO.IsConnected)
                socketIO.Close();
            socketIO = null;
        }

        public void Disconnect()
        {
            connStatus = ConnectionStatus.DISCONNECTING;
            try
            {
                socketIO.Close();
                if (socketIO.IsConnected == false)
                {
                    connStatus = ConnectionStatus.DISCONNECTED;
                    updateStatus(connStatus, ConnectionErrors.NO_ERROR, null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} exception caught.", e);
            }
        }

        public void SendObject(JObject obj)
        {
            if (connStatus == ConnectionStatus.CONNECTED)
            {
                socketIO.Emit("hMessage", obj);
            }
                

            else
                Debug.WriteLine("Not connected.");
                
        }

        /// <summary>
        /// make an endpoint adress from endpoints components (ie http://host:port/path)
        /// </summary>
        /// <param name="endpointHost"></param>
        /// <param name="endpointPort"></param>
        /// <param name="endpointPath"></param>
        /// <returns></returns>
        private string ToEndpointAdress(string endpointHost, int endpointPort, string endpointPath)
        {
            string endpointAdress = "";
            endpointAdress += "http://";
            if (endpointHost != null)
                endpointAdress += endpointHost;
            if (endpointPort != 0)
                endpointAdress += ":" + endpointPort;
            if (endpointPath != null)
                endpointAdress += endpointPath;
            return endpointAdress;
        }

        private void Close()
        {
            isFullJidSet = false;
            if (this.socketIO != null)
            {
                socketIO.Message -= socketIO_Message;
                socketIO.SocketConnectionClosed -= socketIO_SocketConnectionClosed;
                socketIO.Error -= socketIO_Error;
                this.socketIO.Dispose(); // close & dispose of socket client
            }
        }


    }
}
