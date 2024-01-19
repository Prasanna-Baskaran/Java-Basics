USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblOptions]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblOptions](
	[OptionId] [int] IDENTITY(1,1) NOT NULL,
	[OptionNeumonic] [char](20) NOT NULL,
	[OptionName] [varchar](50) NULL,
	[OptionParentNeumonic] [char](20) NULL,
	[URL] [varchar](max) NULL,
	[Active] [bit] NULL,
	[DisplayOrder] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CaptionButtons] [varchar](100) NULL,
	[GlyphiconClass] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[OptionNeumonic] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblOptions] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[TblOptions] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
