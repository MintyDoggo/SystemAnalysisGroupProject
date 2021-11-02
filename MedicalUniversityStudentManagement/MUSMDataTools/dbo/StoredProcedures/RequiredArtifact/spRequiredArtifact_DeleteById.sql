CREATE PROCEDURE [dbo].[spRequiredArtifact_DeleteById]
	@inId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	IF EXISTS (SELECT * FROM [tblRequiredArtifact] WHERE Id = @inId)
	BEGIN;
		DELETE FROM [tblRequiredArtifact] WHERE Id = @inId;
	END
	ELSE
	BEGIN;
	   -- We tried to delete a row that didn't exist
	   THROW 50000, 'Tried deleting row that did not exist', 1;
	END

END
RETURN 0
