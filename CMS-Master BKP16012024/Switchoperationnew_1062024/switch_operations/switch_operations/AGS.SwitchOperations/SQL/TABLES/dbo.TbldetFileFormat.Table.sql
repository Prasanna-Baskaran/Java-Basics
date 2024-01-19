USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TbldetFileFormat]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TbldetFileFormat](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [bigint] NULL,
	[FileFormat] [varchar](500) NULL,
	[FieldName] [varchar](800) NULL,
	[DataType] [varchar](500) NULL,
	[StartPos] [varchar](500) NULL,
	[EndPos] [varchar](500) NULL,
	[Length] [varchar](500) NULL,
	[SPParameter] [varchar](500) NULL,
 CONSTRAINT [PK_TbldetFileFormat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
