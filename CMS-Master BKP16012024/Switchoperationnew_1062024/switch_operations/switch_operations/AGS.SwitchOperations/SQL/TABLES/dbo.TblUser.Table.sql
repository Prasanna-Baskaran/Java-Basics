USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TblUser]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblUser](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[UserName] [varchar](50) NULL,
	[UserRoleID] [int] NULL,
	[MobileNo] [varchar](12) NULL,
	[EmailId] [varchar](100) NULL,
	[IsActive] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[UsrSessionKey] [varchar](200) NULL,
	[UserPassword] [varbinary](200) NULL,
	[SystemID] [varchar](500) NULL,
	[BankID] [varchar](20) NULL,
	[FailCount] [smallint] NULL,
 CONSTRAINT [PK_TblUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
