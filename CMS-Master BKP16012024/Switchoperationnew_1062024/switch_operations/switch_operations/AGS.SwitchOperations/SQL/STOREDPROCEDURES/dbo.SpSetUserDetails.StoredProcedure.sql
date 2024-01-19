USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetUserDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----***********************************************
CREATE PROCEDURE [dbo].[SpSetUserDetails] 
 @UserName   varchar(30),
 @FirstName  varchar(30),
 @LastName   varchar(30),
 @MobileNo   varchar(15),
 @Emailid    varchar(50),
 @UserStatus  int ,
 @CreatedBy   int,
 @UserRole    int,
 @SystemID VARCHAR(200)=1,
 @BankID VARCHAR(50)=1

AS  
BEGIN  
BEGIN TRANSACTION

	Begin Try  
	
	
			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)=''
			
	        if Exists(select top 1 UserID from dbo.tbluser with(nolock) where ((UserName=@UserName) --or ( MobileNo=@MobileNo)
			))
	        begin
	        
	                    Set @StrOutput='1'
						
						Set @StrOutputDesc=  +' '+' '+ 'User is already exists.'
	        
	        end
	        
	       
	        else
	          begin

			  DECLARE @UserIdentity VARCHAR(20)			
	       
	           insert into TblUser (FirstName,LastName,UserName,MobileNo,EmailId,IsActive,CreatedDate,CreatedBy,UserPassword,UserRoleID,SystemID,BankID) 
                  values(@FirstName,@LastName,@UserName,@MobileNo,@Emailid,@UserStatus,getdate(),@CreatedBy,HASHBYTES('SHA1',RTRIM(LTRIM(dbo.FunGetPassword(@FirstName,@MobileNo)))),@UserRole,@SystemID,@BankID)
	           	 if(@@rowcount>0)
	           	 begin
	           	  set @StrOutput ='0'
	           		
				 Set @StrOutputDesc=  'User is added successfully.'
	           	 end
	           	 
	           	 else
	           	 begin
	           	  set @StrOutput ='1'
	           		
				 Set @StrOutputDesc=  'User is  not added.'
	           	 end
	           	
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
