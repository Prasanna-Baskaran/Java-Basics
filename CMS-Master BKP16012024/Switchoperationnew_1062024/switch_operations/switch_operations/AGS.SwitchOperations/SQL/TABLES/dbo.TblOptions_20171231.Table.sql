USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblOptions_20171231]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblOptions_20171231](
	[OptionId] [int] IDENTITY(1,1) NOT NULL,
	[OptionNeumonic] [char](20) NOT NULL,
	[OptionName] [varchar](50) NULL,
	[OptionParentNeumonic] [char](20) NULL,
	[URL] [varchar](max) NULL,
	[Active] [bit] NULL,
	[DisplayOrder] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CaptionButtons] [varchar](100) NULL,
	[GlyphiconClass] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
