CREATE PROCEDURE [dbo].[spStudent_CreateAndOutputId]
	@inStudent udtStudent READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	-- Create a Login for this new Student
	DECLARE @newLogin AS udtLogin;
	INSERT INTO @newLogin VALUES ('username', 'TEMPORARY_PASSWORD', 4);
	EXEC spLogin_CreateAndOutputId @inLogin = @newLogin, @outId = @outId OUTPUT;


	-- Create the Student data for this new Login
	INSERT INTO tblStudent(Id, StaffId, StudentIdNumber, FirstName, LastName, Birthday, [Address], Major, FirstYearEnrolled, HighSchoolAttended, UndergraduateSchoolAttended)
	SELECT
		@outId,

		StaffId,
		StudentIdNumber,
		FirstName,
		LastName,
		Birthday,
		[Address],
		Major,
		FirstYearEnrolled,
		HighSchoolAttended,
		UndergraduateSchoolAttended
		FROM
		@inStudent;

END
RETURN 0
