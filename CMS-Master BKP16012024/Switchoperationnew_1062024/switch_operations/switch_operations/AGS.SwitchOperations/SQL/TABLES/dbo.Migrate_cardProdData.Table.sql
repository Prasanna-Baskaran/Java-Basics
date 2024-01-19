USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[Migrate_cardProdData]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Migrate_cardProdData](
	[CIF ID] [varchar](25) NULL,
	[Customer Name] [varchar](100) NULL,
	[Customer Preferred name] [varchar](100) NULL,
	[Card Type and Subtype] [varchar](max) NULL,
	[AC ID] [varchar](max) NULL,
	[AC open date] [int] NULL,
	[CIF Creation Date] [int] NULL,
	[Address Line 1] [varchar](100) NULL,
	[Address Line 2] [varchar](100) NULL,
	[Address Line 3] [varchar](1) NOT NULL,
	[City] [varchar](40) NULL,
	[State] [char](3) NULL,
	[Pin Code] [varchar](20) NULL,
	[Country code] [int] NULL,
	[Mothers Maiden Name] [varchar](1) NOT NULL,
	[DOB] [char](8) NULL,
	[Country Dial code] [int] NULL,
	[City Dial code] [int] NULL,
	[Mobile phone number] [varchar](50) NULL,
	[Email id] [varchar](320) NULL,
	[Scheme code] [int] NULL,
	[Branch code] [varchar](10) NULL,
	[Entered date] [int] NULL,
	[Verified Date] [int] NULL,
	[PAN Number] [int] NULL,
	[Mode Of Operation] [varchar](2) NOT NULL,
	[Fourth Line Embossing] [varchar](1) NOT NULL,
	[Debit Card Linkage Flag] [varchar](1) NOT NULL,
	[Uploaded On] [datetime] NOT NULL,
	[Rejected] [int] NOT NULL,
	[Reason] [varchar](1) NOT NULL,
	[Processed] [int] NOT NULL,
	[Downloaded] [int] NOT NULL,
	[Login] [int] NULL,
	[AccountLinkage] [int] NOT NULL,
	[ExistingCustomer] [int] NOT NULL,
	[Skip] [int] NOT NULL,
	[BatchNo] [int] NULL,
	[AccountLinkageSMSSent] [int] NOT NULL,
	[AccountLinkageSMSGUID] [int] NULL,
	[Aadhaar] [varchar](1) NOT NULL,
	[AddOnCards] [int] NOT NULL,
	[Bank] [int] NOT NULL,
	[ProcessedOn] [datetime] NOT NULL,
	[Bc Branch Code] [varchar](1) NOT NULL,
	[Center Name] [varchar](1) NOT NULL,
	[Orig Card Type and Subtype] [varchar](1) NOT NULL,
	[ResidentCustomer] [int] NULL,
	[IsAuthorised] [int] NOT NULL,
	[AuthorisedBy] [int] NULL,
	[SystemID] [int] NOT NULL,
	[BankID] [int] NULL,
	[Date] [datetime] NOT NULL,
	[CIF_FileName] [int] NULL,
	[account_type] [varchar](3) NOT NULL,
	[Pin_Mailer] [int] NULL,
	[ProcessBatch] [int] NULL,
	[SwitchCardProgram] [varchar](20) NOT NULL,
	[PGKValue] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
