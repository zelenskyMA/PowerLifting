CREATE TABLE [usr].[UserRoles]
(
    [UserId] INT NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([RoleId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([RoleId]) REFERENCES [usr].[Roles] ([Id])
)
