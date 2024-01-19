USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardProductionReissue]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardProductionReissue](
	[Code] [bigint] IDENTITY(1,1) NOT NULL,
	[Bank] [int] NULL,
	[PAN] [varchar](200) NULL,
	[Remarks] [varchar](200) NULL,
	[RequestedOn] [datetime] NULL,
	[RequestedBy] [bigint] NULL,
	[Processed] [bit] NULL,
	[ProcessedOn] [datetime] NULL,
	[ProcessedBy] [bigint] NULL,
	[Rejected] [bit] NULL,
	[RejectedOn] [datetime] NULL,
	[RejectedBy] [bigint] NULL,
	[RejectedReason] [varchar](200) NULL,
	[Generated] [bit] NULL,
	[GeneratedOn] [datetime] NULL,
	[GeneratedBy] [bigint] NULL,
	[BIN] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CardProductionReissue] ADD  DEFAULT (getdate()) FOR [RequestedOn]
GO
ALTER TABLE [dbo].[CardProductionReissue] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [dbo].[CardProductionReissue] ADD  DEFAULT ((0)) FOR [Rejected]
GO
ALTER TABLE [dbo].[CardProductionReissue] ADD  DEFAULT ((0)) FOR [Generated]
GO
