CREATE PROCEDURE [dbo].[spArtifact_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblArtifact];
END
RETURN 0
