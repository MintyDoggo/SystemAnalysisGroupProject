CREATE PROCEDURE [dbo].[spStudent_CreateAndOutputId]
	@inStudent udtStudent READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	INSERT INTO tblStudent(StaffId, StudentIdNumber, FirstName, LastName, Birthday, Address, Major, FirstYearEnrolled, HighSchoolAttended, UndergraduateSchoolAttended)
	SELECT StaffId, StudentIdNumber, FirstName, LastName, Birthday, Address, Major, FirstYearEnrolled, HighSchoolAttended, UndergraduateSchoolAttended FROM @inStudent;
	
	-- SCOPE_IDENTITY() returns the most recent modified Id within the scope of this procedure (last identity created in the same session and the same scope)
	SELECT @outId = SCOPE_IDENTITY();

END
RETURN 0
