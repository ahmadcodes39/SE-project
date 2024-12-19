using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ParkingSpace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static bool AdminLogin { get; set; } = false;
        public static int userId { get; set; } = -1;
        public static string userEmail { get; set; }
    }
}
