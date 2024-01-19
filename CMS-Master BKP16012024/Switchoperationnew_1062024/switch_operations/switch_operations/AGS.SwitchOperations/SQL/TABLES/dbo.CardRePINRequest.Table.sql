USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardRePINRequest]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardRePINRequest](
	[code] [int] IDENTITY(1,1) NOT NULL,
	[cardNo] [varbinary](max) NULL,
	[CIFID] [varchar](max) NULL,
	[AccountNo] [varchar](max) NULL,
	[bankid] [int] NULL,
	[fileName] [varchar](max) NULL,
	[RequestedDate] [datetime] NULL,
	[rejected] [bit] NULL,
	[processed] [bit] NULL,
	[reason] [varchar](max) NULL,
	[rejectedDate] [datetime] NULL,
	[ProcessedDate] [datetime] NULL,
	[updated] [bit] NULL,
	[updateddate] [datetime] NULL,
	[switchRespCode] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
