USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblBulkFileUploadHistory]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBulkFileUploadHistory](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[UploadID] [varchar](1000) NULL,
	[FileID] [varchar](1000) NULL,
	[FileType] [varchar](200) NULL,
	[OldCardNumber] [varchar](200) NULL,
	[HoldRSPCode] [varchar](200) NULL,
	[NewBINPrefix] [varchar](200) NULL,
	[Remark] [varchar](800) NULL,
	[Extra1] [varchar](800) NULL,
	[Extra2] [varchar](800) NULL,
	[Extra3] [varchar](800) NULL,
	[ProcessID] [varchar](200) NULL,
	[IsRejected] [bit] NULL,
	[RejectReason] [varchar](800) NULL,
	[IsProcessed] [bit] NULL,
	[BatchNo] [varchar](100) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_TblBulkFileUploadHistory] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
