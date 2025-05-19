CREATE TABLE [dbo].[Users] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [username]       VARCHAR (50)  NOT NULL,
    [email]          VARCHAR (100) NOT NULL,
    [hashedPassword] VARCHAR (256) NOT NULL, 
    [passwordSalt] VARCHAR(256) NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([id])
);

