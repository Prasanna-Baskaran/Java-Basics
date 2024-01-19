USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardAutomation]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardAutomation](
	[Code] [bigint] IDENTITY(1,1) NOT NULL,
	[Bank] [varchar](1000) SPARSE  NULL,
	[IssuerNo] [int] NULL,
	[SwitchInstitutionID] [varchar](11) SPARSE  NULL,
	[SourceIP] [varchar](100) SPARSE  NULL,
	[SourcePort] [varchar](10) SPARSE  NULL,
	[SourcePath] [varchar](max) SPARSE  NULL,
	[OutputPath] [varchar](max) SPARSE  NULL,
	[Standard2Batch] [varchar](max) SPARSE  NULL,
	[Standard2ExePath] [varchar](max) SPARSE  NULL,
	[Standard2Input] [varchar](max) SPARSE  NULL,
	[Standard2Archive] [varchar](max) SPARSE  NULL,
	[Standard4Batch] [varchar](max) SPARSE  NULL,
	[Standard4ExePath] [varchar](max) SPARSE  NULL,
	[Standard4Output] [varchar](max) SPARSE  NULL,
	[CardProcessType] [smallint] SPARSE  NULL,
	[EnableState] [bit] SPARSE  NULL,
	[Status] [smallint] SPARSE  NULL,
	[StatusRemark] [varchar](max) SPARSE  NULL,
	[LastRunTime] [datetime] SPARSE  NULL,
	[NextRunDate] [datetime] SPARSE  NULL,
	[CreatedBy] [varchar](20) SPARSE  NULL,
	[CreatedDate] [datetime] SPARSE  NULL,
	[ModifiedBy] [varchar](20) SPARSE  NULL,
	[ModifiedDate] [datetime] SPARSE  NULL,
	[BankID] [varchar](200) NULL,
	[WinSCPExePath] [varchar](800) NULL,
	[WinSCP_User] [varchar](200) NULL,
	[WinSCP_PWD] [varchar](200) NULL,
	[WinSCP_IP] [varchar](100) NULL,
	[WinSCP_Port] [varchar](20) NULL,
	[WinSCP_LogPath] [varchar](800) NULL,
	[CustAcctFileUploadBatch] [varchar](200) NULL,
	[CustAcctfileInputPath] [varchar](800) NULL,
	[CustAcctfileDestPath] [varchar](800) NULL,
	[CardFileUploadBatch] [varchar](200) NULL,
	[BatchFilesPath] [varchar](800) NULL,
	[CardfileInputPath] [varchar](800) NULL,
	[CardfileDestPath] [varchar](800) NULL,
	[GetCardOutputBatch] [varchar](200) NULL,
	[OutCardFile_InputPath] [varchar](800) NULL,
	[OutCardFile_DestPath] [varchar](800) NULL,
	[OutCardFile_BackUpPath] [varchar](800) NULL,
	[CardOutput] [varchar](max) NULL,
	[CardError] [varchar](max) NULL,
	[Standard3Batch] [varchar](200) NULL,
	[RCVREmailID] [nvarchar](1000) NULL,
	[NotificationEmailID] [nvarchar](2000) NULL,
	[GetFileSizeBatch] [varchar](4000) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
