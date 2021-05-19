using MongoDB.Driver;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.Text.Json;
using System.Security.Cryptography;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Properties

        private string UserName { get; set; }

        private string Password { get; set; }

        private string[] SaltHash { get; set; }

        #endregion Properties


        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("passwordmanager");
        static IMongoCollection<UserandPassword> collectionAccount = db.GetCollection<UserandPassword>("users");

        static IMongoCollection<User> collectionUser = db.GetCollection<User>("users");

        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIteration = 10000;
        private byte[] Salt = new byte[SaltByteSize];


        public MainWindow()
        {
            InitializeComponent();
        }



        private async void loginInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Password; 
            

            bool userBreak = true;

            var accounts = await collectionAccount.Find(_ => true).ToListAsync();

            int userInt = 0;

            while (userBreak)
            {
                if (UserName.Equals(accounts[userInt].User) && Password.Equals(accounts[userInt].Password))
                {
                    PasswordViewer passwordviewer = new PasswordViewer();
                    passwordviewer.Activate();
                    passwordviewer.Show();
                    Close();
                    break;
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

            usernameInput.Clear();
            passwordInput.Clear();
        }

        private void signUpInfoBut_Click(object sender, RoutedEventArgs e)
        {
            SaltHash = new string[] { GenerateSalt(), HashPassword() };
            UserandPassword account = new UserandPassword(usernameInput.Text, passwordInput.Password, SaltHash);
            collectionAccount.InsertOne(account);

            usernameInput.Clear();
            passwordInput.Clear();
        }

        //This saves the user info using the User class, serializes it to Json and it saves it to a specified file
        //This will change to save it to MongoDB
        private void SaveUserInfo()
        {

            List<AccountEntry> newAccounts = new List<AccountEntry>();

            var users = JsonConvert.SerializeObject(new User
            {
                UserName = this.UserName,
                Password = Password,
                Accounts = newAccounts
            }, Formatting.Indented);

            //You'd have to change the file location for now to a place you can find
            string path = @"C:\Neumont College\Year2\QuarterSeven\IntroductorySoftwareProjects\ProjectThingy\PRO100\PasswordManager\Models\json1.json";

            string rFile;

            try
            {
                rFile = File.ReadAllText(path);

                rFile = rFile.Substring(0, rFile.Length - 3);

                rFile += ",";

            }
            catch (Exception)
            {
                using (File.CreateText(path))
                    Console.WriteLine("OOP");
                rFile = "[";
            }

            using (StreamWriter file = File.CreateText(path))
            {
                file.WriteLine(rFile + users + "]");
            }
        }


        //Generates 24 bit random characters to append to the password before hashing
        private string GenerateSalt()
        {
            RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();
            saltGenerator.GetBytes(Salt);
            return Convert.ToBase64String(Salt);
        }

        //Takes the generated salt and password and makes a 24 bit hash
        private string HashPassword()
        {
            Salt = Convert.FromBase64String(GenerateSalt());
            string password = passwordInput.Password;
            Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, Salt);
            hashGenerator.IterationCount = HashingIteration;
            return Convert.ToBase64String(hashGenerator.GetBytes(HashByteSize));
        }

        private void ValidatePassword()
        {
            //retrieve user's salt and hash from database
            //add salt to current password input and hash using same hash
            //compare the new hash with the one in the database
        }
    }
}
