USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsertUpdateBin]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_InsertUpdateBin]
	@ID INT =0,
	@BIN VARCHAR(20)='',
	@INSTID VARCHAR(20)='',
	@BINDesc VARCHAR(50)='',
	@MakerID BIGINT=NULL    

AS
BEGIN
Begin Transaction  
	Begin Try
	Declare @StrPriOutput varchar(1)='1'					
	Declare @StrPriOutputDesc varchar(200)='Bin information is not saved'

  IF(@BIN <> '' AND @INSTID <>'' AND @BINDesc <>'')
  BEGIN  
    IF(@ID = 0)
	 BEGIN
	 --Add new Bin
	   If NOT EXISTS (SELECT 1 FROM TblBIN WITH(NOLOCK) WHERE RTRIM(LTRIM(CardPrefix))=RTRIM(LTRIM(@BIN)) AND InstitutionID=@INSTID )
	   BEGIN
	
	     INSERT INTO  TblBIN (CardPrefix,InstitutionID,FormStatusID,MakerID,CreatedDate,BinDesc) VALUES(@BIN,@INSTID,0,@MakerID,GETDATE(),@BINDesc)
		 
		 IF(@@ROWCOUNT>0)
		 BEGIN
	
		    SET @StrPriOutput ='0'					
	        SET @StrPriOutputDesc ='Bin information is saved'
		 END
		 ELSE
		  BEGIN
		    SET @StrPriOutput ='1'					
	        SET @StrPriOutputDesc ='Bin information is not saved'
		  END
	   END
	   ELSE
	   BEGIN
	       SET @StrPriOutput ='1'					
	       SET @StrPriOutputDesc ='Bin information is already exists'
	   END
	 END
	 --Update Bin
	 ELSE IF EXISTS(SELECT 1 FROM TblBIN WITH(NOLOCK) WHERE ID=@ID)
	 BEGIN
	   UPDATE TblBIN SET BinDesc=@BINDesc,CardPrefix=@BIN,InstitutionID=@INSTID,MakerID=@MakerID,ModifiedDate=GETDATE(),FormStatusID=0,Remark='' WHERE ID=@ID 
	     SET @StrPriOutput ='0'					
	     SET @StrPriOutputDesc ='Bin information is saved'
	 END
	 ELSE
	 BEGIN
	      SET @StrPriOutput ='1'					
	      SET @StrPriOutputDesc ='Bin information is not saved'
	 END
  END
  ELSE
  BEGIN
     SET @StrPriOutput ='1'					
	SET @StrPriOutputDesc ='Bin information is not saved'
  END

  Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
	 		SELECT 1  As Code,'Bin information is not saved' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  



END

GO
