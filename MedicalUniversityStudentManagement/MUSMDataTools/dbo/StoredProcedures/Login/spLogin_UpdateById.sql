CREATE PROCEDURE [dbo].[spLogin_UpdateById]
	@inId INT,
	@inLogin udtLogin READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE [tblLogin]
	SET
	tblLogin.Username = inLogin.Username,
	tblLogin.Password = inLogin.Password,
	tblLogin.UserType = inLogin.UserType
	FROM @inLogin inLogin
	WHERE tblLogin.Id = @inId;

END
RETURN 0
