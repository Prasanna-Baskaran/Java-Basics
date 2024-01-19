USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblParameter]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblParameter](
	[ParamCode] [int] IDENTITY(1,1) NOT NULL,
	[SwitchIP] [varchar](50) SPARSE  NULL,
	[SwitchPort] [varchar](50) SPARSE  NULL,
	[CreatedBy] [varchar](50) SPARSE  NULL,
	[CreatedDate] [datetime] SPARSE  NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
