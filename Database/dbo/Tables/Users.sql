CREATE TABLE [dbo].[Users] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [username]       VARCHAR (50)  NOT NULL,
    [email]          VARCHAR (100) NOT NULL,
    [hashedPassword] VARCHAR (500) NOT NULL
);

