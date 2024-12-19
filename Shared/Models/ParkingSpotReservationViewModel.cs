using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpace.Models
{
    public class ParkingSpotReservationViewModel : INotifyPropertyChanged
    {
        private double totalCost;
        private DateTime startTime;
        private DateTime endTime;
        private string reservationStatus;
        private string remainingTime;
        private TimeSpan duration;
        private string location;
        private int reservationId;
        private int spotId;
        private string userName;
        private string spotStatus;

        public string SpotStatus
        {
            get { return spotStatus; }
            set 
            { 
                spotStatus = value; 
                OnPropertyChanged(nameof(SpotStatus));
            }
        }

        public string UserName
        {
            get { return userName; }
            set 
            { 
                userName = value; 
                OnPropertyChanged(nameof(UserName));
            }
        }


        public string RemainingTime
        {
            get { return remainingTime; }
            set 
            {
                remainingTime = value; 
                OnPropertyChanged(nameof(RemainingTime));
            }
        }


     
        public int SpotId
        {
            get { return spotId; }
            set 
            { 
                spotId = value;
                OnPropertyChanged(nameof(SpotId));

            }
        }

        public int ReservationId
        {
            get { return reservationId; }
            set 
            {
                reservationId = value; 
                OnPropertyChanged(nameof(ReservationId));
            }
        }


        public string ReservationStatus
        {
            get => reservationStatus;
            set { reservationStatus = value; OnPropertyChanged(nameof(ReservationStatus)); }
        }

        public TimeSpan Duration
        {
            get => duration;
            set { duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        public string Location
        {
            get => location;
            set { location = value; OnPropertyChanged(nameof(Location)); }
        }

        
        public double TotalCost
        {
            get => totalCost;
            set { totalCost = value; OnPropertyChanged(nameof(TotalCost)); }
        }

        public DateTime StartTime
        {
            get => startTime;
            set { startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public DateTime EndTime
        {
            get => endTime;
            set { endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
