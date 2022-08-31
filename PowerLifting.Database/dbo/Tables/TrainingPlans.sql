CREATE TABLE [dbo].[TrainingPlans] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [StartDate] DATE NOT NULL, 
    [Comments] NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

