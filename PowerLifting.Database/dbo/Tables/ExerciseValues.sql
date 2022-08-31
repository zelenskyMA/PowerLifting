CREATE TABLE [dbo].[ExerciseValues]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [Weight] INT NOT NULL DEFAULT 0, 
    [Iterations] INT NOT NULL DEFAULT 0,
    [ExercisePart1] INT NOT NULL DEFAULT 0,
    [ExercisePart2] INT NOT NULL DEFAULT 0,
    [ExercisePart3] INT NOT NULL DEFAULT 0, 
    [Comments] NVARCHAR(250) NULL, 
    CONSTRAINT [PK_ExerciseValues] PRIMARY KEY ([Id])
)
