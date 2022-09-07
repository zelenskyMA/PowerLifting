CREATE TABLE [dbo].[PlanExercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlanDayId] INT NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Order] INT NOT NULL, 
    [LiftCounter] INT NOT NULL DEFAULT 0,
    [WeightLoad] INT NOT NULL DEFAULT 0,
    [Intensity] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_PlanExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PlanDayId]) REFERENCES [dbo].[PlanDays] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [dbo].[Exercises] ([Id])
)
