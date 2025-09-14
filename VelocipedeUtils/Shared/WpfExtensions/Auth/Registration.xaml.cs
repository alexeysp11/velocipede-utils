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

namespace VelocipedeUtils.Shared.WpfExtensions.Auth
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFirstName.Text) ||
                string.IsNullOrWhiteSpace(tbLastName.Text) ||
                string.IsNullOrWhiteSpace(tbUsername.Text) ||
                string.IsNullOrWhiteSpace(tbEmail.Text) ||
                string.IsNullOrWhiteSpace(tbPassword.Password) ||
                string.IsNullOrWhiteSpace(tbPasswordConfirm.Password))
            {
                MessageBox.Show("Fill in all fields, please!");
            }
            else
            {
                if (tbPassword.Password == tbPasswordConfirm.Password)
                {
                    txtMessage.Text = "Successfully submitted!";

                    var timer = new System.Windows.Threading.DispatcherTimer 
                    { 
                        Interval = TimeSpan.FromSeconds(1) 
                    };
                    timer.Start();
                    timer.Tick += (sender, args) =>
                    {
                        Login a = new Login();
                        Close();
                        a.ShowDialog();
                    };
                }
                else
                {
                    txtMessage.Text = "Passwords do not match!";
                }
            }
        }
    }
}
