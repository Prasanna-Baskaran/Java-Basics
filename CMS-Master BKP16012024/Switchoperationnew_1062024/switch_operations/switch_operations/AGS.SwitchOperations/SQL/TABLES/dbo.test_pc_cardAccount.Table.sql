USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[test_pc_cardAccount]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test_pc_cardAccount](
	[issuer_nr] [int] NOT NULL,
	[pan] [varchar](66) NOT NULL,
	[seq_nr] [char](3) NOT NULL,
	[account_id] [varchar](66) NOT NULL,
	[account_type_nominated] [varchar](3) NOT NULL,
	[account_type_qualifier] [int] NOT NULL,
	[last_updated_date] [datetime] NOT NULL,
	[last_updated_user] [varchar](20) NOT NULL,
	[account_type] [varchar](3) NOT NULL,
	[date_deleted] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
