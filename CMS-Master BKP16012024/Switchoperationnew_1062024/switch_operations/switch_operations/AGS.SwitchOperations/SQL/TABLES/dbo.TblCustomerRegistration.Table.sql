USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblCustomerRegistration]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblCustomerRegistration](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [varchar](255) NOT NULL,
	[CardNo] [varchar](255) NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[LastName] [varchar](255) NULL,
	[EmailID] [varchar](255) NULL,
	[Q1] [varchar](max) NULL,
	[Q2] [varchar](max) NULL,
	[CVV] [varchar](255) NULL,
	[CreatedBy] [varchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[Password] [varchar](200) NULL,
	[MobileNo] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
