USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblBulkCustomerInfo]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBulkCustomerInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UploadID] [varchar](800) NULL,
	[FileID] [varchar](800) NULL,
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
	[ProductType] [varchar](800) NULL,
	[NameOnCard] [varchar](800) NULL,
	[BatchNo] [varchar](800) NULL,
	[Rejected] [bit] NULL,
	[Processed] [bit] NULL,
	[Reason] [varchar](800) NULL,
	[Authorized] [smallint] NULL,
	[CreatedDate] [datetime] NULL,
	[CustomerID] [varchar](800) NULL,
 CONSTRAINT [PK_TblBulkCustomerInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
