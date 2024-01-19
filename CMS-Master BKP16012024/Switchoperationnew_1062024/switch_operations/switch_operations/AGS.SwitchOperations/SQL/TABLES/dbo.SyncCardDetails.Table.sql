USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[SyncCardDetails]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[SyncCardDetails](
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
	[InsertedOn] [datetime] NULL,
	[Customer_Name] [varchar](100) NULL,
	[last_updated_date] [datetime] NULL,
	[expiry_date] [varchar](8) NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[SyncCardDetails] ADD [Issuerno] [varchar](5) NULL
ALTER TABLE [dbo].[SyncCardDetails] ADD [AccountType] [varchar](20) NULL
ALTER TABLE [dbo].[SyncCardDetails] ADD [CurrencyCode] [varchar](20) NULL

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SyncCardDetails] ADD  DEFAULT (getdate()) FOR [InsertedOn]
GO
