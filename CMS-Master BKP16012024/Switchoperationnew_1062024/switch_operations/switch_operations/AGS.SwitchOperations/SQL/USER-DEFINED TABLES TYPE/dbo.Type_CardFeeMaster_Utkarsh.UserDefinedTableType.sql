USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[Type_CardFeeMaster_Utkarsh]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[Type_CardFeeMaster_Utkarsh] AS TABLE(
	[Bin] [varchar](100) NULL,
	[FeeAmount] [varchar](100) NULL,
	[FeeCategory] [varchar](10) NULL
)
GO
