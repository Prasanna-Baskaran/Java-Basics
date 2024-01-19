USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[Issuer_NR_BK_22_01_2018]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issuer_NR_BK_22_01_2018](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[IssuerNr] [int] NULL,
	[SwitchIssNr] [int] NULL,
	[BANKID] [int] NULL
) ON [PRIMARY]

GO
