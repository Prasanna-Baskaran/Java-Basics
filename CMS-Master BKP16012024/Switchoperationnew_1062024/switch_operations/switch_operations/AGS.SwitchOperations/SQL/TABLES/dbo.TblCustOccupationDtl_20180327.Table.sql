USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustOccupationDtl_20180327]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustOccupationDtl_20180327](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [bigint] NULL,
	[ProfessionTypeID] [varchar](50) NULL,
	[OrganizationTypeID] [varchar](10) NULL,
	[OrganizationTypeDesc] [varchar](200) NULL,
	[PreviousEmployment] [varchar](200) NULL,
	[Designation] [varchar](100) NULL,
	[CompanyName] [varchar](200) NULL,
	[BusinessType] [varchar](200) NULL,
	[WorkSince] [varchar](200) NULL,
	[AnnualSalary] [varchar](200) NULL,
	[AnnualIncentive] [varchar](200) NULL,
	[AnnualBuisnessIncome] [varchar](200) NULL,
	[RentalIncome] [varchar](200) NULL,
	[Agriculture] [varchar](200) NULL,
	[Income] [varchar](200) NULL,
	[TotalAnnualIncome] [varchar](200) NULL,
	[IsOtherCreditCard] [varchar](10) NULL,
	[PrincipalBankName] [varchar](max) NULL,
	[AccountTypeID] [int] NULL,
	[AccountTypeDesc] [varchar](200) NULL,
	[IsPrabhuBankAcnt] [bit] NULL,
	[PrabhuBankAccountNo] [varchar](50) NULL,
	[IsCollectStatement] [bit] NULL,
	[IsEmailStatemnt] [bit] NULL,
	[EmailForStatement] [varchar](200) NULL,
	[ReffName1] [varchar](200) NULL,
	[ReffDesignation1] [varchar](200) NULL,
	[ReffPhoneNo1] [varchar](200) NULL,
	[ReffName2] [varchar](200) NULL,
	[ReffDesignation2] [varchar](200) NULL,
	[ReffPhoneNo2] [varchar](200) NULL,
	[Documentation] [varchar](200) NULL,
	[ProfessionType] [varchar](50) NULL,
	[ModifiedDate_NE] [datetime] NULL,
	[ModifiedDate_IND] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
