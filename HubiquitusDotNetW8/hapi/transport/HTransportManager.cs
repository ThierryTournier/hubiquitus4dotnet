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
using System;
using Windows.Networking.Connectivity;
using Windows.System.Threading;
using Windows.Web;

namespace HubiquitusDotNetW8.hapi.transport
{
    public class HTransportManager
    {
        private NetworkStatusChangedEventHandler networkStatusChangedCallback = null;

        private bool registeredNetworkStatusNotif = false;
        private bool shouldConnect = false;
        private bool hasNetwork = false;

        private HTransportOptions tOptions = null;
        private ConnectionStatus connStatus = ConnectionStatus.DISCONNECTED;
        public HTransport Transport {get; set;}

        public DataEventHandler onData;
        public StatusEventHandler onStatus;
       

        public HTransportManager(HTransport tranpsport) 
        {
            this.Transport = tranpsport;
            this.Transport.onData += Transport_onData;
            this.Transport.onStatus += Transport_onStatus;
            ConnectionProfile internetConnectProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectProfile == null)
                this.hasNetwork = false;
            else
            {
                switch (internetConnectProfile.GetNetworkConnectivityLevel())
                { 
                    case NetworkConnectivityLevel.None:
                        this.hasNetwork = false;
                        break;
                    case NetworkConnectivityLevel.LocalAccess:
                    case NetworkConnectivityLevel.InternetAccess:
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        this.hasNetwork = true;
                        break;
                }
            }
            networkStatusChangedCallback = new NetworkStatusChangedEventHandler(OnNetworkStatusChanged);
            if (!registeredNetworkStatusNotif)
            {
                NetworkInformation.NetworkStatusChanged += networkStatusChangedCallback;
                registeredNetworkStatusNotif = true;
            }

        }

        
       

        public async void Connect(HTransportOptions tOptions)
        {
            this.tOptions = tOptions;
            this.shouldConnect = true;
            if (connStatus != ConnectionStatus.CONNECTED && connStatus != ConnectionStatus.CONNECTING)
                await ThreadPool.RunAsync(TryToConnectDisconnect, WorkItemPriority.High);
            else if (connStatus == ConnectionStatus.CONNECTING)
                this.onStatus(ConnectionStatus.CONNECTING, ConnectionErrors.CONN_PROGRESS, ErrorMessage.connWhileConnecting);
            else if (connStatus == ConnectionStatus.CONNECTED)
                this.onStatus(ConnectionStatus.CONNECTED, ConnectionErrors.ALREADY_CONNECTED, ErrorMessage.alreadyConn);
        }

        public async void Disconnect()
        {
            this.shouldConnect = false;
            if (connStatus != ConnectionStatus.DISCONNECTED && connStatus != ConnectionStatus.DISCONNECTING)
                await ThreadPool.RunAsync(TryToConnectDisconnect, WorkItemPriority.High);
            else if (connStatus == ConnectionStatus.DISCONNECTING)
                this.onStatus(ConnectionStatus.DISCONNECTING, ConnectionErrors.NOT_CONNECTED, ErrorMessage.disconnWhileDisconnecting);
            else if (connStatus == ConnectionStatus.DISCONNECTED)
                this.onStatus(ConnectionStatus.DISCONNECTED, ConnectionErrors.NOT_CONNECTED, ErrorMessage.alreadyDisconn);
        }

        public void SendObject(JObject obj)
        {
            this.Transport.SendObject(obj);
        }

        private void TryToConnectDisconnect(object state) 
        {
            if (this.hasNetwork)
            {
                if (shouldConnect && connStatus != ConnectionStatus.CONNECTED && connStatus != ConnectionStatus.CONNECTING)
                {
                    Transport.Connect(this.tOptions);
                }
                else if (!shouldConnect && connStatus != ConnectionStatus.DISCONNECTED && connStatus != ConnectionStatus.DISCONNECTING)
                {
                    connStatus = ConnectionStatus.DISCONNECTING;
                    Transport.Disconnect();
                }
            }
            else
                this.onStatus(connStatus, ConnectionErrors.TECH_ERROR, ErrorMessage.noConnectivity);
        }

        void Transport_onData(string type, Newtonsoft.Json.Linq.JObject obj)
        {
            if (this.onData != null)
            {
                this.onData(type, obj);
            }
        }

        void Transport_onStatus(ConnectionStatus status, ConnectionErrors error, string errorMsg)
        {
            this.connStatus = status;
            if (onStatus != null)
            {

                if (WebErrorStatus.ConnectionAborted.ToString().Equals(errorMsg))
                {
                    if (this.shouldConnect)
                        onStatus(status, error, errorMsg + " " + ErrorMessage.reconnIn5s);
                    else
                        onStatus(status, error, errorMsg);
                    ThreadPoolTimer timer = ThreadPoolTimer.CreateTimer(
                       async (obj) =>
                       {
                           await ThreadPool.RunAsync(TryToConnectDisconnect, WorkItemPriority.High);
                       }, new TimeSpan(0, 0, 0, 0, 5000));

                }
                else
                {
                    onStatus(status, error, errorMsg);
                    if (error == ConnectionErrors.AUTH_FAILED)
                    {
                        this.shouldConnect = false;
                    }
                }
            }
            else
                throw new ArgumentNullException("Error: " + this.GetType() + " require a StatusEventHandler onStatus");
        }



        private async void OnNetworkStatusChanged(object sender)
        {
            ConnectionProfile internetConnectProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectProfile == null)
                this.hasNetwork = false;
            else
            {
                switch (internetConnectProfile.GetNetworkConnectivityLevel())
                {
                    case NetworkConnectivityLevel.None:
                        this.hasNetwork = false;
                        break;
                    case NetworkConnectivityLevel.LocalAccess:
                    case NetworkConnectivityLevel.InternetAccess:
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        this.hasNetwork = true;
                        await ThreadPool.RunAsync(TryToConnectDisconnect, WorkItemPriority.High);
                        break;
                }
            }
        }
    }
}
