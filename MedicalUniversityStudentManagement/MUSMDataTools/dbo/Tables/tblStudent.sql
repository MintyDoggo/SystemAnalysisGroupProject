CREATE TABLE [dbo].[tblStudent]
(
	[Id] INT NOT NULL FOREIGN KEY REFERENCES tblLogin(Id) ON DELETE CASCADE,
	[StaffId] INT NOT NULL FOREIGN KEY REFERENCES tblLogin(Id) ON DELETE/* CASCADE*/ NO ACTION, -- the associated staff for this student
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
