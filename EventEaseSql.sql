--paste it here
CREATE TABLE Venues(
VenueID INT IDENTITY(1,1) PRIMARY KEY,
VenueName NVARCHAR(100) NOT NULL,
VenueLocation NVARCHAR(150) NOT NULL,
Capacity INT NOT NULL,
ImageURL NVARCHAR(200)
);

CREATE TABLE Events(
EventID INT IDENTITY(1,1) PRIMARY KEY,
EventName NVARCHAR(100) NOT NULL,
StartDate DATETIME NOT NULL,
EndDate DATETIME NOT NULL,
EventDescription NVARCHAR(200),
EventImageURL NVARCHAR (200)
);

CREATE TABLE Bookings (
BookingID INT IDENTITY(1,1) PRIMARY KEY,
EventID INT NOT NULL,
SpecialistName NVARCHAR(100) NOT NULL,
SpecialistEmail NVARCHAR(200),
BookingDate DATETIME NOT NULL,
VenueID INT NOT NULL,

CONSTRAINT FK_Event
FOREIGN KEY (VenueID)
REFERENCES Venues(VenueID)
ON DELETE CASCADE,

CONSTRAINT FK_Bookings
FOREIGN KEY (EventID)
REFERENCES Events(EventID)
ON DELETE CASCADE
);
--Making changes to Database
--Adding availability attribute to Venues
ALTER TABLE Venues
ADD Availability BIT NOT NULL DEFAULT 1;

--Adding a lookup table for EventTypes
CREATE TABLE EventTypes(
EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
EventType NVARCHAR(100) NOT NULL
);

--Adding EventType field to Event table
ALTER TABLE Events
ADD EventTypeID INT NOT NULl;

ALTER TABLE Events
ADD CONSTRAINT FK_Events_EventTypes FOREIGN KEY (EventTypeID) REFERENCES EventTypes(EventTypeID);

INSERT INTO EventTypes(EventType)
VALUES
('Entertainment'),
('Formal'),
('Fundraiser'),
('Outdoor'),
('Cultural');