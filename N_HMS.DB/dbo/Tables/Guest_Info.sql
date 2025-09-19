CREATE TABLE [dbo].[Guest_Info] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (500) NULL,
    [Passport_No]  NVARCHAR (255) NULL,
    [Gender_Id]    INT            NULL,
    [Created_Date] DATETIME       NULL,
    CONSTRAINT [PK_Guest_Info] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Guest_Info_Guest_Info] FOREIGN KEY ([Gender_Id]) REFERENCES [dbo].[Guest_Info] ([Id])
);

