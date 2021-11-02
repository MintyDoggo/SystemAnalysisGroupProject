CREATE PROCEDURE [dbo].[spLogin_CreateAndOutputId]
	@inLogin udtLogin READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	INSERT INTO tblLogin(Username, Password, UserType)
	SELECT Username, Password, UserType FROM @inLogin;
	
	SELECT @outId = SCOPE_IDENTITY();		-- SCOPE_IDENTITY() returns the most recent modified Id within the scope of this procedure (last identity created in the same session and the same scope)

END
RETURN 0
