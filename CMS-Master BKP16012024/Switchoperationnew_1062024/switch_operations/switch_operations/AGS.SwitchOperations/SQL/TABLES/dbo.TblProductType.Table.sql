USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblProductType]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblProductType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CardType_ID] [int] NULL,
	[BIN_ID] [int] NULL,
	[INST_ID] [int] NULL,
	[ProductType] [varchar](200) NULL,
	[ProductTypeDesc] [varchar](50) NULL,
	[FormStatusID] [int] NULL,
	[MakerID] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[CheckerID] [bigint] NULL,
	[CheckedDate] [datetime] NULL,
	[Remark] [varchar](50) NULL,
	[ModifiedByID] [bigint] NULL,
	[SystemID] [varchar](200) NULL,
	[BankID] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
