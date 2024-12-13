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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : UserControl
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void BackBtnLabel(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                SignIn sign = new SignIn();
                mainWindow.MainContent.Content = sign;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //}

        private void resetPass_btn(object sender, RoutedEventArgs e)
        {
            string email = email_txt.Text;
            if (BL.isEmailExist(email))
            {

                ValidateOTP validateOTP = new ValidateOTP(email);
                UpdatePassword updatePassword = new UpdatePassword(email);
                if (BL.generateAndSendOTP(email))
                {
                    MessageBox.Show("Check your Email for reset password OTP", "Succes", MessageBoxButton.OK);
                    var mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        ValidateOTP otp = new ValidateOTP(email);
                        mainWindow.MainContent.Content = otp;
                    }
                }
                else
                {
                    MessageBox.Show("An error occured OTP not send", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
            else
            {
                MessageBox.Show("Such Email not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
