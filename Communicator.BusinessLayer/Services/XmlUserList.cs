﻿using System;
using System.IO;
using System.Xml.Linq;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Protocol.Requests;

namespace Communicator.BusinessLayer.Services
{
    public class XmlUserList : IUsersListOperationer
    {
        public static void CreateNewUser(string path, CreateUserReq user)
        {
            if (File.Exists(path))
                Add(user, path);
            else
                CreateArchivizeXmlFile(user, path);
        }

        //Dodaje nowego użytkownika jesli jego login nie jest zajęty
        /*  public bool CreateNewUser(CreateUserReq user, string path)
          {
              bool createSucces = false;
              if (File.Exists(path))
              {
                  if (!CheckAvailability(user, path)) // jeśli nie ma
                  {
                      Add(user, path);
                      createSucces = true;
                  }
              }
              else
              {
                  CreateArchivizeXmlFile(user, path);
                  CommonUserList.RefreshCommonList(path); // odświeżenie z pliku
                  createSucces = true;
              }

              return createSucces;
          }
          */
        /*
        // autentykacja użytkonika
        public bool AuthenticationUser(AuthRequest user, string path)
        {
            XDocument doc = XDocument.Load(path);

            var query = from u in doc.Descendants("User")
                        where (string)u.Attribute("Login") == user.Login && (string)u.Attribute("Password") == user.Password
                        select u.Value;

            return query.Any(); // jeli jest true jesli nie false
        }
        */
        /*
        // zwraca listę wszystkich użytkowników
        public List<User> ReadList(User user, string path)
        {
            XDocument doc = XDocument.Load(path);
            var users = (from c in doc.Descendants("User")
                         where (string)c.Attribute("Login") != user.Login
                         select new User
                         {
                             Login = (string)c.Attribute("Login"),
                         }).ToList();
            return users;
        }
        */
        // pierwszy wspis do pliku, stworzenie pliku z użytkownikami
        private static void CreateArchivizeXmlFile(CreateUserReq usr, string pathToArchivize)
        {
            try
            {
                var newUser =
                    new XElement("Users",
                        new XElement("User",
                            new XAttribute("Login", usr.Login),
                            new XAttribute("Password", usr.Password)));

                newUser.Save(pathToArchivize);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w tworzeniu pliku z użytkownikami");
            }
        }

        // wpisanie użytkownika do pliku
        private static void Add(CreateUserReq user, string path)
        {
            try
            {
                XDocument xmlFile = XDocument.Load(path);

                var newUser =
                    new XElement("User",
                        new XAttribute("Login", user.Login),
                        new XAttribute("Password", user.Password));

                xmlFile.Element("Users").Add(newUser);
                xmlFile.Save(path);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w załadowaniu pliku lub dodaniu kolejnej pozycji do pliku XML z użytkownikami");
            }
        }

        /*
        // sprawdza czy dany login jest zajęty
        private bool CheckAvailability(CreateUserReq user, string path)
        {
            XDocument doc = XDocument.Load(path);

            var query = from u in doc.Descendants("User")
                        where (string)u.Attribute("Login") == user.Login
                        select u.Value;

            return query.Any(); // jeli jest true jesli nie false
        }
         * */

        //funkcja testowa 
        public static void TestFunctionality()
        {
            /*
            User u1 = new User();
            u1.Login = "koala";
            u1.Password = "tajnehaslo";

            User u2 = new User();
            u2.Login = "koal";
            u2.Password = "tajnehaslo";

            User u3 = new User();
            u3.Login = "koala";
            u3.Password = "thaslo";

            XmlUserList ul = new XmlUserList();
            bool ok = ul.CreateNewUser(u1, "ListaUżytkowników.xml");
            Console.WriteLine(ok);

            bool ok1 = ul.CreateNewUser(u2, "ListaUżytkowników.xml");
            Console.WriteLine(ok1);

            bool ok2 = ul.CreateNewUser(u3, "ListaUżytkowników.xml");
            Console.WriteLine(ok2);

            List<User> nl = ul.ReadList(u2, "ListaUżytkowników.xml");

            foreach (User u in nl)
                Console.WriteLine(u.Login);
            */
        }
    }
}