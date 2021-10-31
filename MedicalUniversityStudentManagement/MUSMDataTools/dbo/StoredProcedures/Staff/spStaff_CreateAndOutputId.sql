CREATE PROCEDURE [dbo].[spStaff_CreateAndOutputId]
	@inStaff udtStaff READONLY,
	@outId INT OUT		-- our out parameter so we can keep track of it in C# for when we want to destroy it
AS
BEGIN
	SET NOCOUNT ON;		-- don't give how many rows affected

	INSERT INTO tblStaff([FirstName], [LastName]) SELECT [FirstName], [LastName] FROM @inStaff;
	SELECT @outId = SCOPE_IDENTITY();		-- SCOPE_IDENTITY() returns the most recent modified Id within the scope of this procedure (last identity created in the same session and the same scope)

END
RETURN 0
