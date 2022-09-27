CREATE TABLE [trn].[TrainingRequests]
(
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [UserId] INT NOT NULL,
    [CoachId] INT NOT NULL,
    [CreationDate] DATE NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [PK_TrainingRequests] PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([CoachId]) REFERENCES [usr].[Users] ([Id])
)
