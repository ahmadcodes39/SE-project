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
        }
        private void LoadData()
        {
            List<ParkingSpotReservationViewModel> paymentDataList = BL.ViewPaymentData();
            paymentData.ItemsSource = paymentDataList;
        }
    }
}
