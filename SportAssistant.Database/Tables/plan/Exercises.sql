CREATE TABLE [plan].[Exercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [ExerciseTypeId] INT NOT NULL,
    [ExerciseSubTypeId] INT NOT NULL,
    [Name] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [UserId] INT NULL,
    [Closed] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Exercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([ExerciseTypeId]) REFERENCES [dbo].[Dictionaries] ([Id]),
    FOREIGN KEY ([ExerciseSubTypeId]) REFERENCES [dbo].[Dictionaries] ([Id])
)
