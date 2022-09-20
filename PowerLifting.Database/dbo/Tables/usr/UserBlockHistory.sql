CREATE TABLE [usr].[UserBlockHistory]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [CreationDate] DATE NOT NULL DEFAULT GETDATE(), 
    [Reason] NVARCHAR (500) NOT NULL,
    [BlockerId] INT NOT NULL,
    FOREIGN KEY ([BlockerId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    CONSTRAINT [PK_UserBlockHistory] PRIMARY KEY ([Id])
)
