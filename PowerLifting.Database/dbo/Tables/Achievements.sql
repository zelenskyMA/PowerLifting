CREATE TABLE [dbo].[Achievements]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    CONSTRAINT [PK_Achievements] PRIMARY KEY ([Id]),
)
