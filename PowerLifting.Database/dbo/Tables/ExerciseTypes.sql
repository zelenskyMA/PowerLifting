CREATE TABLE [dbo].[ExerciseTypes]
(
    [Id] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(150) NULL, 
    CONSTRAINT [PK_ExerciseTypes] PRIMARY KEY ([Id]) 
)
