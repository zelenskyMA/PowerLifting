CREATE TABLE [plan].[PlanExercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlanDayId] INT NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Order] INT NOT NULL,
    [Comments] NVARCHAR(500) NULL, 
    [ExtPlanData] NVARCHAR(500) NULL, 
    CONSTRAINT [PK_PlanExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PlanDayId]) REFERENCES [plan].[PlanDays] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [plan].[Exercises] ([Id])
)
