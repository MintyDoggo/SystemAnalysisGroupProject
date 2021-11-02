CREATE PROCEDURE [dbo].[spStaff_UpdateById]
	@inId INT,
	@inStaff udtStaff READONLY
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	UPDATE [tblStaff]
	SET
	tblStaff.FirstName = inStaff.FirstName,
	tblStaff.LastName = inStaff.LastName
	FROM @inStaff inStaff
	WHERE tblStaff.Id = @inId;

END
RETURN 0
