USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_TranTemp]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[Sp_TranTemp]
 @StrRequestData VARCHAR(Max),
 @StrFunctionName VARCHAR(2000),
 @StrResponseData VARCHAR(Max)=''
 ,@StrPara1 VARCHAR(10)=NULL,
 @StrPara2 VARCHAR(10)=NULL

AS
BEGIN
	DECLARE @TotalRowAffected INT = 0;
	BEGIN TRANSACTION    
	  BEGIN TRY      

		INSERT INTO PrepaidTranTemp (FunctionName,RequestData,ResponseData,CreatedDate) VALUES (@StrFunctionName,@StrRequestData,@StrResponseData,GETDATE())

		SET @TotalRowAffected =  @@ROWCOUNT  


	    Commit Transaction  
	  End Try    
	 BEGIN CATCH    
	RollBack Transaction  
	SET @TotalRowAffected =  0
  
	  INSERT INTO tblerTblErrorDetail( ErrorNumber,Severity,State,ProcedureName,LineNumber,Message,CreatedDate)  
		VALUES (ERROR_NUMBER(),ERROR_SEVERITY(),ERROR_STATE(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),GetDate())    
	 END CATCH;  
  
  SELECT @TotalRowAffected as Code


END
GO
