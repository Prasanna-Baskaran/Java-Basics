USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_SaveFileInfo]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATe PROCEDURE [dbo].[SP_SaveFileInfo]
	@FileInfo FileInfoType readonly
AS
BEGIN
 Begin Transaction  
 Begin Try   

	INSERT INTO TbldetFileUpload (FileID,[FileName],FullFilePath,UploadedDate,[Status])
	SELECT FileID,[FileName],FilePath,GETDATE(),0 from @FileInfo

	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),''
		   
	END CATCH;  
END

GO
