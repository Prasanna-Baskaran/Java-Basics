USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[tblbanks_20180326]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblbanks_20180326](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BankName] [varchar](500) NULL,
	[BankCode] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[SystemID] [varchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[tblbanks_20180326] ADD [CustIdentity] [varchar](20) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [CustomerIDLen] [varchar](20) NULL
SET ANSI_PADDING ON
ALTER TABLE [dbo].[tblbanks_20180326] ADD [SourceNodes] [varchar](max) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [SinkNodes] [varchar](max) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [UserPrefix] [varchar](10) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [ValidationSP] [varchar](500) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [IsBATLink] [bit] NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [Validate_Add_SP] [varchar](500) NULL
ALTER TABLE [dbo].[tblbanks_20180326] ADD [SchemeCodeBased] [smallint] NULL

GO
SET ANSI_PADDING OFF
GO
