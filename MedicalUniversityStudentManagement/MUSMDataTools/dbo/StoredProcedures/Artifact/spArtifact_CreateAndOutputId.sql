CREATE PROCEDURE [dbo].[spArtifact_CreateAndOutputId]
	@inArtifact udtArtifact READONLY,
	@outId INT OUT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	INSERT INTO tblArtifact(RequiredArtifactId, StudentId, Document, CheckedOff)
	SELECT RequiredArtifactId, StudentId, Document, CheckedOff FROM @inArtifact;
	
	SELECT @outId = SCOPE_IDENTITY();

END
RETURN 0
