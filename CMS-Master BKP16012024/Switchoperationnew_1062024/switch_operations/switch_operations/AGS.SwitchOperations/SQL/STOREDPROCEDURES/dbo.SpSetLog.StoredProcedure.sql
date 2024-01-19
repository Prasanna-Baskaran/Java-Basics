USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SpSetLog]   
    
 @StrPriTransactionType varchar(20)=null,  
 @StrPriRequestData varchar(max)=null ,
 @StrPriOutPutData varchar(max) =null,
 @RequestData  varchar(max) =null

AS  
BEGIN  
   	Begin Transaction  
	Begin Try  
	if (@StrPriTransactionType = null)
	Begin
		set @StrPriTransactionType = ''
	End
	if (@StrPriRequestData = null)
	Begin
		if(@RequestData = null)
		set @StrPriRequestData = ''
		else
		set @StrPriRequestData = @RequestData
	End
	if (@StrPriOutPutData = null)
	Begin
		set @StrPriOutPutData = ''
	End
	 Insert Into TblLog (TransactionType,RequestData,OutPutMsg) Values   
   (@StrPriTransactionType,@StrPriRequestData,@StrPriOutPutData) 

		COMMIT TRANSACTION;    

 End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT   
			ERROR_NUMBER() AS ErrorNumber  
			,ERROR_SEVERITY() AS ErrorSeverity  
			,ERROR_STATE() AS ErrorState  
			,ERROR_PROCEDURE() AS ErrorProcedure  
			,ERROR_LINE() AS ErrorLine  
			,ERROR_MESSAGE() AS ErrorMessage;  
  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  
   
END  
  
  
GO
