using ParkingSpace.BusinessLayer;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace ParkingSpace.Components
{
    public partial class MyReservations : UserControl
    {
        private DispatcherTimer timer;
        private List<ParkingSpotReservationViewModel> currentReservationList = new List<ParkingSpotReservationViewModel>();

        public MyReservations()
        {
            InitializeComponent();
            LoadReservations();
            InitializeTimer();
        }

        private void LoadReservations()
        {
            try
            {
                LoadCurrentReservations();
                LoadPastReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reservations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPastReservations()
        {
            try
            {
                var data = BL.FetchPastReservations(App.userId);
                var pastReservationList = new List<ParkingSpotReservationViewModel>();

                foreach (var reservation in data)
                {
                    pastReservationList.Add(new ParkingSpotReservationViewModel
                    {
                        StartTime = (DateTime)reservation["StartTime"],
                        EndTime = (DateTime)reservation["EndTime"],
                        TotalCost = (int)reservation["TotalCost"],
                        ReservationStatus = reservation["ReservationStatus"].ToString(),
                        Location = reservation["Location"].ToString(),
                        Duration = ((DateTime)reservation["EndTime"] - (DateTime)reservation["StartTime"])
                    });
                }

                pastGrid.ItemsSource = pastReservationList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading past reservations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

   
        private void LoadCurrentReservations()
        {
            try
            {
                currentReservationList.Clear();
                var data = BL.FetchCurrentReservations(App.userId);

                foreach (var reservation in data)
                {
                    var remainingTime = GetRemainingTime((DateTime)reservation["EndTime"]);

                    currentReservationList.Add(new ParkingSpotReservationViewModel
                    {
                        SpotId = (int)reservation["SpotId"],
                        ReservationId = (int)reservation["ReservationId"],
                        StartTime = (DateTime)reservation["StartTime"],
                        EndTime = (DateTime)reservation["EndTime"],
                        TotalCost = (int)reservation["TotalCost"],
                        Location = reservation["Location"].ToString(),
                        RemainingTime = remainingTime
                    });
                }

                // Efficiently refresh the UI
                currentGrid.ItemsSource = currentReservationList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading current reservations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetRemainingTime(DateTime endTime)
        {
            var timeLeft = endTime - DateTime.Now;
            return timeLeft.TotalSeconds > 0 ? timeLeft.ToString(@"hh\:mm\:ss") : "00:00:00";
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += UpdateRemainingTime;
            timer.Start();
        }

      
        private void UpdateRemainingTime(object sender, EventArgs e)
        {
            try
            {
                var expiredReservations = new List<ParkingSpotReservationViewModel>();

                // Update remaining time for each reservation
                foreach (var reservation in currentReservationList)
                {
                    if (DateTime.Now >= reservation.StartTime) // Show remaining time only if reservation has started
                    {
                        var timeLeft = reservation.EndTime - DateTime.Now;
                        reservation.RemainingTime = timeLeft.TotalSeconds > 0
                            ? timeLeft.ToString(@"hh\:mm\:ss")
                            : "00:00:00";

                        // Update reservation status when time has expired
                        if (timeLeft.TotalSeconds <= 0)
                        {
                            if (BL.UpdateReservationStatus(reservation.ReservationId, "Complete"))
                            {
                                expiredReservations.Add(reservation);
                            }
                            else
                            {
                                MessageBox.Show("Reservation Status Not updated to complete");
                            }
                        }
                    }
                    else
                    {
                        reservation.RemainingTime = string.Empty; // No remaining time before the reservation starts
                    }
                }

                // Process expired reservations
                foreach (var reservation in expiredReservations)
                {
                    currentReservationList.Remove(reservation); // Remove expired reservation
                    bool isReserved = false;

                    // Retrieve all reservations for the current spot
                    List<Reservations> allReservationsOfSpot = BL.GetAllReservationsOfSpot(reservation.SpotId);

                    // Check for upcoming reservations for the spot
                    var upcomingReservation = allReservationsOfSpot
                        .Where(Slot => Slot.StartTime > DateTime.Now && Slot.ReservationStatus != "Cancelled")
                        .OrderBy(Slot => Slot.StartTime)
                        .FirstOrDefault();

                    if (upcomingReservation != null)
                    {
                        // If a reservation exists in the future, mark the spot as Reserved
                        isReserved = true;
                        if (BL.UpdateSpotStatus(reservation.SpotId, "Reserved"))
                        {
                            // Spot reserved successfully
                        }
                    }

                    // If no upcoming reservations, mark the spot as Free
                    if (!isReserved)
                    {
                        if (BL.UpdateSpotStatus(reservation.SpotId, "Free"))
                        {
                            // Spot freed successfully
                        }
                    }
                }

                // Refresh the grid with the updated list of current reservations
                currentGrid.ItemsSource = null;
                currentGrid.ItemsSource = currentReservationList;

                // Load past reservations (if necessary)
                LoadPastReservations();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display an error message
                MessageBox.Show($"Error updating remaining time: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                var choice = MessageBox.Show("Do you want to cancel this reservation?", "Cancel Reservation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (choice == MessageBoxResult.Yes)
                {
                    var button = sender as Button;
                    if (button?.Tag is ParkingSpotReservationViewModel reservation)
                    {
                        // Cancel the specific reservation
                        bool isCancelled = BL.UpdateReservationStatus(reservation.ReservationId, "Cancelled");

                        if (isCancelled)
                        {
                            // Remove the canceled reservation from the current list
                            currentReservationList.Remove(reservation);

                            // Retrieve all reservations for the current spot
                            List<Reservations> allReservationsOfSpot = BL.GetAllReservationsOfSpot(reservation.SpotId);

                            // Check if any active or upcoming reservations exist for the same spot
                            var hasOtherActiveReservations = allReservationsOfSpot.Any(r =>
                                r.StartTime <= DateTime.Now && r.EndTime >= DateTime.Now && r.ReservationStatus != "Cancelled");

                            var hasFutureReservations = allReservationsOfSpot.Any(r =>
                                r.StartTime > DateTime.Now && r.ReservationStatus != "Cancelled");

                            if (!hasOtherActiveReservations && !hasFutureReservations)
                            {
                                // If no other active or upcoming reservations, mark spot as Free
                                if (BL.UpdateSpotStatus(reservation.SpotId, "Free"))
                                {
                                    MessageBox.Show($"Spot {reservation.SpotId} is now free.", "Spot Status Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update the spot status to Free.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                // If there are future or active reservations, mark the spot as Reserved
                                if (BL.UpdateSpotStatus(reservation.SpotId, "Reserved"))
                                {
                                    MessageBox.Show($"Spot {reservation.SpotId} remains reserved for other reservations.", "Spot Status Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update the spot status to Reserved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                            // Refresh the grid with the updated list
                            currentGrid.ItemsSource = null;
                            currentGrid.ItemsSource = currentReservationList;
                        }
                        else
                        {
                            MessageBox.Show("Failed to cancel the reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling reservation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void viewBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button?.Tag is ParkingSpotReservationViewModel reservation)
                {
                    var updatedReservation = new ParkingSpotReservationViewModel
                    {
                        SpotId = reservation.SpotId,
                        ReservationId = reservation.ReservationId,
                        Location = reservation.Location,
                        StartTime = reservation.StartTime,
                        EndTime = reservation.EndTime,
                        TotalCost = reservation.TotalCost,
                        RemainingTime = reservation.RemainingTime
                    };


                    var mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow!=null)
                    {
                        var updatedPage = new ReservationDetailsPage(updatedReservation);
                        mainWindow.MainContent.Content = updatedPage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing reservation details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
