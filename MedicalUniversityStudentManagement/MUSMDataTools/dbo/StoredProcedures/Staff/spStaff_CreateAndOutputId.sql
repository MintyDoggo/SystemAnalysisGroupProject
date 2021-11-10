CREATE PROCEDURE [dbo].[spStaff_CreateAndOutputId]
	@inUsername VARCHAR(32),
	@inPassword VARCHAR(256),
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	-- Create a Login for this new Staff
	DECLARE @newLogin AS udtLogin;
	INSERT INTO @newLogin VALUES (@inUsername, @inPassword, 2);
	EXEC spLogin_CreateAndOutputId @inLogin = @newLogin, @outId = @outId OUTPUT;


	-- Create the Staff data for this new Login
	INSERT INTO tblStaff(Id)
	SELECT @outId;

END
RETURN 0
