USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[CustomerInfoType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[CustomerInfoType] AS TABLE(
	[FirstName] [varchar](800) NULL,
	[LastName] [varchar](500) NULL,
	[DOB] [varchar](500) NULL,
	[MobileNo] [varchar](500) NULL,
	[Email] [varchar](500) NULL,
	[Gender] [varchar](50) NULL,
	[Nationality] [varchar](800) NULL,
	[Passport_IdentiNo] [varchar](500) NULL,
	[IssueDate] [varchar](500) NULL,
	[StatementDelivery] [varchar](800) NULL,
	[HouseNo] [varchar](800) NULL,
	[StreetName] [varchar](800) NULL,
	[City] [varchar](800) NULL,
	[District] [varchar](800) NULL,
	[AccountType] [varchar](800) NULL,
	[AccountNo] [varchar](800) NULL,
	[CardPrefix] [varchar](800) NULL,
	[NameOnCard] [varchar](800) NULL,
	[Remark] [varchar](800) NULL
)
GO
