CREATE PROCEDURE [dbo].[spArtifact_SelectByStudentId]
	@inStudentId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblArtifact] WHERE StudentId = @inStudentId;
END
RETURN 0
