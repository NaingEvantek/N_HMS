CREATE TABLE [dbo].[Booking_Guest_Mapping_Info] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [Booking_Id] INT NULL,
    [Guest_Id]   INT NULL,
    CONSTRAINT [PK_Booking_Guest_Mapping_Info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

