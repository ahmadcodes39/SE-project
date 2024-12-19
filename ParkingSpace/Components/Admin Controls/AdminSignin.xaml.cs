using ParkingSpace.BusinessLayer;
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

namespace ParkingSpace.Components.Admin_Controls
{
    /// <summary>
    /// Interaction logic for AdminSignin.xaml
    /// </summary>
    public partial class AdminSignin : UserControl
    {
        public AdminSignin()
        {
            InitializeComponent();
        }

        private void LoginBtn(object sender, RoutedEventArgs e)
        {
            string email = email_txt.Text;
            string password = pass_txt.Password;

            if (!BL.IsAdminExist(email, password))
            {
                MessageBox.Show("Invalid Cradentials", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Login Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.AdminLogin = true;
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    AdminDashboard dashboard = new AdminDashboard();
                    mainWindow.MainContent.Content = dashboard;
                }

            }
        }
    }
}
