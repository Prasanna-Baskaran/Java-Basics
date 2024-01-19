USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[AccountLinkingDeLinkingRequest_20180503]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccountLinkingDeLinkingRequest_20180503](
	[Cid] [bigint] IDENTITY(1,1) NOT NULL,
	[EncPan] [varbinary](1000) NULL,
	[EncAcc] [varbinary](1000) NULL,
	[AccountType] [nvarchar](2) NULL,
	[AccountQualifier] [nvarchar](2) NULL,
	[LinkingFlag] [varchar](2) NULL,
	[Response] [varchar](1000) NULL,
	[ResponseDesc] [varchar](max) NULL,
	[GeneratedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
