﻿using System;
using Communicator.Queue.Interfaces;
using Communicator.Untils.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communicator.Queue.Services
{
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    public class RabbitMqClientService : IQueueClientService
    {
        private readonly IQueueConnection _queueConnection;
        private readonly ISerializerService _serializerService;
        private IModel _model;

        public RabbitMqClientService(IQueueConnection queueConnection, ISerializerService serializerService)
        {
            _queueConnection = queueConnection;
            _serializerService = serializerService;
        }

        public event MessageReceivedEventHandler MessageReceived;

        public void Initialize(string host, string userName, string password, string exchangeName)
        {
            _model = _queueConnection.CreateModel(host, userName, password, ExchangeType.Topic);
            _model.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        }

        public void CreateConsumer(string routingKey, string exchangeName)
        {
            QueueDeclareOk queue = _model.QueueDeclare();
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received +=
                (_, msg) =>
                {
                    MessageReceived(this, CreateMessage(msg));
                    _model.BasicAck(msg.DeliveryTag, false);
                };
            _model.QueueBind(queue.QueueName, exchangeName, routingKey);
            _model.BasicConsume(queue.QueueName, false, consumer);
        }

        public void SendData<T>(string queueName, string routingKey, string exchangeName, T data)
        {
            IBasicProperties properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);
            properties.ReplyTo = routingKey;
            properties.Type = typeof (T).AssemblyQualifiedName;

            byte[] buffer = _serializerService.Serialize(data);

            _model.BasicPublish(exchangeName, queueName, properties, buffer);
        }

        public string GetUniqueTopic(string login)
        {
            return String.Format("client.{0}", login);
        }

        private MessageReceivedEventArgs CreateMessage(BasicDeliverEventArgs e)
        {
            var msg = new MessageReceivedEventArgs
            {
                Message = e.Body,
                ContentType = e.BasicProperties.Type,
                TopicSender = e.BasicProperties.ReplyTo
            };
            return msg;
        }
    }
}