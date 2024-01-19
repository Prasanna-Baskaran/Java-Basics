USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblCardProduction]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCardProduction](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
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
	[Pin Code] [varchar](200) NULL,
	[Country code] [varchar](200) NULL,
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
	[Debit Card Linkage Flag] [varchar](200) NULL,
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
	[ResidentCustomer] [varchar](200) NULL,
	[IsAuthorised] [bit] NULL,
	[AuthorisedBy] [int] NULL,
	[SystemID] [smallint] NULL,
	[BankID] [varchar](20) NULL,
	[IsBulkUpload] [smallint] NULL,
	[CIF_FileName] [varchar](800) NULL,
	[Account_Type] [varchar](20) NULL,
	[Pin_Mailer] [varchar](20) NULL,
	[SwitchCardProgram] [varchar](500) NULL,
	[PGKValue] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Uploa__075714DC]  DEFAULT (getdate()) FOR [Uploaded On]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Rejec__084B3915]  DEFAULT ((0)) FOR [Rejected]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Proce__093F5D4E]  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Downl__0A338187]  DEFAULT ((0)) FOR [Downloaded]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Accou__0B27A5C0]  DEFAULT ((0)) FOR [AccountLinkage]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Exist__0C1BC9F9]  DEFAULT ((0)) FOR [ExistingCustomer]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPro__Skip__0D0FEE32]  DEFAULT ((0)) FOR [Skip]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Accou__0E04126B]  DEFAULT ((0)) FOR [AccountLinkageSMSSent]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__AddOn__0EF836A4]  DEFAULT ((0)) FOR [AddOnCards]
GO
ALTER TABLE [dbo].[tblCardProduction] ADD  CONSTRAINT [DF__tblCardPr__Proce__0FEC5ADD]  DEFAULT (getdate()) FOR [ProcessedOn]
GO
