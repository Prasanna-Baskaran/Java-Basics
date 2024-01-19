USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_ChangeUserPassward]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_ChangeUserPassward]
 @UserId  BIGINT=0,
 @IntPara int =0,
 @OldPassward VARCHAR(50)='',
 @NewPassward VARCHAR(50)='' 	
AS
BEGIN
Begin Transaction  
	Begin Try    
    			Declare @StrPriOutput varchar(1)		
			    Declare @StrPriOutputDesc varchar(200)

	--Resert Passward
	IF(@IntPara=0)
	BEGIN
		SET @StrPriOutput ='1'		
		SET @StrPriOutputDesc ='Passward is not reset'
	   IF EXISTS (Select 1 From TblUser WITH(NOLOCK) Where UserID=@UserId)
		BEGIN
     	  Update TblUser SET UserPassword=HASHBYTES('SHA1',RTRIM(LTRIM(dbo.FunGetPassword(FirstName,MobileNo)))) WHERE  UserID=@UserId
		  SET @StrPriOutput=0
          Set @StrPriOutputDesc='Passward is reset successfully'
	    END
		ELSE
		 BEGIN
			SET @StrPriOutput=1
			SET @StrPriOutputDesc='Passward is not reset'
	     END

	END

	--for change passward
	ELSE IF(@IntPara=1)
	BEGIN
		IF(@OldPassward <> '' AND @NewPassward  <> '' AND @UserId <>0)
		BEGIN
		 IF EXISTS ( SELECT 1 FROM TblUser WITH(NOLOCK) WHERE UserID=@UserId  AND UserPassword=@OldPassward)
		 BEGIN
		    UPDATE TblUser SET UserPassword=HASHbytes('SHA1',(RTRIM(LTRIM(@NewPassward)))) WHERE (UserID=@UserId)  AND (UserPassword=@OldPassward)
			SET @StrPriOutput=0
            SET @StrPriOutputDesc='Passward is changed successfully'
		 END
		 ELSE
		 BEGIN
		    SET @StrPriOutput=1
			SET @StrPriOutputDesc='Incorrect old passward'
		 END
		END
	END
 Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  


END

GO
