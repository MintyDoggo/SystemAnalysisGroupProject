CREATE PROCEDURE [dbo].[spStaff_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		--Don't give how many rows affected

	SELECT * FROM [tblStaff];
END
RETURN 0