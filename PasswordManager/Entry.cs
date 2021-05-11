using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class Entry
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }


        public Entry(string username, string password, string url)
        {
            UserName = "UserName- " + username;
            Password = "Password- " + password;
            Url = "URL- " + url;
        }

        public override string ToString()
        {
            return "UserName- " + UserName + ", Password- " + Password + ", URL- " + Url;
        }
    }
}
