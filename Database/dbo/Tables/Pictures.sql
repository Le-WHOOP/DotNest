﻿CREATE TABLE [dbo].[Pictures] (
    [id]     INT  IDENTITY (1, 1) NOT NULL,
    [base64] TEXT NOT NULL, 
    CONSTRAINT [PK_Pictures] PRIMARY KEY ([id])
);

