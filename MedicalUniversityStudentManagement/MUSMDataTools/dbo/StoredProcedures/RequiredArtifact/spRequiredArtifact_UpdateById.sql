CREATE PROCEDURE [dbo].[spRequiredArtifact_UpdateById]
	@inId INT,
	@inRequiredArtifact udtRequiredArtifact READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE [tblRequiredArtifact]
	SET
	tblRequiredArtifact.[Name] = inRequiredArtifact.[Name]
	FROM @inRequiredArtifact inRequiredArtifact
	WHERE tblRequiredArtifact.Id = @inId;

END
RETURN 0
