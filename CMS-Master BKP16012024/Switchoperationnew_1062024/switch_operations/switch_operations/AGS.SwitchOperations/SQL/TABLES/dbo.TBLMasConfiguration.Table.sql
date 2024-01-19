USE [SwitchOperations]
GO
/****** Object:  Table [dbo].[TBLMasConfiguration]    Script Date: 14-06-2018 15:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[TBLMasConfiguration](
	[Code] [int] IDENTITY(1,1) NOT NULL,
	[BankId] [int] NULL,
	[IssuerNr] [int] NULL,
	[ServerIp] [varchar](20) NULL,
	[ServerPort] [int] NULL,
	[FilePathInput] [varchar](max) NULL,
	[FilePathOutPut] [varchar](max) NULL,
	[FilePathArchive] [varchar](max) NULL,
	[Username] [varchar](100) NULL,
	[password] [varbinary](max) NULL,
	[keyPath] [varchar](100) NULL,
	[Keypassphrase] [varbinary](max) NULL,
	[sequence] [int] NULL,
	[Enable] [bit] NULL,
	[filepath] [varchar](max) NULL,
	[FileHeader] [varchar](max) NULL,
	[FilePathInput_RePIN] [varchar](max) NULL,
	[FilePathOutPut_RePIN] [varchar](max) NULL,
	[FilePathArchive_RePIN] [varchar](max) NULL,
	[FilePath_RePIN] [varchar](max) NULL,
	[fileHeader_RePIN] [varchar](max) NULL,
	[IsPGP] [bit] NOT NULL,
	[Trace] [bit] NOT NULL,
	[PublicKeyFilePath] [varchar](max) NULL,
	[PrivateKeyFilePath] [varchar](max) NULL,
	[Password_PGP] [varbinary](max) NULL,
	[InputFilePath_PGP] [varchar](max) NULL,
	[FiledCount] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[TBLMasConfiguration] ADD [DataValidation_SP] [varchar](max) NULL

GO
SET ANSI_PADDING OFF
GO
