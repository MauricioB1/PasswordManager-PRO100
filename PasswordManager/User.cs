using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class User
    {
        //[BsonId]
        //public ObjectId Id { get; set; }
        //[BsonElement("UserId")]
        //public String userID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<AccountEntry> Accounts { get; set; }

    }
}
