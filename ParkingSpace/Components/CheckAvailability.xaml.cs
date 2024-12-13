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
using ParkingSpace.Components.Admin_Controls;
namespace ParkingSpace.Components
{
    /// <summary>
    /// Interaction logic for CheckAvailability.xaml
    /// </summary>
    public partial class CheckAvailability : UserControl
    {
        List<ParkingSpot> ps = new List<ParkingSpot>();
        string selected_Section;
        string selected_Level;
        string selected_Spot;
        int SpotID;
        public CheckAvailability()
        {
            InitializeComponent();
            LoadData();
            SpotLocationFilter.TextChanged += FilterData;
            SectionFilter.SelectionChanged += FilterData;
            LevelFilter.SelectionChanged += FilterData;
        }
        private void LoadData()
        {
            ps = BL.GetAllParkingSpots();
            ParkingStatusDatagrid.ItemsSource = ps;
            this.DataContext = ps;

            var sections = ps.Select(p => p.Section).Distinct().ToList();
            var levels = ps.Select(p => p.Level).Distinct().ToList();

            SectionFilter.Items.Clear();
            LevelFilter.Items.Clear();

            SectionFilter.Items.Add(new ComboBoxItem { Content = "All" });
            LevelFilter.Items.Add(new ComboBoxItem { Content = "All" });

            foreach (var section in sections)
            {
                SectionFilter.Items.Add(new ComboBoxItem { Content = section });
            }

            foreach (var level in levels)
            {
                LevelFilter.Items.Add(new ComboBoxItem { Content = level });
            }
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



        private void viewBtn(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if(btn != null && btn.Tag is ParkingSpot spot)
            {
                ParkingSpot spotInfo = new ParkingSpot
                {
                    Location = spot.Location,
                    Section = spot.Section,
                    Level = spot.Level,
                    SpotID = spot.SpotID,
                };
                
                BookSpot holdSpot = new BookSpot(spotInfo);
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.MainContent.Content = holdSpot;
                }
            }
        }
    }
}
