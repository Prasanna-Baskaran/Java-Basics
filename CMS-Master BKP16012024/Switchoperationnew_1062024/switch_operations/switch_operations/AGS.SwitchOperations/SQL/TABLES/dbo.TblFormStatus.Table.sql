USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblFormStatus]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblFormStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormStatusID] [int] NULL,
	[FormStatus] [varchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
