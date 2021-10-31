CREATE TYPE [dbo].[udtStudent] AS TABLE
(
	[StaffId] INT NOT NULL,
	[StudentIdNumber] INT NOT NULL,
	[FirstName] VARCHAR(64) NOT NULL,
	[LastName] VARCHAR(64) NOT NULL,
	[Birthday] DATETIME2 NOT NULL,
	[Address] VARCHAR(128) NOT NULL,
	[Major] VARCHAR(32) NOT NULL,
	[FirstYearEnrolled] INT NOT NULL,
	[HighSchoolAttended] VARCHAR(64) NOT NULL,
	[UndergraduateSchoolAttended] VARCHAR(64) NOT NULL
)
