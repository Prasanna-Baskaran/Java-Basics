USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[ccc]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ccc](
	[OldCardNumber] [varchar](200) NULL,
	[HoldRSPCode] [varchar](200) NULL,
	[NewBINPrefix] [varchar](200) NULL,
	[Remark] [varchar](800) NULL,
	[Reason] [varchar](800) NULL,
	[Extra1] [varchar](800) NULL,
	[Extra2] [varchar](800) NULL,
	[Extra3] [varchar](800) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
