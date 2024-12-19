using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpace.Models
{
    public class Payments
    {
        public int PaymentID { get; set; }
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public double PaymentAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
