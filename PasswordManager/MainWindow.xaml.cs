using System;
using System.Collections.Generic;
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserName = usernameInput.Text;
            Password = passwordInput.Text;

            if (!(string.IsNullOrEmpty(UserName) && string.IsNullOrWhiteSpace(UserName)) && !(string.IsNullOrEmpty(Password) && string.IsNullOrWhiteSpace(Password)))
            {
                /*Check database for a matching username
                //PasswordDB.Users.find( { username: usernameInput.Text } );
                    If found, check if the password matches
                        //PasswordDB.Users.find( { username: usernameInput.Text, password: passwordInput.Text } );
                        If it does, successful log in
                        else, incorrect password
                            MessageBox.Show("Incorrect Password.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                else username not found; prompt for new user creation
                */
                MessageBox.Show("The user does not exist. Please create user.", "Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
