USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustomersDetails_20180327]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustomersDetails_20180327](
	[CustomerID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[FirstName] [varchar](200) NULL,
	[MiddleName] [varchar](200) NULL,
	[LastName] [varchar](200) NULL,
	[MobileNo] [varchar](50) NULL,
	[DOB_BS] [datetime] NULL,
	[DOB_AD] [datetime] NULL,
	[Nationality] [varchar](200) NULL,
	[GenderID] [varchar](10) NULL,
	[MaritalStatusID] [varchar](10) NULL,
	[PassportNo_CitizenShipNo] [varchar](200) NULL,
	[IssueDate_District] [varchar](max) NULL,
	[ResidenceTypeID] [varchar](50) NULL,
	[ResidenceDesc] [varchar](200) NULL,
	[VehicleTypeID] [varchar](10) NULL,
	[VehicleType] [varchar](200) NULL,
	[VehicleNo] [varchar](20) NULL,
	[CardTypeID] [int] NULL,
	[SpouseName] [varchar](100) NULL,
	[MotherName] [varchar](100) NULL,
	[FatherName] [varchar](100) NULL,
	[GrandFatherName] [varchar](100) NULL,
	[FormStatusID] [int] NULL,
	[Maker_Date_NE] [datetime] NULL,
	[Maker_Date_IND] [datetime] NULL,
	[MakerID] [int] NULL,
	[ModifiedByID] [int] NULL,
	[ModifiedDate_NE] [datetime] NULL,
	[ModifiedDate_IND] [datetime] NULL,
	[Checker_Date_NE] [datetime] NULL,
	[Checker_Date_IND] [datetime] NULL,
	[CheckerID] [int] NULL,
	[ApplicationFormNo] [varchar](20) NULL,
	[CardLimitStatusID] [int] NULL,
	[Remark] [varchar](200) NULL,
	[IsCardSuccess] [smallint] NULL,
	[ProductType_ID] [int] NULL,
	[INST_ID] [int] NULL,
	[NameOnCard] [varchar](200) NULL,
	[IsSuccess] [tinyint] NULL,
	[SystemID] [int] NULL,
	[UpdateSWResponse] [varchar](500) NULL,
	[BankID] [varchar](20) NULL,
	[AccNo] [varbinary](max) NULL,
	[AccType] [varchar](20) NULL,
	[BankCustID] [varchar](50) NULL,
	[BulkUpload] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
