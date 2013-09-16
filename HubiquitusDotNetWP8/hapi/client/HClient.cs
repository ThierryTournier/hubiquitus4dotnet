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



using HubiquitusDotNetPortable.hapi.exceptions;
using HubiquitusDotNetPortable.hapi.hStructures;
using HubiquitusDotNetPortable.hapi.transport;
using HubiquitusDotNetPortable.hapi.util;
using HubiquitusDotNetWP8.hapi.transport;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.System.Threading;

namespace HubiquitusDotNetWP8.hapi.client
{
    /// <summary>
    /// Hubiquitus client, public api version v0.5
    /// </summary>
    public class HClient
    {
        private ConnectionStatus connectionStatus = ConnectionStatus.DISCONNECTED;
        private HOptions options = null;
        private HTransport transport = null;
        private HTransportManager transportManager = null;
        private HTransportOptions transportOptions;
        private bool isEventHandlerAdded = false;
        private HCondition filter = new HCondition();

        public string FullJid { get { return this.transportOptions.FullUrn; } }
        public string BareJid { get { return this.transportOptions.FullUrn.Split('/')[0]; } }
        public string Resource { get { return this.transportOptions.Resource; } }
        public ConnectionStatus Status { get { return this.connectionStatus; } }

        public delegate void StatusEventHandler(HStatus status);
        public delegate void MessageEventHandler(HMessage message);

        /// <summary>
        /// Status event handler recieve all connection status events
        /// </summary>
        public event StatusEventHandler onStatus;

        /// <summary>
        /// Message event handler recieve all message events
        /// </summary>
        public event MessageEventHandler onMessage;


        private Dictionary<string, Action<HMessage>> messageDelegates = new Dictionary<string, Action<HMessage>>();
        private Dictionary<string, ThreadPoolTimer> timerOutDictionary = new Dictionary<string, ThreadPoolTimer>();

        public HClient()
        {
            transportOptions = new HTransportOptions();
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="login">user jid i.e.(my_user@domain/resource)</param>
        /// <param name="password"></param>
        /// <param name="options"></param>
        /// <param name="context"></param>
        public void Connect(string login, string password, HOptions options, JObject context)
        {
            bool shouldConnect = false;
            bool connInprogress = false;
            bool disconnInprogress = false;
            this.options = new HOptions(options);

            if (this.connectionStatus == ConnectionStatus.DISCONNECTED)
            {
                shouldConnect = true;
                connectionStatus = ConnectionStatus.CONNECTING;
            }
            if (this.connectionStatus == ConnectionStatus.CONNECTING)
                connInprogress = true;
            if (this.connectionStatus == ConnectionStatus.DISCONNECTING)
                disconnInprogress = true;

            if (shouldConnect)
            {
                
                fillTransportOptions(login, password, options, context);

                if (options.GetTransport() == "socketio")
                {
                    if (transport == null || !(transport is HTransportSocketIO))
                    {
                        this.transport = new HTransportSocketIO();
                    }
                    if (transportManager == null)
                        this.transportManager = new HTransportManager(this.transport);
                    if (!isEventHandlerAdded)
                    {
                        this.transportManager.onStatus = transport_onStatus;
                        this.transportManager.onData = transport_onData;
                        isEventHandlerAdded = true;
                    }
                    notifyStatus(ConnectionStatus.CONNECTING, ConnectionErrors.NO_ERROR, null);
                    this.transportManager.Connect(this.transportOptions);
                }
            }
            else
            {
                if (connInprogress)
                    notifyStatus(ConnectionStatus.CONNECTING, ConnectionErrors.CONN_PROGRESS, null);
                else if (disconnInprogress)
                    notifyStatus(ConnectionStatus.DISCONNECTING, ConnectionErrors.ALREADY_CONNECTED, null);
                else
                    notifyStatus(ConnectionStatus.CONNECTED, ConnectionErrors.ALREADY_CONNECTED, null);
            }
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="options"></param>
        public void Connect(string login, string password, HOptions options)
        {
            this.Connect(login, password, options, null);
        }

        void transport_onData(string type, JObject obj)
        {
            if (type.Equals("hmessage", StringComparison.OrdinalIgnoreCase))
                notifyMessage(new HMessage(obj),  null);
        }

        private void transport_onStatus(ConnectionStatus status, ConnectionErrors error, string errrorMsg)
        {
            notifyStatus(status, error, errrorMsg);
        }

        /// <summary>
        /// disconnect from server
        /// </summary>
        public void Disconnect()
        {
            bool shouldDisconnect = false;
            bool connInprogress = false;

            if (this.connectionStatus == ConnectionStatus.CONNECTED)
            {
                shouldDisconnect = true;
                this.connectionStatus = ConnectionStatus.DISCONNECTING;
            }
            else if (this.connectionStatus == ConnectionStatus.CONNECTING)
            {
                connInprogress = true;
            }

            if (shouldDisconnect)
            {
                notifyStatus(ConnectionStatus.DISCONNECTING, ConnectionErrors.NO_ERROR, null);
                transportManager.Disconnect();
            }
            else if (connInprogress)
                notifyStatus(ConnectionStatus.CONNECTING, ConnectionErrors.CONN_PROGRESS, "Can not disconnect while a connection is in progress");
            else
                notifyStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.NOT_CONNECTED, null);
        }

       

        
        /// <summary>
        /// removed since v0.5
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="messageDelegate"></param>
        public void Send(HMessage message, Action<HMessage> messageDelegate)
        {
            if (this.connectionStatus != ConnectionStatus.CONNECTED)
            { 
                notifyResultError(message.GetMsgid(), ResultStatus.NOT_CONNECTED, "Not connected.", messageDelegate);
                return;
            }
            if (message == null)
            {
                notifyResultError(null, ResultStatus.MISSING_ATTR, "Provided message is null.", messageDelegate);
                return;
            }
            if (message.GetActor() == null)
            {
                notifyResultError(message.GetMsgid(), ResultStatus.MISSING_ATTR, "Actor is missing.", messageDelegate);
                return;
            }


            message.SetSent(DateTime.UtcNow);
            message.SetPublished(DateTime.UtcNow);
            message.SetMsgid(Guid.NewGuid().ToString());
            message.SetPublisher(transportOptions.FullUrn);
            

            if (message.GetTimeout() > 0)
            {
                // hAPI will do correlation. If no answer within the
                // timeout, a timeout error will be sent.
                if (messageDelegate != null)
                { 
                    messageDelegates.Add(message.GetMsgid(), messageDelegate);

                    ThreadPoolTimer timeOutTimer = ThreadPoolTimer.CreateTimer((obj) =>
                    {
                        notifyResultError(message.GetMsgid(), ResultStatus.EXEC_TIMEOUT, "The response of message is time out.", null);
                    }, new TimeSpan(0, 0, 0, 0, message.GetTimeout()));
                    
                    timerOutDictionary.Add(message.GetMsgid(), timeOutTimer);
                }
                else
                {
                    //when there is no callback, timeout has no sense. delete timeout.
                    message.SetTimeout(0);
                }
            }
            transportManager.SendObject(message);
        }

        /// <summary>
        /// The client MUST be connected to access to this service.
        /// Demands the server a subscription to a channel.
        /// The server will check if not already subscribed and if authorized subscribe him.
        /// </summary>
        /// <param name="actor">channel id</param>
        /// <param name="messageDelegate">An action notified when a result issued</param>
        public void Subscribe(string actor, Action<HMessage> messageDelegate)
        {
            if (messageDelegate == null)
                throw new MissingAttrException("messageDelegate");
         
            HMessage cmdMessage = BuildCommand(actor, "hSubscribe", null, null, null);
            cmdMessage.SetTimeout(options.GetMsgTimeout());
            this.Send(cmdMessage, messageDelegate);
        }

        /// <summary>
        /// The client MUST be connected to access to this service.
        /// Demands the server an unsubscription to the channel
        /// </summary>
        /// <param name="messageDelegate"></param>
        public void Unsubscribe(string actor, Action<HMessage> messageDelegate)
        {
            if (actor == null || actor.Length <= 0)
                throw new MissingAttrException("actor");
            if (messageDelegate == null)
                throw new MissingAttrException("messageDelegate");
            JObject @params = new JObject();
            @params["channel"] = actor;
            HMessage cmdMessage = BuildCommand("session", "hUnsubscribe", @params, null, null);
            cmdMessage.SetTimeout(options.GetMsgTimeout());
            this.Send(cmdMessage, messageDelegate);
        }


        /// <summary>
        /// Demands the server a list of the publisher’s subscriptions.
	    /// Nominal response : a hMessage with a hResult payload contains an array of channel id which are all active.
        /// </summary>
        /// <param name="messageDelegate"></param>
        public void GetSubscriptions(Action<HMessage> messageDelegate)
        {
            if (messageDelegate == null)
                throw new MissingAttrException("messageDelegate");
            HMessage cmdMessage = BuildCommand("session", "hGetSubscriptions", null, null, null);
            cmdMessage.SetTimeout(options.GetMsgTimeout());
            Send(cmdMessage, messageDelegate);
        }


        /// <summary>
        /// Sets a filter to be applied to upcoming messages at the session level.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="messageDelegate"></param>
        public void SetFilter(HCondition filter, Action<HMessage> messageDelegate)
        {
            HMessage cmdMessage = BuildCommand("session", "hSetFilter", filter, null, null);
            this.filter = filter;
            cmdMessage.SetTimeout(options.GetMsgTimeout());
            Send(cmdMessage, messageDelegate);
        }

       
        
        //Builders
        /// <summary>
        /// The client MUST be connected to access to this service.
        /// Allow a hubapp client to create a hMessage.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="type"></param>
        /// <param name="payload"></param>
        /// <param name="mOptions"></param>
        /// <returns></returns>
        public HMessage BuildMessage(string actor, string type, JObject payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, JArray payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, string payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, bool payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, int payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, double payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, HCommand payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        public HMessage BuildMessage(string actor, string type, HResult payload, HMessageOptions mOptions)
        {
            return InnerBuildMessage(actor, type, payload, mOptions);
        }

        /// <summary>
        /// since v0.5
        /// The client MUST be connected to access to this service.
        /// Allow a hubapp client to create a hMessage with a hCommand payload.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="cmd"></param>
        /// <param name="params"></param>
        /// <param name="filter"></param>
        /// <param name="mOptions"></param>
        /// <returns></returns>
        public HMessage BuildCommand(string actor, string cmd, JToken @params, HCondition filter, HMessageOptions mOptions)
        {
            if (actor == null || actor.Length <= 0)
                throw new MissingAttrException("actor");
            if (cmd == null || cmd.Length <= 0)
                throw new MissingAttrException("cmd");

            HCommand hcommand = new HCommand(cmd, @params, filter);
           
            HMessage hmessage = BuildMessage(actor, "hCommand", hcommand, mOptions);
            return hmessage;
        }

        /// <summary>
        /// since v0.5
        /// The client MUST be connected to access to this service.
        /// Allow a hubapp client to create a hMessage with a hResult payload.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="status"></param>
        /// <param name="result"></param>
        /// <param name="mOptions"></param>
        /// <returns></returns>
        public HMessage BuildResult(string actor, string @ref, ResultStatus status, JObject result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        public HMessage BuildResult(string actor, string @ref, ResultStatus status, JArray result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        public HMessage BuildResult(string actor, string @ref, ResultStatus status, string result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        public HMessage BuildResult(string actor, string @ref, ResultStatus status, bool result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        public HMessage BuildResult(string actor, string @ref, ResultStatus status, int result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        public HMessage BuildResult(string actor, string @ref, ResultStatus status, double result, HMessageOptions mOptions)
        {
            return InnerBuildResult(actor, @ref, status, result, mOptions);
        }

        //---private methods---

        private HMessage InnerBuildResult(string actor, string @ref, ResultStatus? status, JToken result, HMessageOptions mOptions)
        {
            if (actor == null || actor.Length <= 0)
                throw new MissingAttrException("actor");
            if (@ref == null || @ref.Length <= 0)
                throw new MissingAttrException("ref");
            if (status == null)
                throw new MissingAttrException("status");

            HResult hResult = new HResult();
            hResult.SetResult(result);
            hResult.SetStatus(status);
            if (mOptions == null)
                mOptions = new HMessageOptions();
            mOptions.Ref = @ref;

            return BuildMessage(actor, "hResult", hResult, mOptions);
        }


        private HMessage InnerBuildMessage(string actor, string type, JToken payload, HMessageOptions mOptions)
        {
            if (actor == null || actor.Length <= 0)
                throw new MissingAttrException("actor");

            HMessage message = new HMessage();
            message.SetActor(actor);
            message.SetType(type);
            if (mOptions != null)
            {
                message.SetRef(mOptions.Ref);
                message.SetConvid(mOptions.Convid);
                message.SetPriority(mOptions.Priority);
                message.SetAuthor(mOptions.Author);
                message.SetHeaders(mOptions.Headers);
                message.SetLocation(mOptions.Location);
                message.SetPublished(mOptions.Published);
                message.SetPersistent(mOptions.Persistent);
                message.SetTimeout(mOptions.Timeout);
                if (mOptions.RelevanceOffset != null)
                {
                    Debug.WriteLine("----   " + mOptions.RelevanceOffset);
                    message.SetRelevance((DateTime.UtcNow).AddMilliseconds(mOptions.RelevanceOffset.Value));
                    Debug.WriteLine("++++   " + message.GetRelevance());
                }
                else
                    message.SetRelevance(mOptions.Relevance);
            }
            if (transportOptions != null && transportOptions.Login != null)
                message.SetPublisher(transportOptions.FullUrn);
            else
                message.SetPublisher(null);
            message.SetPayload(payload);
            return message;
        }

        private void notifyStatus(ConnectionStatus status, ConnectionErrors error, string errorMsg)
        {
            connectionStatus = status;
            if (this.onStatus != null)
            {
                HStatus hstatus = new HStatus();
                hstatus.SetStatus(status);
                hstatus.SetErrorCode(error);
                hstatus.SetErrorMsg(errorMsg);

                IAsyncAction threadPoolWorkItem = ThreadPool.RunAsync(
                    (source) =>
                    {
                        this.onStatus(hstatus);
                    }
                    );
            }
        }

        private void notifyMessage(HMessage message, Action<HMessage> messageDelegate)
        {
            // 1 - we search the delegate with the ref if any in delegate dictionnary.
            if(messageDelegates.Count >0 && message.GetRef() != null && messageDelegates.ContainsKey(HUtil.GetApiRef(message.GetRef())))
            {
                string msgRef = HUtil.GetApiRef(message.GetRef());
                if (timerOutDictionary.ContainsKey(msgRef))
                {
                    ThreadPoolTimer timer = timerOutDictionary[msgRef];
                    timerOutDictionary.Remove(msgRef);
                    if (timer != null)
                        timer.Cancel();
                }
                Action<HMessage> action = messageDelegates[msgRef];
                messageDelegates.Remove(msgRef);
                if (action != null)
                {
                    IAsyncAction threadPoolWorkItem = ThreadPool.RunAsync(
                        (source) =>
                        {
                            action(message);
                        }
                        );
                }
            }

            // 2 - if the ref can not provide a delegate, we try the parameter sent 
            else if (messageDelegate != null)
            {
                IAsyncAction threadPoolWorkItem = ThreadPool.RunAsync(
                        (source) =>
                        {
                            messageDelegate(message);
                        }
                        );
            }

            else 
            {
                // 3 - in other case, try the default message delegate onMessage. 
                if (this.onMessage != null)
                {
                    IAsyncAction threadPoolWorkItem = ThreadPool.RunAsync(
                       (source) =>
                       {
                           this.onMessage(message);
                       }
                       );
                }
            }
        }

      

        private void notifyResultError(string @ref, ResultStatus resultStatus, string errorMsg, Action<HMessage> messageDelegate)
        {
            JObject obj = new JObject();
            obj["errorMsg"] = errorMsg;
            
            HResult result = new HResult();
            result.SetStatus(resultStatus);
            result.SetResult(obj);

            HMessage message = new HMessage();
            message.SetRef(@ref);
            message.SetType("hResult");
            message.SetPayload(result);

            this.notifyMessage(message, messageDelegate);

        }

        private void fillTransportOptions(string login, string password, HOptions options, JObject context)
        {
            try
            {
                
                this.transportOptions.Login = login;
                this.transportOptions.Password = password;
                this.transportOptions.Timeout = options.GetTimeout();
                this.transportOptions.AuthCb = options.AuthCb;
             

                //for endpoints, pick one randomly and fill transport options
                if (options.GetEndpoints().Count() > 0)
                {
                    int endpointIndex = HUtil.PickIndex(options.GetEndpoints());
                    string endpoint = options.GetEndpoints()[endpointIndex].ToString();

                    transportOptions.EndpointHost = HUtil.GetHost(endpoint);
                    transportOptions.EndpointPort = HUtil.GetPort(endpoint);
                    transportOptions.EndpointPath = HUtil.GetPath(endpoint);
                }
                else
                {
                    transportOptions.EndpointHost = null;
                    transportOptions.EndpointPort = 0;
                    transportOptions.EndpointPath = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : ", e);
            }
        }

    }
}
