USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_CreateUpdateOTP]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_CreateUpdateOTP]
(
	@Mobile varchar(10)='',
	@CardNum Varchar(30)=''
)
AS
/*CHANGE MANAGEMENT  
CREATED BY: Prerna PAtil
CREATED DATE: 02/08/2017  
CREATED REASON: CREATE AND UPDATE OTP
*/  
BEGIN
DECLARE @StrStatusCode char(2),@StrStatusDescription varchar(100)  
DECLARE @StrOTP varchar(6)

BEGIN TRAN
BEGIN TRY
SET @StrOTP=CAST((convert(numeric(6,0),rand() * 899999) + 100000) AS VARCHAR(6))  
IF(ISNULL(@StrOTP,'')<>'')
BEGIN
	IF NOT EXISTS(SELECT TOP 1 1 FROM TblOTP WITH(NOLOCK) WHERE MOBILENO=@Mobile And dbo.ufn_DecryptPAN([CardNumber])=@CardNum)
	BEGIN
	INSERT INTO TblOTP(MobileNo,[CardNumber],OTP,CreatedDateTime)
				VALUES(@Mobile,dbo.ufn_EncryptPAN(@CardNum),@StrOTP,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE TblOTP SET OTP=@StrOTP,ModifyDateTime=GETDATE() WHERE MobileNo=@Mobile And dbo.ufn_DecryptPAN([CardNumber])=@CardNum
	END	
	SET @StrStatusCode='00'
	SET @StrStatusDescription='Success.'
	
END
ELSE
BEGIN
	SET @StrStatusCode='98'
	SET @StrStatusDescription='OTP value not created properly.'
END

COMMIT TRAN		
END	TRY
BEGIN CATCH
	-- CATCH BLOCK  
 SET @StrStatusCode='99'    
 SET @StrStatusDescription='Unexpected Error Occurred while creating OTP.'    
 
 DECLARE @StrProcedure_Name varchar(500), @ErrorDetail varchar(1000), @ParameterList varchar(2000)    
 SET @StrProcedure_Name=ERROR_PROCEDURE()    
 SET @ErrorDetail=ERROR_MESSAGE()    
 SET @ParameterList='@Mobile='+ISNULL(@Mobile,'')
   
	EXEC Usp_InsertErrorDetail @StrProcedure_Name=@StrProcedure_Name,    
        @ErrorDetail=@ErrorDetail,@ParameterList=@ParameterList    
END CATCH
--Final Result
SELECT @StrStatusCode [StatusCode],@StrStatusDescription [Description]   ,@StrOTP [OTP]

END
GO
