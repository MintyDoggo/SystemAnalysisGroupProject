CREATE PROCEDURE [dbo].[spHost_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		--Don't give how many rows affected

	SELECT * FROM [tblStudent];
END
RETURN 0