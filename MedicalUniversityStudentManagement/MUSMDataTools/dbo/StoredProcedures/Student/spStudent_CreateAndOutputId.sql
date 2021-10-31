CREATE PROCEDURE [dbo].[spStudent_CreateAndOutputId]
	@inStudent udtStudent READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	--IF EXISTS (SELECT * FROM [tblStaff] WHERE Id = )
	--BEGIN;
		INSERT INTO tblStudent([StaffId], [StudentIdNumber], [FirstName], [LastName], [Birthday], [Address], [Major], [FirstYearEnrolled], [HighSchoolAttended], [UndergraduateSchoolAttended]) SELECT [StaffId], [StudentIdNumber], [FirstName], [LastName], [Birthday], [Address], [Major], [FirstYearEnrolled], [HighSchoolAttended], [UndergraduateSchoolAttended] FROM @inStudent;
		SELECT @outId = SCOPE_IDENTITY();		-- SCOPE_IDENTITY() returns the most recent modified Id within the scope of this procedure (last identity created in the same session and the same scope)
	--END
	--ELSE
	--BEGIN;
	--   THROW 50000, 'The given StaffId does not exist', 1;
	--END

END
RETURN 0
