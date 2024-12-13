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
    /// Interaction logic for ReservationDetailsPage.xaml
    /// </summary>
    public partial class ReservationDetailsPage : UserControl
    {
        ParkingSpotReservationViewModel _updatedData;
        Reservations reservation;
        bool isSpotReservedAtAnyTime = false;
        DateTime startDateTime;
        DateTime endDateTime;
        double totalCost;
        TimeSpan duration;

        public ReservationDetailsPage(ParkingSpotReservationViewModel updatedData)
        {
            InitializeComponent();
            this.DataContext = updatedData;
            _updatedData = updatedData;
        }

        private void CalculateFareBtn(object sender, RoutedEventArgs e)
        {
            double ratePerHour = 15.0;

            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? startTime = StartTimePicker.SelectedDateTime;
            DateTime? endDate = EndDatePicker.SelectedDate;
            DateTime? endTime = EndTimePicker.SelectedDateTime;

            if (!startDate.HasValue || !startTime.HasValue)
            {
                MessageBox.Show("Please select both start date and time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!endDate.HasValue || !endTime.HasValue)
            {
                MessageBox.Show("Please select both end date and time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

             startDateTime = startDate.Value.Date + startTime.Value.TimeOfDay;
             endDateTime = endDate.Value.Date + endTime.Value.TimeOfDay;

            if (startDateTime < DateTime.Now)
            {
                MessageBox.Show("Start date and time cannot be in the past.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endDateTime <= startDateTime)
            {
                MessageBox.Show("End date and time must be after the start date and time.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            reservation = new Reservations
            {
                SpotId = _updatedData.SpotId,
            };

            List<Reservations> allReservationsOfSpot = BL.GetOverlappingReservations(reservation.SpotId, startDateTime, endDateTime);


            foreach (var item in allReservationsOfSpot)
            {
                if ((startDateTime < item.EndTime && endDateTime > item.StartTime) && item.ReservationStatus == "Active")
                {
                    if (item.UserId == App.userId)
                    {
                        isSpotReservedAtAnyTime = false;
                        break;
                    }

                    isSpotReservedAtAnyTime = true;
                    MessageBox.Show(
                                 $"This Spot is already reserved during the selected time \nStart Time: {item.StartTime.ToString("MMM dd, yyyy hh:mm tt")}\nEnd Time:  {item.EndTime.ToString("MMM dd, yyyy hh:mm tt")}\n Please choose adifferent time.",
                                 "Already Reserved",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Warning
                             );
                    break;
                }
            }
            if (!isSpotReservedAtAnyTime)
            {
                 duration = endDateTime - startDateTime;
                DurationTextBox.Text = duration.TotalHours.ToString("0.00") + " hours";

                totalCost = duration.TotalHours * ratePerHour;
                TotalCostTextBox.Text = $"{totalCost:F2} PKR";

                reservation.StartTime = startDateTime;
                reservation.EndTime = endDateTime;
                reservation.ReservationStatus = "Reserved";
                reservation.TotalCost = totalCost;
                reservation.UserId = App.userId;
            }
        }

        private void BackBtn(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                MyReservations myReservations = new MyReservations();
                mainWindow.MainContent.Content = myReservations;
            }
        }

        private void extendReservationBtn(object sender, RoutedEventArgs e)
        {
            if (isSpotReservedAtAnyTime==false)
            {
                if(BL.ExtendReservation(_updatedData.ReservationId, App.userId, startDateTime, endDateTime))
                {
                    int updatedCost = 0;
                    int TotalFare = 0;
                    int previousCost = BL.GetPaymentAmount(_updatedData.ReservationId, App.userId);
                    if (previousCost > 0)
                    {
                        updatedCost = Convert.ToInt32(totalCost) - previousCost;
                        TotalFare = updatedCost + previousCost;
                    }
                    if ((BL.UpdatePayment(_updatedData.ReservationId, App.userId, TotalFare))&&(BL.UpdateReservationPayment(_updatedData.ReservationId,App.userId, TotalFare)))
                    {
                        MessageBox.Show($"Reservation Time Extended Successfully for Spot {_updatedData.Location}\n New Start Time : {startDateTime}\n New End Time: {endDateTime}",
                                    "Reservation Extended", MessageBoxButton.OK, MessageBoxImage.Information);

                        var PaymentData = new ParkingSpotReservationViewModel
                        {
                            Location = _updatedData.Location,
                            Duration = duration,
                            TotalCost = updatedCost,
                            StartTime = startDateTime,
                            EndTime = endDateTime,
                            ReservationId = _updatedData.ReservationId,
                        };

                        PaymentPage payment = new PaymentPage(PaymentData);
                        var mainwindow = Window.GetWindow(this) as MainWindow;
                        if (mainwindow != null)
                        {
                            mainwindow.MainContent.Content = payment;
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Reservation not extended", "Reservation not Extended", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


        }
    }
}
