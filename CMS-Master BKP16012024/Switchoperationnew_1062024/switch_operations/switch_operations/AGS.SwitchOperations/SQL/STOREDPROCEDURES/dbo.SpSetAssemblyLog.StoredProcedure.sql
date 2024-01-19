USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetAssemblyLog]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[SpSetAssemblyLog]       
 @RequestData varchar(max)='' ,
 @StrPriOutPutData varchar(max) =''
  
AS  
BEGIN  
   	Begin Transaction  
	Begin Try  

 Insert Into TblAssemblyLog (RequestData,ResponseData,RequestDateTime,ResponseDateTime) Values   
   (@RequestData,@StrPriOutPutData,GETDATE(),GETDATE()) 

   select '00' StatusCode,SCOPE_IDENTITY() ScopeIdentity,'Success' Description
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
