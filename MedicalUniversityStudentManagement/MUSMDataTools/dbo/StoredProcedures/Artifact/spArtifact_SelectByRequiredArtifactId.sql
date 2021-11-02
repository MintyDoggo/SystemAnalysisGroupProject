CREATE PROCEDURE [dbo].[spArtifact_SelectByRequiredArtifactId]
	@inRequiredArtifactId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblArtifact] WHERE RequiredArtifactId = @inRequiredArtifactId;
END
RETURN 0
