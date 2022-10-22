CREATE TABLE [plan].[TemplateDays]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [TemplatePlanId] INT NOT NULL,
    [DayNumber] INT NOT NULL, 
    CONSTRAINT [PK_TemplateDays] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TemplatePlanId]) REFERENCES [plan].[TemplatePlans] ([Id])
)
