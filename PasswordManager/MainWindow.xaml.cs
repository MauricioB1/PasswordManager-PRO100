using MongoDB.Driver;
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
        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("passwordmanager");
        static IMongoCollection<Users> collectionUser = db.GetCollection<Users>("users");

        public void GetUsers()
        {
            List<Users> list = collectionUser.AsQueryable().ToList<Users>();
            dgUsers.ItemsSource = list;
            

        }
        public MainWindow()
        {
            InitializeComponent();
            GetUsers();
        }

    }
}
