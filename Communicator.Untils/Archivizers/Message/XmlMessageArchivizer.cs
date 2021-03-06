﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;

namespace Communicator.Untils.Archivizers.Message
{
    public class XmlMessageArchivizer : IMessageArchivizer
    {
        // string pathToArchivize = "Archiwum_Wiadomości.xml";

        public void Save(MessageReq msg, string pathToArchivize)
        {
            if (File.Exists(pathToArchivize))
                AddNewElementToXmlFile(msg, pathToArchivize);
            else
                CreateArchivizeXmlFile(msg, pathToArchivize);
        }

        public List<MessageNotification> Read(string sender, string pathToArchivize)
        {
            XDocument doc = XDocument.Load(pathToArchivize);
            List<MessageNotification> Messages = (from c in doc.Descendants("Message")
                where (string) c.Attribute("Recipient") == sender || (string) c.Attribute("Sender") == sender
                select new MessageNotification
                {
                    Sender = (string) c.Attribute("Sender"),
                    Recipient = (string) c.Attribute("Recipient"),
                    Message = (string) c.Element("Body"),
                    SendTime = (DateTime) c.Attribute("Date"),
                }
                ).ToList();
            return Messages;
        }

        private void CreateArchivizeXmlFile(MessageReq msg, string pathToArchivize)
        {
            try
            {
                var newMessage =
                    new XElement("Messages",
                        new XElement("Message",
                            new XAttribute("Recipient", msg.Recipient),
                            new XAttribute("Sender", msg.Login),
                            new XAttribute("Date", DateTime.Now),
                            new XElement("Body", msg.Message)));

                newMessage.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w tworzeniu pliku archiwizacji wiadomości");
            }
        }

        private void AddNewElementToXmlFile(MessageReq msg, string pathToArchivize)
        {
            try
            {
                XDocument xmlFile = XDocument.Load(pathToArchivize);

                var newMessage =
                    new XElement("Message",
                        new XAttribute("Recipient", msg.Recipient),
                        new XAttribute("Sender", msg.Login),
                        new XAttribute("Date", DateTime.Now),
                        new XElement("Body", msg.Message)
                        );

                xmlFile.Element("Messages").Add(newMessage);
                xmlFile.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w załadowaniu pliku " + pathToArchivize +
                                  " lub dodaniu kolejnej pozycji do pliku XML");
            }
        }

        // funkcja testowa do zapisuss
        /*  public static void TestWrite()
        {
            var msg = new MessageReq();
            msg.Message = "Siema Sławek!";
            msg.Login = "Michał";
            msg.Recipient = "Sławek";
            msg.Attachment = new Attachment();

            var msg2 = new MessageReq();
            msg.Message = "Siema Monika!";
            msg.Login = "Sławek";
            msg.Recipient = "Monika";
            msg.Attachment = new Attachment();

            var msg3 = new MessageReq();
            msg.Message = "Siema Adrian!";
            msg.Login = "Monika";
            msg.Recipient = "Adrian";
            msg.Attachment = new Attachment();

            var arch = new XmlMessageArchivizer();
            arch.Save(msg, "Archiwum_Wiadomości.xml");
            arch.Save(msg2, "Archiwum_Wiadomości.xml");
            arch.Save(msg3, "Archiwum_Wiadomości.xml");
        }*/
    }
}