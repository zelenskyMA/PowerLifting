CREATE TABLE [usr].[UserInfo]
(
    [UserId] INT NOT NULL,
    [FirstName] NVARCHAR (50) NULL, 
    [Surname] NVARCHAR(50) NULL, 
    [Patronimic] NVARCHAR(50) NULL, 
    [Weight] INT NULL, 
    [Height] INT NULL, 
    [Age] INT NULL, 
    [CoachId] INT NULL,
    [CoachOnly] BIT NOT NULL DEFAULT 0,
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([CoachId]) REFERENCES [usr].[Users] ([Id]), 
    CONSTRAINT [PK_UserInfo] PRIMARY KEY ([UserId])
)
