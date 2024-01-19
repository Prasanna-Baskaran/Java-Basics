USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[SwitchMasterTable]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SwitchMasterTable](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](25) NULL,
	[CardTable] [varchar](1000) NULL,
	[CardAccountsTable] [varchar](1000) NULL,
	[AccountTable] [varchar](1000) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
