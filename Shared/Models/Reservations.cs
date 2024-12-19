using System;
using System.ComponentModel;

namespace Shared.Models
{
    public class Reservations : INotifyPropertyChanged
    {
        private int id;
        private DateTime startTime;
        private DateTime endTime;
        private string reservationStatus;
        private DateTime reservationDate;
        private int userId;
        private int spotId;
        private double totalCost;

        // Event for property change notification
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double TotalCost
        {
            get { return totalCost; }
            set
            {
                if (totalCost != value)
                {
                    totalCost = value;
                    OnPropertyChanged(nameof(TotalCost));
                }
            }
        }

        public int SpotId
        {
            get { return spotId; }
            set
            {
                if (spotId != value)
                {
                    spotId = value;
                    OnPropertyChanged(nameof(SpotId));
                }
            }
        }

        public int UserId
        {
            get { return userId; }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        public DateTime ReservationDate
        {
            get { return reservationDate; }
            set
            {
                if (reservationDate != value)
                {
                    reservationDate = value;
                    OnPropertyChanged(nameof(ReservationDate));
                }
            }
        }

        public string ReservationStatus
        {
            get { return reservationStatus; }
            set
            {
                if (reservationStatus != value)
                {
                    reservationStatus = value;
                    OnPropertyChanged(nameof(ReservationStatus));
                }
            }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
    }
}
