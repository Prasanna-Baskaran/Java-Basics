USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblAssemblyLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblAssemblyLog](
	[SrNo] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestData] [varchar](2000) NULL,
	[ResponseData] [nvarchar](max) NULL,
	[RequestDateTime] [datetime] NOT NULL,
	[ResponseDateTime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblAssemblyLog] ADD  DEFAULT (getdate()) FOR [RequestDateTime]
GO
