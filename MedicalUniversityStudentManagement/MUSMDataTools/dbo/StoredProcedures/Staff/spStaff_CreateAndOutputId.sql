CREATE PROCEDURE [dbo].[spStaff_CreateAndOutputId]
	@inStaff udtStaff READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	-- Create a Login for this new Staff
	DECLARE @newLogin AS udtLogin;
	INSERT INTO @newLogin VALUES ('username', 'TEMPORARY_PASSWORD', 3);
	EXEC spLogin_CreateAndOutputId @inLogin = @newLogin, @outId = @outId OUTPUT;


	-- Create the Staff data for this new Login
	INSERT INTO tblStaff(Id, FirstName, LastName)
	SELECT
		@outId,

		FirstName,
		LastName
		FROM
		@inStaff;

END
RETURN 0
