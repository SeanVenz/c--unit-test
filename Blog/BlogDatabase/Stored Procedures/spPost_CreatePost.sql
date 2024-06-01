CREATE PROC spPost_CreatePost
	@UserId int,
	@Title nvarchar(100),
    @Content text,
    @Categories varchar(40),
    @Status nvarchar(15),
    @DateTimeCreated datetime,
    @DateTimeUpdated datetime
AS
BEGIN
    -- INSERT POST
    INSERT INTO [dbo].[Post] ([UserId], [Title], [Content], [Status], [DateTimeCreated], [DateTimeUpdated]) 
    VALUES (@UserId, @Title, @Content, @Status, @DateTimeCreated, @DateTimeUpdated);

    DECLARE @PostId INT = (SELECT SCOPE_IDENTITY());

    -- INSERT CATEGORIES IN [dbo].[PostCategory]
    DECLARE @Counter INT = 0;
    DECLARE @TotalNumberOfCategories INT = (SELECT COUNT(value) AS CategoryId FROM STRING_SPLIT(@Categories, ','));

    WHILE @Counter <= @TotalNumberOfCategories
    BEGIN
        DECLARE @CategoryId INT = (
            SELECT value AS CategoryId 
            FROM STRING_SPLIT(@Categories, ',') 
            ORDER BY CategoryId DESC OFFSET @TotalNumberOfCategories - @Counter ROWS FETCH NEXT 1 ROWS ONLY
        );

        IF @CategoryId != ''
        BEGIN
            INSERT INTO [dbo].[PostCategory] ([PostId], [CategoryId]) 
            VALUES (@PostId, @CategoryId);
        END

        SET @Counter = @Counter + 1;
    END

    -- RETURN ID OF NEWLY CREATED POST
	SELECT @PostId;
END