CREATE PROCEDURE [dbo].[spLogin_SelectByUsernameAndPassword]
	@inUsername VARCHAR(32),
	@inPassword VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	SELECT * FROM [tblLogin]
	WHERE tblLogin.Username = @inUsername AND tblLogin.[Password] = @inPassword;
	
END
RETURN 0
