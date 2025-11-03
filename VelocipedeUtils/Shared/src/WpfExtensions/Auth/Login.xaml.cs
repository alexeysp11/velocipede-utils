using System;
using System.Windows;

namespace VelocipedeUtils.Shared.WpfExtensions.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUsername.Text) || string.IsNullOrWhiteSpace(tbPassword.Password))
                txtMessage.Text = "Fill in all fields, please!";
            else
            {
                txtMessage.Text = "Successfully submitted!";
                
                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    // Open main window.
                };
            }
        }
    }
}
