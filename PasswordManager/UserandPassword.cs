using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class UserandPassword
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("User")]
        public string User { get; set; }
        //[BsonElement("Password")]
        //public string Password { get; set; }
        [BsonElement("SaltHash")]
        public string[] SaltHash { get; set; }
        [BsonElement("Accounts")]   
        public List<AccountEntry> Accounts { get; set; }

        public UserandPassword(string user, /*string password,*/ string[] salthash)
        {
            User = user;
            //Password = password;
            SaltHash = salthash;
            Accounts = new List<AccountEntry>();

        }

    }
   
}
