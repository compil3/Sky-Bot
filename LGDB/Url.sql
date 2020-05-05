﻿CREATE TABLE [dbo].[Url]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PlayerName] NVARCHAR(50) NOT NULL, 
    [PlayerUrl] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_Url_ToTable] FOREIGN KEY (Id) REFERENCES PlayerCareer(Id)
)
