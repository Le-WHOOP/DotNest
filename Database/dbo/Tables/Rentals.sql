CREATE TABLE [dbo].[Rentals] (
    [id]        INT IDENTITY (1, 1) NOT NULL,
    [pictureId] INT NULL,
    [userId]    INT NOT NULL,
    CONSTRAINT [PK_Rentals] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Rentals_Pictures] FOREIGN KEY ([pictureId]) REFERENCES [dbo].[Pictures] ([id]),
    CONSTRAINT [FK_Rentals_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([id])
);

