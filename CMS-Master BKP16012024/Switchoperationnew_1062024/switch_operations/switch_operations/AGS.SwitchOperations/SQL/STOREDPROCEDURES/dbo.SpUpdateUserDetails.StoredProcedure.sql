USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpUpdateUserDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SpUpdateUserDetails] 

 @FirstName  varchar(30),
 @LastName   varchar(30),
 @MobileNo   varchar(15),
 @Emailid    varchar(50),
 @Password   varchar(50),
 @UserStatus  int ,
 @ModifiedBy   int,
 @UserRole     int,
 @UserId       int,
 @SystemID VARCHAR(500)=1,
 @BankID VARCHAR(500)=1

AS  
BEGIN  
BEGIN TRANSACTION

	Begin Try  

			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)=''
			
	         if(@Password='')
	          begin
	           update  TblUser 
	           set FirstName=@FirstName, LastName =@LastName, MobileNo=@MobileNo, EmailId=@Emailid,  IsActive=@UserStatus,
	           LastModifieddate=getdate(), ModifiedBy=@ModifiedBy, UserRoleID=@UserRole --,SystemID=@SystemID
               where UserID=@UserId
	           set @StrOutput ='0'
	           		
			   Set @StrOutputDesc=  'User Details is updated successfully.'	
	           	
			 end
			 
			 else
			 begin
			 update  TblUser 
	           set FirstName =@FirstName, LastName =@LastName, MobileNo=@MobileNo, EmailId=@Emailid,  IsActive=@UserStatus,
	            LastModifieddate=getdate(), ModifiedBy=@ModifiedBy, UserPassword=HASHBYTES('SHA1',@Password),
	            UserRoleID=@UserRole--,SystemID=@SystemID
				,FailCount=NULL
               where UserID=@UserId
               
               set @StrOutput ='0'
	           		
				 Set @StrOutputDesc=  'User Details is updated successfully.'	
	           	
	           	
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
