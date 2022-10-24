CREATE TABLE [plan].[TemplateExerciseSettings]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [TemplateExerciseId] INT NOT NULL,
    [PercentageId] INT NOT NULL,
    [WeightPercentage] INT NOT NULL,
    [Iterations] INT NOT NULL DEFAULT 0,
    [ExercisePart1] INT NOT NULL DEFAULT 0,
    [ExercisePart2] INT NOT NULL DEFAULT 0,
    [ExercisePart3] INT NOT NULL DEFAULT 0,    CONSTRAINT [PK_TemplateExerciseSettings] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TemplateExerciseId]) REFERENCES [plan].[TemplateExercises] ([Id])
)
