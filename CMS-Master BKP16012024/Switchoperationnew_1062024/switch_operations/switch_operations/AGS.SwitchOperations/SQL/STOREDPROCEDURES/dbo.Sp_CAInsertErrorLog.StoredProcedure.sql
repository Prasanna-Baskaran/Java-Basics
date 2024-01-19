USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CAInsertErrorLog]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[Sp_CAInsertErrorLog]
(
	@ProcedureName	VARCHAR(800),
	@ErrorDesc		VARCHAR(MAX),
	@ParameterList	VARCHAR(MAX) = '',
	@IssuerNo VARCHAR(100)='',
	@BatchNo VARCHAR(200)=''
)
AS
BEGIN
	SET	NOCOUNT	ON;
	INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo,BatchNo) 
	VALUES(@procedureName,@errorDesc,GETDATE(),@parameterList,@IssuerNo,@BatchNo)
END
GO
