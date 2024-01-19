USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[EmailAlert]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailAlert](
	[code] [int] IDENTITY(1,1) NOT NULL,
	[SMTPCLIENT] [varchar](20) NULL,
	[EmailFrom] [varchar](100) NULL,
	[EmailPort] [int] NULL,
	[EmailUserName] [varchar](100) NULL,
	[EmailPassWord] [varbinary](max) NULL,
	[EmailTo] [varchar](max) NULL,
	[EmailBCC] [varchar](max) NULL,
	[EmailMsg] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
