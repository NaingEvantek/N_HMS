CREATE TABLE [dbo].[Floor_Info] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (255) NULL,
    [Modified_Date] DATETIME       NULL,
    CONSTRAINT [PK_Floor_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

