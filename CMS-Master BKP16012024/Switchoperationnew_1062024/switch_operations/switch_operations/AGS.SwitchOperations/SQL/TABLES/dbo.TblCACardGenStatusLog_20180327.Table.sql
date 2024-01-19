USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCACardGenStatusLog_20180327]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCACardGenStatusLog_20180327](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](200) NULL,
	[ProcessID] [int] NULL,
	[Status] [int] NULL,
	[SeqNo] [int] NULL,
	[ErrorDesc] [varchar](800) NULL,
	[Remark] [varchar](800) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
