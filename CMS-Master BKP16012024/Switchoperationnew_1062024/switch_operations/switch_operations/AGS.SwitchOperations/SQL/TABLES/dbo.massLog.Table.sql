USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[massLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[massLog](
	[code] [int] IDENTITY(1,1) NOT NULL,
	[Method] [varchar](max) NULL,
	[Exception] [varchar](max) NULL,
	[Date_Insert] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
