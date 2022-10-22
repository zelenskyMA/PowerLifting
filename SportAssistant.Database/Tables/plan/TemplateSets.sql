CREATE TABLE [plan].[TemplateSets]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [CoachId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_TemplateSets] PRIMARY KEY ([Id]),
    FOREIGN KEY ([CoachId]) REFERENCES [usr].[Users] ([Id])
)
