CREATE TABLE [dbo].[Dictionaries]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [TypeId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    CONSTRAINT [PK_Dictionaries] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TypeId]) REFERENCES [dbo].[DictionaryTypes] ([Id]),
)
