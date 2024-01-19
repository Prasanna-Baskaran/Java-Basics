USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblPREStandard]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblPREStandard](
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
	[Direction] [varchar](20) NULL,
 CONSTRAINT [PK_TblPREStandard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
