CREATE PROCEDURE [dbo].[spAdmin_CreateAndOutputId]
	@inUsername VARCHAR(32),
	@inPassword VARCHAR(256),
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	-- Create a Login for this new Admin
	DECLARE @newLogin AS udtLogin;
	INSERT INTO @newLogin VALUES (@inUsername, @inPassword, 1);
	EXEC spLogin_CreateAndOutputId @inLogin = @newLogin, @outId = @outId OUTPUT;


	-- Create the Admin data for this new Login
	INSERT INTO tblAdmin(Id)
	SELECT @outId;

END
RETURN 0
