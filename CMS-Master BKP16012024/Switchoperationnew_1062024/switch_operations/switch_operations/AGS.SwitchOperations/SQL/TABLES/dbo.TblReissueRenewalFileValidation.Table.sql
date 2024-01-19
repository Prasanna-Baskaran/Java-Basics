USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblReissueRenewalFileValidation]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblReissueRenewalFileValidation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](5) NULL,
	[CardNoValidation] [varchar](100) NULL,
	[BINValidation] [varchar](100) NULL,
	[CIFIDValidation] [varchar](100) NULL,
	[AccountValidation] [varchar](100) NULL,
	[CardLength] [int] NULL,
	[BinLength] [int] NULL,
	[AccountLength] [int] NULL,
	[CIFLength] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
