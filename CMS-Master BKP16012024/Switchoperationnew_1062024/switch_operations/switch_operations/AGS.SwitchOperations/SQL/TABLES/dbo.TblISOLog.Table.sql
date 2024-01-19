USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblISOLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[TblISOLog](
	[LogCode] [bigint] IDENTITY(1,1) NOT NULL,
	[FunctionName] [varchar](500) SPARSE  NULL,
	[Parameter] [varchar](max) SPARSE  NULL,
	[ISOString] [varchar](max) SPARSE  NULL,
	[OutPutMsg] [varchar](max) SPARSE  NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TblISOLog] PRIMARY KEY CLUSTERED 
(
	[LogCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblISOLog] ADD  CONSTRAINT [DF_TblISOLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
