USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[test_pc_Account]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test_pc_Account](
	[issuer_nr] [int] NOT NULL,
	[account_id] [varchar](66) NOT NULL,
	[account_type] [varchar](3) NOT NULL,
	[currency_code] [char](3) NOT NULL,
	[last_updated_date] [datetime] NOT NULL,
	[last_updated_user] [varchar](20) NOT NULL,
	[hold_rsp_code] [char](2) NULL,
	[date_deleted] [datetime] NULL,
	[account_product] [varchar](50) NULL,
	[extended_fields] [text] NULL,
	[overdraft_limit] [numeric](12, 0) NULL,
	[account_id_encrypted] [varchar](70) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
