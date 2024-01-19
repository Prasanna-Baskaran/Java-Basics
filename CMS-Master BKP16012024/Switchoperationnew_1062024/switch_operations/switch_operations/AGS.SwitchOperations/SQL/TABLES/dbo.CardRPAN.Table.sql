USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardRPAN]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardRPAN](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EncPAN] [varchar](200) NULL,
	[DecPAN] [varbinary](200) NULL,
	[IssuerNo] [numeric](18, 0) NULL,
	[customer_id] [varchar](200) NULL,
	[BankID] [varchar](20) NULL,
	[mksp] [varchar](50) NULL,
 CONSTRAINT [PK_CardRPAN] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
