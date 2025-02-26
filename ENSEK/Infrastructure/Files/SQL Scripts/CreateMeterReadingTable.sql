CREATE TABLE [dbo].[MeterReading] (
    [MeterReadingId]       INT      NOT NULL IDENTITY,
    [AccountId]            INT      NULL,
    [MeterReadingDateTime] DATETIME NULL,
    [MeterReadValue]       INT      NULL,
    PRIMARY KEY CLUSTERED ([MeterReadingId] ASC),
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([AccountId])
);