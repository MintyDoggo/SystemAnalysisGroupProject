CREATE PROCEDURE [dbo].[spRequiredArtifact_CreateAndOutputId]
	@inRequiredArtifact udtRequiredArtifact READONLY,
	@outId INT OUT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected


	INSERT INTO tblRequiredArtifact([Name])
	SELECT [Name] FROM @inRequiredArtifact;

	SELECT @outId = SCOPE_IDENTITY();

END
RETURN 0
