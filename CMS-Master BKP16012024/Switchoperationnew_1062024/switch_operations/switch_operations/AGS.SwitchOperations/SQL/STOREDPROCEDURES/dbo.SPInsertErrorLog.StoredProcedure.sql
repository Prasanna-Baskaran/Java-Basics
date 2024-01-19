USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPInsertErrorLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[SPInsertErrorLog]
(
	@StrProcedureName	VARCHAR(500),
	@StrErrorDesc		VARCHAR(4000),
	@StrParameterList	VARCHAR(400) = NULL
)
AS
BEGIN
	SET	NOCOUNT	ON;
	INSERT INTO TblErrorDetail([Procedure_Name],Error_Desc,Error_Date,ParameterList) 
	VALUES(@StrProcedureName,@StrErrorDesc,GETDATE(),@StrParameterList)
END
GO
