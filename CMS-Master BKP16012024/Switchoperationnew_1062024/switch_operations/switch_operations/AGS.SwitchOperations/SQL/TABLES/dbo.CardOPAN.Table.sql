USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardOPAN]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardOPAN](
	[ID] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[EncPAN] [varchar](200) NULL,
	[DecPAN] [varbinary](200) NULL,
	[IssuerNo] [numeric](18, 0) NULL,
	[BankID] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_CardOPAN] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
