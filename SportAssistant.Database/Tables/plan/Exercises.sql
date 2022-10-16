CREATE TABLE [plan].[Exercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [ExerciseTypeId] INT NOT NULL,
    [ExerciseSubTypeId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    [UserId] INT NULL,
    [Closed] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Exercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([ExerciseTypeId]) REFERENCES [dbo].[Dictionaries] ([Id]),
    FOREIGN KEY ([ExerciseSubTypeId]) REFERENCES [dbo].[Dictionaries] ([Id])
)
