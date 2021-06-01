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

        //Information that a user puts in an entry
        private string UserName { get; set; }

        private string Password { get; set; }

        private string Url { get; set; }

        //Sets a connection to the database
        static readonly MongoClient client = new();
        static readonly IMongoDatabase db = client.GetDatabase("passwordmanager");
        static readonly IMongoCollection<UserandPassword> collectionAccount = db.GetCollection<UserandPassword>("users");
        public UserandPassword LoggedInUser { get; set; }

        #endregion Properties

        public PasswordViewer()
        {
            InitializeComponent();
        }

        private void AddEntryBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Text;
            Url = urlInput.Text;

            if (UserName.Trim() != "" && Password.Trim() != "")
            {
                LoggedInUser.Accounts.Add(new AccountEntry(UserName, Password, Url));
                var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
                collectionAccount.FindOneAndUpdate(
                    item => item.Id == LoggedInUser.Id,
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

        private static string GeneratePassword()
        {
            Random random = new();
            StringBuilder builder = new();
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
                        builder.Append(RandNum);
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

        private void GeneratePassBut_Click(object sender, RoutedEventArgs e)
        {
            passwordInput.Text = GeneratePassword();
        }

        private void DeleteEntryBut_Click(object sender, RoutedEventArgs e)
        {
            AccountEntry entry = (AccountEntry)LstEntries.SelectedItem;

            LoggedInUser.Accounts.Remove(entry);

            var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
            collectionAccount.FindOneAndUpdate(
                item => item.Id == LoggedInUser.Id,
                update);
            LstEntries.Items.Remove(entry);
        }

        private void Backbut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Activate();
            mainWindow.Show();
            Close();
        }

        private void LstEntries_CopyingRowClipboardContent(object sender, System.Windows.Controls.DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[LstEntries.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }

        private void UpdateEntryBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Text;
            Url = urlInput.Text;

            AccountEntry entry = (AccountEntry)LstEntries.SelectedItem;

            if (entry != null)
            {

                LstEntries.Items.Remove(entry);
                LoggedInUser.Accounts.Remove(entry);

                if (UserName.Trim() != "")
                {
                    entry.AccountUserName = UserName;
                }
                if (Password.Trim() != "")
                {
                    entry.AccountPassword = Password;
                }
                if (Url.Trim() != "")
                {
                    entry.AccountUrl = Url;
                }
            

                LstEntries.Items.Add(entry);
                LoggedInUser.Accounts.Add(entry);

                var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
                collectionAccount.FindOneAndUpdate(
                    item => item.Id == LoggedInUser.Id,
                    update);


                UserName = null;
                Password = null;
                Url = null;

                usernameInput.Text = "";
                passwordInput.Text = "";
                urlInput.Text = "";
            }



        }
    }
}
