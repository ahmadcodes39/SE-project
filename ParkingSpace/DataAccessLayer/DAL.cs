using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Shared.Models;
using System.Windows.Media;
using System.Windows;
//using ParkingSpace.fonts;
using System.Collections.ObjectModel;
using ControlzEx.Standard;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;
using ControlzEx.Theming;
namespace ParkingSpace.DataAccessLayer
{
    internal class DAL
    {
        public static string connectionString = "Data Source=DESKTOP-0LM8NSU\\MSSQLSERVER02;Initial Catalog=ParkingSystem;Persist Security Info=True;User ID=sa;Password=ahmad531616;";
        public static SqlConnection conn = new SqlConnection(connectionString);

        public bool registerUser(Users obj)
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO Users (UserName, Email, Password, Phone) " +
                               "VALUES (@Name, @email, @password, @Phone)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@email", obj.Email);
                cmd.Parameters.AddWithValue("@password", obj.Password);
                cmd.Parameters.AddWithValue("@Phone", obj.Phone);


                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool IsUserExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        int userCount = (int)cmd.ExecuteScalar();
                        return userCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        public Users CheckLoginCredentials(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserId, Email, Password, UserName, Phone, RegistrationDate FROM Users WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Users user = new Users
                                {
                                    ID = (int)reader["UserId"],
                                    Email = reader["Email"].ToString(),
                                    Name = reader["UserName"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    RegistrationDate = (DateTime)reader["RegistrationDate"]
                                };
                                return user;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        public List<ParkingSpot> GetParkingSpots()
        {
            List<ParkingSpot> parkingSpots = new List<ParkingSpot>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SpotId, Location, Section, Level, SpotStatus FROM ParkingSpot";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ParkingSpot spot = new ParkingSpot
                            {
                                SpotID = Convert.ToInt32(reader["SpotId"]),
                                Location = reader["Location"].ToString(),
                                Section = reader["Section"].ToString(),
                                Level = reader["Level"].ToString(),
                                SpotStatus = Convert.ToString(reader["SpotStatus"])
                            };

                            parkingSpots.Add(spot);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return parkingSpots;
        }

        public (int ReservationId, int SpotId) insertReservationData(Reservations obj)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Reservation (StartTime, EndTime, TotalCost, ReservationStatus, UserId, SpotId) " +
                                   "OUTPUT INSERTED.ReservationId, INSERTED.SpotId " +
                                   "VALUES (@start, @end, @cost, @status, @userId, @spotId)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", obj.StartTime);
                        cmd.Parameters.AddWithValue("@end", obj.EndTime);
                        cmd.Parameters.AddWithValue("@cost", obj.TotalCost);
                        cmd.Parameters.AddWithValue("@status", obj.ReservationStatus);
                        cmd.Parameters.AddWithValue("@userId", obj.UserId);
                        cmd.Parameters.AddWithValue("@spotId", obj.SpotId);


                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int reservationId = reader.GetInt32(0);
                                int spotId = reader.GetInt32(1);
                                return (reservationId, spotId);
                            }
                        }
                    }
                }
                return (0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return (0, 0);
            }
        }

        public List<Dictionary<string, object>> getPastReservations(int UserId)
        {
            List<Dictionary<string, object>> pastReservations = new List<Dictionary<string, object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                                    SELECT 
                                        ( p.Location + '-' + p.Section + '-' + p.Level) AS Location, 
                                        p.Section, 
                                        p.Level, 
                                        p.Location,
                                        r.StartTime, 
                                        r.EndTime, 
                                        r.TotalCost, 
                                        r.ReservationStatus
                                    FROM Reservation AS r
                                    INNER JOIN ParkingSpot AS p ON p.SpotId = r.SpotId
                                    INNER JOIN Users AS u ON u.UserId = r.UserId
                                    WHERE (r.ReservationStatus = 'Cancelled' OR r.ReservationStatus = 'Complete')
                                      AND u.UserId = @UserId";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Dictionary<string, object> reservationData = new Dictionary<string, object>
                            {
                                {"Location",reader["Location"].ToString() },
                                {"Section",reader["Section"].ToString() },
                                {"Level",reader["Level"].ToString() },
                                {"StartTime",reader.GetDateTime(reader.GetOrdinal("StartTime")) },
                                {"EndTime",reader.GetDateTime(reader.GetOrdinal("EndTime")) },
                                { "TotalCost", reader.GetInt32(reader.GetOrdinal("TotalCost")) },
                                { "ReservationStatus", reader["ReservationStatus"].ToString() }
                            };
                            pastReservations.Add(reservationData);
                        }
                    }
                }
                return pastReservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public List<Dictionary<string, object>> getCurrentReservations(int UserId)
        {
            List<Dictionary<string, object>> currentReservations = new List<Dictionary<string, object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                                    SELECT 
                                        ( p.Location + '-' + p.Section + '-' + p.Level) AS Location, 
                                        r.SpotId,
                                        r.ReservationId,
                                        p.Section, 
                                        p.Level, 
                                        p.Location,
                                        r.StartTime, 
                                        r.EndTime, 
                                        r.TotalCost
                                    FROM Reservation AS r
                                    INNER JOIN ParkingSpot AS p ON p.SpotId = r.SpotId
                                    INNER JOIN Users AS u ON u.UserId = r.UserId
                                    WHERE r.ReservationStatus = 'Active' 
                                        AND r.EndTime > GETDATE()
                                        AND u.UserId = @UserId";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Dictionary<string, object> reservationData = new Dictionary<string, object>
                            {
                                {"Location",reader["Location"].ToString() },
                                {"SpotId", reader["SpotId"]},
                                {"ReservationId", reader["ReservationId"]},
                                {"Section",reader["Section"].ToString() },
                                {"Level",reader["Level"].ToString() },
                                {"StartTime",reader.GetDateTime(reader.GetOrdinal("StartTime")) },
                                {"EndTime",reader.GetDateTime(reader.GetOrdinal("EndTime")) },
                                { "TotalCost", reader.GetInt32(reader.GetOrdinal("TotalCost")) },
                            };
                            currentReservations.Add(reservationData);
                        }
                    }
                }
                return currentReservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        //public bool insertPaymentData(Payments obj)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string query = @"
        //                            INSERT INTO Payments (ReservationID, UserId, PaymentAmount, PaymentStatus) 
        //                            VALUES (@id, @userId, @amount, @status)";

        //            using (SqlCommand cmd = new SqlCommand(query,conn))
        //            {
        //                cmd.Parameters.AddWithValue("@id", obj.ReservationId);
        //                cmd.Parameters.AddWithValue("@userId", obj.UserId);
        //                cmd.Parameters.AddWithValue("@amount", obj.PaymentAmount);
        //                cmd.Parameters.AddWithValue("@status", obj.PaymentStatus);
        //                int rowAffected = cmd.ExecuteNonQuery();
        //                return rowAffected > 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //}
        public bool insertPaymentData(Payments obj)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Payments WHERE ReservationID = @id AND UserId = @userId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", obj.ReservationId);
                        checkCmd.Parameters.AddWithValue("@userId", obj.UserId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            string updateQuery = @"
                        UPDATE Payments
                        SET PaymentAmount = @amount, PaymentStatus = @status
                        WHERE ReservationID = @id AND UserId = @userId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@id", obj.ReservationId);
                                updateCmd.Parameters.AddWithValue("@userId", obj.UserId);
                                updateCmd.Parameters.AddWithValue("@amount", obj.PaymentAmount);
                                updateCmd.Parameters.AddWithValue("@status", obj.PaymentStatus);
                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                return rowsAffected > 0;
                            }
                        }
                        else
                        {
                            string query = @"
                                        INSERT INTO Payments (ReservationID, UserId, PaymentAmount, PaymentStatus) 
                                        VALUES (@id, @userId, @amount, @status)";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", obj.ReservationId);
                                cmd.Parameters.AddWithValue("@userId", obj.UserId);
                                cmd.Parameters.AddWithValue("@amount", obj.PaymentAmount);
                                cmd.Parameters.AddWithValue("@status", obj.PaymentStatus);
                                int rowAffected = cmd.ExecuteNonQuery();
                                return rowAffected > 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool UpdateReservationStatus(int reservationId, string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Reservation SET ReservationStatus = @status WHERE ReservationId = @reservationId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reservationId", reservationId);
                        cmd.Parameters.AddWithValue("@status", status);
                        int rowAffected = cmd.ExecuteNonQuery();
                        return rowAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool updateSpotStatus(int spotId, string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE ParkingSpot SET SpotStatus = @status WHERE SpotId = @spotId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@spotId", spotId);

                        int rowAffected = cmd.ExecuteNonQuery();
                        return rowAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        public List<Reservations> GetAllReservationsOfSpot(int spotId)
        {
            var dataList = new List<Reservations>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT StartTime, ReservationStatus, EndTime FROM Reservation WHERE SpotId = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", spotId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reservation = new Reservations
                                {
                                    StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                                    ReservationStatus = reader.GetString(reader.GetOrdinal("ReservationStatus")),
                                };
                                dataList.Add(reservation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching reservations: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return dataList;
        }

        public List<Reservations> GetOverlappingReservations(int spotId, DateTime startDateTime, DateTime endDateTime)
        {
            var overlappingReservations = new List<Reservations>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch active reservations that overlap with the provided time range
                    string query = @"
                                    SELECT UserId, StartTime, EndTime, ReservationStatus 
                                    FROM Reservation
                                    WHERE SpotId = @SpotId
                                      AND ReservationStatus = 'Active'
                                      AND @startDateTime < EndTime
                                      AND @endDateTime > StartTime";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Adding parameters to the query
                        cmd.Parameters.AddWithValue("@SpotId", spotId);
                        cmd.Parameters.AddWithValue("@startDateTime", startDateTime);
                        cmd.Parameters.AddWithValue("@endDateTime", endDateTime);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reservation = new Reservations
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                                    EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime")),
                                    ReservationStatus = reader.GetString(reader.GetOrdinal("ReservationStatus")),
                                };

                                overlappingReservations.Add(reservation);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching overlapping reservations: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return overlappingReservations;
        }

        public List<ParkingSpotReservationViewModel> getScheduleSpot(string location)
        {
            List<ParkingSpotReservationViewModel> spotList = new List<ParkingSpotReservationViewModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    string query = @"
                        select ( p.Location + '-' + p.Section + '-' + p.Level)  AS SpotLocation,
                               p.SpotStatus,
                               r.StartTime,
                               r.EndTime,
                               r.TotalCost, 
                               u.UserName
                        from ParkingSpot as p
                        inner join Reservation as r ON p.SpotId = r.SpotId
                        inner join Users as u ON r.UserId = u.UserId 
                        where r.ReservationStatus = 'Active' 
                        AND p.Location = @location   
                        AND r.EndTime > GETDATE()
                        AND P.SpotStatus = 'Reserved'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@location", location);

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var sp = new ParkingSpotReservationViewModel
                            {
                                UserName = reader["UserName"].ToString(),
                                Location = reader["SpotLocation"].ToString(),
                                StartTime = Convert.ToDateTime(reader["StartTime"]),
                                EndTime = Convert.ToDateTime(reader["EndTime"]),
                                TotalCost = Convert.ToInt32(reader["TotalCost"]),
                                SpotStatus = reader["SpotStatus"].ToString()
                            };
                            spotList.Add(sp);
                        }
                    }
                }
                return spotList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public bool UpdatePayment(int ReservationID, int UserId, int PaymentAmount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                            UPDATE Payments
                            SET PaymentAmount = @PaymentAmount
                            WHERE ReservationID = @ReservationID
                            AND UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentAmount", PaymentAmount);
                        cmd.Parameters.AddWithValue("@ReservationID", ReservationID);
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public bool UpdateReservationPayment(int ReservationID, int UserId, int PaymentAmount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                            UPDATE Reservation
                            SET TotalCost = @PaymentAmount
                            WHERE ReservationID = @ReservationID
                            AND UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentAmount", PaymentAmount);
                        cmd.Parameters.AddWithValue("@ReservationID", ReservationID);
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public int GetPaymentAmount(int ReservationID, int UserId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT TotalCost
                    FROM Reservation
                    WHERE ReservationId = @ReservationID
                    AND UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationID", ReservationID);
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        var result = cmd.ExecuteScalar();

                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        public bool ExtendReservation(int reservationId, int userId, DateTime startTime, DateTime endTime)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Reservation
                                    SET StartTime = @startTime,
                                        EndTime = @endTime
                                    FROM Reservation r
                                    INNER JOIN Users u
                                    ON r.UserId = u.UserId
                                    WHERE r.ReservationId = @reservationId AND u.UserId=@userId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reservationId", reservationId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@startTime", startTime);
                        cmd.Parameters.AddWithValue("@endTime", endTime);

                        int roeAffected = cmd.ExecuteNonQuery();
                        return roeAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // Admin Panel starts here
        public bool isSpotExist(string spotLocation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) FROM ParkingSpot WHERE Location = @spotLocation";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@spotLocation", spotLocation);

                        int count = (int)cmd.ExecuteScalar();


                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool AddParkingSpot(ParkingSpot obj)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ParkingSpot (Location,Section,Level,SpotStatus) VALUES (@Location,@Section,@Level,@SpotStatus)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Location", obj.Location);
                        cmd.Parameters.AddWithValue("@Section", obj.Section);
                        cmd.Parameters.AddWithValue("@Level", obj.Level);
                        cmd.Parameters.AddWithValue("@SpotStatus", obj.SpotStatus);
                        int rowAffected = cmd.ExecuteNonQuery();
                        return rowAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool IsAdminExist(string email, string password)
        {
            try
            {

                string query = "SELECT COUNT(*) FROM Admins WHERE Email = @Email AND Password = @Password";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int count = (int)cmd.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }
        public List<ParkingSpotReservationViewModel> ViewPaymentData()
        {
            try
            {
                List<ParkingSpotReservationViewModel> paymentData = new List<ParkingSpotReservationViewModel>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"   select   u.UserName,
		                                (p.Location + '-' + p.Section + '-' + p.Level) as SpotLocation,
		                                r.StartTime , r.EndTime,r.TotalCost,r.ReservationStatus
                                        from Reservation as r
                                        INNER JOIN Users as u ON u.UserId = r.UserId
                                        INNER JOIN ParkingSpot as p on p.SpotId = r.SpotId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var data = new ParkingSpotReservationViewModel
                            {
                                UserName = reader["UserName"].ToString(),
                                Location = reader["SpotLocation"].ToString(),
                                StartTime = (DateTime)reader["StartTime"],
                                EndTime = (DateTime)reader["EndTime"],
                                TotalCost = (int)reader["TotalCost"],
                                ReservationStatus = reader["ReservationStatus"].ToString()
                            };
                            paymentData.Add(data);
                        }
                    }
                }
                return paymentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }
        }

        public DashboardData GetDashboardData()
        {
            DashboardData data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                                    SELECT
                                        (SELECT COUNT(*) FROM Users) AS TotalUsers,
                                        (SELECT COUNT(*) FROM ParkingSpot) AS TotalParkingSpots,
                                        (SELECT COUNT(*) FROM ParkingSpot WHERE SpotStatus = 'Free') AS TotalFreeSpots,
                                        (SELECT COUNT(*) FROM ParkingSpot WHERE SpotStatus = 'Reserved') AS TotalReservedSpots,
                                        (SELECT COUNT(*) FROM Reservation WHERE ReservationStatus = 'Complete') AS TotalCompleteReservations,
                                        (SELECT COUNT(*) FROM Reservation WHERE ReservationStatus = 'Cancelled') AS TotalCancelReservations";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            data = new DashboardData
                            {
                                TotalUsers = Convert.ToInt32(reader["TotalUsers"]),
                                TotalSpots = Convert.ToInt32(reader["TotalParkingSpots"]),
                                ReservedSpots = Convert.ToInt32(reader["TotalReservedSpots"]),
                                FreeSpots = Convert.ToInt32(reader["TotalFreeSpots"]),
                                CompleteReservations = Convert.ToInt32(reader["TotalCompleteReservations"]),
                                CancelReservation = Convert.ToInt32(reader["TotalCancelReservations"]),
                            };
                        }
                    }
                }
                return data;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }
        }

        public bool GenerateAndSendOTP(string email)
        {
            try
            {
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();
                DateTime expiryTime = DateTime.Now.AddMinutes(10);

                string query = "UPDATE Users SET ResetToken = @ResetToken, ResetTokenExpiry = @ResetTokenExpiry WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ResetToken", otp);
                    cmd.Parameters.AddWithValue("@ResetTokenExpiry", expiryTime);
                    cmd.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        SendEmail(email, otp);
                        return true;
                    }
                    else
                    {
                        throw new Exception("Email not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        private void SendEmail(string toEmail, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ahmadcodes39@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Password Reset OTP";
                mail.Body = $"Your OTP for password reset is: {otp}\nThis OTP will expire in 10 minutes.";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("ahmadcodes39@gmail.com", "kita grao itrk xxiq"); // password
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                MessageBox.Show("OTP sent to your email.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email. " + ex.Message);
            }
        }
        public bool VerifyOTP(string email, string enteredOTP)
        {
            try
            {
                conn.Open();
                string query = "SELECT ResetToken, ResetTokenExpiry FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedOTP = reader["ResetToken"].ToString();
                    DateTime expiry = Convert.ToDateTime(reader["ResetTokenExpiry"]);

                    if (storedOTP == enteredOTP && DateTime.Now <= expiry)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid or expired OTP.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Email not found.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool findUser(string email)
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool ResetPassword(string email, string newPassword)
        {
            try
            {
                string updatedPassword = newPassword;

                conn.Open();
                string query = "UPDATE Users SET Password = @Password, ResetToken = NULL, ResetTokenExpiry = NULL WHERE Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Password", updatedPassword);
                cmd.Parameters.AddWithValue("@Email", email);

                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public void ConfirmPaymentEmail(string toEmail, string holderName, double amount)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                // Use environment variables or a secure method to get credentials
                string fromEmail = "ahmadcodes39@gmail.com";
                string fromPassword = "kita grao itrk xxiq"; // Replace with a secure method

                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = "Spot Reservation Email";
                mail.Body = $"Your Payment has been processed successfully.\n" +
                            $"Holder Name: {holderName}\n" +
                            $"Amount: ${amount}";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential(fromEmail, fromPassword);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message);
            }
        }
    }
}

//