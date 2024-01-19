USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[BulkReissueTblType]    Script Date: 08-06-2018 16:58:16 ******/
CREATE TYPE [dbo].[BulkReissueTblType] AS TABLE(
	[Extra1] [varchar](800) NULL,
	[OldCardNumber] [varchar](200) NULL,
	[NewBINPrefix] [varchar](200) NULL,
	[HoldRSPCode] [varchar](200) NULL,
	[Remark] [varchar](800) NULL,
	[Reason] [varchar](800) NULL,
	[Extra2] [varchar](800) NULL,
	[Extra3] [varchar](800) NULL
)
GO
