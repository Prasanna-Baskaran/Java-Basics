USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardProduction]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardProduction](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[CIF ID] [varchar](max) NULL,
	[Customer Name] [varchar](max) NULL,
	[Customer Preferred name] [varchar](max) NULL,
	[Card Type and Subtype] [varchar](max) NULL,
	[AC ID] [varchar](max) NULL,
	[AC open date] [varchar](max) NULL,
	[CIF Creation Date] [varchar](max) NULL,
	[Address Line 1] [varchar](max) NULL,
	[Address Line 2] [varchar](max) NULL,
	[Address Line 3] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[State] [varchar](max) NULL,
	[Pin Code] [varchar](max) NULL,
	[Country code] [varchar](max) NULL,
	[Mothers Maiden Name] [varchar](max) NULL,
	[DOB] [varchar](max) NULL,
	[Country Dial code] [varchar](max) NULL,
	[City Dial code] [varchar](max) NULL,
	[Mobile phone number] [varchar](max) NULL,
	[Email id] [varchar](max) NULL,
	[Scheme code] [varchar](max) NULL,
	[Branch code] [varchar](max) NULL,
	[Entered date] [varchar](max) NULL,
	[Verified Date] [varchar](max) NULL,
	[PAN Number] [varchar](max) NULL,
	[Mode Of Operation] [varchar](max) NULL,
	[Fourth Line Embossing] [varchar](max) NULL,
	[Debit Card Linkage Flag] [varchar](max) NULL,
	[Uploaded On] [datetime] NULL,
	[Rejected] [bit] NULL,
	[Reason] [varchar](max) NULL,
	[Processed] [bit] NULL,
	[Downloaded] [bit] NULL,
	[Login] [numeric](18, 0) NULL,
	[AccountLinkage] [bit] NULL,
	[ExistingCustomer] [bit] NULL,
	[Skip] [bit] NULL,
	[UploadFileName] [varchar](200) NULL,
	[AccountLinkageSMSSent] [bit] NULL,
	[AccountLinkageSMSGUID] [varchar](max) NULL,
	[Aadhaar] [varchar](50) NULL,
	[AddOnCards] [bit] NOT NULL,
	[Bank] [numeric](18, 0) NULL,
	[ProcessedOn] [datetime] NULL,
	[Bc Branch Code] [varchar](max) NULL,
	[Center Name] [varchar](max) NULL,
	[Orig Card Type and Subtype] [varchar](10) NULL,
	[ResidentCustomer] [varchar](3) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT (getdate()) FOR [Uploaded On]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [Rejected]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [Downloaded]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [AccountLinkage]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [ExistingCustomer]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [Skip]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [AccountLinkageSMSSent]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT ((0)) FOR [AddOnCards]
GO
ALTER TABLE [dbo].[CardProduction] ADD  DEFAULT (getdate()) FOR [ProcessedOn]
GO
