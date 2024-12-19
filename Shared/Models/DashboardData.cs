using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DashboardData
    {
        public int TotalUsers { get; set; }
        public int TotalSpots { get; set; }
        public int ReservedSpots { get; set; }
        public int FreeSpots { get; set; }
        public int CompleteReservations { get; set; }
        public int CancelReservation { get; set; }
    }
}
