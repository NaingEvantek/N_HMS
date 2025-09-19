CREATE TABLE [dbo].[Room_Info] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [Room_Name]           NVARCHAR (500)  NULL,
    [Floor_Id]            INT             NULL,
    [Room_Type_Id]        INT             NULL,
    [Room_Status_Id]      INT             NULL,
    [Price_Per_Day]       DECIMAL (18, 2) NULL,
    [Currency_Type_Id]    INT             NULL,
    [Room_Capacity_Adult] INT             NULL,
    [Room_Capacity_Child] INT             NULL,
    [Modify_Date]         DATETIME        NULL,
    CONSTRAINT [PK_Room_Info] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Room_Info_Currency_Type] FOREIGN KEY ([Currency_Type_Id]) REFERENCES [dbo].[Currency_Type] ([Id]),
    CONSTRAINT [FK_Room_Info_Floor_Info] FOREIGN KEY ([Floor_Id]) REFERENCES [dbo].[Floor_Info] ([Id]),
    CONSTRAINT [FK_Room_Info_Room_Status] FOREIGN KEY ([Room_Status_Id]) REFERENCES [dbo].[Room_Status] ([Id]),
    CONSTRAINT [FK_Room_Info_Room_Type_Info] FOREIGN KEY ([Room_Type_Id]) REFERENCES [dbo].[Room_Type_Info] ([Id])
);

