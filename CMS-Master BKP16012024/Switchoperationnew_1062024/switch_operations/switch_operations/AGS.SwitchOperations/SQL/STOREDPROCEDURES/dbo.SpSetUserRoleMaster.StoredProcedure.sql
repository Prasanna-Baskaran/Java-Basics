USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetUserRoleMaster]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpSetUserRoleMaster] 
 @UserRole varchar(30),
 @CreatedBy  int,
 @SystemID Varchar(200)=1,
 @BankID Varchar(200)=1

AS  
BEGIN  
BEGIN TRANSACTION
BEGIN TRY
     
    
			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)=''
			
	        if Exists(select UserRoleID from dbo.tbluserrole with(nolock) where RoleName=@UserRole And SystemID=@SystemID AND BankID=@BankID )
	        begin
	        Set @StrOutput='1'
			Set @StrOutputDesc=  +' '+' '+ ' User Role is already added.'
	        
	        end
	        
	        else
	        begin
	         
	           insert into dbo.tbluserrole (RoleName,CreatedDate,CreatedBy,SystemID,BankID) values(@UserRole,getdate(),@CreatedBy,@SystemID,@BankID)
	           	Set @StrOutput='0'
				 Set @StrOutputDesc= @UserRole +' '+' '+  'User Role  is set successfully.'
			 end
				Select @StrOutput As Code,@StrOutputDesc As [OutputDescription]

	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH

ROLLBACK TRANSACTION;

SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
END CATCH;  	
   
END  

GO
