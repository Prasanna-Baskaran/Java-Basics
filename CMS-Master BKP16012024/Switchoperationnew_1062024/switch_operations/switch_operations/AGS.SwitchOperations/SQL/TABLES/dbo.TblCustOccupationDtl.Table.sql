USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustOccupationDtl]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustOccupationDtl](
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
	[ModifiedDate_IND] [datetime] NULL,
 CONSTRAINT [PK_TblCustOccupationDtl_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Salaried,2-Self Employed,3-Retired' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TblCustOccupationDtl', @level2type=N'COLUMN',@level2name=N'ProfessionTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Gov,2-Public,3-Private,4-Others' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TblCustOccupationDtl', @level2type=N'COLUMN',@level2name=N'OrganizationTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-Savings,2-Current,3-Fixed,4-Loan,5-Others' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TblCustOccupationDtl', @level2type=N'COLUMN',@level2name=N'AccountTypeID'
GO
