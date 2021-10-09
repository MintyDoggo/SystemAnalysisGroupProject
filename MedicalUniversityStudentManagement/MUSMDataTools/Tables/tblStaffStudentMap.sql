CREATE TABLE [dbo].[tblStaffStudentMap]
(
	[StaffId]	INT FOREIGN KEY REFERENCES tblStaff(Id) NOT NULL,
	[StudentId]	INT FOREIGN KEY REFERENCES tblStudent(Id) NOT NULL
)
