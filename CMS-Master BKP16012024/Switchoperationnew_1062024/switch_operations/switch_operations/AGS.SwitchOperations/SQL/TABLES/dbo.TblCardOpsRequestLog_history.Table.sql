USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCardOpsRequestLog_history]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCardOpsRequestLog_history](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RequestTypeID] [int] NULL,
	[CustomerID] [bigint] NULL,
	[CardRPANID] [bigint] NULL,
	[FormStatusID] [int] NULL,
	[MakerID] [bigint] NULL,
	[ModifiedByID] [bigint] NULL,
	[ModifiedDate] [datetime] NULL,
	[CheckerID] [bigint] NULL,
	[CheckedDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[Remark] [varchar](200) NULL,
	[RejectReason] [varchar](200) NULL,
	[IsSuccess] [smallint] NULL,
	[SwitchHRC] [varchar](10) NULL,
	[SwitchResponse] [varchar](200) NULL,
	[SystemID] [int] NULL,
	[BankID] [varchar](20) NULL,
 CONSTRAINT [PK_TblCardOpsRequestLog_history] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
