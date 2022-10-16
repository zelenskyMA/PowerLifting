CREATE TABLE [plan].[Plans] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [StartDate] DATE NOT NULL, 
    [Comments] NVARCHAR (250) NULL,
    CONSTRAINT [PK_Plans] PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id])
);

