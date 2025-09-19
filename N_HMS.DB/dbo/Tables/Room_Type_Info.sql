CREATE TABLE [dbo].[Room_Type_Info] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (255) NULL,
    [Modified_Date] DATETIME       NULL,
    CONSTRAINT [PK_Room_Type_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

