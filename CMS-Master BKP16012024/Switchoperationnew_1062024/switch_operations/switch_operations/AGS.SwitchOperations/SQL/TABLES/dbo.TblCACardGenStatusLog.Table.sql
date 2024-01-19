USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCACardGenStatusLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCACardGenStatusLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](200) NULL,
	[ProcessID] [int] NULL,
	[Status] [int] NULL,
	[SeqNo] [int] NULL,
	[ErrorDesc] [varchar](800) NULL,
	[Remark] [varchar](800) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TblCACardGenStatusLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblCACardGenStatusLog] ADD  CONSTRAINT [DF_TblCACardGenStatusLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
