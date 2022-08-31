CREATE TABLE [dbo].[Exercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [TrainingDayId] INT NOT NULL,
    [ExerciseTypeId] INT NOT NULL,
    [Order] INT NOT NULL, 
    [LiftCounter] INT NOT NULL DEFAULT 0,
    [WeightLoad] INT NOT NULL DEFAULT 0,
    [Intensity] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DailyExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TrainingDayId]) REFERENCES [dbo].[TrainingDays] ([Id]),
    FOREIGN KEY ([ExerciseTypeId]) REFERENCES [dbo].[ExerciseTypes] ([Id])
)
