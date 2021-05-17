﻿using MongoDB.Driver;
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

        #endregion Properties


        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("passwordmanager");
        static IMongoCollection<UserandPassword> collectionAccount = db.GetCollection<UserandPassword>("users");

        static IMongoCollection<User> collectionUser = db.GetCollection<User>("users");

        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIteration = 10000;


        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void loginInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Text;

            if (!(string.IsNullOrEmpty(UserName) && string.IsNullOrWhiteSpace(UserName)) && !(string.IsNullOrEmpty(Password) && string.IsNullOrWhiteSpace(Password)))
            {
                /*Check database (json file) for a matching username
                //PasswordDB.Users.find( { username: usernameInput.Text } );
                    If found, check if the password matches
                        //PasswordDB.Users.find( { username: usernameInput.Text, password: passwordInput.Text } );
                        If it does, successful log in
                        else, incorrect password
                            //MessageBox.Show("Incorrect Password.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                else username not found; prompt for new user creation
                */
                //var result = collectionUser.Find(u => u.UserName == "Josh");
                //UserName = result.ToString();
                string path = @"C:\Neumont College\Year2\QuarterSeven\IntroductorySoftwareProjects\ProjectThingy\PRO100\PasswordManager\Models\json1.json";
                string jsonString;
                using (var reader = new StreamReader(path))
                {
                    jsonString = reader.ReadToEnd();
                }

                if (jsonString.Contains($"\"UserName\": \"{UserName}\"") && jsonString.Contains($"\"Password\": \"{Password}\""))
                {
                    PasswordViewer passwordviewer = new PasswordViewer();
                    var entryJson = JsonConvert.DeserializeObject<List<User>>(jsonString);
                    User currUser = null;
                    foreach (var c1 in entryJson)
                    {
                        if (c1.UserName.Equals(UserName))
                        {
                            currUser = c1;
                            foreach (var c2 in c1.Accounts)
                            {
                                passwordviewer.AddEntry(c2);
                            }

                        }
                    }
                    passwordviewer.UsersList = entryJson;
                    passwordviewer.CurrUser = currUser;
                    passwordviewer.Path = path;
                    passwordviewer.Activate();
                    passwordviewer.Show();
                    Close();
                }
                else
                    MessageBox.Show("The user does not exist. Please create user.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            usernameInput.Clear();
            passwordInput.Clear();
        }

        private void signUpInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserandPassword account = new UserandPassword(usernameInput.Text, passwordInput.Text);
            collectionAccount.InsertOne(account);


            /*UserName = usernameInput.Text;

            Password = passwordInput.Text;*/

            //saveUserInfo();
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

        private string GenerateSalt()
        {
            RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            saltGenerator.GetBytes(salt);
            usernameInput.Text = Convert.ToBase64String(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
