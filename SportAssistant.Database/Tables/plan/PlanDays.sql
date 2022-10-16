CREATE TABLE [plan].[PlanDays]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlanId] INT NOT NULL,
    [ActivityDate] DATE NOT NULL, 
    [LiftCounterSum] INT NOT NULL DEFAULT 0, 
    [WeightLoadSum] INT NOT NULL DEFAULT 0,
    [IntensitySum] INT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_PlanDay] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PlanId]) REFERENCES [plan].[Plans] ([Id])
)
