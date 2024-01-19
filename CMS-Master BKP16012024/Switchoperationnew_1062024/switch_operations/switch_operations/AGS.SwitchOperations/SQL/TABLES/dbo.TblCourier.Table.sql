USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCourier]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCourier](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CourierName] [varchar](50) NULL,
	[OfficeAddress] [varchar](200) NULL,
	[ContactNo] [varchar](12) NULL,
	[Status] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[SystemID] [varchar](200) NULL,
	[BankID] [varchar](200) NULL,
 CONSTRAINT [PK_TblCourier] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
