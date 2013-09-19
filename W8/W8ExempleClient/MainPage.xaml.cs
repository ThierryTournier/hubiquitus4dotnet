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


using HubiquitusDotNet.hapi.hStructures;
using HubiquitusDotNet.hapi.client;
using HubiquitusDotNet.hapi.transport;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace W8ExempleClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HClient client;
        private HOptions options;
        public MainPage()
        {
            this.InitializeComponent();
            client = new HClient();
            options = new HOptions();

            client.onMessage += client_onMessage;
            client.onStatus += client_onStatus;

        }

        void client_onStatus(HStatus status)
        {
            Debug.WriteLine(">>>client_onStatus: " + status.ToString());
            Debug.WriteLine("--> fulljid : " + client.FullJid);
            Debug.WriteLine("--> resource : " + client.Resource);
            if (!status.GetErrorCode().Equals("NO_ERROR"))
                Update_TextBlock_UI(errorScreen, status.GetErrorMsg());
            Update_TextBlock_UI(statusScreen, status.ToString());
        }

        void client_onMessage(HMessage message)
        {
            Debug.WriteLine(">>>client_onMessage: " + message.ToString());
            Update_TextBlock_UI(msgScreen, ">>>onMessage<<< \n" + message.ToString());
        }

        private void callback(HMessage msg)
        {
            Debug.WriteLine(">>>[Callback]<<< \n " + msg.ToString() + "\n");
            Update_TextBlock_UI(msgScreen, ">>>CallBack<<< \n" + msg.ToString());
        }


        private async void Update_TextBlock_UI(TextBlock tb, string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
                 () =>
                 {
                     if ("clear".Equals(text, StringComparison.OrdinalIgnoreCase))
                         tb.Text = "";
                     else
                         tb.Text += text;
                 }
                 );
        }

        private void connBt_Click(object sender, RoutedEventArgs e)
        {
            
            if (options.GetEndpoints() != null)
                options.GetEndpoints().Clear();

            string endpoint = serverTbx.Text;
            if (!string.IsNullOrEmpty(endpoint))
            {
                JArray ja = new JArray();
                ja.Add(endpoint);
                options.SetEndpoints(ja);
            }
            
            client.Connect(usernamTbx.Text, passwordTbx.Text, options);

        }


        

        private void disConnBt_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }

        private void sendBt_Click(object sender, RoutedEventArgs e)
        {
            HMessageOptions mOptions = new HMessageOptions();

            if (persistentCb.IsChecked.Value)
                mOptions.Persistent = true;
            else
                mOptions.Persistent = false;

            if (!string.IsNullOrEmpty(timeoutTbx.Text))
                mOptions.Timeout = int.Parse(timeoutTbx.Text);

            if (!string.IsNullOrEmpty(relevantTbx.Text))
                mOptions.RelevanceOffset = int.Parse(relevantTbx.Text);

            HMessage hMsg = client.BuildMessage(actorTbx.Text, "string", msgTbx.Text, mOptions);
            client.Send(hMsg, null);
            
            Debug.WriteLine(">>>Send Message<<<\n" + hMsg.ToString() + "\n");
            Debug.WriteLine(">>>BareJid<<<\n" + client.BareJid + "\n");

        }
                
        private void getsubBt_Click(object sender, RoutedEventArgs e)
        {
            client.GetSubscriptions(callback);
        }

        private void setFilterBt_Click(object sender, RoutedEventArgs e)
        {
            HCondition filter = new HCondition();
            HArrayOfValue valueArray = new HArrayOfValue();
            valueArray.SetName("publisher");
            JArray ja = new JArray();
            ja.Add("urn:localhost:u1");
            ja.Add("urn:localhost:u2");
            valueArray.SetValues(ja);
            filter.SetInValue(valueArray);

            Debug.WriteLine("\n-- set filer --");
            Debug.WriteLine(filter);
            Debug.WriteLine("---------------\n");
          
            //Remove the filter.
            //HCondition filter = new HCondition(JObject.Parse("{}")); 
            client.SetFilter(filter, callback);
        }

        private void subBt_Click(object sender, RoutedEventArgs e)
        {
            client.Subscribe(actorTbx.Text, callback);
        }

        private void unsubBt_Click(object sender, RoutedEventArgs e)
        {
            client.Unsubscribe(actorTbx.Text, callback);
        }

        private void clearBt_Status_Click(object sender, RoutedEventArgs e)
        {
            Update_TextBlock_UI(statusScreen, "clear");
        }
       
        private void clearBt_Error_Click(object sender, RoutedEventArgs e)
        {
            Update_TextBlock_UI(errorScreen, "clear");
        }

        private void clearBt_Message_Click(object sender, RoutedEventArgs e)
        {
            Update_TextBlock_UI(msgScreen, "clear");
        }

    }
}
