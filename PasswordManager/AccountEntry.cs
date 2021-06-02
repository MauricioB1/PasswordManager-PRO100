using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class AccountEntry
    {
        public string AccountUserName { get; set; }
        public string AccountPassword { get; set; }
        public string AccountUrl { get; set; }


        public AccountEntry(string username, string password, string url)
        {
            AccountUserName = username;
            AccountPassword = password;
            AccountUrl = url;
        }

        public override string ToString()
        {
            return "UserName- " + AccountUserName + ", Password- " + AccountPassword + ", URL- " + AccountUrl;
        }
    }
}
