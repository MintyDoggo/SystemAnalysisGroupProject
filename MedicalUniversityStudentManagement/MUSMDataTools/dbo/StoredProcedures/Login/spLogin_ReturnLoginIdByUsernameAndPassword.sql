CREATE PROCEDURE [dbo].[spLogin_ReturnLoginIdByUsernameAndPassword]
	@inLogin udtLogin READONLY,
	@outId INT OUT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	
	SELECT @outId = Id FROM tblLogin
		WHERE tblLogin.Username = [Username] AND tblLogin.Password = [Password];
END
RETURN 0
