CREATE PROCEDURE [dbo].[spStudent_SelectByLoginId]
	@inLoginId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblStudent] WHERE Id = @inLoginId;
END
RETURN 0
