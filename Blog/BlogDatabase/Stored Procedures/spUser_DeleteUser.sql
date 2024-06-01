CREATE PROC spUser_DeleteUser
	@UserId int
AS
BEGIN
    -- DELETE ALL POSTS OF USER
    DELETE FROM [dbo].[Post] WHERE [UserId] = @UserId;

    -- DELETE USER
    DELETE FROM [dbo].[User] WHERE [Id] = @UserId;
END