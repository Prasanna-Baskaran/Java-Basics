USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardProductionRenewal]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardProductionRenewal](
	[Code] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[IssuerNumber] [numeric](18, 0) NULL,
	[Cif Id] [varchar](50) NULL,
	[Pan] [varbinary](max) NULL,
	[SwitchPan] [varchar](max) NULL,
	[Bin] [varchar](20) NULL,
	[schemecode] [varchar](10) NULL,
	[Account 1] [varchar](50) NULL,
	[Account 2] [varchar](50) NULL,
	[Account 3] [varchar](50) NULL,
	[Account 4] [varchar](50) NULL,
	[Account 5] [varchar](50) NULL,
	[Reserved 1] [varchar](max) NULL,
	[Reserved 2] [varchar](max) NULL,
	[Reserved 3] [varchar](max) NULL,
	[Reserved 4] [varchar](max) NULL,
	[Reserved 5] [varchar](max) NULL,
	[Branch_Code] [varchar](20) NULL,
	[Expiry_Date] [varchar](8) NULL,
	[New_Card] [varchar](max) NULL,
	[Customer_Name] [varchar](max) NULL,
	[New_Card_Activation_Date] [datetime] NULL,
	[RRN] [varchar](1) NULL,
	[AuthCode] [varchar](10) NULL,
	[ResponseCode] [varchar](10) NULL,
	[STAN] [varchar](10) NULL,
	[RequestedOn] [datetime] NULL,
	[Processed] [bit] NULL,
	[ProcessedOn] [datetime] NULL,
	[Rejected] [bit] NULL,
	[RejectedReason] [varchar](50) NULL,
	[RejectedDate] [datetime] NULL,
	[UploadFileName] [varchar](max) NULL,
	[Process_Type] [varchar](20) NULL,
	[Block_Status] [bit] NULL,
	[Block_Date] [datetime] NULL,
	[Block_Type] [varchar](100) NULL,
	[Remarks] [varchar](max) NULL,
	[AdditionalBlockCard] [varchar](max) NULL,
	[CurrencyCode] [varchar](20) NULL,
	[AccountType] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
