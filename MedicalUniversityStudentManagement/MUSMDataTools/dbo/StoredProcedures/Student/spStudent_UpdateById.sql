CREATE PROCEDURE [dbo].[spStudent_UpdateById]
	@inId INT,
	@inStudent udtStudent READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE [tblStudent]
	SET
	tblStudent.StaffId = inStudent.StaffId,
	tblStudent.StudentIdNumber = inStudent.StudentIdNumber,
	tblStudent.FirstName = inStudent.FirstName,
	tblStudent.LastName = inStudent.LastName,
	tblStudent.Birthday = inStudent.Birthday,
	tblStudent.Address = inStudent.Address,
	tblStudent.Major = inStudent.Major,
	tblStudent.FirstYearEnrolled = inStudent.FirstYearEnrolled,
	tblStudent.HighSchoolAttended = inStudent.HighSchoolAttended,
	tblStudent.UndergraduateSchoolAttended = inStudent.UndergraduateSchoolAttended
	FROM @inStudent inStudent
	WHERE tblStudent.Id = @inId;

END
RETURN 0
