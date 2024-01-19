USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SaveKYCDocuments]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_SaveKYCDocuments]
	@CustomerID BIGINT=0,
	@Signature VARCHAR(200)='',
	@Photo VARCHAR(200)='',
	@IdProof VARCHAR(200)=''
AS
BEGIN
Begin Transaction  
	Begin Try    		

	 Declare @StrPriOutput varchar(1)='1'		
      Declare @StrPriOutputDesc varchar(200)='KYC Documents are not saved'

	IF(@CustomerID <>0)
	BEGIN
	   IF NOT EXISTS(SELECT 1 FROM TblKYCDocuments WITH(NOLOCK) WHERE CustomerID=@CustomerID)
	    BEGIN
	         INSERT INTO  TblKYCDocuments (CustomerID,SignatureFileName,Photo,IDProof) VALUES(@CustomerID,@Signature,@Photo,@IdProof) 
			 IF(@@ROWCOUNT>0)
			  BEGIN
			   SET @StrPriOutput='0'
		       SET  @StrPriOutputDesc ='KYC Documents are saved'
			  END
			  ELSE
			  BEGIN
			   SET @StrPriOutput='0'
		       SET  @StrPriOutputDesc ='KYC Documents are not saved'
			  END
		END
		ELSE
		BEGIN
		  UPDATE TblKYCDocuments SET SignatureFileName=@Signature,Photo=@Photo,IDProof=@IdProof WHERE CustomerID=@CustomerID
		     SET @StrPriOutput='0'
		     SET  @StrPriOutputDesc ='KYC Documents are saved'
		END	    
	END
	ELSE
	BEGIN
	  SET @StrPriOutput='1'
	  SET  @StrPriOutputDesc ='KYC Documents are not saved'
	END

COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 

		SELECT 1  As Code,'KYC Documents are not saved' As [OutputDescription]

	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	

	
END

GO
