CREATE TABLE [plan].[PlanDays]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [PlanId] INT NOT NULL,
    [ActivityDate] DATE NOT NULL,
    CONSTRAINT [PK_PlanDay] PRIMARY KEY ([Id]),
    FOREIGN KEY ([PlanId]) REFERENCES [plan].[Plans] ([Id])
)
