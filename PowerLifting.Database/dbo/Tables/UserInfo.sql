CREATE TABLE [dbo].[UserInfo]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [UserId] INT NOT NULL,
    [FirstName] NVARCHAR (50) NULL, 
    [Surname] NVARCHAR(50) NULL, 
    [Patronimic] NVARCHAR(50) NULL, 
    [Weight] INT NULL, 
    [Height] INT NULL, 
    [Age] INT NULL, 
    [CoachId] INT NULL,
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([CoachId]) REFERENCES [dbo].[Users] ([Id]), 
    CONSTRAINT [PK_UserInfo] PRIMARY KEY ([Id])
)
