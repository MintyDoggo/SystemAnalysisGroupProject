CREATE PROCEDURE [dbo].[spStudent_UpdateById]
	@inId INT,
	@inStudent udtStudent READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE tblStudent
	SET StaffId = [StaffId], StudentIdNumber = [StudentIdNumber], FirstName = [FirstName], LastName = [LastName], Birthday = [Birthday], Address = [Address], Major = [Major], FirstYearEnrolled = [FirstYearEnrolled], HighSchoolAttended = [HighSchoolAttended], UndergraduateSchoolAttended = [UndergraduateSchoolAttended]
	WHERE Id = @inId;

END
RETURN 0
