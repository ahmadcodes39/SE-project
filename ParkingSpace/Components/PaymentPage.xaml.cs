using ParkingSpace.BusinessLayer;
using ParkingSpace.Models;
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
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : UserControl
    {
        public ParkingSpotReservationViewModel ViewModel { get; }
        public PaymentPage(ParkingSpotReservationViewModel viewModel)
        {
            InitializeComponent(); 
            ViewModel = viewModel;
            this.DataContext = ViewModel;
        }

        private void PayNowBtn(object sender, RoutedEventArgs e)
        {
            int reservationId = this.ViewModel.ReservationId;
            string holderName = holderName_txt.Text.Trim();
            string cardNumber = cardNumber_txt.Text.Trim();
            string expieryDate = expiryDate_txt.Text.Trim();
            string cvv = cvv_passwordBox.Password.Trim();

            if (string.IsNullOrEmpty(holderName) || holderName.Length<3 )
            {
                MessageBox.Show("Invalid Name", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 3 || !long.TryParse(cardNumber, out _))
            {
                MessageBox.Show("Invalid Card Number", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(expieryDate) && !expieryDate.Contains("/"))
            {
                MessageBox.Show("Invalid Date input", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var dateParts = expieryDate.Split('/');
                if (dateParts.Length != 2 || !int.TryParse(dateParts[0], out int month) || !int.TryParse(dateParts[1], out int year)
                    || month < 1 || month > 12 || year < 0)
                {
                    MessageBox.Show("Invalid Expiry Date. Ensure MM/YY format and valid month/year.",
                                     "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var currentYear = DateTime.Now.Year % 100;
                var currentMonth = DateTime.Now.Month;
                if (year < currentYear || (currentYear == year && month < currentMonth))
                {
                    MessageBox.Show("Card is expired.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(cvv) || cvv.Length != 3 || !int.TryParse(cvv, out _))
                {
                    MessageBox.Show("Invalid CVV. It must be a 3-digit number.",
                                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Payments payments = new Payments
                {
                    ReservationId = reservationId,
                    UserId = App.userId,
                    PaymentAmount = ViewModel.TotalCost,
                    PaymentStatus = "Payed",
                };

                if (BL.InsertPaymentData(payments))
                {
                    bool isPayed = BL.UpdateReservationStatus(this.ViewModel.ReservationId, "Active");

                    if (isPayed)
                    {

                        if (BL.UpdateSpotStatus(this.ViewModel.SpotId, "Reserved"))
                        {
                            MessageBox.Show("successfuly update the spot status to Reserved.", "succcess", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                                              
                    }
                    else
                    {
                        //if (!BL.UpdateSpotStatus(this.ViewModel.SpotId, "Reserved"))
                        //{
                            MessageBox.Show("Failed to update the spot status to Reserved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //}
                           
                    }
                    SuccessPage success = new SuccessPage(this.ViewModel.Location);
                    var mainwindow = Window.GetWindow(this) as MainWindow;
                    if (mainwindow!=null)
                    {
                        mainwindow.MainContent.Content = success;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to update status of  the reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}

