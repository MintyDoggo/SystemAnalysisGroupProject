CREATE PROCEDURE [dbo].[spArtifact_UpdateById]
	@inId INT,
	@inArtifact udtArtifact READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE [tblArtifact]
	SET
	tblArtifact.RequiredArtifactId = inArtifact.RequiredArtifactId,
	tblArtifact.StudentId = inArtifact.StudentId,
	tblArtifact.DocumentReference = inArtifact.DocumentReference,
	tblArtifact.CheckedOff = inArtifact.CheckedOff
	FROM @inArtifact inArtifact
	WHERE tblStudent.Id = @inId;

END
RETURN 0
