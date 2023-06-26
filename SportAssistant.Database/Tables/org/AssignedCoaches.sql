CREATE TABLE [org].[AssignedCoaches]
(
    [ManagerId] INT NOT NULL, 
    [CoachId] INT NOT NULL, 
    CONSTRAINT [PK_AssignedCoaches] PRIMARY KEY ([ManagerId], [CoachId]),
    FOREIGN KEY ([ManagerId]) REFERENCES [org].[Managers] ([UserId]),
    FOREIGN KEY ([CoachId]) REFERENCES [usr].[Users] ([Id]),
)
