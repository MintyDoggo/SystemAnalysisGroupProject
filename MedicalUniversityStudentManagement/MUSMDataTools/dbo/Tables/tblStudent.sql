CREATE TABLE [dbo].[tblStudent]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[StaffId] INT NOT NULL FOREIGN KEY REFERENCES tblStaff(Id), -- the associated staff for this student
	[StudentIdNumber] INT,
	[FirstName] VARCHAR(64),
	[LastName] VARCHAR(64),
	[Birthday] DATETIME2,
	[Address] VARCHAR(128),
	[Major] VARCHAR(32),
	[FirstYearEnrolled] INT,
	[HighSchoolAttended] VARCHAR(64),
	[UndergraduateSchoolAttended] VARCHAR(64)

)
