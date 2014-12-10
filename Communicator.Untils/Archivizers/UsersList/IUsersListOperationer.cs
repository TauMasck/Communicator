﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Untils.Archivizers.UsersList
{
    interface IUsersListOperationer
    {
        bool CreateNewUser(User user, string path);
        bool AuthenticationUser(User user, string path);

        // user żeby nie zczytać własnego loginu
        List<User> ReadList(User user, string path); 
    }
}