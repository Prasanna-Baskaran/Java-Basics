USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CustBulkDataType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[CustBulkDataType] AS TABLE(
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
	[PGKValue] [varchar](50) NULL,
	[SystemID] [varchar](200) NULL
)
GO
