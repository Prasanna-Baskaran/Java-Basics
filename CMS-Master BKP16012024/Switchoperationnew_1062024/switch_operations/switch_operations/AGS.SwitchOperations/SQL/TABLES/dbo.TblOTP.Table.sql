USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblOTP]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblOTP](
	[SrNo] [bigint] IDENTITY(1,1) NOT NULL,
	[MobileNo] [varchar](10) NULL,
	[CardNumber] [varbinary](max) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[ModifyDateTime] [datetime] NULL,
	[ISACTIVE] [bit] NULL,
	[ISSUCCESS] [bit] NULL,
	[OTP] [varbinary](max) NULL,
	[CardHolderID] [varchar](100) NULL,
	[TryCount] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TblOTP] ADD  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[TblOTP] ADD  DEFAULT ((1)) FOR [ISACTIVE]
GO
ALTER TABLE [dbo].[TblOTP] ADD  DEFAULT ((0)) FOR [ISSUCCESS]
GO
ALTER TABLE [dbo].[TblOTP] ADD  DEFAULT ((0)) FOR [TryCount]
GO
