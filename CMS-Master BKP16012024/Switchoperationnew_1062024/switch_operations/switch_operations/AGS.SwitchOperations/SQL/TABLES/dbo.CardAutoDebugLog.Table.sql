USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardAutoDebugLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardAutoDebugLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [int] NULL,
	[CardProgram] [varchar](max) NULL,
	[ProcessID] [varchar](max) NULL,
	[Message] [varchar](max) NULL,
	[InsertedOn] [datetime] NULL,
	[MessageData] [varchar](max) NULL,
 CONSTRAINT [PK_CardAutoDebugLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
