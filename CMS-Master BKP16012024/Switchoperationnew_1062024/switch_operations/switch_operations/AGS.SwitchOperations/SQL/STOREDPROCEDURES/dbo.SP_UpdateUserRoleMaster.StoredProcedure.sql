USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUserRoleMaster]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SP_UpdateUserRoleMaster] --[dbo].[USP_UpdateFeeMasterDetails] 101,1,2
@UserRole varchar(30),
@UserID int  ,
@ModifiedBy int,
@SystemID VARCHAR(200)=1,
@BankID Varchar(200)=1
As    
Begin 
BEGIN TRANSACTION
BEGIN TRY
 
	Declare @StrPriOutput varchar(1)='1'		
	Declare @StrPriOutputDesc varchar(200)='' 

	update  dbo.TblUserRole set RoleName =@UserRole ,ModifiedBy=@ModifiedBy , ModifiedOn=getdate() where UserRoleID=@UserID And SystemID=@SystemID AND BankID=@BankID

	Set @StrPriOutput='0'
	Set @StrPriOutputDesc='User Role Updated Successfully.'

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH

	ROLLBACK TRANSACTION;
	  
	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
	SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
	Set @StrPriOutputDesc='Error occurred: ' + ERROR_MESSAGE()
END CATCH;  			
Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]

End    


GO
