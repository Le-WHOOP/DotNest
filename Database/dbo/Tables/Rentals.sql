CREATE TABLE [dbo].[Rentals] (
    [id]        INT IDENTITY (1, 1) NOT NULL,
    [pictureId] INT NULL,
    [userId]    INT NOT NULL,
    [name] VARCHAR(100) NOT NULL, 
    [description] VARCHAR(2000) NOT NULL, 
    [city] VARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_Rentals] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Rentals_Pictures] FOREIGN KEY ([pictureId]) REFERENCES [dbo].[Pictures] ([id]),
    CONSTRAINT [FK_Rentals_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([id])
);

