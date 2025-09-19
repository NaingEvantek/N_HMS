CREATE TABLE [dbo].[Payment_Status_Info] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Status] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Payment_Status_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

