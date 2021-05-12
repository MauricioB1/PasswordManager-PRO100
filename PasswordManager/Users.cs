using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class Users
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("UserId")]
        public string userID { get; set; }
        public string UserName { get; set; }
        public int Password { get; set; }
        public string URL { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public IDictionary<string, string> Accounts = new Dictionary<string, string>();

        public Users()
        {
            Accounts.Add("url", URL);
            Accounts.Add("username", username);
            Accounts.Add("password", password);
        }

    }
}
