USE [SwitchOperations]
GO
/****** Object:  UserDefinedTableType [dbo].[FileInfoType]    Script Date: 08-06-2018 16:58:17 ******/
CREATE TYPE [dbo].[FileInfoType] AS TABLE(
	[FileName] [varchar](500) NULL,
	[FilePath] [varchar](800) NULL,
	[FileID] [varchar](200) NULL
)
GO
