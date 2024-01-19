USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_CustomerRegisteration]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_CustomerRegisteration] 
 @USERTYPE varchar(20),
 @CARDNO   varchar(100),
 @USERNAME  varchar(100),
 @FIRSTNAME varchar(100),
 @LASTNAME  varchar(100),
 @EMAILADDRS varchar(100),
 @Q1 varchar(max),
 @Q2 varchar(max),
 @CVV varchar(100),
 @LogUser int

AS  
BEGIN  		
Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='User Registered Failed.'
   	Begin Transaction  
	Begin Try  
	If Not Exists(Select 1 From TblCustomerRegistration With(Nolock) Where UserName=@USERNAME)
		Begin
			Insert Into TblCustomerRegistration (UserType,CardNo,UserName,FirstName,LastName,EmailID,Q1,Q2,CVV,CreatedBy,CreatedDate)
			Values (@USERTYPE,@CARDNO,@USERNAME,@FIRSTNAME,@LASTNAME,@EMAILADDRS,@Q1,@Q2,@CVV,@LogUser,GetDate())					
			Set @StrPriOutput='0'
		 Set @StrPriOutputDesc='User Registered successfully.'
		End	   
		
		Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
		
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
