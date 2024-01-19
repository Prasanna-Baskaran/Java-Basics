USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblUserManagement_20180212]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblUserManagement_20180212](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NULL,
	[CM] [varchar](500) NULL,
	[CMACO] [varchar](500) NULL,
	[CMCL] [varchar](500) NULL,
	[CMCO] [varchar](500) NULL,
	[CMCP] [varchar](500) NULL,
	[E] [varchar](500) NULL,
	[ECR] [varchar](500) NULL,
	[ESC] [varchar](500) NULL,
	[M] [varchar](500) NULL,
	[MBIN] [varchar](500) NULL,
	[MCM] [varchar](500) NULL,
	[MCT] [varchar](500) NULL,
	[MIM] [varchar](500) NULL,
	[MPT] [varchar](500) NULL,
	[MUD] [varchar](500) NULL,
	[MUM] [varchar](500) NULL,
	[MUR] [varchar](500) NULL,
	[CMCCO] [varchar](500) NULL,
	[CMCR] [varchar](500) NULL,
	[SystemID] [varchar](500) NULL,
	[CMCD] [varchar](500) NULL,
	[RC] [varchar](500) NULL,
	[RT] [varchar](500) NULL,
	[BF] [varchar](500) NULL,
	[CMCCR] [varchar](500) NULL,
	[CMMCR] [varchar](500) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
