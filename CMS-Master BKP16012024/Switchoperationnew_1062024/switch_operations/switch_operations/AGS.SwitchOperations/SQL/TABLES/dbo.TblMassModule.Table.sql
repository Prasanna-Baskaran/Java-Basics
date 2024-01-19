USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblMassModule]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMassModule](
	[MassModuleCode] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](1000) SPARSE  NULL,
	[Frequency] [int] NULL,
	[FrequencyUnit] [varchar](10) SPARSE  NULL,
	[EnableState] [bit] SPARSE  NULL,
	[Status] [smallint] SPARSE  NULL,
	[LastRunTime] [datetime] SPARSE  NULL,
	[NextRunDate] [datetime] SPARSE  NULL,
	[DllPath] [varchar](max) SPARSE  NULL,
	[ClassName] [varchar](500) SPARSE  NULL,
	[CreatedBy] [varchar](20) SPARSE  NULL,
	[CreatedDate] [datetime] SPARSE  NULL,
	[ModifiedBy] [varchar](20) SPARSE  NULL,
	[ModifiedDate] [datetime] SPARSE  NULL,
	[IssuerNum] [int] NULL,
	[IP] [varchar](30) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
