CREATE TABLE [dbo].[Bookings] (
    [id]       INT      IDENTITY (1, 1) NOT NULL,
    [userId]   INT      NOT NULL,
    [rentalId] INT      NOT NULL,
    [fromDate] DATETIME NOT NULL,
    [toDate]   DATETIME NOT NULL
);

