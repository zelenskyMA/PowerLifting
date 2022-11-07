CREATE TABLE [trn].[TrainingGroups]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [Name] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(500) NULL,
    [CoachId] INT NULL,
    CONSTRAINT [PK_TrainingGroups] PRIMARY KEY ([Id]),
    FOREIGN KEY ([CoachId]) REFERENCES [usr].[Users] ([Id])
)
