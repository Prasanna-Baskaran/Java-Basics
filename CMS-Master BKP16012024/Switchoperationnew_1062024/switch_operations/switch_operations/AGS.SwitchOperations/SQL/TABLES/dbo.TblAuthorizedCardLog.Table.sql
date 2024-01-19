USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblAuthorizedCardLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblAuthorizedCardLog](
	[ID] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
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
	[PGKValue] [varchar](50) NULL,
 CONSTRAINT [PK_TblAuthorizedCardLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Uploa__1387E197]  DEFAULT (getdate()) FOR [Uploaded On]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Rejec__147C05D0]  DEFAULT ((0)) FOR [Rejected]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Proce__15702A09]  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Downl__16644E42]  DEFAULT ((0)) FOR [Downloaded]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Accou__1758727B]  DEFAULT ((0)) FOR [AccountLinkage]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Exist__184C96B4]  DEFAULT ((0)) FOR [ExistingCustomer]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthori__Skip__1940BAED]  DEFAULT ((0)) FOR [Skip]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Accou__1A34DF26]  DEFAULT ((0)) FOR [AccountLinkageSMSSent]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__AddOn__1B29035F]  DEFAULT ((0)) FOR [AddOnCards]
GO
ALTER TABLE [dbo].[TblAuthorizedCardLog] ADD  CONSTRAINT [DF__TblAuthor__Proce__1C1D2798]  DEFAULT (getdate()) FOR [ProcessedOn]
GO
