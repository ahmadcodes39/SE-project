using ParkingSpace.BusinessLayer;
using ParkingSpace.fonts;
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

namespace ParkingSpace.Components.Admin_Controls
{
    /// <summary>
    /// Interaction logic for AddParkingSpot.xaml
    /// </summary>
    public partial class AddParkingSpot : UserControl
    {
        public AddParkingSpot()
        {
            InitializeComponent();
        }

        private void saveBtn(object sender, RoutedEventArgs e)
        {
            string location = LocationInput.Text;
            string section = SectionInput.Text;
            string level = LevelInput.Text;

            if (string.IsNullOrEmpty(location)||string.IsNullOrEmpty(section)||string.IsNullOrEmpty(level))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BL.isSpotExist(location))
            {
                MessageBox.Show("This Spot Already Exist.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return ;
            }
            ParkingSpot obj = new ParkingSpot
            {
                Location = location,
                Section = section,
                Level = level,
                SpotStatus = "Free"
            };
            if (BL.AddParkingSpot(obj))
            {
                MessageBox.Show("Parking Spot Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LocationInput.Text = string.Empty;
                SectionInput.Text = string.Empty;
                LevelInput.Text = string.Empty;
            }

        }
        private void cancelBtn(object sender, RoutedEventArgs e)
        {


        }
    }
}
