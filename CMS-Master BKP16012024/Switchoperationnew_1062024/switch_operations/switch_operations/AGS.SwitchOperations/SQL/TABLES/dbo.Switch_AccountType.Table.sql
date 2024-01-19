USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[Switch_AccountType]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Switch_AccountType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](2) NULL,
	[AccountType] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
