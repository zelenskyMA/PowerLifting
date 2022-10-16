CREATE TABLE [trn].[TrainingGroupUsers]
(
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    CONSTRAINT [PK_TrainingGroupUsers] PRIMARY KEY ([GroupId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([GroupId]) REFERENCES [trn].[TrainingGroups] ([Id])
)
