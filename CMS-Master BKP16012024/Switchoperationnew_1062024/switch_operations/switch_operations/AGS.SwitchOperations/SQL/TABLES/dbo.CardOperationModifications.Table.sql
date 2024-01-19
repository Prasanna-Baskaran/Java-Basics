USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[CardOperationModifications]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CardOperationModifications](
	[Code] [bigint] IDENTITY(1,1) NOT NULL,
	[Bank] [int] NULL,
	[CustId] [varchar](50) NULL,
	[Previous] [varchar](500) NULL,
	[Updated] [varchar](500) NULL,
	[Login] [bigint] NULL,
	[UpdatedOn] [datetime] NULL,
	[PAN] [varchar](200) NULL,
	[Remark] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CardOperationModifications] ADD  DEFAULT (getdate()) FOR [UpdatedOn]
GO
