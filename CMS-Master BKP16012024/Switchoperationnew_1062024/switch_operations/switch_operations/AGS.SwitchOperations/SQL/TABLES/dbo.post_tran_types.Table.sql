USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[post_tran_types]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[post_tran_types](
	[category] [varchar](30) NOT NULL,
	[code] [varchar](4) NOT NULL,
	[description] [varchar](60) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
