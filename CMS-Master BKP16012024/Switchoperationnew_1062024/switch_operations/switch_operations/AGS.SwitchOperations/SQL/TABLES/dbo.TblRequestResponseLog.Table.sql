USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblRequestResponseLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblRequestResponseLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FunctionName] [varchar](500) NULL,
	[RequestData] [varchar](max) NULL,
	[ResponseData] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_TblRequestResponseLog_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
