using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MongoDB.Driver;

namespace PasswordManager
{
    public partial class PasswordViewer : Window
    {
        #region Properties

        public User CurrUser { get; set; }

        public List<User> UsersList = new List<User>();

        public string Path { get; set; }

        private string UserName { get; set; }

        private string Password { get; set; }

        private string Url { get; set; }

        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("passwordmanager");
        static IMongoCollection<UserandPassword> collectionAccount = db.GetCollection<UserandPassword>("users");
        public UserandPassword loggedInUser { get; set; }
        #endregion Properties
        public PasswordViewer()
        {
            InitializeComponent();
        }

        private void addEntryBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Text;
            Url = urlInput.Text;

            if (UserName.Trim() != "" && Password.Trim() != "")
            {
                loggedInUser.Accounts.Add(new AccountEntry(UserName, Password, Url));
                var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, loggedInUser.Accounts);
                collectionAccount.FindOneAndUpdate(
                    item => item.Id == loggedInUser.Id,
                    update);

                LstEntries.Items.Add(new AccountEntry(UserName, Password, Url));

                UserName = null;
                Password = null;
                Url = null;

                usernameInput.Text = "";
                passwordInput.Text = "";
                urlInput.Text = "";
            }
        }
        public void AddEntry(AccountEntry entry)
        {
            LstEntries.Items.Add(entry);
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 16; i++)
            {
                int randomNum = random.Next(1, 4);
                switch (randomNum)
                {
                    case 1:
                        ch = (char)random.Next('A', 'Z');
                        if (i % 2 == 1)
                        {
                            builder.Append(char.ToLower(ch));
                        }
                        else
                            builder.Append(ch);
                        break;
                    case 2:
                        int RandNum = random.Next(0, 9);
                        builder.Append(RandNum.ToString());
                        break;
                    case 3:
                        char[] SymbolArray = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '{', '}', '[', ']' };
                        ch = SymbolArray[random.Next(0, 15)];
                        builder.Append(ch);
                        break;
                    default:
                        break;
                }
            }
            return builder.ToString();
        }

        private void generatePassBut_Click(object sender, RoutedEventArgs e)
        {
            passwordInput.Text = GeneratePassword();
        }

        private void deleteEntryBut_Click(object sender, RoutedEventArgs e)
        {
            AccountEntry entry = (AccountEntry)LstEntries.SelectedItem;

            loggedInUser.Accounts.Remove(entry);

            var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, loggedInUser.Accounts);
            collectionAccount.FindOneAndUpdate(
                item => item.Id == loggedInUser.Id,
                update);
            LstEntries.Items.Remove(entry);
        }

        private void backbut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Activate();
            mainWindow.Show();
            Close();
        }
    }
}
