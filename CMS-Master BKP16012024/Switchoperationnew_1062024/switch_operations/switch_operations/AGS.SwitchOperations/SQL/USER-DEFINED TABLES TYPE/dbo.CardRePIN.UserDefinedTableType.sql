USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CardRePIN]    Script Date: 08-06-2018 16:58:16 ******/
CREATE TYPE [dbo].[CardRePIN] AS TABLE(
	[cardNo] [varchar](30) NULL,
	[CIFID] [varchar](max) NULL,
	[AccountNo] [varchar](max) NULL
)
GO
