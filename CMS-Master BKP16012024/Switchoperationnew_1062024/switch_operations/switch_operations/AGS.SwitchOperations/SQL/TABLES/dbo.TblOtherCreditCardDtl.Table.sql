USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblOtherCreditCardDtl]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblOtherCreditCardDtl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [bigint] NULL,
	[CardType] [varchar](50) NULL,
	[IssuedBy] [varchar](50) NULL,
	[IssuedDate] [datetime] NULL,
	[Limit] [varchar](50) NULL,
	[Overdue] [varchar](50) NULL,
	[ExpiryDate] [datetime] NULL,
 CONSTRAINT [PK_TblOtherCreditCardDtl_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
