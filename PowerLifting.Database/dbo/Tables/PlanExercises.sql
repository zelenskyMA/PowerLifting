CREATE TABLE [dbo].[PlanExercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlanDayId] INT NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Order] INT NOT NULL, 
    CONSTRAINT [PK_PlanExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PlanDayId]) REFERENCES [dbo].[PlanDays] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [dbo].[Exercises] ([Id])
)
