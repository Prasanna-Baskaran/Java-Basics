USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardLimits_History]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardLimits_History](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseNo] [int] NULL,
	[PurchaseDailyLimit] [numeric](12, 2) NULL,
	[PurchasePTLimit] [numeric](12, 2) NULL,
	[WithDrawNO] [int] NULL,
	[WithDrawDailyLimit] [numeric](12, 2) NULL,
	[WithDrawPTLimit] [numeric](12, 2) NULL,
	[PaymentNO] [int] NULL,
	[PaymentDailyLimit] [numeric](12, 2) NULL,
	[PaymentPTLimit] [numeric](12, 2) NULL,
	[MakerID] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedByID] [bigint] NULL,
	[ModifiedDate] [datetime] NULL,
	[CheckerID] [bigint] NULL,
	[CheckedDate] [datetime] NULL,
	[FormStatusID] [int] NULL,
	[Remark] [varchar](100) NULL,
	[CustomerID] [bigint] NULL,
	[CardRPAN_ID] [bigint] NULL,
	[UpdateRemark] [varchar](200) NULL,
	[PurchaseNo_O] [int] NULL,
	[PurchaseDailyLimit_O] [numeric](12, 2) NULL,
	[PurchasePTLimit_O] [numeric](12, 2) NULL,
	[WithDrawNO_O] [int] NULL,
	[WithDrawDailyLimit_O] [numeric](12, 2) NULL,
	[WithDrawPTLimit_O] [numeric](12, 2) NULL,
	[PaymentNO_O] [int] NULL,
	[PaymentDailyLimit_O] [numeric](12, 2) NULL,
	[PaymentPTLimit_O] [numeric](12, 2) NULL,
	[CNPDailyLimit] [numeric](12, 2) NULL,
	[CNPPTLimit] [numeric](12, 2) NULL,
	[CNPDailyLimit_O] [numeric](12, 2) NULL,
	[CNPPTLimit_O] [numeric](12, 2) NULL,
	[Delete_Date] [datetime] NULL,
	[RequestTypeID] [int] NULL,
	[SwitchResponse] [varchar](200) NULL,
	[SystemID] [int] NULL,
	[BankID] [varchar](20) NULL,
 CONSTRAINT [PK_TblCardLimits_History] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
