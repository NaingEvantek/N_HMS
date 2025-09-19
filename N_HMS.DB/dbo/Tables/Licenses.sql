CREATE TABLE [dbo].[Licenses] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     INT            NOT NULL,
    [LicenseKey] NVARCHAR (100) NOT NULL,
    [StartDate]  DATETIME       NOT NULL,
    [ExpiryDate] DATETIME       NOT NULL,
    [IsActive]   BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

