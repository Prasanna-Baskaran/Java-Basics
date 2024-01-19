USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TaxDetails]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaxDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaxType] [varchar](20) NULL,
	[TaxInFixedAmount] [numeric](18, 2) NULL,
	[TaxInPercent] [numeric](18, 2) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TaxDetails] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[TaxDetails] ADD  DEFAULT (getdate()) FOR [ModifiedOn]
GO
