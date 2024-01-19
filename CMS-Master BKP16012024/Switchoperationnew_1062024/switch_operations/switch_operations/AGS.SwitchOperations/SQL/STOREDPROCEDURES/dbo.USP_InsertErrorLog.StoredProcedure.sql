USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertErrorLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[USP_InsertErrorLog]
(
	@ProcedureName	VARCHAR(200),
	@ErrorDesc		VARCHAR(MAX),
	@ParameterList	VARCHAR(MAX) = NULL
)
AS
BEGIN
	SET	NOCOUNT	ON;
	INSERT INTO TblErrorDetail([Procedure_Name],Error_Desc,Error_Date,ParameterList) 
	VALUES(@procedureName,@errorDesc,GETDATE(),@parameterList)
END
GO
