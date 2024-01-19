USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TbldetFileUpload]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TbldetFileUpload](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [bigint] NULL,
	[FileName] [varchar](500) NULL,
	[FullFilePath] [varchar](800) NULL,
	[UploadedDate] [datetime] NULL,
	[Status] [smallint] NULL,
	[StartDate] [varchar](500) NULL,
	[EndDate] [varchar](500) NULL,
	[ReconCount] [varchar](500) NULL,
	[SuccessCount] [varchar](500) NULL,
	[FailedCount] [varchar](50) NULL,
	[Error] [varchar](800) NULL,
 CONSTRAINT [PK_TbldetFileUpload] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
