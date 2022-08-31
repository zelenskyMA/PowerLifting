CREATE TABLE [dbo].[TrainingDays]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [TrainingPlanId] INT NOT NULL,
    [ActivityDate] DATE NOT NULL, 
    [LiftCounterSum] INT NOT NULL DEFAULT 0, 
    [WeightLoadSum] INT NOT NULL DEFAULT 0,
    [IntensitySum] INT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_TrainingDay] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TrainingPlanId]) REFERENCES [dbo].[TrainingPlans] ([Id])
)
