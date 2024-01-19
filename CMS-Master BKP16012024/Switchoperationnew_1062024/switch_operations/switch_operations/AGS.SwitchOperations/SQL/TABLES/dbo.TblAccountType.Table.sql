USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblAccountType]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblAccountType](
	[AccountTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AccountTypeName] [varchar](100) NULL,
	[AccountTypeCode] [varchar](2) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
