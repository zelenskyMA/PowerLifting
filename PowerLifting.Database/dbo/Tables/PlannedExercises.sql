CREATE TABLE [dbo].[PlannedExercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [TrainingDayId] INT NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Order] INT NOT NULL, 
    [LiftCounter] INT NOT NULL DEFAULT 0,
    [WeightLoad] INT NOT NULL DEFAULT 0,
    [Intensity] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_PlannedExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TrainingDayId]) REFERENCES [dbo].[TrainingDays] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [dbo].[Exercises] ([Id])
)
