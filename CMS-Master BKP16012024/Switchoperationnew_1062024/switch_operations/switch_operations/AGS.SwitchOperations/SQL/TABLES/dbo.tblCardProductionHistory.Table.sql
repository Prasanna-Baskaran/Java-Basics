USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblCardProductionHistory]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[tblCardProductionHistory](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[CIF ID] [varchar](16) NULL,
	[Customer Name] [varchar](200) NULL,
	[Customer Preferred name] [varchar](30) NULL,
	[Card Type and Subtype] [varchar](20) NULL,
	[AC ID] [varchar](20) NULL,
	[AC open date] [varchar](8) NULL,
	[CIF Creation Date] [varchar](8) NULL,
	[Address Line 1] [varchar](50) NULL,
	[Address Line 2] [varchar](50) NULL,
	[Address Line 3] [varchar](50) NULL,
	[City] [varchar](40) NULL,
	[State] [varchar](20) NULL,
	[Pin Code] [varchar](10) NULL,
	[Country code] [varchar](10) NULL,
	[Mothers Maiden Name] [varchar](50) NULL,
	[DOB] [varchar](8) NULL,
	[Country Dial code] [varchar](4) NULL,
	[City Dial code] [varchar](4) NULL,
	[Mobile phone number] [varchar](10) NULL,
	[Email id] [varchar](100) NULL,
	[Scheme code] [varchar](8) NULL,
	[Branch code] [varchar](4) NULL,
	[Entered date] [varchar](8) NULL,
	[Verified Date] [varchar](8) NULL,
	[PAN Number] [varchar](15) NULL,
	[Mode Of Operation] [varchar](2) NULL,
	[Fourth Line Embossing] [varchar](20) NULL,
	[Debit Card Linkage Flag] [varchar](50) NULL,
	[Uploaded On] [datetime] NULL,
	[Rejected] [bit] NULL,
	[Reason] [varchar](500) NULL,
	[Processed] [bit] NULL,
	[Downloaded] [bit] NULL,
	[Login] [numeric](18, 0) NULL,
	[AccountLinkage] [bit] NULL,
	[ExistingCustomer] [bit] NULL,
	[Skip] [bit] NULL,
	[BatchNo] [varchar](20) NULL,
	[AccountLinkageSMSSent] [bit] NULL,
	[AccountLinkageSMSGUID] [varchar](50) NULL,
	[Aadhaar] [varchar](19) NULL,
	[AddOnCards] [bit] NOT NULL,
	[Bank] [numeric](18, 0) NULL,
	[ProcessedOn] [datetime] NULL,
	[Bc Branch Code] [varchar](20) NULL,
	[Center Name] [varchar](100) NULL,
	[Orig Card Type and Subtype] [varchar](10) NULL,
	[ResidentCustomer] [varchar](3) NULL,
	[IsAuthorised] [bit] NULL,
	[AuthorisedBy] [int] NULL,
	[SystemID] [smallint] NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[tblCardProductionHistory] ADD [BankID] [varchar](20) NULL

GO
SET ANSI_PADDING OFF
GO
