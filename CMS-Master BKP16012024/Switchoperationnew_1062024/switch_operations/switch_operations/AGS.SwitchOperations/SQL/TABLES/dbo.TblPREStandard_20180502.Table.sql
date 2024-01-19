USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblPREStandard_20180502]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblPREStandard_20180502](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](50) NULL,
	[BankID] [varchar](200) NULL,
	[CardProgram] [varchar](200) NULL,
	[Token] [varchar](200) NULL,
	[OutputPosition] [varchar](20) NULL,
	[Padding] [varchar](20) NULL,
	[PadChar] [varchar](20) NULL,
	[FixLength] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[BinID] [bigint] NULL,
	[StartPos] [varchar](20) NULL,
	[EndPos] [varchar](20) NULL,
	[Direction] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
