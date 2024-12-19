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
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        public SideBar()
        {
            InitializeComponent();
        }
        private void AvailabilityLable(object sender, MouseButtonEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login before checking available spots .", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    CheckAvailability availability = new CheckAvailability();
                    mainWindow.MainContent.Content = availability;
                }
            }
        }

        private void ReserveSpotLable(object sender, MouseButtonEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login to first to Reserve a spot.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    ReserveSpot spot = new ReserveSpot("",-1);
                    mainWindow.MainContent.Content = spot;
                }
            }
        }

        private void MyreservationLable(object sender, MouseButtonEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login to access My Reservation information.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    MyReservations myReservations = new MyReservations();
                    mainWindow.MainContent.Content = myReservations;
                }
            }
        }

        private void HelpInfoLable(object sender, MouseButtonEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login first.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    HelpInfo help = new HelpInfo();
                    mainWindow.MainContent.Content = help;
                }
            }
            
        }

        private void HomeLable(object sender, RoutedEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login first.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                   
                    CheckAvailability availability = new CheckAvailability();
                    mainWindow.MainContent.Content = availability;
                }
            }
        } 
        private void LogoutBtn(object sender, RoutedEventArgs e)
        {
            if (!App.IsLoggedIn)
            {
                MessageBox.Show("You must Login first.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    App.IsLoggedIn = false;
                    SignIn signIn = new SignIn();
                    mainWindow.MainContent.Content = signIn;
                }
            }
        }
    }
}
