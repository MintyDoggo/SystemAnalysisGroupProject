﻿CREATE TABLE [dbo].[tblLogin]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Username] VARCHAR(32) NOT NULL,
	[Password] VARCHAR(256) NOT NULL,
	[UserType] VARCHAR(16) NOT NULL

)
