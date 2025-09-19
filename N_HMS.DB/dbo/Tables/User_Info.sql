CREATE TABLE [dbo].[User_Info] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [User_Name]     NVARCHAR (255) NULL,
    [Password_Hash] NVARCHAR (500) NULL,
    [Role_Id]       INT            NULL,
    [IsActive]      BIT            NULL,
    [Created_Date]  DATETIME       NULL,
    CONSTRAINT [PK_User_Info] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Info_Role_Info] FOREIGN KEY ([Role_Id]) REFERENCES [dbo].[Role_Info] ([Id])
);

