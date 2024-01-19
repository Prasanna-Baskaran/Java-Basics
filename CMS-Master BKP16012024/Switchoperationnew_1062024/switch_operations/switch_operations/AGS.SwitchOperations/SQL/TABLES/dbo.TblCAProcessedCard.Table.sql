USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCAProcessedCard]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCAProcessedCard](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[UniqueID] [varchar](100) NULL,
	[CardCustomerID] [varchar](50) NULL,
	[CardSequenceNum] [varchar](10) NULL,
	[CardStatus] [varchar](5) NULL,
	[CardStatusRemark] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[IssuerNum] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblCAProcessedCard] ADD  CONSTRAINT [DF_TblCAProcessedCard_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
