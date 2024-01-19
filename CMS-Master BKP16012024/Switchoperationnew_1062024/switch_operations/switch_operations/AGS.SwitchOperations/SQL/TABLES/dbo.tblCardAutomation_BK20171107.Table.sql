USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblCardAutomation_BK20171107]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCardAutomation_BK20171107](
	[Code] [bigint] IDENTITY(1,1) NOT NULL,
	[Bank] [varchar](1000) NULL,
	[IssuerNo] [int] NULL,
	[SwitchInstitutionID] [varchar](11) NULL,
	[SourceIP] [varchar](100) NULL,
	[SourcePort] [varchar](10) NULL,
	[SourcePath] [varchar](max) NULL,
	[OutputPath] [varchar](max) NULL,
	[Standard2Batch] [varchar](max) NULL,
	[Standard2ExePath] [varchar](max) NULL,
	[Standard2Input] [varchar](max) NULL,
	[Standard2Archive] [varchar](max) NULL,
	[Standard4Batch] [varchar](max) NULL,
	[Standard4ExePath] [varchar](max) NULL,
	[Standard4Output] [varchar](max) NULL,
	[CardProcessType] [smallint] NULL,
	[EnableState] [bit] NULL,
	[Status] [smallint] NULL,
	[StatusRemark] [varchar](max) NULL,
	[LastRunTime] [datetime] NULL,
	[NextRunDate] [datetime] NULL,
	[CreatedBy] [varchar](20) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [varchar](20) NULL,
	[ModifiedDate] [datetime] NULL,
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
	[RCVREmailID] [nvarchar](1000) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
