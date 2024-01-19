USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblInstitutionDtl]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblInstitutionDtl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InstitutionID] [varchar](50) NULL,
	[INSTDesc] [varchar](200) NULL,
	[SystemID] [varchar](200) NULL,
	[BankID] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
