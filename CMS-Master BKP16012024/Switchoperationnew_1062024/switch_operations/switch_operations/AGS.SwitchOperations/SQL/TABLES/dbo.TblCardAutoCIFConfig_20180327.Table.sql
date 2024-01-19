USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardAutoCIFConfig_20180327]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardAutoCIFConfig_20180327](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CardProgram] [varchar](800) NULL,
	[BankID] [varchar](800) NULL,
	[IsMandatory] [varchar](800) NULL,
	[MinLen] [varchar](200) NULL,
	[MaxLen] [varchar](200) NULL,
	[FieldName] [varchar](200) NULL,
	[IsNum] [bit] NULL,
	[IsAlpha] [bit] NULL,
	[IsAlphanumeric] [bit] NULL,
	[FixedValue] [varchar](800) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
