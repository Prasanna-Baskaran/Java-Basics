USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpValidateUserLogin_Test]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpValidateUserLogin_Test]   
    
 @StrUserName varchar(50),  
 @StrUserPassword varchar(200),
 @StrSessionKey varchar(200)
AS  
BEGIN  
   	Begin Transaction  
	Begin Try  

Begin   
			Declare @StrPriOutput varchar(1)='1'		
			Declare @StrPriOutputDesc varchar(50)='Invalid Username/Password..!'

			If Exists (Select  1 From TblUser with (ReadPast) Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) And ((Ltrim(RTrim(UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))) )
			Begin	

			---- check user is active 
				If Exists (Select  1 From TblUser with (ReadPast) Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) And ((Ltrim(RTrim(UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))) ANd IsActive=1  AND ISNULL(FailCount,0)<3 )
				Begin	
				Set @StrPriOutput='0'
				Set @StrPriOutputDesc='Success..!'

				--get SystemName
				declare @tbl table(value varchar(200),RowID int) DECLARE @SystemLst VARCHAR(500)
				 Select @SystemLst= systemId from tbluser Usr WITH(NOLOCK)
				  Where ((@StrUserName ='') OR (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) )
					And ((@StrUserPassword ='') OR ((Ltrim(RTrim(Usr.UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))))

	 					 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@SystemLst,','))

						 DECLARE @Result VARCHAR(MAX)

					Select @Result= COALESCE(@Result+',' ,'')+ replace(SystemName,' ','')   from TblSystem Sy WITH(NOLOCK)
				INNER JOIN @tbl temp ON Sy.SystemID=temp.value

				----- start diksha change for authkey logic
				Update TblUser Set UsrSessionKey=(Ltrim(RTrim(@StrSessionKey))),FailCount=NULL 
				Where ((Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))))
					And 
				 (((Ltrim(RTrim(UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))))

				 DECLARE @OptionNeumonic VARCHAR(MAX)
				 ,@UserID VARCHAR(200)=(SELECT top 1 UserID FROM TblUser WITH(NOLOCK)Where ((Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))))
					And 
				 (((Ltrim(RTrim(UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword))))))

				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription],UserID,FirstName,LastName,
				MobileNo,EmailId,UserRoleID,Usr.SystemID AS [SystemID],BankID,@Result AS [SystemName],Usr.UsrSessionKey AS[AuthKey]
				From TblUser Usr with (ReadPast)				
				Where ((@StrUserName ='') OR (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) )
					And 
				 ((@StrUserPassword ='') OR ((Ltrim(RTrim(Usr.UserPassword)))=HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))))	

				
				 --Get Access rights
					Select  @OptionNeumonic=dbo.FunGetOptionNeumonic()


					DECLARE @Query varchar(max) = '

					SELECT
    
						OptionNeumonic,Value AS [AccessCaptions]
					FROM
					(
						SELECT
						   '+@OptionNeumonic+'
						FROM
							TblUserManagement AS t
							where UserID='+@UserID+'
					) AS SourceTable
					UNPIVOT
					(
						Value FOR  [OptionNeumonic] IN	    
						('+@OptionNeumonic+')
					) AS unpvt
					'
					print(@Query)
					exec(@Query)


			END
			----- check pwd Fail Count exeeds
			    else if exists(Select  1 From TblUser with (ReadPast) Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName))))  ANd IsActive=1  AND ISNULL(FailCount,0)>3)
				BEGIN
						Set @StrPriOutput='1'
						Set @StrPriOutputDesc='User account is locked..!'
						Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
				END
				Else
				Begin
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc='User not activated..!'
					Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
				End

			END
			------ Check User pwd FailedCount Exeeds
			else if exists(Select  1 From TblUser with (ReadPast) Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) AND ISNULL(FailCount,0)>3)
			BEGIN
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc='User account is locked.Please contact administrator ..!'
					Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
			END

			--------- update  pwd fail count for wrong PWD
			else if exists(Select  1 From TblUser with (ReadPast) Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName)))) And ((Ltrim(RTrim(UserPassword)))<>HASHBYTES('SHA1',LTrim(RTrim(@StrUserPassword)))) )           
			BEGIN
			  Update TblUser SET FailCount=ISNULL(FailCount,0)+1  Where (Upper(Ltrim(RTrim(UserName)))=Upper(Ltrim(RTrim(@StrUserName))))
			  Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
			END
			Else
			Begin
				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
			End

		End   

		COMMIT TRANSACTION;    

 End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
       Select '1' As Code,'Error occurs' As [OutputDescription]
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  
   
END  

GO
