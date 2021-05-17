﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class UserandPassword
    {
        [BsonElement("User")]
        public string User { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        
        public UserandPassword(string user, string password)
        {
            User = user;
            Password = password;

        }


    }
   
}
