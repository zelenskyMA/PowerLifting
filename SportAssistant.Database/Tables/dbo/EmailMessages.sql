CREATE TABLE [dbo].[EmailMessages]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL, 
    [Body] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_EmailMessages] PRIMARY KEY ([Id])
)
