USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPGetDetails_20180213]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROC [dbo].[SPGetDetails_20180213]

AS
BEGIN
Begin Try
	Select MassModuleCode,ModuleName,Frequency,FrequencyUnit,EnableState,[Status],LastRunTime,NextRunDate,DllPath,ClassName,Isnull(IssuerNum,0) IssuerNum,CreatedBy,CreatedDate
	From TblMassModule With(Nolock)
	Where isnull(NextRunDate,'')<=GETDATE() And EnableState=1 AND Isnull([Status],0)<>2
	--NextRunDate<=GETDATE() And 	EnableState=1 --AND [Status] In (1,3)
 End Try  
	 BEGIN CATCH 
		SELECT   
			ERROR_NUMBER() AS ErrorNumber 
			,ERROR_PROCEDURE() AS ErrorProcedure  
			,ERROR_LINE() AS ErrorLine  
			,ERROR_MESSAGE() AS ErrorMessage;  
  
	--	 INSERT INTO TBLERRORDETAIL([Procedure_Name],Error_Desc,Error_Date,ParameterList)                                        
 --VALUES(ERROR_PROCEDURE(),ERROR_MESSAGE(),GETDATE()                                        
 --,'@StrPriMerchantID= '+ISNULL(@StrPriMerchantID,'')+',@StrMerchantRefNo= '+ISNULL(@StrMerchantRefNo,'')+                                        
 --',@IntTraceID= '+Cast(ISNULL(@IntTraceID,0) as varchar(20)) )     
		    
		    
	END CATCH;  
END

GO
