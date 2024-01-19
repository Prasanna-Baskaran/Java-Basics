USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CardReissue]    Script Date: 08-06-2018 16:58:16 ******/
CREATE TYPE [dbo].[CardReissue] AS TABLE(
	[CustomerID] [varchar](500) NULL,
	[CARDNO] [varchar](500) NULL,
	[BIN] [varchar](100) NULL,
	[HoldResCode] [varchar](100) NULL,
	[Account] [varchar](500) NULL
)
GO
