CREATE TABLE [org].[Managers]
(
    [UserId] INT NOT NULL,
    [AllowedCoaches] INT NULL, 
    [OrgId] INT NULL,
    CONSTRAINT [PK_Managers] PRIMARY KEY ([UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([OrgId]) REFERENCES [org].[Organizations] ([Id])
)
