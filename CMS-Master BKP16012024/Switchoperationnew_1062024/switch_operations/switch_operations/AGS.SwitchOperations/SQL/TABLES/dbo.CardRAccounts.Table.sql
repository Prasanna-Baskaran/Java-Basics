USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardRAccounts]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardRAccounts](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EncAcc] [varchar](100) NULL,
	[DecAcc] [varbinary](1000) NULL,
	[IssuerNo] [numeric](18, 0) NULL,
	[SchemeCode] [varchar](10) NULL,
	[DefaultCurrency] [varchar](5) NULL,
	[AccountType] [varchar](3) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_CardRAccounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
