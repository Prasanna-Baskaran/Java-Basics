USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblBulkFileConfig]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBulkFileConfig](
	[ID] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[FileType] [int] NULL,
	[BankID] [varchar](200) NULL,
	[FieldName] [varchar](200) NULL,
	[IsMandatory] [bit] NULL,
	[MinLen] [varchar](200) NULL,
	[MaxLen] [varchar](200) NULL,
	[IsNum] [bit] NULL,
	[IsAlpha] [bit] NULL,
	[IsAlphanumeric] [bit] NULL,
	[IsEmail] [bit] NULL,
	[IsDateValue] [bit] NULL,
	[FixedValue] [varchar](800) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_TblBulkFileConfig] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblBulkFileConfig] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
