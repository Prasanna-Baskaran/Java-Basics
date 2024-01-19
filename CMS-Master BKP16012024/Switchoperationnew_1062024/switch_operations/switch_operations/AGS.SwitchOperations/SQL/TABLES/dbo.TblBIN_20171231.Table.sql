USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblBIN_20171231]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBIN_20171231](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CardPrefix] [varchar](12) NULL,
	[Default] [bit] NULL,
	[InstitutionID] [int] NULL,
	[AccountType] [varchar](10) NULL,
	[CardType] [varchar](200) NULL,
	[CardProgram] [varchar](50) NULL,
	[FormStatusID] [int] NULL,
	[MakerID] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[CheckerID] [bigint] NULL,
	[CheckedDate] [datetime] NULL,
	[BinDesc] [varchar](100) NULL,
	[Currency] [varchar](50) NULL,
	[Switch_SchemeCode] [varchar](10) NULL,
	[Switch_Bank] [int] NULL,
	[Switch_AccountType] [varchar](2) NULL,
	[Switch_CardType] [varchar](25) NULL,
	[Switch_ResidentCustomer] [varchar](3) NULL,
	[SystemID] [varchar](200) NULL,
	[BankID] [varchar](200) NULL,
	[PREFormat] [varchar](8000) NULL,
	[IsMagstrip] [smallint] NULL,
	[ATMlimit] [bigint] NULL,
	[POSlimit] [bigint] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
