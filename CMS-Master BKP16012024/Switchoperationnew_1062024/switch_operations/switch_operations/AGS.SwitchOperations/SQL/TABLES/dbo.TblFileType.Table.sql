USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblFileType]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblFileType](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileType] [varchar](500) NULL,
	[Desc] [varchar](800) NULL,
	[CreatedDate] [datetime] NULL,
	[InsertValidationSP] [varchar](200) NULL,
	[ProcessID] [int] NULL,
 CONSTRAINT [PK_TblFileType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
