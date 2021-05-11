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
using System.Windows.Shapes;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for PasswordViewer.xaml
    /// </summary>
    public partial class PasswordViewer : Window
    {

        #region Properties

        List<Entry> entries = new List<Entry>();

        

        #endregion Properties

        public PasswordViewer()
        {
            InitializeComponent();
            
        }

        private void addEntryBut_Click(object sender, RoutedEventArgs e)
        {
            entries.Add(new Entry(usernameInput.Text, passwordInput.Text, urlInput.Text));

            LstEntries.Items.Add(new Entry(usernameInput.Text, passwordInput.Text, urlInput.Text));
        }


    }

    
}
