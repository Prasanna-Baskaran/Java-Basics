USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardFeesDataCustWise_Utkarsh]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardFeesDataCustWise_Utkarsh](
	[Rw] [bigint] NOT NULL,
	[RowNo] [bigint] NULL,
	[Id] [bigint] NULL,
	[Pan] [varchar](100) NULL,
	[AccountNo] [varchar](20) NULL,
	[IssuedDate] [datetime] NULL,
	[CustomerId] [varchar](20) NULL,
	[CustomerName] [varchar](1000) NULL,
	[hold_rsp_code] [varchar](2) NULL,
	[FeeAmount] [numeric](10, 2) NULL,
	[FeeType] [varchar](2) NULL,
	[BranchCode] [varchar](20) NULL,
	[SchemeCode] [varchar](20) NULL,
	[Remarks] [varchar](1000) NULL,
	[CardPrefix] [varchar](10) NULL,
	[NewReIssueCardPrefix] [varchar](8) NULL,
	[expiry_date] [varchar](8) NULL,
	[IsCardIssued] [bit] NULL,
	[IsCardexpired] [bit] NULL,
	[IssuerNo] [numeric](9, 0) NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
