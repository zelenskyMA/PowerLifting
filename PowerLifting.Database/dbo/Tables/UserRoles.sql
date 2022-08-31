CREATE TABLE [dbo].[UserRoles]
(
    [UserId] INT NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([RoleId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id])
)
