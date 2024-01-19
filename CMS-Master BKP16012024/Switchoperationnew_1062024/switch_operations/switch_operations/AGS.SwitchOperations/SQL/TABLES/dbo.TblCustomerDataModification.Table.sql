USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustomerDataModification]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustomerDataModification](
	[code] [int] IDENTITY(1,1) NOT NULL,
	[Cifid] [varchar](100) NULL,
	[Add 1] [varchar](100) NULL,
	[Add 2] [varchar](100) NULL,
	[Add 3] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](30) NULL,
	[Pincode] [varchar](20) NULL,
	[Country] [varchar](20) NULL,
	[CUSTOMER NAME] [varchar](100) NULL,
	[NAME ON CARD] [varchar](100) NULL,
	[DATE] [varchar](20) NULL,
	[EMAIL] [varchar](100) NULL,
	[COUNTRY_DIAL_CODE] [varchar](20) NULL,
	[CITY_DIAL_CODE] [varchar](20) NULL,
	[MOBILE_NUMBER] [varchar](12) NULL,
	[DOB] [varchar](20) NULL,
	[PAN] [varchar](500) NULL,
	[FOURTH LINE EMBOSSING] [varchar](500) NULL,
	[AADHAR_NO] [varchar](500) NULL,
	[PIN_MAILER] [varchar](100) NULL,
	[FileName] [varchar](max) NULL,
	[BANK] [int] NULL,
	[Processed] [bit] NULL,
	[Rejected] [bit] NULL,
	[Reason] [varchar](max) NULL,
	[Updated] [bit] NULL,
	[Requested_Date] [datetime] NULL,
	[Processed_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL,
	[isreport] [bit] NULL,
	[switchRespCode] [varchar](30) NULL,
	[PGKValue] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
