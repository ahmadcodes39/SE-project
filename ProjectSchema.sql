CREATE DATABASE ParkingSystem;
USE ParkingSystem;

-- Users Table
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,  
    Email NVARCHAR(50) UNIQUE NOT NULL,   
    UserName NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL, 
    Phone NVARCHAR(15) NOT NULL, 
    RegistrationDate DATE DEFAULT GETDATE()
);

--select * from Users
--SELECT 
--    ReservationId,
--    FORMAT(StartTime, 'dddd, MMMM dd, yyyy, hh:mm tt') AS StartTime,
--    FORMAT(EndTime, 'dddd, MMMM dd, yyyy, hh:mm tt') AS EndTime,
--    TotalCost,
--    ReservationStatus,
--    SpotId,
--    UserId
--FROM Reservation;
--select * from ParkingSpot
-- where SpotId = 25
--DELETE FROM Reservation
--WHERE ReservationId NOT IN (
--    SELECT MAX(ReservationId)
--    FROM Reservation
--    WHERE SpotId = 25 -- Assuming SpotId is 25 in this case
--);
--
--select *  from Reservation
--select * from ParkingSpot
--delete  from Reservation

--alter table Users
--Add ResetTokenExpiry DATETIME 
--ResetToken NVARCHAR(6),
    

-- ParkingSpot Table


--DROP COLUMN SpotTime
--ALTER TABLE ParkingSpot
--DROP CONSTRAINT DF__ParkingSp__SpotT__5CD6CB2B
--ALTER TABLE ParkingSpot
--DROP COLUMN  SpotTime  
--UPDATE Reservation SET ReservationStatus ='Done' WHERE SpotId = 3
--select * from Reservation
--ALTER TABLE ParkingSpot
--ALTER COLUMN SpotStatus NVARCHAR(20) NOT NULL;

CREATE TABLE ParkingSpot (
    SpotId INT IDENTITY(1,1) PRIMARY KEY,
    Location NVARCHAR(255) NOT NULL,
	Section NVARCHAR(100),
	Level NVARCHAR(100),
    SpotStatus BIT NOT NULL   
);
--select * from ParkingSpot
-- Reservation Table
CREATE TABLE Reservation (
    ReservationId INT IDENTITY(1,1) PRIMARY KEY,-- automatically set
    StartTime DATETIME DEFAULT GETDATE() NOT NULL,
    EndTime DATETIME NOT NULL,
    TotalCost INT,
    ReservationStatus NVARCHAR(20) NOT NULL ,-- set when hit a btn
  
    
    UserId INT NOT NULL,
    SpotId INT NOT NULL, -- come from precious page
    
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (SpotId) REFERENCES ParkingSpot(SpotId) ON DELETE CASCADE
);
--ALTER TABLE Reservation
--DROP COLUMN ReservationDate
--select * from Users where UserId = 5
--select * from Reservation
--delete from Reservation where ReservationId = 120
--update Reservation set ReservationStatus = 'Complete' 
--select*from ParkingSpot
--select*from Users
--ALTER TABLE Reservation
--DROP CONSTRAINT DF__Reservati__Reser__3F466844;
--SELECT name
--FROM sys.default_constraints
--WHERE parent_object_id = OBJECT_ID('Reservation')
--  AND parent_column_id = COLUMNPROPERTY(object_id('Reservation'), 'ReservationStatus', 'ColumnId');




-- Payments Table
CREATE TABLE Payments (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    ReservationID INT NOT NULL,  
    UserId INT NOT NULL,         
    PaymentAmount INT,
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending',
    PaymentDate DATETIME DEFAULT GETDATE(),
    
    FOREIGN KEY (ReservationID) REFERENCES Reservation(ReservationId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) 
);
--select * from Payments
--select * from Reservation where SpotId=14
--select * from ParkingSpot where SpotId = 14
-- Admins Table
CREATE TABLE Admins (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Email NVARCHAR(30) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    CreatedAt DATE DEFAULT GETDATE()
);

--INSERT INTO Admins (Name, Email, Password)
--VALUES ('Ahmad', 'ahmadcodes39@gmail.com', 'admin123');
--select * from Admins

--select   u.UserName,
--		(p.Location + '-' + p.Section + '-' + p.Level) as SpotLocation,
--		r.StartTime , r.EndTime,r.TotalCost,r.ReservationStatus
--
--from Reservation as r
--INNER JOIN Users as u ON u.UserId = r.UserId
--INNER JOIN ParkingSpot as p on p.SpotId = r.SpotId

SELECT
    (SELECT COUNT(*) FROM Users) AS TotalUsers,

    (SELECT COUNT(*) FROM ParkingSpot) AS TotalParkingSpots,

    (SELECT COUNT(*) FROM ParkingSpot WHERE SpotStatus = 'Free') AS TotalFreeSpots,

    (SELECT COUNT(*) FROM ParkingSpot WHERE SpotStatus = 'Reserved') AS TotalReservedSpots,

    (SELECT COUNT(*) FROM Reservation WHERE ReservationStatus = 'Complete') AS TotalCompleteReservations,

    (SELECT COUNT(*) FROM Reservation WHERE ReservationStatus = 'Cancelled') AS TotalCancelReservations;

	select * from Reservation
	 select   u.UserName,
		                                (p.Location + '-' + p.Section + '-' + p.Level) as SpotLocation,
		                                r.StartTime , r.EndTime,r.TotalCost,r.ReservationStatus
                                        from Reservation as r
                                        INNER JOIN Users as u ON u.UserId = r.UserId
                                        INNER JOIN ParkingSpot as p on p.SpotId = r.SpotId