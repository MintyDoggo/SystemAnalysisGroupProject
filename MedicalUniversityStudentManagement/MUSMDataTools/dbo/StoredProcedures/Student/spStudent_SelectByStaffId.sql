CREATE PROCEDURE [dbo].[spStudent_SelectByStaffId]
	@inStaffId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblStudent] WHERE StaffId = @inStaffId;
END
RETURN 0
