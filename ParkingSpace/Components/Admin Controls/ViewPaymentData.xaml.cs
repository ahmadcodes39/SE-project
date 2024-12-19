using ParkingSpace.BusinessLayer;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ParkingSpace.Components.Admin_Controls
{
    /// <summary>
    /// Interaction logic for ViewPaymentData.xaml
    /// </summary>
    public partial class ViewPaymentData : UserControl
    {
        private List<ParkingSpotReservationViewModel> paymentDataList;

        public ViewPaymentData()
        {
            InitializeComponent();
            LoadData();

            // Event handlers for filters
            SpotLocationFilter.TextChanged += FilterData;
            StatusComboBox.SelectionChanged += FilterData;
        }

        private void LoadData()
        {
            // Fetch data from the business layer
            paymentDataList = BL.ViewPaymentData();
            paymentData.ItemsSource = paymentDataList;
        }

        private void FilterData(object sender, EventArgs e)
        {
            var filterData = paymentDataList.Where(item =>
            (
                string.IsNullOrEmpty(SpotLocationFilter.Text) || item.Location.Contains(SpotLocationFilter.Text)
            ) &&
            (
                StatusComboBox.SelectedItem == null ||
                ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString() == "All" ||
                ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString() == item.ReservationStatus
            )).ToList();

            paymentData.ItemsSource = filterData;
        }
    }
}
