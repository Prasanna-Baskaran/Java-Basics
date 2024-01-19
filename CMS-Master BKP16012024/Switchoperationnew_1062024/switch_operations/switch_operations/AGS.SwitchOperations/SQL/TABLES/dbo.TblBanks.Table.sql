USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblBanks]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBanks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BankName] [varchar](500) NULL,
	[BankCode] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[SystemID] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[TblBanks] ADD [CustIdentity] [varchar](20) NULL
ALTER TABLE [dbo].[TblBanks] ADD [CustomerIDLen] [varchar](20) NULL
SET ANSI_PADDING ON
ALTER TABLE [dbo].[TblBanks] ADD [SourceNodes] [varchar](max) NULL
ALTER TABLE [dbo].[TblBanks] ADD [SinkNodes] [varchar](max) NULL
ALTER TABLE [dbo].[TblBanks] ADD [UserPrefix] [varchar](10) NULL
ALTER TABLE [dbo].[TblBanks] ADD [ValidationSP] [varchar](500) NULL
ALTER TABLE [dbo].[TblBanks] ADD [IsBATLink] [bit] NULL
ALTER TABLE [dbo].[TblBanks] ADD [Validate_Add_SP] [varchar](500) NULL
ALTER TABLE [dbo].[TblBanks] ADD [SchemeCodeBased] [smallint] NULL
ALTER TABLE [dbo].[TblBanks] ADD [SourceId] [varchar](1000) NULL
ALTER TABLE [dbo].[TblBanks] ADD [SyncCustomerData] [bit] NULL
ALTER TABLE [dbo].[TblBanks] ADD [IsAccountBalanceFile] [bit] NULL
 CONSTRAINT [PK_TblBanks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
