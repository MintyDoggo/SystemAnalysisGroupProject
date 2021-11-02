CREATE PROCEDURE [dbo].[spLogin_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblLogin];
END
RETURN 0
