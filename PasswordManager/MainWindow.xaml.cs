using MongoDB.Driver;
using System;
using System.Windows;
using System.Security.Cryptography;
namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        #region Properties

        //These are the properties that get stored into the database
        private string UserName { get; set; }
        private string Password { get; set; }
        private string[] SaltHash { get; set; }

        //This establishes a connection to the database
        static readonly MongoClient client = new();
        static readonly IMongoDatabase db = client.GetDatabase("passwordmanager");
        static readonly IMongoCollection<UserandPassword> collectionAccount = db.GetCollection<UserandPassword>("users");

        //Parameters to create the salt and the hash
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIteration = 10000;
        private readonly byte[] Salt = new byte[SaltByteSize];

        #endregion Properties


        public MainWindow()
        {
            InitializeComponent();
        }

        //Takes the information provided by the user and logs in to a prexisting account if one is found with the same username and password
        private void LoginInfoBut_Click(object sender, RoutedEventArgs e)
        {
            //Grabs the information that user passed in
            UserName = usernameInput.Text;

            if (passwordInput.Password.Equals("")) { Password = showpassInput.Text; }

            else { Password = passwordInput.Password; }

            if (!(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password)))
                //After the check to see if there are no empty entries, it then goes to validate the password with the found user
                ValidatePassword();
            else
                MessageBox.Show("Username and Password cannot be empty","Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);

            usernameInput.Clear();
            passwordInput.Clear();
        }

        //Creates a new user, using the information provided
        private async void SignUpInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Password;
            int userInt = 0;
            bool userBreak = true;
            if (!(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password)))
            {
                string salt = GenerateSalt();
                byte[] PasswordSalt = Convert.FromBase64String(salt);
                string password = Password;

                SaltHash = new string[] { salt, HashPassword(PasswordSalt, password) };
                UserandPassword account = new(UserName, SaltHash);
                var AccountsList = await collectionAccount.Find(_ => true).ToListAsync(); ;
                while (userBreak)
                {
                    if (UserName.Equals(AccountsList[userInt].User))
                    {
                        MessageBox.Show("This username already exists", "Invalid Username", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        userBreak = false;
                    }
                    else if (userInt < AccountsList.Count - 1)
                    {
                        userInt++;
                    }
                    else
                    {
                        collectionAccount.InsertOne(account);
                        LoginInfoBut_Click(sender, e);
                        userBreak = false;
                    }
                }
            }
            else
                MessageBox.Show("Username and Password cannot be empty", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Generates 24 bit random characters to append to the password before hashing
        private string GenerateSalt()
        {
            RNGCryptoServiceProvider saltGenerator = new();
            saltGenerator.GetBytes(Salt);
            return Convert.ToBase64String(Salt);
        }

        //Takes the generated salt and password and makes a 24 bit hash
        private static string HashPassword(byte[] Salt, string password)
        {
            Rfc2898DeriveBytes hashGenerator = new(password, Salt) { IterationCount = HashingIteration };

            return Convert.ToBase64String(hashGenerator.GetBytes(HashByteSize));
        }

        private async void ValidatePassword()
        {
            UserName = usernameInput.Text;

            if (passwordInput.Password.Equals("")) { Password = showpassInput.Text; }

            else { Password = passwordInput.Password; }

            int userInt = 0;
            bool userBreak = true;
            var accounts = await collectionAccount.Find(_ => true).ToListAsync();
            while (userBreak)
            {
                if (UserName.Equals(accounts[userInt].User))
                {
                    byte[] salt = Convert.FromBase64String(accounts[userInt].SaltHash[0]);
                    string hash = accounts[userInt].SaltHash[1];
                    string HashedUserPass = HashPassword(salt, Password);
                    if (HashedUserPass.Equals(hash))
                    {
                        PasswordViewer passwordviewer = new() { LoggedInUser = accounts[userInt] };

                        foreach (var c in accounts[userInt].Accounts)
                        {
                            passwordviewer.LstEntries.Items.Add(c);
                        }

                        passwordviewer.Activate();
                        passwordviewer.Show();
                        Close();
                        userBreak = false;
                    }
                    else
                    {
                        MessageBox.Show("The password is incorrect. Please try again.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                        userBreak = false;
                    }
                }
                else if (userInt < accounts.Count - 1)
                {
                    userInt++;
                }
                else
                {
                    MessageBox.Show("The user does not exist. Please create user.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                    userBreak = false;
                }
            }
        }


        private void ShowPassBut_Checked(object sender, RoutedEventArgs e)
        {
            showpassInput.Text = passwordInput.Password;
            passwordInput.Visibility = Visibility.Collapsed;
            showpassInput.Visibility = Visibility.Visible;
        }

        private void ShowPassBut_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordInput.Password = showpassInput.Text;
            showpassInput.Visibility = Visibility.Collapsed;
            passwordInput.Visibility = Visibility.Visible;
        }

    }
}
