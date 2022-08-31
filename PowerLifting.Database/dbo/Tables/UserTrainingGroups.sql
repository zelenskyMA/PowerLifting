CREATE TABLE [dbo].[UserTrainingGroups]
(
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    CONSTRAINT [PK_UserTrainingGroups] PRIMARY KEY ([GroupId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].[TrainingGroups] ([Id])
)
