CREATE TABLE [dbo].[ExercisePercentages]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PercentageId] INT NOT NULL, 
    [DailyExerciseId] INT NOT NULL, 
    [ExerciseId] INT NOT NULL, 
    CONSTRAINT [PK_ExercisePercentages] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PercentageId]) REFERENCES [dbo].[Percentages] ([Id]),
    FOREIGN KEY ([DailyExerciseId]) REFERENCES [dbo].[Exercises] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [dbo].[ExerciseValues] ([Id])
)
