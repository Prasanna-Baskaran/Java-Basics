USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[ProcessedCardFeesData_Utkarsh]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessedCardFeesData_Utkarsh](
	[CId] [bigint] IDENTITY(1,1) NOT NULL,
	[Pan] [varchar](100) NULL,
	[AccountNo] [varchar](20) NULL,
	[IssuedDate] [datetime] NULL,
	[CustomerId] [varchar](20) NULL,
	[CustomerName] [varchar](1000) NULL,
	[hold_rsp_code] [varchar](2) NULL,
	[FeeAmount] [numeric](10, 2) NULL,
	[FeeType] [varchar](2) NULL,
	[IssuerNo] [numeric](9, 0) NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
