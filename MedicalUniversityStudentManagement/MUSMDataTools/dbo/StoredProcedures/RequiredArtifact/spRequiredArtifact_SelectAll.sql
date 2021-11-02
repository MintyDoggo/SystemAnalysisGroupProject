CREATE PROCEDURE [dbo].[spRequiredArtifact_SelectAll]
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblRequiredArtifact];
END
RETURN 0
