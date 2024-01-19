USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardFeeMaster_Generic]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardFeeMaster_Generic](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [int] NULL,
	[Bin] [varchar](8) NULL,
	[SchemeCode] [varchar](20) NULL,
	[CardProgram] [varchar](50) NULL,
	[FeeAmount] [varchar](20) NULL,
	[FeeCategory] [varchar](10) NULL,
	[FeesType] [bit] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
