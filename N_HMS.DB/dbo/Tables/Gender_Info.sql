CREATE TABLE [dbo].[Gender_Info] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Gender_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

