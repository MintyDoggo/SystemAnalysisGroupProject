CREATE TABLE [dbo].[tblStudent]
(
	-- Maybe make some of these NULL as part of the extra information the Students need to fill out
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
