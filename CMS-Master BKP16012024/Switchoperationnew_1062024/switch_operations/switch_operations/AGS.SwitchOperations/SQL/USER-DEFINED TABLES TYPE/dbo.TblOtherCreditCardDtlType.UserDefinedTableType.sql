USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[TblOtherCreditCardDtlType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[TblOtherCreditCardDtlType] AS TABLE(
	[CardType] [varchar](100) NULL,
	[IssuedBy] [varchar](100) NULL,
	[IssuedDate] [datetime] NULL,
	[Limit] [varchar](100) NULL,
	[Overdue] [varchar](100) NULL,
	[ExpiryDate] [datetime] NULL
)
GO
