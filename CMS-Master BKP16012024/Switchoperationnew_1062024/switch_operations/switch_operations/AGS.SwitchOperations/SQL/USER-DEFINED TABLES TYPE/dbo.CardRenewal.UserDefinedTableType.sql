USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CardRenewal]    Script Date: 08-06-2018 16:58:16 ******/
CREATE TYPE [dbo].[CardRenewal] AS TABLE(
	[CardNo] [varchar](50) NULL,
	[Hold_rsp_code] [varchar](50) NULL,
	[BIN] [varchar](20) NULL,
	[Remarks] [varchar](max) NULL,
	[SchemeCode] [varchar](20) NULL,
	[CIF ID] [varchar](50) NULL,
	[Account 1] [varchar](50) NULL,
	[Account 2] [varchar](50) NULL,
	[Account 3] [varchar](50) NULL,
	[Account 4] [varchar](50) NULL,
	[Account 5] [varchar](50) NULL,
	[Reserved1] [varchar](50) NULL,
	[Reserved2] [varchar](50) NULL,
	[Reserved3] [varchar](50) NULL,
	[Reserved4] [varchar](50) NULL,
	[Reserved5] [varchar](50) NULL
)
GO
