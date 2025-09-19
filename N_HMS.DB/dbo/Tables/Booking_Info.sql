CREATE TABLE [dbo].[Booking_Info] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [Room_Id]           INT             NULL,
    [CheckIn_Date]      DATETIME        NULL,
    [CheckOut_Date]     DATETIME        NULL,
    [Payment_Status_Id] INT             NULL,
    [Num_Guests]        INT             NULL,
    [No_Of_Days]        INT             NULL,
    [Total_Amount]      DECIMAL (18, 2) NULL,
    [Paid_Amount]       DECIMAL (18, 2) NULL,
    [Net_Amount]        DECIMAL (18, 2) NULL,
    [Created_Date]      DATETIME        NULL,
    [Modified_Date]     DATETIME        NULL,
    CONSTRAINT [PK_Booking_Info] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Booking_Info_Payment_Status_Info] FOREIGN KEY ([Payment_Status_Id]) REFERENCES [dbo].[Payment_Status_Info] ([Id]),
    CONSTRAINT [FK_Booking_Info_Room_Info] FOREIGN KEY ([Room_Id]) REFERENCES [dbo].[Room_Info] ([Id])
);

