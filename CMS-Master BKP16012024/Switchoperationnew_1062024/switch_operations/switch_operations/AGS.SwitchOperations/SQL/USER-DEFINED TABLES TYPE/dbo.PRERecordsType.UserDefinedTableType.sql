USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[PRERecordsType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[PRERecordsType] AS TABLE(
	[CustID] [varchar](200) NULL,
	[CardProgram] [varchar](200) NULL,
	[AccountNo] [varchar](200) NULL
)
GO
