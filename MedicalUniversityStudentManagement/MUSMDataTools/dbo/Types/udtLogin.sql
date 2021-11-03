CREATE TYPE [dbo].[udtLogin] AS TABLE
(
	[Username] VARCHAR(32) NOT NULL,
	[Password] VARCHAR(256) NOT NULL,
	[UserType] INT NOT NULL
)
