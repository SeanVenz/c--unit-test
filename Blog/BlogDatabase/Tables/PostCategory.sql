CREATE TABLE [dbo].[PostCategory] 
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PostId] INT NOT NULL,
    [CategoryId] INT NOT NULL,
	CONSTRAINT FK_PostCategoryPost FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post](Id) ON DELETE CASCADE,
    CONSTRAINT FK_PostCategoryCategory FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category](Id) ON DELETE CASCADE
)
GO
