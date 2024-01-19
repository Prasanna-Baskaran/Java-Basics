USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[cardrpan_20180605]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cardrpan_20180605](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EncPAN] [varchar](200) NULL,
	[DecPAN] [varbinary](200) NULL,
	[IssuerNo] [numeric](18, 0) NULL,
	[customer_id] [varchar](200) NULL,
	[BankID] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
