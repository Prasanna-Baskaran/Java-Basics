USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[SyncGenericCardDetails]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SyncGenericCardDetails](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[customer_id] [varchar](25) NULL,
	[account_id] [varchar](66) NULL,
	[pan_encrypted] [varchar](70) NULL,
	[account_id_encrypted] [varchar](70) NULL,
	[hold_rsp_code] [char](2) NULL,
	[account_type_qualifier] [int] NULL,
	[branch_code] [varchar](10) NULL,
	[card_status] [varchar](3) NULL,
	[date_deleted] [datetime] NULL,
	[date_issued] [datetime] NULL,
	[date_activated] [datetime] NULL,
	[card_program] [varchar](20) NULL,
	[Customer_Name] [varchar](100) NULL,
	[last_updated_date] [datetime] NULL,
	[expiry_date] [varchar](8) NULL,
	[IssuerNo] [numeric](9, 0) NULL,
	[InsertedOn] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
