﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Communicator.Protocol.Requests;
using Communicator.Queue.Services;
using Communicator.Untils;
//using Communicator.Untils.Serializers;
using Communicator.Untils.Serializers;

namespace Communicator.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public sealed partial class MainWindow : ApplicationWindowBase
    {        
        public MainWindow()
        {
            InitializeComponent();
            /*
            var jsonSerializer = new JSonSerializer();
            var client = new RabbitMqClientService(new RabbitMqConnection());
            client.Initialize(ConfigurationApp.Host, ConfigurationApp.UserName, ConfigurationApp.Password, ConfigurationApp.ExchangeName);

            client.MessageReceived += (_, ee) =>
            {
                MessageBox.Show( Encoding.UTF8.GetString(ee.Message));
            };
            var token1 = client.GetUniqueTopic("login1");
            client.CreateConsumer(token1);
            
            /*var createUserRequest = new CreateUserReq()
            {
                Login = "login1"
            };
            byte[] data = jsonSerializer.Serialize(createUserRequest);
            client.SendData(ConfigurationApp.MainQueueName, token1, data, typeof(CreateUserReq));
            

            var authRequest = new AuthRequest()
            {
                Login = "login1"
            };
            byte[] data = jsonSerializer.Serialize(authRequest);
            client.SendData(ConfigurationApp.MainQueueName, token1, data, typeof(AuthRequest));
            
            var msgRequest = new MessageReq()
            {
                Message = "dupaArchive",
                Recipient = "login2"
            };

             data = jsonSerializer.Serialize(msgRequest);
            client.SendData(ConfigurationApp.MainQueueName, token1, data, typeof(MessageReq));*/
        }

        private void Button_WinClose_Click(object sender, RoutedEventArgs e)
        {
          
            this.Close();
        }

        private void Button_WinMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ContactList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
        }

    }
}
