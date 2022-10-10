CREATE TABLE [dbo].[PlanExerciseSettings]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [PlanExerciseId] INT NOT NULL, 
    [PercentageId] INT NOT NULL, 
    [Weight] INT NOT NULL DEFAULT 0, 
    [Iterations] INT NOT NULL DEFAULT 0,
    [ExercisePart1] INT NOT NULL DEFAULT 0,
    [ExercisePart2] INT NOT NULL DEFAULT 0,
    [ExercisePart3] INT NOT NULL DEFAULT 0,
    [Completed] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_PlanExerciseSettings] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PercentageId]) REFERENCES [dbo].[Percentages] ([Id]),
    FOREIGN KEY ([PlanExerciseId]) REFERENCES [dbo].[PlanExercises] ([Id])
)
