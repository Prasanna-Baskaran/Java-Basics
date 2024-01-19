 
CREATE TABLE [dbo].[CardAutomationStandard4Log](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [varchar](20) NULL,
	[FileId] [varchar](20) NULL,
	[ProcessId] [varchar](10) NULL,
	[CardProgram] [varchar](100) NULL,
	[Message] [varchar](max) NULL,
	[MessageData] [varchar](max) NULL,
	[InsertedOn] [datetime] NULL
)  