using ParkingSpace.BusinessLayer;
//using ParkingSpace.fonts;

//using ParkingSpace.fonts;
//using ParkingSpace.Models;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for BookSpot.xaml
    /// </summary>
    public partial class BookSpot : UserControl,INotifyPropertyChanged
    {
        ParkingSpot _spotInfo;
        List<ParkingSpotReservationViewModel> Currentlist = new List<ParkingSpotReservationViewModel>();
        //List<ParkingSpotReservationViewModel> expiredReservations = new List<ParkingSpotReservationViewModel>();

        public BookSpot(object spotInfo)
        {
            InitializeComponent();
           
            if (spotInfo != null)
            {
                _spotInfo = (ParkingSpot)spotInfo;
                this.DataContext = _spotInfo;
                SpotTitle.Text = $"{_spotInfo.Location}  - {_spotInfo.Section} - {_spotInfo.Level}";
                LoadData();
            }
            else
            {
                MessageBox.Show("Invalid parking spot data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // INotifyPropertyChanged IMPLEMENTATION
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isDataGrid;

        public bool IsDataGrid
        {
            get { return isDataGrid; }
            set 
            { 
                isDataGrid = value; 
                OnPropertyChanged(nameof(IsDataGrid));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadData()
        {
            List<ParkingSpotReservationViewModel> data = BL.GetScheduleSpot(_spotInfo.Location);

            // check for height of data grid
            IsDataGrid = data == null || data.Count == 0;
            foreach (var item in data)
            {
                var reservation = new ParkingSpotReservationViewModel
                {
                    UserName = item.UserName,
                    Location = item.Location,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    TotalCost = item.TotalCost,
                    SpotStatus = item.SpotStatus,
                };
                Currentlist.Add(reservation);
            }
            //foreach (var item in data)
            //{
            //    if (item.EndTime <= DateTime.Now)
            //    {
            //        if (!BL.completeReservation(_spotInfo.SpotID, "Complete", "Free"))
            //        {
            //            MessageBox.Show("Reservation is not updating after time ends");
            //        }
            //        else
            //        {
            //           Currentlist.Remove(item);
            //        }
            //    }
            //}
            dataGrid.ItemsSource = Currentlist;
        }

       
      
        private void BackBtn(object sender, RoutedEventArgs e)
        {

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow!=null)
            {
                CheckAvailability checkAvailability = new CheckAvailability();
                mainWindow.MainContent.Content = checkAvailability;
            }
        }

        private void reserveBtn(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                ReserveSpot reserveSpot = new ReserveSpot(SpotTitle.Text,_spotInfo.SpotID);
                mainWindow.MainContent.Content = reserveSpot;
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
