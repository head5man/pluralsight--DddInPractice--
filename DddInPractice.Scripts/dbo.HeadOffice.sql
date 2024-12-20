CREATE TABLE [dbo].[HeadOffice]
(
	[HeadOfficeID] INT NOT NULL PRIMARY KEY, 
    [Balance] DECIMAL(18, 2) NULL, 
    [OneCentCount] INT NOT NULL, 
    [TenCentCount] INT NOT NULL, 
    [QuarterCount] INT NOT NULL, 
    [OneDollarCount] INT NOT NULL, 
    [FiveDollarCount] INT NOT NULL, 
    [TwentyDollarCount] INT NOT NULL
)
