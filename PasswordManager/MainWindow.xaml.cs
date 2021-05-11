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


        public MainWindow()
        {
            InitializeComponent(); 
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
        }

        private void signUpInfoBut_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;

            Password = passwordInput.Text;

            saveUserInfo();
        }

        //This saves the user info using the User class, serializes it to Json and it saves it to a specified file
        //This will change to save it to MongoDB
        private void saveUserInfo()
        {      

            User newUser = new User();

            Entry newEntry = new Entry("Josh", "JoshIsCool123", "PandaExpress.com");

            List<Entry> newAccounts = new List<Entry>();

            newAccounts.Add(newEntry);

            newUser.Username = UserName;

            newUser.Password = Password;

            newUser.Accounts = newAccounts;

            //You'd have to change the file location for now to a place you can find
            using (StreamWriter file = File.CreateText(@"C:\Neumont College\Year2\QuarterSeven\IntroductorySoftwareProjects\" + UserName + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, newUser);
            }

        }


    }

    
}
