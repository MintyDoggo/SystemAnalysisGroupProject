CREATE PROCEDURE [dbo].[spStudent_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblStudent];
END
RETURN 0
