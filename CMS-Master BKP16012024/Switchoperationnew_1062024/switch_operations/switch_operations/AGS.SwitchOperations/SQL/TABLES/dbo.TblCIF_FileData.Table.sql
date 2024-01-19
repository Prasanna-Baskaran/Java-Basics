USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCIF_FileData]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCIF_FileData](
	[ID] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[CIF_FileName] [varchar](500) NULL,
	[BatchNo] [varchar](200) NULL,
	[BankID] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[CIF_ID] [varchar](200) NULL,
	[CustomerName] [varchar](200) NULL,
	[NameOnCard] [varchar](200) NULL,
	[Bin_Prefix] [varchar](200) NULL,
	[AccountNo] [varchar](200) NULL,
	[AccountOpeningDate] [varchar](20) NULL,
	[CIF_Creation_Date] [varchar](20) NULL,
	[Address1] [varchar](200) NULL,
	[Address2] [varchar](200) NULL,
	[Address3] [varchar](200) NULL,
	[City] [varchar](200) NULL,
	[State] [varchar](200) NULL,
	[PinCode] [varchar](200) NULL,
	[Country] [varchar](200) NULL,
	[Mothers_Name] [varchar](200) NULL,
	[DOB] [varchar](20) NULL,
	[CountryCode] [varchar](200) NULL,
	[STDCode] [varchar](200) NULL,
	[MobileNo] [varchar](200) NULL,
	[EmailID] [varchar](200) NULL,
	[SCHEME_Code] [varchar](200) NULL,
	[BRANCH_Code] [varchar](200) NULL,
	[Entered_Date] [varchar](20) NULL,
	[Verified_Date] [varchar](20) NULL,
	[PAN_No] [varchar](200) NULL,
	[Mode_Of_Operation] [varchar](200) NULL,
	[Fourth_Line_Embossing] [varchar](200) NULL,
	[Aadhar_No] [varchar](200) NULL,
	[Issue_DebitCard] [varchar](20) NULL,
	[Pin_Mailer] [varchar](20) NULL,
	[Account_Type] [varchar](20) NULL,
	[SystemID] [varchar](200) NULL,
	[Status] [smallint] NULL,
	[StatusRemark] [varchar](800) NULL,
	[PGKValue] [varchar](50) NULL,
 CONSTRAINT [PK_TblCIF_FileData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
