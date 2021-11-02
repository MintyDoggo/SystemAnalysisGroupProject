CREATE TYPE [dbo].[udtLogin] AS TABLE
(
	[Username] varchar(32) NOT NULL,
	[Password] varchar(256) NOT NULL,
	[UserType] INT NOT NULL
)
