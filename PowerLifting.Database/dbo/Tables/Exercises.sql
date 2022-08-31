CREATE TABLE [dbo].[Exercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [ExerciseTypeId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    CONSTRAINT [PK_Exercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([ExerciseTypeId]) REFERENCES [dbo].[ExerciseTypes] ([Id])
)
