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
    /// Interaction logic for SuccessPage.xaml
    /// </summary>
    public partial class SuccessPage : UserControl
    {
        public SuccessPage(string spotLocation)
        {
            InitializeComponent();
            message_txt.Text = spotLocation;
        }

        private void myReservationBtn(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow!=null)
            {
                MyReservations myReservations = new MyReservations();
                mainWindow.MainContent.Content = myReservations;
            }
        }
    }
}
