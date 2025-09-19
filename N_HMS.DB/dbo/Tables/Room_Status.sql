CREATE TABLE [dbo].[Room_Status] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Status] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Room_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

