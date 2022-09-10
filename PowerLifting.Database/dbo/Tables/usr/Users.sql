CREATE TABLE [usr].[Users] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Email] NVARCHAR (250) NOT NULL,
    [Password] NVARCHAR (250) NOT NULL,
    [Salt] NVARCHAR (50)  NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

