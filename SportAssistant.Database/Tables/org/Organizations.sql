CREATE TABLE [org].[Organizations]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [OwnerId] INT NULL, 
    [MaxCoaches] INT NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY ([Id]),
    FOREIGN KEY ([OwnerId]) REFERENCES [usr].[Users] ([Id])
)
