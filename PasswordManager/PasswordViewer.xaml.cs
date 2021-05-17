using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for PasswordViewer.xaml
    /// </summary>
    public partial class PasswordViewer : Window
    {

        #region Properties

        public List<AccountEntry> entries = new List<AccountEntry>();

        public User CurrUser { get; set; }

        public List<User> UsersList = new List<User>();

        public string Path { get; set; }

        #endregion Properties

        public PasswordViewer()
        {
            InitializeComponent();

            

        }

        private void addEntryBut_Click(object sender, RoutedEventArgs e)
        {
            entries.Add(new AccountEntry(usernameInput.Text, passwordInput.Text, urlInput.Text));

            LstEntries.Items.Add(new AccountEntry(usernameInput.Text, passwordInput.Text, urlInput.Text));

            CurrUser.Accounts.Add(new AccountEntry(usernameInput.Text, passwordInput.Text, urlInput.Text));

            foreach (var c1 in UsersList)
            {
                if (c1.UserName.Equals(CurrUser.UserName))
                {
                    

                    using (StreamWriter file = File.CreateText(Path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, UsersList );
                    }
                }
            }

            usernameInput.Clear();
            passwordInput.Clear();
            urlInput.Clear();

        }

        public void AddEntry(AccountEntry entry)
        {
            entries.Add(entry);
            LstEntries.Items.Add(entry);

        }

    }

    
}
