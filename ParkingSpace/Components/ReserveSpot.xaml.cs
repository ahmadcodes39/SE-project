using ParkingSpace.BusinessLayer;
using ParkingSpace.fonts;
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
    /// Interaction logic for ReserveSpot.xaml
    /// </summary>
    public partial class ReserveSpot : UserControl
    {
        readonly string SpotLocation = " ";
        readonly int ID;
        Reservations reservation;
        
        TimeSpan duration;



        public ReserveSpot(string spotLocation,int SpotId)
        {
            InitializeComponent();
            SpotLocation = spotLocation;
            ID = SpotId;
            LoadSpotInfoData();
            
           
        }
        private void LoadSpotInfoData()
        {
            SpotInfoTextBox.Text = SpotLocation;
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

            DateTime startDateTime = startDate.Value.Date + startTime.Value.TimeOfDay;
            DateTime endDateTime = endDate.Value.Date + endTime.Value.TimeOfDay;

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

            // Initialize the reservation object with SpotId
            reservation = new Reservations
            {
                SpotId = ID
            };

            List<Reservations> allReservationsOfSpot = BL.GetOverlappingReservations(reservation.SpotId,startDateTime,endDateTime);

            bool isSpotReservedAtAnyTime = false;

            foreach (var item in allReservationsOfSpot)
            {
                // Check if the new reservation's time overlaps with any active reservation
                if ((startDateTime < item.EndTime && endDateTime > item.StartTime) && item.ReservationStatus == "Active")
                {
                //MessageBox.Show($"{item.StartTime}-{item.EndTime} - {item.ReservationStatus}");
                //MessageBox.Show($"{startTime}-{endTime}");
                    isSpotReservedAtAnyTime = true;
                    //var choice = MessageBox.Show("This Spot is already reserved during the selected time. Please change the time.\nDo you want to see the schedule of this Spot?", "Already Reserved", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

                double totalCost = duration.TotalHours * ratePerHour;
                TotalCostTextBox.Text = $"{totalCost:F2} PKR";

                reservation.StartTime = startDateTime;
                reservation.EndTime = endDateTime;
                reservation.ReservationStatus = "Reserved";
                reservation.TotalCost = totalCost;
                reservation.UserId = App.userId;
            }
        }


        private void ResetBtn(object sender, RoutedEventArgs e)
        {
            var output = MessageBox.Show("Do you want to clear all fields ?", "Question",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (output == MessageBoxResult.Yes)
            {
                StartDatePicker.SelectedDate = null;
                StartTimePicker.SelectedDateTime = null;
                EndDatePicker.SelectedDate = null;
                EndTimePicker.SelectedDateTime = null;
                TotalCostTextBox.Text = string.Empty;
                DurationTextBox.Text = string.Empty;
            }
        }

        private void ConfirmReservationBtn(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"spot id: {ID}");
            //MessageBox.Show($"{reservation.StartTime}");
            //MessageBox.Show($"{reservation.EndTime}");
            //MessageBox.Show($"{reservation.ReservationStatus}");
            //MessageBox.Show($"{reservation.UserId}");
            //MessageBox.Show($"{reservation.SpotId}");
            (int ReservationId, int SpotId) = BL.InsertReservationData(reservation);
            if (ReservationId!=0 && SpotId!=0)
            {
                MessageBox.Show("Your Reservation is in process, please make payment to confirm it", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    var ViewModel = new ParkingSpotReservationViewModel
                    {
                        TotalCost = reservation.TotalCost,
                        StartTime = reservation.StartTime,
                        EndTime = reservation.EndTime,
                        Duration = duration,
                        Location = SpotInfoTextBox.Text,
                        SpotId = SpotId,
                        ReservationId = ReservationId,
                    };

                    PaymentPage payment = new PaymentPage(ViewModel);
                    mainWindow.MainContent.Content = payment;
                }
            }
            else
            {
                MessageBox.Show("Your Reservation is not confirmed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //LoadData();
        }

     
    }
}
