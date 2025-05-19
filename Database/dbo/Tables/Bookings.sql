CREATE TABLE [dbo].[Bookings] (
    [id]       INT      IDENTITY (1, 1) NOT NULL,
    [userId]   INT      NOT NULL,
    [rentalId] INT      NOT NULL,
    [fromDate] DATETIME NOT NULL,
    [toDate]   DATETIME NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Bookings_Rentals] FOREIGN KEY ([rentalId]) REFERENCES [dbo].[Rentals] ([id]),
    CONSTRAINT [FK_Bookings_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([id])
);

