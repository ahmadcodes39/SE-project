using ParkingSpace.BusinessLayer;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using PMS_WCF_Service.ServiceReference1;
using PMS_WCF_Service;
namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                SignIn sign = new SignIn();
                mainWindow.MainContent.Content = sign;
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }
        public void RegisterBtn(object sender, RoutedEventArgs e)
        {
            string name = name_txt.Text;
            string email = email_txt.Text;
            string password = pass_txt.Password;
            string phone = phone_txt.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("All fields are required. Please fill in all the details.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (name.Length <= 3 || name.Any(char.IsDigit))
            {
                MessageBox.Show("Name must be more than 3 characters long and cannot contain numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters long.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (phone.Length != 11 || !phone.All(char.IsDigit))
            {
                MessageBox.Show("Phone number must be 11 digits and contain only numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Users users = new Users
            {
                Name = name,
                Email = email,
                Password = password,
                Phone = phone
            };

            PMS_WCF_Service.Service1 client = new PMS_WCF_Service.Service1();
            if (client.CheckIfUserExists(email))
            {
                MessageBox.Show("User already exists. Please log in.", "User Exists", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (client.RegisterUser(users))
            {
                MessageBox.Show("User registered successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                name_txt.Text = string.Empty;
                email_txt.Text = string.Empty;
                pass_txt.Password = string.Empty;
                phone_txt.Text = string.Empty;

                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    SignIn signIn = new SignIn();
                    mainWindow.MainContent.Content = signIn;
                }
            }
            else
            {
                MessageBox.Show("Error while registering user. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
