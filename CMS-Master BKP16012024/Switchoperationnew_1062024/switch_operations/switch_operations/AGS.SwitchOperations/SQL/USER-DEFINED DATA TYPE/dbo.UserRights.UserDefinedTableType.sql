USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[UserRights]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[UserRights] AS TABLE(
	[RoleId] [int] NULL,
	[OptionNeumonic] [char](20) NULL,
	[AccessCaption] [varchar](100) NULL
)
GO
