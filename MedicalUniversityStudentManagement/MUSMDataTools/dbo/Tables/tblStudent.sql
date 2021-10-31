CREATE TABLE [dbo].[tblStudent]
(
	-- Maybe make some of these NULL as part of the extra information the Students need to fill out
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[StaffId] INT NOT NULL FOREIGN KEY REFERENCES tblStaff(Id),
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
