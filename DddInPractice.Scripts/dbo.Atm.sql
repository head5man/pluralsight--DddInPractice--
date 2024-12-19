CREATE TABLE [dbo].[Atm]
(
	[AtmID] BIGINT NOT NULL PRIMARY KEY, 
    [MoneyCharged] DECIMAL(18, 2) NOT NULL, 
    [OneCentCount] INT NOT NULL, 
    [TenCentCount] INT NOT NULL, 
    [QuarterCount] INT NOT NULL, 
    [OneDollarCount] INT NOT NULL, 
    [FiveDollarCount] INT NOT NULL, 
    [TwentyDollarCount] INT NOT NULL
)
