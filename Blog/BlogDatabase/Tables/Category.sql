﻿CREATE TABLE [dbo].[Category]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(250) NOT NULL, 
)
GO