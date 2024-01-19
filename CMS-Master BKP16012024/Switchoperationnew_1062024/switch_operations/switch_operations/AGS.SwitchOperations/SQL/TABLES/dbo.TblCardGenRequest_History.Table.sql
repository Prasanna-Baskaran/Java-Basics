USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardGenRequest_History]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardGenRequest_History](
	[Code] [numeric](28, 0) IDENTITY(1,1) NOT NULL,
	[ID] [numeric](28, 0) NULL,
	[CustomerID] [varchar](800) NULL,
	[OldCardRPANID] [varchar](800) NULL,
	[NewBinPrefix] [varchar](800) NULL,
	[HoldRSPCode] [varchar](800) NULL,
	[RSPCode] [varchar](800) NULL,
	[SwitchResponse] [varchar](800) NULL,
	[STAN] [varchar](200) NULL,
	[RRN] [varchar](200) NULL,
	[AuthID] [varchar](200) NULL,
	[Remark] [varchar](800) NULL,
	[FormStatusID] [int] NULL,
	[IsRejected] [smallint] NULL,
	[RejectReason] [varchar](800) NULL,
	[MakerID] [varchar](800) NULL,
	[CreatedDate] [datetime] NULL,
	[CheckerID] [varchar](800) NULL,
	[CheckedDate] [datetime] NULL,
	[IsAuthorized] [smallint] NULL,
	[UploadFileName] [varchar](800) NULL,
	[BankID] [varchar](800) NULL,
	[SystemID] [varchar](800) NULL,
	[ProcessID] [int] NULL,
	[schemecode] [varchar](100) NULL,
	[Account1] [varchar](50) NULL,
	[Account2] [varchar](50) NULL,
	[Account3] [varchar](50) NULL,
	[Account4] [varchar](50) NULL,
	[Account5] [varchar](50) NULL,
	[Reserved1] [varchar](500) NULL,
	[Reserved2] [varchar](500) NULL,
	[Reserved3] [varchar](500) NULL,
	[Reserved4] [varchar](500) NULL,
	[Reserved5] [varchar](500) NULL,
	[Branch_Code] [varchar](50) NULL,
	[ExpiryDate] [varchar](50) NULL,
	[New_Card] [varchar](50) NULL,
	[Customer_Name] [varchar](200) NULL,
	[New_Card_Activation_Date] [varchar](200) NULL,
	[AccountLinkage] [varchar](800) NULL,
	[Downloaded] [smallint] NULL,
	[Date] [datetime] NULL,
	[CardGenStatus] [varchar](20) NULL,
	[PREStatus] [varchar](20) NULL,
	[CardGenStatusRemark] [varchar](100) NULL,
	[UploadID] [varchar](800) NULL,
 CONSTRAINT [PK_TblCardGenRequest_History] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
