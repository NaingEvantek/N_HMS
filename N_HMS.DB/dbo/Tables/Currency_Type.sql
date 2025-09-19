CREATE TABLE [dbo].[Currency_Type] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Code]        NVARCHAR (10) NULL,
    [Symbol]      NVARCHAR (10) NULL,
    [Modify_Date] DATETIME      NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([Id] ASC)
);

