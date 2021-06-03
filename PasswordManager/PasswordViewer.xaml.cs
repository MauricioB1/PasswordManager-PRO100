using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var destination = ((Hyperlink)e.OriginalSource).NavigateUri;
            Trace.WriteLine("Browsing to " + destination);

            using (Process browser = new())
            {
                browser.StartInfo = new ProcessStartInfo
                {
                    FileName = destination.ToString(),
                    UseShellExecute = true,
                    ErrorDialog = true
                };
                browser.Start();
            } 
        }

        private void AddEntryBut_Click(object sender, RoutedEventArgs e)
        {
            //Grabs the text the user put in into the entry boxes
            UserName = usernameInput.Text;
            Password = passwordInput.Text;
            Url = urlInput.Text;
            if (UserName.Trim() != "" && Password.Trim() != "")
            {
                //It adds that entry to the logged in user and updates the database with the new information
                LoggedInUser.Accounts.Add(new AccountEntry(UserName, Password, Url));
                var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
                collectionAccount.FindOneAndUpdate(
                    item => item.Id == LoggedInUser.Id,
                    update);

                //displays it visually
                LstEntries.Items.Add(new AccountEntry(UserName, Password, Url));

                //resets the values of the class properties
                UserName = null;
                Password = null;
                Url = null;

                //Clears the entry fields
                usernameInput.Text = "";
                passwordInput.Text = "";
                urlInput.Text = "";
            }
        }

        //Creates a 16 character length string that has random characters
        private static string GeneratePassword()
        {
            Random random = new();
            StringBuilder builder = new();
            char ch;

            for (int i = 0; i < 16; i++)
            {
                //Randomly chooses to insert a letter, number, or special character
                int randomNum = random.Next(1, 4);
                switch (randomNum)
                {
                    case 1:
                        //Randomly chooses a letter and whether it's even or not will make it either lowercase or uppercase
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

        //Generates a random password when you click this button
        private void GeneratePassBut_Click(object sender, RoutedEventArgs e)
        {
            passwordInput.Text = GeneratePassword();
        }

        private void DeleteEntryBut_Click(object sender, RoutedEventArgs e)
        {
            //Stores the information of the currently selected cell
            AccountEntry entry = (AccountEntry)LstEntries.SelectedItem;

            //Removes it from the logged in user and updates the database
            LoggedInUser.Accounts.Remove(entry);
            var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
            collectionAccount.FindOneAndUpdate(
                item => item.Id == LoggedInUser.Id,
                update);

            //Removes it from the screen
            LstEntries.Items.Remove(entry);
        }

        //Goes back to the previous window.
        private void Backbut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Activate();
            mainWindow.Show();
            Close();
        }

        
        private void LstEntries_CopyingRowClipboardContent(object sender, System.Windows.Controls.DataGridRowClipboardEventArgs e)
        {
            //It stores the contents of the current cell.
            var currentCell = e.ClipboardRowContent[LstEntries.CurrentCell.Column.DisplayIndex];
            //Clears the clipboard so that there is no old data in there.
            e.ClipboardRowContent.Clear();
            //Adds the selected cell to the clipboard
            e.ClipboardRowContent.Add(currentCell);
        }

        private void UpdateEntryBut_Click(object sender, RoutedEventArgs e)
        {
            //Grabs the text that the user put in in the entries
            UserName = usernameInput.Text;
            Password = passwordInput.Text;
            Url = urlInput.Text;

            //Stores the currently selected entry
            AccountEntry entry = (AccountEntry)LstEntries.SelectedItem;

            if (entry != null)
            {
                //Removes that entry ffrom the screen and logged in user
                LstEntries.Items.Remove(entry);
                LoggedInUser.Accounts.Remove(entry);

                //Updates the values of entry to the user inputted values.
                //If the entry is empty, then it doesn't change it
                if (UserName.Trim() != "") { entry.AccountUserName = UserName; }

                if (Password.Trim() != "") { entry.AccountPassword = Password; }

                if (Url.Trim() != "") { entry.AccountUrl = Url; }
            
                //It adds the newly modified entry to the screen and logged in uesr
                LstEntries.Items.Add(entry);
                LoggedInUser.Accounts.Add(entry);

                //Updates the database with the new information
                var update = Builders<UserandPassword>.Update.Set(o => o.Accounts, LoggedInUser.Accounts);
                collectionAccount.FindOneAndUpdate(
                    item => item.Id == LoggedInUser.Id,
                    update);

                //Resets the class properties and clears the entry boxes
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
