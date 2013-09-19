using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP8ExempleClient.Resources;
using HubiquitusDotNet.hapi.hStructures;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using HubiquitusDotNet.hapi.client;

namespace WP8ExempleClient
{
    public partial class MainPage : PhoneApplicationPage
    {

        private HClient client;
        private HOptions options;

        // Constructeur
        public MainPage()
        {
            InitializeComponent();          
            options = new HOptions();

            client = new HClient();
            client.onMessage += client_onMessage;
            client.onStatus += client_onStatus;
        }
        void client_onStatus(HStatus status)
        {
            Debug.WriteLine(">>>client_onStatus: " + status.ToString());
            Debug.WriteLine("--> fulljid : " + client.FullJid);
            Debug.WriteLine("--> resource : " + client.Resource);
            Update_TextBlock_UI(tbStatusScreen, status.ToString());
        }

        void client_onMessage(HMessage message)
        {
            Debug.WriteLine(">>>client_onMessage: " + message.ToString());
            Update_TextBlock_UI(tbMessageScreen, ">>>onMessage<<< \n" + message.ToString());
        }
        private void callback(HMessage msg)
        {
            Debug.WriteLine(">>>[Callback]<<< \n " + msg.ToString() + "\n");
            Update_TextBlock_UI(tbMessageScreen, ">>>CallBack<<< \n" + msg.ToString());
        }
        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            if (options.GetEndpoints() != null)
                options.GetEndpoints().Clear();

            string endpoint = tbServer.Text;
            if (!string.IsNullOrEmpty(endpoint))
            {
                JArray ja = new JArray();
                ja.Add(endpoint);
                options.SetEndpoints(ja);
            }

            client.Connect(tbUserName.Text, tbPassword.Text, options);
        }

        private void btDisconnect_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }

        private void btClearConnect_Click(object sender, RoutedEventArgs e)
        {
            Update_TextBlock_UI(tbStatusScreen, "clear");
        }
        

        private void btSubscribe_Click(object sender, RoutedEventArgs e)
        {
            client.Subscribe(tbActor.Text, callback);
        }

        private void btUnsbscribe_Click(object sender, RoutedEventArgs e)
        {
            client.Unsubscribe(tbActor.Text, callback);
        }

        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            HMessageOptions mOptions = new HMessageOptions();

            //if (persistentCb.IsChecked.Value)
            //    mOptions.Persistent = true;
            //else
            //    mOptions.Persistent = false;

            //if (!string.IsNullOrEmpty(timeoutTbx.Text))
            //    mOptions.Timeout = int.Parse(timeoutTbx.Text);

            //if (!string.IsNullOrEmpty(relevantTbx.Text))
            //    mOptions.RelevanceOffset = int.Parse(relevantTbx.Text);

            HMessage hMsg = client.BuildMessage(tbActor.Text, "string", tbMessage.Text, mOptions);
            client.Send(hMsg, null);

            Debug.WriteLine(">>>Send Message<<<\n" + hMsg.ToString() + "\n");
            Debug.WriteLine(">>>BareJid<<<\n" + client.BareJid + "\n");

        }

        private void btClearMessage_Click(object sender, RoutedEventArgs e)
        {
            Update_TextBlock_UI(tbMessageScreen, "clear");
        }

        private void Update_TextBlock_UI(TextBlock tb, string text)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if ("clear".Equals(text, StringComparison.OrdinalIgnoreCase))
                        tb.Text = "";
                    else
                        tb.Text += text;
                });
        }
    }
}