using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
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
        public String userID { get; set; }
    }
}
