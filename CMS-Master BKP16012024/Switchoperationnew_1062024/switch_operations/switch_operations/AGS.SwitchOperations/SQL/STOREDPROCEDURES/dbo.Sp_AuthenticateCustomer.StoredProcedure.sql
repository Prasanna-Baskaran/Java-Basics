USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_AuthenticateCustomer]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_AuthenticateCustomer]
	@StrUsername VARCHAR(500)='',
	@StrPassword VARCHAR(500)='',
	@StrIMEI   VARCHAR(50),    
	@StrGCM_ID   VARCHAR(1000),    
	@StrAppVERSION  VARCHAR(10) 
AS
/*CHANGE MANAGEMENT    
CREATED BY: Prerna Patil   
CREATED DATE: 20/01/2017    
CREATED REASON: validate Login user detail    
*/ 
BEGIN
	--IF EXISTS(SELECT 1 FROM TblCustomerRegisteration WITH(NOLOCK) WHERE UserName=@StrUsername AND RTRIM(LTRIM([Password])) =HASHBYTES('SHA1',RTRIM(LTRIM(@StrPassword)))  )
	--BEGIN
	--   SELECT 0 AS [Code],'Login successful' as [Description]

	--END
	--ELSE
	--BEGIN
	--  SELECT 1 AS [Code],'Invalid username/password' as [Description]
	--END
DECLARE @StrStatusCode char(2),@StrStatusDescription varchar(100)      
DECLARE @StrSessionKey varchar(16)    
DECLARE @IsLocked bit=0,@TryCount int=0,@UserName VARCHAR(10),@UserRole char(1)    
DECLARE @FName VARCHAR(30),@LName VARCHAR(30) 
DECLARE @PWD VARBINARY(500)   
    
BEGIN TRAN    
BEGIN TRY    
--Get User Detail by UID and PWD    
SELECT @IsLocked=IsLocked, @TryCount=TryCount, @UserName=UserName, @UserRole=UserRole 
,@PWD   =USERPASS
FROM [TblWSUserDetail] with(nolock)    
WHERE USERNAME LIKE @strUserName --AND USERPASS=HASHBYTES('SHA1',@StrPwd)    

    
--IF USER IS LOCKED THEN GIV LOCKED MSG    
IF(ISNULL(@IsLocked,0)=1)    
BEGIN    
 SET @StrStatusCode='97'      
 SET @StrStatusDescription='User account is temporarily locked.'     
 GOTO LblResult    
END    

        
--Validate Invalid User name and update try count,Locked value.    
IF(ISNULL(@UserName,'')='' OR @PWD<>HASHBYTES('SHA1',@StrPassword))    
BEGIN    
 SET @StrStatusCode='98'      
 SET @StrStatusDescription='Invalid user credential.'     
     
 UPDATE [TblWSUserDetail] SET TryCount =isnull(TryCount,0)+1,IsLocked= CASE WHEN TryCount>=3 THEN 1 ELSE 0 END    
 WHERE USERNAME LIKE @strUserName    
     
 GOTO LblResult    
END    
    
    
--CREATE 16 DIGIT SESSION KEY    
select @StrSessionKey=CAST((convert(numeric(16,0),rand() * 8999999999999999) + 1000000000000000) AS VARCHAR(16))    
    
    
--UPDATE SESSION KEY AGAINT USER NAME   ,Update GCM ID  
UPDATE [TblWSUserDetail] SET SessionID=@StrSessionKey,GCM_ID=@StrGCM_ID WHERE USERNAME LIKE @strUserName    

  
--Get user primary details    
SELECT @FName=FName,@LName=LName FROM [TblWSUserDetail] d WITH(NOLOCK) WHERE MOBILE=@strUserName    
    
SET @StrStatusCode='00'      
SET @StrStatusDescription='Success'     
LblResult:        
COMMIT TRAN    
END TRY    
BEGIN CATCH    
ROLLBACK TRAN    
-- CATCH BLOCK    
 SET @StrStatusCode='99'      
 SET @StrStatusDescription='Unexpected Error Occurred while Logging user.'      
 DECLARE @StrProcedure_Name varchar(500), @ErrorDetail varchar(1000), @ParameterList varchar(2000)      
 SET @StrProcedure_Name=ERROR_PROCEDURE()      
 SET @ErrorDetail=ERROR_MESSAGE()      
 SET @ParameterList='@strUserName='+ISNULL(@strUserName,'')+',@StrIMEI='+ISNULL(@StrIMEI,'')+',@StrAppVERSION='+ISNULL(@StrAppVERSION,'')    
     
 EXEC USP_InsertErrorLog @ProcedureName=@StrProcedure_Name,      
        @ErrorDesc=@ErrorDetail,@ParameterList=@ParameterList      
END CATCH    
    

--RETURN FINAL RESULT    
 SELECT @StrStatusCode [StatusCode],@StrStatusDescription [Description]     
   ,@UserRole [UserRole],@StrSessionKey [SessionKey],@FName [FirstName],@LName [LastName]    
END

GO
