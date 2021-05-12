using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("UserId")]
        public String userID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<AccountEntry> Accounts { get; set; }

    }
}
