CREATE TABLE [dbo].[LiftingIntensity]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [Value] INT NOT NULL DEFAULT (0), 
    [PercentageId] INT NOT NULL, 
    [PlanDayId] INT NOT NULL, 
    CONSTRAINT [PK_LiftingIntensity] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PercentageId]) REFERENCES [dbo].[Percentages] ([Id]),
    FOREIGN KEY ([PlanDayId]) REFERENCES [dbo].[PlanDays] ([Id])
)
