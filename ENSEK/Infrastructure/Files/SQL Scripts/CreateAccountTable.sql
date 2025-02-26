CREATE TABLE [dbo].[Account] (
    [AccountId] INT           NOT NULL,
    [FirstName] NVARCHAR (50) NULL,
    [LastName]  NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([AccountId] ASC)
);