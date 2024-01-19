USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpMvisaLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpMvisaLog]   
    
 @StrPriTransactionType varchar(20),  
 @StrPriRequestData varchar(max) ,
 @StrPriOutPutData varchar(max) 

 
  
AS  
BEGIN  
Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Saving failed.'
   	Begin Transaction  
	Begin Try  

 Insert Into TblMvisaLog (TransactionType,RequestData,OutPutMsg) Values  (@StrPriTransactionType,@StrPriRequestData,@StrPriOutPutData) 
 Set @StrPriOutput='0'
		 Set @StrPriOutputDesc='Log Save successfully.'
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
