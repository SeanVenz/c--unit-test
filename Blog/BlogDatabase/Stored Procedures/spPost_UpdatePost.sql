CREATE PROC spPost_UpdatePost
	@PostId int,
	@Title nvarchar(100),
    @Content text,
    @Categories varchar(40),
    @Status nvarchar(15),
    @DateTimeUpdated datetime
AS
BEGIN
    -- UPDATE POST
    UPDATE [dbo].[Post] 
    SET 
        Title = @Title, 
        Content = @Content, 
        Status = @Status, 
        DateTimeUpdated = @DateTimeUpdated
    WHERE Id = @PostId

    -- DELETE EXISTING POST CATEGORIES IN [dbo].[PostCategory]
    DELETE FROM [dbo].[PostCategory] WHERE [PostId] = @PostId;

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
END