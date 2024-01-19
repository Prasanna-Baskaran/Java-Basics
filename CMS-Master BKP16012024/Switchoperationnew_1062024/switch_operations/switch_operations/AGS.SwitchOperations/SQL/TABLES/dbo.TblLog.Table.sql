USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblLog](
	[LogCode] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionType] [varchar](50) NULL,
	[RequestData] [varchar](max) NULL,
	[OutPutMsg] [varchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TblLog] PRIMARY KEY CLUSTERED 
(
	[LogCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblLog] ADD  CONSTRAINT [DF_TblLog_TransactionType]  DEFAULT ('') FOR [TransactionType]
GO
ALTER TABLE [dbo].[TblLog] ADD  CONSTRAINT [DF_TblLog_ReuestData]  DEFAULT ('') FOR [RequestData]
GO
ALTER TABLE [dbo].[TblLog] ADD  CONSTRAINT [DF_TblLog_RequestData1]  DEFAULT ('') FOR [OutPutMsg]
GO
ALTER TABLE [dbo].[TblLog] ADD  CONSTRAINT [DF_TblLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
