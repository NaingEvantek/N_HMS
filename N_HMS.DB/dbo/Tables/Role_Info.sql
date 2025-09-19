CREATE TABLE [dbo].[Role_Info] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Role_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

