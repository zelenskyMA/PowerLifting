CREATE TABLE [dbo].[ExercisePercentages]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PercentageId] INT NOT NULL, 
    [PlannedExerciseId] INT NOT NULL, 
    [ExerciseSettingsId] INT NOT NULL, 
    CONSTRAINT [PK_ExercisePercentages] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PercentageId]) REFERENCES [dbo].[Percentages] ([Id]),
    FOREIGN KEY ([PlannedExerciseId]) REFERENCES [dbo].[PlannedExercises] ([Id]),
    FOREIGN KEY ([ExerciseSettingsId]) REFERENCES [dbo].[ExerciseSettings] ([Id])
)
