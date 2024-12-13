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

namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePassword : UserControl
    {
        string userEmail = " ";
        public UpdatePassword(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

       
        private void updatePass_btn(object sender, RoutedEventArgs e)
        {
            string password = pass_txt.Password;
            if (BL.updatePassword(userEmail, password))
            {
                MessageBox.Show("Password Updated Successfully ", "Success", MessageBoxButton.OK);
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    SignIn signin = new SignIn();
                    mainWindow.MainContent.Content = signin;
                }
            }

        }
    }
}
