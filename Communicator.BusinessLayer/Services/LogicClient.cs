﻿using Communicator.BusinessLayer.Interfaces;
using Communicator.BusinessLayer.Models;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Interfaces;

namespace Communicator.BusinessLayer.Services
{
    public class LogicClient : ILogicClient
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMessageRecognizerClientService _messageRecognizerClientService;
        private readonly IQueueClientService _queueClientService;

        public LogicClient(IQueueClientService queueClientService, IConfigurationService configurationService,
            IMessageRecognizerClientService messageRecognizerClientService)
        {
            _queueClientService = queueClientService;
            _configurationService = configurationService;
            _messageRecognizerClientService = messageRecognizerClientService;
        }

        public string RouteKey { get; set; }
        public string Login { get; set; }

        public void Initialize()
        {
            _queueClientService.Initialize(_configurationService.Host, _configurationService.UserName,
                _configurationService.Password, _configurationService.ExchangeName);
            _queueClientService.MessageReceived += (_, ee) => _messageRecognizerClientService.ProceedMessage(ee);
            _queueClientService.CreateConsumer(RouteKey, _configurationService.ExchangeName);
            _messageRecognizerClientService.Repeater += OnRepeater;
        }

        public void GetHistory()
        {
            var historyReq = new HistoryReq
            {
                Login = Login
            };

            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, historyReq);
        }

        public event RepeaterEventHandler Repeater;

        public void RegisterUser(UserModel user)
        {
            var createUserReq = new CreateUserReq {Login = user.Login, Password = user.Password};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, createUserReq);
        }

        public void LoginUser(UserModel user)
        {
            var authReq = new AuthRequest {Login = user.Login, Password = user.Password};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, authReq);
        }

        public void GetUserList()
        {
            var userListReq = new UserListReq {Login = Login};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, userListReq);
        }

        public void SendMessage(string recipient, string message, byte[] imageData)
        {
            var messageReq = new MessageReq
            {
                Login = Login,
                Message = message,
                Recipient = recipient
            };
            if (imageData != null)
            {
                messageReq.Attachment = new Attachment
                {
                    Data = imageData
                };
            }

            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, messageReq);
        }

        public void SendPing(PresenceStatus status)
        {
            var presenceNotification = new PresenceStatusNotification {Login = Login, PresenceStatus = status};
            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, presenceNotification);
        }

        public void SendUserWriting(string recipient)
        {
            var activityReq = new ActivityReq
            {
                IsWriting = true,
                Recipient = recipient,
                Login = Login
            };

            _queueClientService.SendData(_configurationService.MainQueueName, RouteKey,
                _configurationService.ExchangeName, activityReq);
        }

        public void OnRepeater(object sender, RepeaterEventArgs e)
        {
            if (Repeater != null)
            {
                Repeater(this, e);
            }
        }
    }
}