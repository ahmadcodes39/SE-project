select * from Users
select * from Reservation where UserId = 5
select*from ParkingSpot
select * from Reservation
INSERT INTO ParkingSpot (Location, Section, Level, SpotStatus)
VALUES
	('A1', 'Section A', 'Level 1', 'Free'),
    ('A2', 'Section A', 'Level 1', 'Free'),
	('A3', 'Section A', 'Level 1', 'Free'),
    ('A4', 'Section A', 'Level 1', 'Free'),

    ('B1', 'Section B', 'Level 1', 'Free'),
    ('B2', 'Section B', 'Level 1', 'Free'),
	('B3', 'Section B', 'Level 1', 'Free'),
    ('B4', 'Section B', 'Level 1', 'Free'),

    ('C1', 'Section C', 'Level 2', 'Free'),
    ('C2', 'Section C', 'Level 2', 'Free'),
	('C3', 'Section C', 'Level 2', 'Free'),
    ('C4', 'Section C', 'Level 2', 'Free'),

    ('D1', 'Section D', 'Level 2', 'Free'),
    ('D2', 'Section D', 'Level 2', 'Free'),
    ('D3', 'Section D', 'Level 2', 'Free'),
    ('D4', 'Section D', 'Level 2', 'Free'),

    ('E1', 'Section E', 'Level 3', 'Free'),
    ('E2', 'Section E', 'Level 3', 'Free'),
	('E3', 'Section E', 'Level 3', 'Free'),
    ('E4', 'Section E', 'Level 3', 'Free'),

	('F1', 'Section F', 'Level 3', 'Free'),
    ('F2', 'Section F', 'Level 3', 'Free'),
	('F3', 'Section F', 'Level 3', 'Free'),
    ('F4', 'Section F', 'Level 3', 'Free');

select * from Reservation
select * from ParkingSpot
UPDATE Reservation

select * from ParkingSpot
update ParkingSpot set SpotStatus = 'Free' where SpotId = 3
SELECT 
    ReservationId,
    FORMAT(StartTime, 'yyyy-MM-dd hh:mm tt') AS FormattedStartTime,
    FORMAT(EndTime, 'yyyy-MM-dd hh:mm tt') AS FormattedEndTime,
    TotalCost,
    ReservationStatus,
    FORMAT(ReservationDate, 'yyyy-MM-dd hh:mm tt') AS FormattedReservationDate,
    UserId,
    SpotId
FROM Reservation;
SELECT * FROM ParkingSpot 
select * from Payments where ReservationId = 160
select * from Users
select * from Payments where UserId=5
select TotalCost from Reservation




-- query to get past reservations
SELECT p.Location, p.Section, p.Level, r.StartTime, r.EndTime, r.TotalCost, r.ReservationStatus
FROM Reservation AS r
INNER JOIN ParkingSpot AS p ON p.SpotId = r.SpotId
WHERE r.ReservationStatus = 'Canceled' OR r.ReservationStatus = 'Done';





select * from ParkingSpot where SpotId = 7
INSERT INTO Reservation (StartTime, EndTime, TotalCost, ReservationStatus, ReservationDate, UserId, SpotId)
VALUES 
(
    '2024-11-28 10:00:00',  -- Start time
    '2024-11-28 14:00:00',  -- End time
    60,                      -- Total cost (assuming 60 PKR)
    'Pending',               -- Reservation status
    GETDATE(),               -- Reservation date (current date and time)
    3,                       -- UserId (assumed to be 3)
    7                        -- SpotId (assumed to be 7)
);


insert into Reservation ()
--SELECT 
--    fk.name AS ForeignKeyName,
--    tp.name AS ParentTable,
--    cp.name AS ParentColumn,
--    tr.name AS ReferencedTable,
--    cr.name AS ReferencedColumn
--FROM 
--    sys.foreign_keys AS fk
--INNER JOIN 
--    sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
--INNER JOIN 
--    sys.tables AS tp ON fkc.parent_object_id = tp.object_id
--INNER JOIN 
--    sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
--INNER JOIN 
--    sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
--INNER JOIN 
--    sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
--WHERE 
--    tr.name = 'ParkingSpot';
--
--
--ALTER TABLE Reservation NOCHECK CONSTRAINT FK__Reservati__SpotI__4222D4EF;
--
--TRUNCATE TABLE ParkingSpot;
--
--
--SELECT 
--    fk.name AS ForeignKeyName,
--    tp.name AS TableName
--FROM 
--    sys.foreign_keys fk
--INNER JOIN 
--    sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
--INNER JOIN 
--    sys.tables tp ON fkc.parent_object_id = tp.object_id
--WHERE 
--    fk.referenced_object_id = OBJECT_ID('ParkingSpot');
--
--ALTER TABLE Reservation DROP CONSTRAINT FK__Reservati__SpotI__4222D4EF;
--
--	
--TRUNCATE TABLE ParkingSpot;
--
--ALTER TABLE Reservation
--ADD CONSTRAINT FK_Reservation_SpotId 
--FOREIGN KEY (SpotId) REFERENCES ParkingSpot(SpotId) ON DELETE CASCADE;
--
--SELECT * FROM ParkingSpot;
--
--SELECT 
--    fk.name AS ForeignKeyName,
--    tp.name AS TableName,
--    cp.name AS ColumnName
--FROM 
--    sys.foreign_keys fk
--INNER JOIN 
--    sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
--INNER JOIN 
--    sys.tables tp ON fkc.parent_object_id = tp.object_id
--INNER JOIN 
--    sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
--WHERE 
--    fk.referenced_object_id = OBJECT_ID('ParkingSpot');
--
--ALTER TABLE Reservation
--ADD CONSTRAINT FK_Reservation_SpotId
--FOREIGN KEY (SpotId)
--REFERENCES ParkingSpot(SpotId)
--ON DELETE CASCADE;
--
--
--SELECT 
--    fk.name AS ForeignKeyName,
--    tp.name AS TableName
--FROM 
--    sys.foreign_keys fk
--INNER JOIN 
--    sys.tables tp ON fk.parent_object_id = tp.object_id
--WHERE 
--    fk.referenced_object_id = OBJECT_ID('ParkingSpot');

 -- Update spots to 'Free'
    UPDATE ParkingSpot
    SET SpotStatus = 'Free'
    WHERE SpotId IN (
        SELECT SpotId
        FROM Reservation
        WHERE ReservationStatus = 'Pending'
          AND DATEDIFF(MINUTE, StartTime, GETDATE()) > 5
    );

    -- Update reservations to 'Cancelled'
    UPDATE Reservation
    SET ReservationStatus = 'Cancelled'
    WHERE ReservationStatus = 'Pending'
      AND DATEDIFF(MINUTE, StartTime, GETDATE()) > 5;

TRUNCATE TABLE Payments;
TRUNCATE TABLE Reservation;
TRUNCATE TABLE ParkingSpot;
TRUNCATE TABLE Users;

ALTER TABLE Payments NOCHECK CONSTRAINT ALL;
ALTER TABLE Reservation NOCHECK CONSTRAINT ALL;

ALTER TABLE Payments CHECK CONSTRAINT ALL;
ALTER TABLE Reservation CHECK CONSTRAINT ALL;

--select* from Reservation
select p.Location,p.Level,p.Section,p.SpotStatus,r.StartTime,r.EndTime,r.TotalCost , u.UserName
from ParkingSpot as p
inner join Reservation as r ON p.SpotId = r.SpotId
inner join Users as u ON r.UserId = u.UserId 
where r.ReservationStatus = 'Active' AND p.Location = 'A3'

update Reservation set ReservationStatus = 'Cancelled'

select * from ParkingSpot
select * from Reservation
update TABLE Reservation 
DROP column ReservationDate 
delete from Reservation
update Reservation set ReservationStatus = 'Active' where ReservationId = 63
 SELECT 
    (p.Location + ' ' + p.Section + ' ' + p.Level) AS SpotLocation,
    p.SpotStatus,
    r.StartTime,
    r.EndTime,
    r.ReservationStatus,
    r.TotalCost
FROM ParkingSpot AS p
INNER JOIN Reservation AS r ON p.SpotId = r.SpotId
WHERE r.ReservationStatus = 'Active' 
  AND p.Location = 'A1';
