using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace PasswordManager
{
    public class UserandPassword
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("User")]
        public string User { get; set; }
        [BsonElement("SaltHash")]
        public string[] SaltHash { get; set; }
        [BsonElement("Accounts")]
        public List<AccountEntry> Accounts { get; set; }

        public UserandPassword(string user, string[] salthash)
        {
            User = user;
            SaltHash = salthash;
            Accounts = new List<AccountEntry>();
        }
    }
}
