USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblMassFileUpload03052018]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMassFileUpload03052018](
	[FileID] [bigint] IDENTITY(1,1) NOT NULL,
	[Descriptions] [varchar](500) NULL,
	[Participant] [varchar](500) NULL,
	[Path] [varchar](800) NULL,
	[FileName] [varchar](500) NULL,
	[Format] [varchar](500) NULL,
	[DBObject] [varchar](500) NULL,
	[SFTP_Server] [varchar](500) NULL,
	[SFTP_Port] [varchar](50) NULL,
	[SFTP_Path] [varchar](800) NULL,
	[SFTP_User] [varchar](500) NULL,
	[SFTP_PWD] [varchar](500) NULL,
	[SFTP_BackUp] [varchar](800) NULL,
	[SFTP_StatusFile] [varchar](800) NULL,
	[SFTP_Failed] [varchar](800) NULL,
	[InputPath] [varchar](800) NULL,
	[OutputPath] [varchar](800) NULL,
	[BackUpPath] [varchar](800) NULL,
	[FailedPath] [varchar](800) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[FileType] [varchar](100) NULL,
	[LogFilePath] [varchar](800) NULL,
	[Seperator] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
