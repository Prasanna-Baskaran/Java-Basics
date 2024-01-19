USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SaveMasterDtl]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_SaveMasterDtl]
	@Code VARCHAR(50),
    @Desc VARCHAR(200),
    @ID int=0,
	@IntPara int=0,
	@SystemID Varchar(200)=1,
	@BankID Varchar(200)=1
AS
BEGIN
Begin Transaction  
	Begin Try    
	   Declare @StrPriOutput varchar(1)='1'		
	   Declare @StrPriOutputDesc varchar(200)='Data is not saved'

	   --Institution Master
	   If(@IntPara=0)
	   BEGIN
	        --Add New
			IF(@ID=0)
			BEGIN
			 IF Not Exists(Select top 1 1  from TblInstitutionDtl WITH(NOLOCK) where (UPPER(REPLACE(InstitutionID,' ',''))=UPPER(REPLACE(@Code, ' ', ''))) AND SystemID=@SystemID AND BankID=@BankID)
			  BEGIN
			   INSERT INTO TblInstitutionDtl (InstitutionID,INSTDesc,SystemID,BankID) VALUES (@Code,@Desc,@SystemID,@BankID)
				 IF(@@ROWCOUNT>0)
				 BEGIN
				   SET @StrPriOutput ='0'		
				   SET @StrPriOutputDesc ='Institution information is saved'
				 END
				 ELSE
				 BEGIN
					SET @StrPriOutput ='1'		
					SET @StrPriOutputDesc ='Institution information is not saved'
				 END
			  END
			  ELSE
			  BEGIN
			    	SET @StrPriOutput ='1'		
					SET @StrPriOutputDesc ='InstitutionID is already exists'
			  END
			 
			END
			--update
			ELSE IF Exists(SELECT 1 FROM TblInstitutionDtl WITH(NOLOCK) WHERE ID=@ID AND SystemID=@SystemID AND BankID=@BankID)
			BEGIN 
			  UPDATE TblInstitutionDtl SET InstitutionID=@Code,INSTDesc=@Desc   WHERE ID=@ID AND SystemID=@SystemID AND BankID=@BankID
				   SET @StrPriOutput ='0'		
				   SET @StrPriOutputDesc ='Institution information is updated successfully'

			END
	   END
	 --Card Type Master
       ELSE If(@IntPara=1)
	    BEGIN
	        --Add New
			IF(@ID=0)
			BEGIN
			 IF Not Exists(Select top 1 1  from TblCardType WITH(NOLOCK) where (UPPER(REPLACE(CardTypeName,' ',''))=UPPER(REPLACE(@Code, ' ', ''))) AND SystemID=@SystemID AND BankID=@BankID)
			  BEGIN
			   INSERT INTO TblCardType (CardTypeName,CardTypeDesc,SystemID,BankID) VALUES (@Code,@Desc,@SystemID,@BankID)
				 IF(@@ROWCOUNT>0)
				 BEGIN
				   SET @StrPriOutput ='0'		
				   SET @StrPriOutputDesc ='Card type is saved'
				 END
				 ELSE
				 BEGIN
					SET @StrPriOutput ='1'		
					SET @StrPriOutputDesc ='Card type is not saved'
				 END
			  END
			  ELSE
			  BEGIN
			    	SET @StrPriOutput ='1'		
					SET @StrPriOutputDesc ='Card type is already exists'
			  END
			 
			END
			--update
			ELSE IF Exists(SELECT 1 FROM TblCardType WITH(NOLOCK) WHERE CardTypeID=@ID AND SystemID=@SystemID AND BankID=@BankID)
			BEGIN 
			  UPDATE TblCardType SET CardTypeName=@Code,CardTypeDesc=@Desc   WHERE CardTypeID=@ID AND SystemID=@SystemID AND BankID=@BankID
				   SET @StrPriOutput ='0'		
				   SET @StrPriOutputDesc ='Card type is updated'

			END
	   END


Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
	 If(@IntPara=0)
	 BEGIN 
		SELECT 1  As Code,'Institution information is not saved' As [OutputDescription]
	 END
	 else IF(@IntPara=1)
	 BEGIN
	  SELECT 1  As Code,'Card type is not saved' As [OutputDescription]
	 END
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	

END

GO
