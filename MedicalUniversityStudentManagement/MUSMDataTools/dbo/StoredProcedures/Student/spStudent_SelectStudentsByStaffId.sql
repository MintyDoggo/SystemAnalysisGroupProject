CREATE PROCEDURE [dbo].[spStudent_SelectStudentsByStaffId]
	@inStaffId INT
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	SELECT * FROM [tblStudent] WHERE StaffId = @inStaffId;
END
RETURN 0
