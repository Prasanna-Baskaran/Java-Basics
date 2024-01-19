USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblUpdateCustDetailsLog]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblUpdateCustDetailsLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [varchar](50) NULL,
	[OldDetails] [varchar](800) NULL,
	[NewDetails] [varchar](800) NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TblUpdateCustDetailsLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
