USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblmassmodule_BKP_27042018]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblmassmodule_BKP_27042018](
	[MassModuleCode] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](1000) NULL,
	[Frequency] [int] NULL,
	[FrequencyUnit] [varchar](10) NULL,
	[EnableState] [bit] NULL,
	[Status] [smallint] NULL,
	[LastRunTime] [datetime] NULL,
	[NextRunDate] [datetime] NULL,
	[DllPath] [varchar](max) NULL,
	[ClassName] [varchar](500) NULL,
	[CreatedBy] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [varchar](20) NULL,
	[ModifiedDate] [datetime] NULL,
	[IssuerNum] [int] NULL,
	[IP] [varchar](30) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
