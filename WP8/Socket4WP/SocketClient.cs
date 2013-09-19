using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Socket4WP.Messages;
using System.Diagnostics;
using WebSocket4Net;
using System.Net.Http;

namespace Socket4WP
{
    public class SocketClient
    {
        #region Fields & Properties
        // Cached Socket object that will be used by each call for the lifetime of this class
        Socket _socket = null;
        WebSocket _webSocket = null;

        /// <summary>
        /// Uri of Websocket server
        /// </summary>
        protected Uri _endpointUri;

        // Signaling object used to notify when an asynchronous operation is completed
        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        public bool IsConnected
        {
            get
            {
                return _socket.Connected;
            }
        }

        /// <summary>
        /// List of "to send" messages
        /// </summary>
        private List<string> _outboundQueue;

        #region EventHandler
        #endregion

        #region constant

        // Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        // timeout period, the call is aborted.
        const int TIMEOUT_MILLISECONDS = 5000;

        // The maximum size of the data buffer to use with the asynchronous socket methods
        const int MAX_BUFFER_SIZE = 2048;

        #endregion

        #endregion

        #region constructor

        public SocketClient(string endpointUrl)
        {
            this._endpointUri = new Uri(endpointUrl);
        }

        #endregion

        #region methods

        /// <summary>
        /// Attemps an asynchronous TCP connection
        /// </summary>
        /// <returns>success of the connection</returns>
        public async Task<bool> ConnectAsync()
        {
            var taskComplete = new TaskCompletionSource<bool>();

            // Create a stream-based, TCP socket using the InterNetwork Address Family. 
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Create a SocketAsyncEventArgs object to be used in the connection request
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            // Create DnsEndPoint. The hostName and port are passed in to this method.
            socketEventArg.RemoteEndPoint = new DnsEndPoint(_endpointUri.Host, _endpointUri.Port); ;

            // Inline event handler for the Completed event.
            // Note: This event handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                if (e.SocketError.ToString().Equals("Success"))
                {
                    Task.Run(async () =>
                    {
                        string handshake = await RequestHandshake();
                        

                        if (!string.IsNullOrEmpty(handshake))
                        {
                            try
                            {
                                
                                string wsScheme = (_endpointUri.Scheme == "https" ? "wss" : "ws");
                                _webSocket = new WebSocket(string.Format("{0}://{1}:{2}/socket.io/1/websocket/{3}", wsScheme, _endpointUri.Host, _endpointUri.Port, handshake));
                                _webSocket.MessageReceived += OnWebSocketMessageReceived;
                                _webSocket.Closed += OnWebSocketClosed;
                                _webSocket.Opened += OnWebSocketOpened;
                                _webSocket.Open();

                                taskComplete.SetResult(true);
                            }
                            catch (Exception)
                            {
                                taskComplete.SetResult(false);
                            }
                        }
                    });
                }
                else
                {
                    taskComplete.SetResult(false);
                }
            });

            // Make an asynchronous??? Connect request over the socket
            _socket.ConnectAsync(socketEventArg);
            return await taskComplete.Task;
        }

        

        private async Task<string> RequestHandshake()
        {
            string handshake=string.Empty;
            using (HttpClient http = new HttpClient())
            {

                    handshake = await http.GetStringAsync(string.Format("{0}://{1}:{2}/socket.io/1/{3}", _endpointUri.Scheme, _endpointUri.Host, _endpointUri.Port, _endpointUri.Query));
                                                  
                  
            }
            if (!string.IsNullOrEmpty(handshake))
                handshake = handshake.Split(':')[0];
            return handshake;
        }

        private void OnWebSocketMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.WriteLine("OnWebSocketMessageReceived : "+e.Message);
            IMessage iMsg = Message.Factory(e.Message);

            if (iMsg.Event == "responseMsg")
                Debug.WriteLine(string.Format("InvokeOnEvent: {0}", iMsg.RawMessage));

            switch (iMsg.MessageType)
            {
                case SocketIOMessageTypes.Disconnect:
                    OnMessageEvent(iMsg);
                    if (string.IsNullOrWhiteSpace(iMsg.Endpoint)) // Disconnect the whole socket
                        Close();
                    break;
                //case SocketIOMessageTypes.Heartbeat:
                //    OnHeartBeatTimerCallback(null);
                //    break;
                case SocketIOMessageTypes.Connect:
                case SocketIOMessageTypes.Message:
                case SocketIOMessageTypes.JSONMessage:
                case SocketIOMessageTypes.Event:
                case SocketIOMessageTypes.Error:
                    OnMessageEvent(iMsg);
                    break;
                //case SocketIOMessageTypes.ACK:
                //    registrationManager.InvokeCallBack(iMsg.AckId, iMsg.Json);
                //    break;
                default:
                    Debug.WriteLine("unknown mws message Received...");
                    break;
            }
        }

        protected void OnMessageEvent(IMessage msg)
        {

            bool skip = false;
            if (!string.IsNullOrEmpty(msg.Event))
                skip = this.registrationManager.InvokeOnEvent(msg); // 

            var handler = this.Message;
            if (handler != null && !skip)
            {
                //Debug.WriteLine(string.Format("webSocket_OnMessage: {0}", msg.RawMessage));
                handler(this, new MessageEventArgs(msg));
            }
        }

        private void OnWebSocketClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("OnWebSocketClosed : "+e.ToString());
        }

        private void OnWebSocketOpened(object sender, EventArgs e)
        {
            Debug.WriteLine("OnWebSocketOpened : "+e.ToString());
        }

        /// <summary>
        /// <para>Asynchronously sends payload using eventName</para>
        /// <para>payload must a string or Json Serializable</para>
        /// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
        /// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="payload">must be a string or a Json Serializable object</param>
        public void Emit(string eventName, dynamic payload)
        {
            this.Emit(eventName, payload, string.Empty, null);
        }

        /// <summary>
        /// <para>Asynchronously sends payload using eventName</para>
        /// <para>payload must a string or Json Serializable</para>
        /// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
        /// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="payload">must be a string or a Json Serializable object</param>
        /// <remarks>ArgumentOutOfRangeException will be thrown on reserved event names</remarks>
        public void Emit(string eventName, dynamic payload, string endPoint = "", Action<dynamic> callback = null)
        {
            string lceventName = eventName.ToLower();
            IMessage msg = null;

            if (!string.IsNullOrWhiteSpace(endPoint) && !endPoint.StartsWith("/"))
                endPoint = "/" + endPoint;
            msg = new EventMessage(eventName, payload, endPoint, callback);

            AddToQueue(msg);

            SendMessages();

        }

        public void AddToQueue(IMessage msg)
        {
            if (_outboundQueue == null)
                _outboundQueue = new List<string>();
            _outboundQueue.Add(msg.Encoded);
        }


        public void SendMessages()
        {

            foreach (string data in _outboundQueue)
            {
                try
                {
                    if (IsConnected)
                    {
                        string response = "Operation Timeout";

                        // We are re-using the _socket object initialized in the Connect method
                        if (_socket != null)
                        {
                            // Create SocketAsyncEventArgs context object
                            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                            // Set properties on context object
                            socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                            socketEventArg.UserToken = null;

                            // Inline event handler for the Completed event.
                            // Note: This event handler was implemented inline in order 
                            // to make this method self-contained.
                            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                            {
                                response = e.SocketError.ToString();

                                // Unblock the UI thread
                                _clientDone.Set();
                            });

                            // Add the data to be sent into the buffer
                            byte[] payload = Encoding.UTF8.GetBytes(data);
                            socketEventArg.SetBuffer(payload, 0, payload.Length);

                            // Sets the state of the event to nonsignaled, causing threads to block
                            _clientDone.Reset();

                            // Make an asynchronous Send request over the socket
                            _socket.SendAsync(socketEventArg);

                            // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                            // If no response comes back within this time then proceed
                            _clientDone.WaitOne(TIMEOUT_MILLISECONDS);

                        }
                        else
                        {
                            response = "Socket is not initialized";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SOCKETCLIENT : Send : " + ex);
                }

            }
        }

        /// <summary>
        /// Receive data from the server using the established socket connection
        /// </summary>
        /// <returns>The data received from the server</returns>
        public string Receive()
        {
            string response = "Operation Timeout";

            // We are receiving over an established socket connection
            if (_socket != null)
            {

                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;

                // Setup the buffer to receive the data
                socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

                // Inline event handler for the Completed event.
                // Note: This even handler was implemented inline in order to make 
                // this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == System.Net.Sockets.SocketError.Success)
                    {
                        // Retrieve the data from the buffer
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

                    _clientDone.Set();
                });

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Receive request over the socket
                _socket.ReceiveAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            }
            else
            {
                response = "Socket is not initialized";
            }

            return response;
        }

        /// <summary>
        /// Closes the Socket connection and releases all associated resources
        /// </summary>
        public void Close()
        {
            if (_socket != null)
            {
                _socket.Close();
            }
        }

        #endregion
    }
}
