USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardType]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardType](
	[CardTypeID] [int] IDENTITY(1,1) NOT NULL,
	[CardTypeName] [varchar](20) NULL,
	[CardTypeDesc] [varchar](50) NULL,
	[SystemID] [varchar](200) NULL,
	[BankID] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
