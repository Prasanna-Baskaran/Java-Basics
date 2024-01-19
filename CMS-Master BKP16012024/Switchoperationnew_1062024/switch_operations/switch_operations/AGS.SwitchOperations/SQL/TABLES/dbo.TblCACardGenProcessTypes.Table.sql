USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCACardGenProcessTypes]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCACardGenProcessTypes](
	[ProcessID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessType] [varchar](200) NULL,
	[Filename] [varchar](200) NULL,
	[ProcessDesc] [varchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TblCACardGenProcessTypes] PRIMARY KEY CLUSTERED 
(
	[ProcessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblCACardGenProcessTypes] ADD  CONSTRAINT [DF_TblCACardGenProcessTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
