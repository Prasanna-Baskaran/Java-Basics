USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblAuthorizedCardLog_20180522_401]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblAuthorizedCardLog_20180522_401](
	[ID] [numeric](28, 0) NOT NULL,
	[Code] [numeric](28, 0) NULL,
	[CIF ID] [varchar](200) NULL,
	[Customer Name] [varchar](200) NULL,
	[Customer Preferred name] [varchar](200) NULL,
	[Card Type and Subtype] [varchar](200) NULL,
	[AC ID] [varchar](200) NULL,
	[AC open date] [varchar](20) NULL,
	[CIF Creation Date] [varchar](20) NULL,
	[Address Line 1] [varchar](200) NULL,
	[Address Line 2] [varchar](200) NULL,
	[Address Line 3] [varchar](200) NULL,
	[City] [varchar](200) NULL,
	[State] [varchar](200) NULL,
	[Pin Code] [varchar](20) NULL,
	[Country code] [varchar](20) NULL,
	[Mothers Maiden Name] [varchar](200) NULL,
	[DOB] [varchar](20) NULL,
	[Country Dial code] [varchar](200) NULL,
	[City Dial code] [varchar](200) NULL,
	[Mobile phone number] [varchar](200) NULL,
	[Email id] [varchar](200) NULL,
	[Scheme code] [varchar](200) NULL,
	[Branch code] [varchar](200) NULL,
	[Entered date] [varchar](20) NULL,
	[Verified Date] [varchar](20) NULL,
	[PAN Number] [varchar](200) NULL,
	[Mode Of Operation] [varchar](200) NULL,
	[Fourth Line Embossing] [varchar](200) NULL,
	[Debit Card Linkage Flag] [varchar](20) NULL,
	[Uploaded On] [datetime] NULL,
	[Rejected] [bit] NULL,
	[Reason] [varchar](800) NULL,
	[Processed] [bit] NULL,
	[Downloaded] [bit] NULL,
	[Login] [numeric](18, 0) NULL,
	[AccountLinkage] [bit] NULL,
	[ExistingCustomer] [bit] NULL,
	[Skip] [bit] NULL,
	[BatchNo] [varchar](200) NULL,
	[AccountLinkageSMSSent] [bit] NULL,
	[AccountLinkageSMSGUID] [varchar](200) NULL,
	[Aadhaar] [varchar](200) NULL,
	[AddOnCards] [bit] NOT NULL,
	[Bank] [numeric](18, 0) NULL,
	[ProcessedOn] [datetime] NULL,
	[Bc Branch Code] [varchar](200) NULL,
	[Center Name] [varchar](200) NULL,
	[Orig Card Type and Subtype] [varchar](200) NULL,
	[ResidentCustomer] [varchar](20) NULL,
	[IsAuthorised] [bit] NULL,
	[AuthorisedBy] [int] NULL,
	[Date] [datetime] NULL,
	[SystemID] [smallint] NULL,
	[BankID] [varchar](20) NULL,
	[IsBulkUpload] [smallint] NULL,
	[CIF_FileName] [varchar](800) NULL,
	[Account_Type] [varchar](20) NULL,
	[Pin_Mailer] [varchar](20) NULL,
	[ProcessBatch] [varchar](100) NULL,
	[CardGenStatus] [varchar](20) NULL,
	[PREStatus] [varchar](20) NULL,
	[CardGenStatusRemark] [varchar](100) NULL,
	[SwitchCardProgram] [varchar](500) NULL,
	[PGKValue] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
