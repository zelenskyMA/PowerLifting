CREATE TABLE [dbo].[UserAchivements]
(
    [UserId] INT NOT NULL, 
    [AchievementId] INT NOT NULL, 
    [Result] INT NOT NULL, 
    [CreationDate] DATE NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [PK_UserAchivements] PRIMARY KEY ([AchievementId], [UserId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([AchievementId]) REFERENCES [dbo].[Dictionaries] ([Id]) 
)
