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
    /// Interaction logic for ValidateOTP.xaml
    /// </summary>
    public partial class ValidateOTP : UserControl
    {
        string userEmail = " ";
        public ValidateOTP(string email)
        {
            InitializeComponent();
            userEmail = email;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string enteredOTP = OTP_txt.Password;
            if (BL.validateOTP(userEmail, enteredOTP))
            {
                MessageBox.Show("OTP verified", "Success", MessageBoxButton.OK);
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    UpdatePassword password = new UpdatePassword(userEmail);
                    mainWindow.MainContent.Content = password;
                }
            }
            else
            {
                MessageBox.Show("OTP not verified", "Erroe", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
