using ParkingSpace.BusinessLayer;
using Shared.Models;
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

namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void NewAccount(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                SignUp sign = new SignUp();
                mainWindow.MainContent.Content = sign;
            }
        }

        private void forgotPasswordLable(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                ForgotPassword forgotPassword = new ForgotPassword();
                mainWindow.MainContent.Content = forgotPassword;
            }
        }

        public void LoginBtn(object sender, RoutedEventArgs e)
        {
            string email = email_txt.Text;
            string password = pass_txt.Password;
            PMS_WCF_Service.Service1 client = new PMS_WCF_Service.Service1();

            Users obj = client.ValidateLogin(email, password);
            if (obj!=null)
            {
                MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.IsLoggedIn = true;
                App.userId = obj.ID;
                App.userEmail = email;
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    CheckAvailability dashboard = new CheckAvailability();
                    mainWindow.MainContent.Content = dashboard;
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
