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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParkingSpace.Components.Admin_Controls;
namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for AdminTopBar.xaml
    /// </summary>
    public partial class AdminTopBar : UserControl
    {
        public AdminTopBar()
        {
            InitializeComponent();
        }

        private void Label_Dashboard(object sender, MouseButtonEventArgs e)
        {
            if (App.AdminLogin==false)
            {
                MessageBox.Show("You First have to Login to access Dashboard", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = adminDashboard;
                }
            }
        }

        private void Label_AddSpot(object sender, MouseButtonEventArgs e)
        {
            if (App.AdminLogin == false)
            {
                MessageBox.Show("You First have to Login to Add Sport", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                AddParkingSpot addParkingSpot = new AddParkingSpot();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = addParkingSpot;
                }
            }
        }

        private void Label_Payment(object sender, MouseButtonEventArgs e)
        {
            if (App.AdminLogin == false)
            {
                MessageBox.Show("You First have to Login to access Payment Page", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ViewPaymentData data = new ViewPaymentData();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = data;
                }
            }
        }

        private void LogoutBtn(object sender, RoutedEventArgs e)
        {
            if (App.AdminLogin == false)
            {
                MessageBox.Show("You First have to Login ", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SignIn sign = new SignIn();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = sign;
                }
                App.AdminLogin = false;

            }

        }
    }
}
