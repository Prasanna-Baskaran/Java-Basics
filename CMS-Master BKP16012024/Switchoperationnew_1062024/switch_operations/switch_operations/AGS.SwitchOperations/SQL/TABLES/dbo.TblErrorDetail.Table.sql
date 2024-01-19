USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblErrorDetail]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblErrorDetail](
	[SrNo] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Procedure_Name] [nvarchar](max) NULL,
	[Error_Desc] [nvarchar](max) NULL,
	[Error_Date] [datetime] NOT NULL,
	[ParameterList] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SrNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[TblErrorDetail] ADD  DEFAULT (getdate()) FOR [Error_Date]
GO
ALTER TABLE [dbo].[TblErrorDetail] ADD  DEFAULT ('') FOR [ParameterList]
GO
