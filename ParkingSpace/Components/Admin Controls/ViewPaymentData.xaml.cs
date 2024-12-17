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

namespace ParkingSpace.Components.Admin_Controls
{
    /// <summary>
    /// Interaction logic for ViewPaymentData.xaml
    /// </summary>
    public partial class ViewPaymentData : UserControl
    {
        public ViewPaymentData()
        {
            InitializeComponent();
            LoadData();
            SpotLocationFilter.TextChanged += FilterData;
        }
        private void LoadData()
        {
            List<ParkingSpotReservationViewModel> paymentDataList = BL.ViewPaymentData();
            paymentData.ItemsSource = paymentDataList;
        }
        private void FilterData(object sender, EventArgs e)
        {
            var filterData = ps.Where(item =>
            (
                string.IsNullOrEmpty(SpotLocationFilter.Text) || item.Location.Contains(SpotLocationFilter.Text)
            ) &&
            (
                SectionFilter.SelectedItem == null ||
               ((ComboBoxItem)SectionFilter.SelectedItem).Content.ToString() == "All" ||
               ((ComboBoxItem)SectionFilter.SelectedItem).Content.ToString() == item.Section
            ) &&
            (
                LevelFilter.SelectedItem == null ||
                ((ComboBoxItem)LevelFilter.SelectedItem).Content.ToString() == "All" ||
                ((ComboBoxItem)LevelFilter.SelectedItem).Content.ToString() == item.Level
            )
            ).ToList();

            ParkingStatusDatagrid.ItemsSource = filterData;
        }

    }
}
