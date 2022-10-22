CREATE TABLE [plan].[TemplatePlans]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [TemplateSetId] INT NOT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_TemplatePlans] PRIMARY KEY ([Id]),
    FOREIGN KEY ([TemplateSetId]) REFERENCES [plan].[TemplateSets] ([Id])
)
