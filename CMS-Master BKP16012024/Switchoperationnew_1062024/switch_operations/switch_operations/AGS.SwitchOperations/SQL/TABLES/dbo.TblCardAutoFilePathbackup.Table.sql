USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardAutoFilePathbackup]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardAutoFilePathbackup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [bigint] NULL,
	[IssuerNo] [int] NULL,
	[AGS_SFTPServer] [varchar](800) NULL,
	[AGS_SFTPPath] [varchar](800) NULL,
	[AGS_SFTP_User] [varchar](800) NULL,
	[AGS_SFTP_Pwd] [varchar](800) NULL,
	[AGS_SFTP_Port] [varchar](20) NULL,
	[SFTP_CIF_Source_Path] [varchar](800) NULL,
	[SFTP_CIF_BackUp_Path] [varchar](800) NULL,
	[CardCIF_Input_Path] [varchar](800) NULL,
	[CardCIF_Backup] [varchar](800) NULL,
	[ZipCardFilesPath] [varchar](max) NULL,
	[CardAutoSourcePath] [varchar](800) NULL,
	[CardAutoOutputPath_SFTP] [varchar](800) NULL,
	[PRE_BackUp_Path] [varchar](800) NULL,
	[B_SFTPServer] [varchar](800) NULL,
	[B_SFTPPath] [varchar](800) NULL,
	[B_SFTP_User] [varchar](800) NULL,
	[B_SFTP_Pwd] [varchar](800) NULL,
	[B_SFTP_Port] [varchar](20) NULL,
	[B_PRE_DestinationPath_SFTP] [varchar](800) NULL,
	[Zip_Exe_Path] [varchar](800) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CardAutoBackUpPath] [varchar](800) NULL,
	[CardAutoFailedPath] [varchar](800) NULL,
	[IsSaveError] [varchar](2) NULL,
	[ErrorLogPath] [varchar](1000) NULL,
	[FailedCIFPath] [varchar](800) NULL,
	[B_SFTP_FailedCIFPath] [varchar](800) NULL,
	[SFTP_OutputFile_Path] [varchar](2000) NULL,
	[SFTP_OutputFile_Failed] [varchar](2000) NULL,
	[C_SFTP_PRE_Path] [varchar](2000) NULL,
	[C_SFTPServer] [varchar](200) NULL,
	[C_SFTP_User] [varchar](2000) NULL,
	[C_SFTP_Pwd] [varchar](2000) NULL,
	[C_SFTP_Port] [varchar](20) NULL,
	[PRE_Input_Path] [varchar](2000) NULL,
	[PRE_Output_Path] [varchar](2000) NULL,
	[Outputfile_failed_Path] [varchar](2000) NULL,
	[PGP_KeyName] [varchar](200) NULL,
	[PGP_PWD] [varchar](200) NULL,
	[PGPExePath] [varchar](2000) NULL,
	[AGS_PGP_KeyName] [varchar](200) NULL,
	[AGS_PGP_PWD] [varchar](200) NULL,
	[PGP_KeyPath] [varchar](1000) NULL,
	[AGS_KeyPath] [varchar](1000) NULL,
	[SFTP_OutputFile_BK_Path] [varchar](800) NULL,
	[OutputFile_BK_Path] [varchar](800) NULL,
	[AGS_PubKey_Path] [varchar](800) NULL,
	[AGS_SecKey_Path] [varchar](800) NULL,
	[B_PubKey_Path] [varchar](800) NULL,
	[B_SecKey_Path] [varchar](800) NULL,
	[PubKey_Path] [varchar](800) NULL,
	[SecKey_Path] [varchar](800) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
