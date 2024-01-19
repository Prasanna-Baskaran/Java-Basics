USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblCustomerAccount]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCustomerAccount](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerId] [varchar](50) NULL,
	[AccountNo] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
