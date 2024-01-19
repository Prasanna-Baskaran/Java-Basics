 
CREATE TABLE [dbo].[CardProgramsForStandard4](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IssuerNo] [numeric](18, 0) NULL,
	[FileId] [varchar](50) NULL,
	[ProcessId] [bigint] NULL,
	[BinPrefix] [varchar](20) NULL,
	[CardProgram] [varchar](100) NULL,
	[RspCode] [varchar](10) NULL,
	[RspDesc] [varchar](200) NULL,
	[Processed] [char](3) NULL,
	[InsertedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL
) 