CREATE TABLE [dbo].[Percentages]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL,
    [MinValue] INT NOT NULL DEFAULT 0, 
    [MaxValue] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Percentages] PRIMARY KEY ([Id])
)
