using ParkingSpace.BusinessLayer;
using ParkingSpace.Components;
using System.Windows;
using ParkingSpace.Components.Admin_Controls;
namespace ParkingSpace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowRoleSelectionPopup();
        }

        private void ShowRoleSelectionPopup()
        {
            RoleSelectionPopup.Visibility = Visibility.Visible;

            TopBarContent.Visibility = Visibility.Collapsed;
            MainContent.Content = null;
        }

        private void AdminSelected(object sender, RoutedEventArgs e)
        {
            RoleSelectionPopup.Visibility = Visibility.Collapsed;

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow!=null)
            {
                AdminSignin signin = new AdminSignin();
                AdminTopBar adminTopBar = new AdminTopBar();
                mainWindow.MainContent.Content = signin; 
                mainWindow.TopBarContent.Content = adminTopBar;
                mainWindow.TopBarContent.Visibility = Visibility.Visible;
            }
          
        }

        private void UserSelected(object sender, RoutedEventArgs e)
        {
            RoleSelectionPopup.Visibility = Visibility.Collapsed;

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                SignIn signin = new SignIn();
                SideBar sidebar = new SideBar();
                mainWindow.MainContent.Content = signin;
                mainWindow.TopBarContent.Content = sidebar;
                mainWindow.TopBarContent.Visibility = Visibility.Visible;
            }
        }
    }
}
