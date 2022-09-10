CREATE TABLE [usr].[UserTrainingGroups]
(
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    CONSTRAINT [PK_UserTrainingGroups] PRIMARY KEY ([GroupId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([GroupId]) REFERENCES [usr].[TrainingGroups] ([Id])
)
