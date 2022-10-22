CREATE TABLE [plan].[TemplateExercises]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [TemplateDayId] INT NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Order] INT NOT NULL,
    [Comments] NVARCHAR(500) NULL, 
    CONSTRAINT [PK_TemplateExercises] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TemplateDayId]) REFERENCES [plan].[TemplateDays] ([Id]),
    FOREIGN KEY ([ExerciseId]) REFERENCES [plan].[Exercises] ([Id])
)
