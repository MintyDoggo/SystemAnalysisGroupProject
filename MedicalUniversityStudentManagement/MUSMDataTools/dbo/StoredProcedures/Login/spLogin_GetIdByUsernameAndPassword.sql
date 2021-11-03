CREATE PROCEDURE [dbo].[spLogin_GetIdByUsernameAndPassword]
	@outId INT OUT,
	@inUsername VARCHAR(32),
	@inPassword VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	
	SELECT @outId = Id FROM tblLogin
		WHERE tblLogin.Username = @inUsername AND tblLogin.[Password] = @inPassword;
	
END
RETURN 0
