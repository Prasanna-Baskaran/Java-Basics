USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[PANIssuerType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[PANIssuerType] AS TABLE(
	[EncPAN] [varchar](800) NULL,
	[IssuerNo] [varchar](200) NULL
)
GO
