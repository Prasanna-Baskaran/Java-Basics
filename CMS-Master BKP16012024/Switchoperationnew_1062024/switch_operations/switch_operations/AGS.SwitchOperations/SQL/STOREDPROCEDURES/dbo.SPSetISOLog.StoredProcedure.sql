USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPSetISOLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[SPSetISOLog]
(
	@StrFunName 	VARCHAR(500),
	@StrParam		VARCHAR(MAX),
	@StrISO		VARCHAR(MAX),
	@StrOutput	VARCHAR(MAX) = NULL
)
AS
BEGIN
	SET	NOCOUNT	ON;
	INSERT INTO [TblISOLog]([FunctionName],[Parameter],[ISOString],[OutPutMsg],[CreatedDate]) 
	VALUES(@StrFunName,@StrParam,@StrISO,@StrOutput,GETDATE())
END
GO
