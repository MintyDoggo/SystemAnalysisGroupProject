CREATE PROCEDURE [dbo].[spStaff_SelectByLoginId]
	@inLoginId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblStaff] WHERE Id = @inLoginId;
END
RETURN 0
