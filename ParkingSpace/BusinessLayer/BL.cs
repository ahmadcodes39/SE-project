using ParkingSpace.DataAccessLayer;
using ParkingSpace.fonts;
using ParkingSpace.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ParkingSpace.BusinessLayer
{
    internal class BL
    {
        public static bool RegisterUser(Users obj)
        {
            DAL dAL = new DAL();
            return dAL.registerUser(obj);
        }

        public static bool CheckIfUserExists(string email)
        {
            DAL dAL = new DAL();
            return dAL.IsUserExists(email);
        }

        public static Users ValidateLogin(string email, string password)
        {
            DAL dal = new DAL();
            return dal.CheckLoginCredentials(email, password);
        }

        public static List<ParkingSpot> GetAllParkingSpots()
        {
            DAL dAL = new DAL();
            return dAL.GetParkingSpots();
        }

        public static (int ReservationId, int SpotId) InsertReservationData(Reservations reservation)
        {
            DAL dAL = new DAL();
            return dAL.insertReservationData(reservation);
        }

        public static bool UpdateSpotStatus(int id, string status)
        {
            DAL dAL = new DAL();
            return dAL.updateSpotStatus(id, status);
        }

        //public static bool updateSpotAndReservationStatus()
        //{
        //    DAL dAL = new DAL();
        //    return dAL.UpdateSpotAndReservationStatus();
        //}

        public static List<Dictionary<string, object>> FetchPastReservations(int UserId)
        {
            DAL dAL = new DAL();
            return dAL.getPastReservations(UserId);
        }
        public static List<Dictionary<string, object>> FetchCurrentReservations(int UserId)
        {
            DAL dAL = new DAL();
            return dAL.getCurrentReservations(UserId);
        }

        public static bool InsertPaymentData(Payments payments)
        {
            DAL dAL = new DAL();
            return dAL.insertPaymentData(payments);
        }

        //public static bool completeReservation(int spotId, string reservationStatus, string spotStatus)
        //{
        //    DAL dAL = new DAL();
        //    return dAL.CompleteReservation(spotId, reservationStatus, spotStatus);
        //}

        public static bool UpdateReservationStatus(int reservationId, string status)
        {
            DAL dAL = new DAL();
            return dAL.UpdateReservationStatus(reservationId, status);
        }
        public static bool UpdatePayment(int ReservationID, int UserId, int PaymentAmount)
        {
            DAL dAL = new DAL();
            return dAL.UpdatePayment(ReservationID, UserId, PaymentAmount);
        }
        public static bool UpdateReservationPayment(int ReservationID, int UserId, int PaymentAmount)
        {
            DAL dAL = new DAL();
            return dAL.UpdateReservationPayment(ReservationID, UserId, PaymentAmount);
        }

        public static int GetPaymentAmount(int ReservationID, int UserId)
        {
            DAL dAL = new DAL();
            return dAL.GetPaymentAmount(ReservationID, UserId);
        }

        public static List<Reservations> GetAllReservationsOfSpot(int reservationId)
        {
            DAL dAL = new DAL();
            return dAL.GetAllReservationsOfSpot(reservationId);
        }
        public static List<Reservations> GetOverlappingReservations(int spotId, DateTime startDateTime, DateTime endDateTime)
        {
            DAL dAL = new DAL();
           return dAL.GetOverlappingReservations(spotId, startDateTime, endDateTime);
        }
        public static bool ExtendReservation(int reservationId, int userId, DateTime startTime, DateTime endTime)
        {
            DAL dAL = new DAL();
            return dAL.ExtendReservation(reservationId, userId, startTime, endTime);
        }
       
        public static List<ParkingSpotReservationViewModel> GetScheduleSpot(string location)
        {
            DAL dAL = new DAL();
            return dAL.getScheduleSpot(location);
        }

        public static bool isEmailExist(string email)
        {
            DAL dAL = new DAL();
            return dAL.findUser(email);
        }

        public static bool generateAndSendOTP(string email)
        {
            DAL dAL = new DAL();
            return dAL.GenerateAndSendOTP(email);
        }

        public static bool validateOTP(string email, string enteredOTP)
        {
            DAL dAL = new DAL();
            return dAL.VerifyOTP(email, enteredOTP);
        }

        public static bool updatePassword(string email, string password)
        {
            DAL dAL = new DAL();
            return dAL.ResetPassword(email, password);
        }

        public static bool isSpotExist(string Location)
        {
            DAL dAL = new DAL();
            return dAL.isSpotExist(Location);
        }
        public static bool AddParkingSpot(ParkingSpot obj)
        {
            DAL dAL = new DAL();
            return dAL.AddParkingSpot(obj);
        }
        public static bool IsAdminExist(string email, string password)
        {
            DAL dAL = new DAL();
            return dAL.IsAdminExist(email,password);
        }

        public static List<ParkingSpotReservationViewModel> ViewPaymentData()
        {
            DAL dAL = new DAL();
            return dAL.ViewPaymentData();
        }
         public static DashboardData GetDashboardData()
        {
            DAL dAL = new DAL();
            return dAL.GetDashboardData();
        }


    }
}
