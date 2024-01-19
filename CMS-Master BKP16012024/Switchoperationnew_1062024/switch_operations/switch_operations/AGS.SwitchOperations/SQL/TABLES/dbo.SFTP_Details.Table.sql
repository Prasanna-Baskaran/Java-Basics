USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[SFTP_Details]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SFTP_Details](
	[code] [int] IDENTITY(1,1) NOT NULL,
	[Application] [varchar](max) NOT NULL,
	[Server_IP] [varchar](20) NOT NULL,
	[Server_Port] [int] NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[password] [varbinary](max) NOT NULL,
	[KeyPath] [varchar](200) NULL,
	[Keypassphrase] [varbinary](200) NULL,
	[InputFilePath] [varchar](max) NULL,
	[ArchiveFilePath] [varchar](max) NULL,
	[outputFilePath] [varchar](max) NULL,
	[FilePath] [varchar](max) NULL,
	[smtpServer] [varchar](max) NULL,
	[SMTPport] [varchar](max) NULL,
	[SFTPUserName] [varchar](max) NULL,
	[SFTPPassword] [varbinary](max) NULL,
	[SFTPDirectory] [varchar](max) NULL,
	[SFTPDirectoryMove] [varchar](max) NULL,
	[SFTPDirectoryReports] [varchar](max) NULL,
	[SFTPDirectoryReissue] [varchar](max) NULL,
	[SFTPDirectoryMoveReisse] [varchar](max) NULL,
	[SFTPDirectoryReissueReports] [varchar](max) NULL,
	[downloadFilePath] [varchar](max) NULL,
	[ProcessdFilePath] [varchar](max) NULL,
	[SwitchFilePath] [varchar](max) NULL,
	[downloadReissueFilePath] [varchar](max) NULL,
	[ProcessdReissueFilePath] [varchar](max) NULL,
	[SwitchReissueFilePath] [varchar](max) NULL,
	[SWITCHSFTP] [varchar](max) NULL,
	[SWITCHPORT] [varchar](max) NULL,
	[SWITCHSFTPDirectory] [varchar](max) NULL,
	[SWITCHSFTPDirectoryReissue] [varchar](max) NULL,
	[SWITCHUserName] [varchar](max) NULL,
	[SWITCHPassword] [varbinary](max) NULL,
	[SP] [varchar](max) NULL,
	[SPReissue] [varchar](max) NULL,
	[rarWorkingDirectory] [varchar](max) NULL,
	[LogPath] [varchar](max) NULL,
	[ISMail] [bit] NULL,
	[ALTEMAILFRM] [varchar](max) NULL,
	[ALTSMTPCLIENT] [varchar](max) NULL,
	[ALTUSER] [varchar](max) NULL,
	[ALTPASSWORD] [varchar](max) NULL,
	[mailTo] [varchar](max) NULL,
	[ErrorMailTo] [varchar](max) NULL,
	[ErrorBCC] [varchar](max) NULL,
	[BCC] [varchar](max) NULL,
	[Mailport] [varchar](max) NULL,
	[RejectedFilePath] [varchar](max) NULL,
	[Issuerno] [varchar](5) NULL,
	[SwicthAccountDetails] [varchar](1000) NULL,
	[CardRenewalHeader] [varchar](1000) NULL,
	[CardReissueHeader] [varchar](1000) NULL,
	[CardUpgrdeHeader] [varchar](1000) NULL,
	[SFTPDirectoryUpgrade] [varchar](1000) NULL,
	[SFTPDirectoryMoveUpgrade] [varchar](1000) NULL,
	[SFTPDirectoryUpgradeReports] [varchar](1000) NULL,
	[downloadUpgradeFilePath] [varchar](1000) NULL,
	[ProcessdUpgradeFilePath] [varchar](1000) NULL,
	[SwitchUpgradeFilePath] [varchar](1000) NULL,
	[SWITCHSFTPDirectoryUpgrade] [varchar](1000) NULL,
	[SPUpgrade] [varchar](1000) NULL,
	[ISKeyRequired] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SFTP_Details] ADD  DEFAULT ((0)) FOR [ISMail]
GO
