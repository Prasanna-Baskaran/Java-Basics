USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CustomerdataModificationType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[CustomerdataModificationType] AS TABLE(
	[CIFID] [varchar](max) NULL,
	[ADD1] [varchar](max) NULL,
	[ADD2] [varchar](max) NULL,
	[ADD3] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[State] [varchar](max) NULL,
	[Pincode] [varchar](max) NULL,
	[Country] [varchar](max) NULL,
	[CUSTOMER_NAME] [varchar](max) NULL,
	[NAME_ON_CARD] [varchar](max) NULL,
	[DATE] [varchar](max) NULL,
	[EMAILID] [varchar](max) NULL,
	[COUNTRY_DIAL_CODE] [varchar](max) NULL,
	[CITY_DIAL_CODE] [varchar](max) NULL,
	[MOBILE_NUMBER] [varchar](max) NULL,
	[DOB] [varchar](max) NULL,
	[PAN] [varchar](max) NULL,
	[FOURTH LINE EMBOSSING] [varchar](max) NULL,
	[AADHAR_NO] [varchar](max) NULL,
	[PIN_MAILER] [varchar](max) NULL,
	[PGKValue] [varchar](max) NULL
)
GO
