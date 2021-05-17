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
        static IMongoCollection<User> collectionUser = db.GetCollection<User>("users");
        DataGrid dgUsers = new DataGrid();

        public void GetUsers()
        {
            List<User> list = collectionUser.AsQueryable().ToList<User>();
            dgUsers.ItemsSource = list;
        }

        public MainWindow()
        {
            InitializeComponent();
            //GetUsers();
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
                        char[] SymbolArray = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '{', '}','[',']'};
                        ch = SymbolArray[random.Next(0, 15)];
                        builder.Append(ch);
                        break;
                    default:
                        break;
                } 
            }
            return builder.ToString();
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

                //Change Path Here!
                string path = @"C:\Users\amina\OneDrive - Neumont College of Computer Science\Year 2\Spring 2021\Software Projects\Password Project\PasswordManager\Models\json1.json";
                string jsonString;
                using (var reader = new StreamReader(path))
                {
                    jsonString = reader.ReadToEnd();
                }

                if(jsonString.Contains($"\"UserName\": \"{UserName}\"") && jsonString.Contains($"\"Password\": {Password}"))
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
                }else
                    MessageBox.Show("The user does not exist. Please create user.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            usernameInput.Clear();
            passwordInput.Clear();
        }

        private void signUpInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;

            Password = passwordInput.Text;

            SaveUserInfo();
        }

        //This saves the user info using the User class, serializes it to Json and it saves it to a specified file
        //This will change to save it to MongoDB
        private void SaveUserInfo()
        {        

            List<AccountEntry> newAccounts = new List<AccountEntry>();      

            var users = JsonConvert.SerializeObject(new User {  UserName= this.UserName , Password = int.Parse(Password),
            Accounts = newAccounts}, Formatting.Indented);

            //You'd have to change the file location for now to a place you can find
            string path = @"C:\Users\amina\OneDrive - Neumont College of Computer Science\Year 2\Spring 2021\Software Projects\Password Project\PasswordManager\Models\json1.json";

            string rFile;

            try
            {
                rFile = File.ReadAllText(path);

                rFile = rFile.Substring(0, rFile.Length - 3);

                rFile += ",";

            }
            catch(Exception)
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
    }
}

