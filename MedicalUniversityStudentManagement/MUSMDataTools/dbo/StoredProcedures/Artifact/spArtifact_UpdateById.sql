CREATE PROCEDURE [dbo].[spArtifact_UpdateById]
	@inId INT,
	@inArtifact udtArtifact READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE tblArtifact
	SET RequiredArtifactId = [RequiredArtifactId], StudentId = [StudentId], DocumentReference = [DocumentReference], CheckedOff = [CheckedOff]
	WHERE Id = @inId;

END
RETURN 0
