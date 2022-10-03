CREATE TABLE [usr].[UserAchivements]
(
    [UserId] INT NOT NULL, 
    [ExerciseTypeId] INT NOT NULL, 
    [CreationDate] DATE NOT NULL DEFAULT GETDATE(), 
    [Result] INT NOT NULL, 
    CONSTRAINT [PK_UserAchivements] PRIMARY KEY ([ExerciseTypeId], [UserId], [CreationDate]),
    FOREIGN KEY ([UserId]) REFERENCES [usr].[Users] ([Id]),
    FOREIGN KEY ([ExerciseTypeId]) REFERENCES [dbo].[Dictionaries] ([Id]) 
)
