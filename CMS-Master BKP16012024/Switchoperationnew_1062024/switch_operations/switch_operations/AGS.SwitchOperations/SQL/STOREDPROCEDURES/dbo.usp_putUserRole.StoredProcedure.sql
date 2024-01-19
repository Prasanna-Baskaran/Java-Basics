USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_putUserRole]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Proc [dbo].[usp_putUserRole]
	@Firstname  VarChar(max),
    @Lastname VarChar(max),
    @Username VarChar(max),
    @password VarChar(max),
    @UserRole VarChar(max),
    @mobileno VarChar(max),
    @emailID VarChar(max),
    @chkactive bit,
    @UserID int,
    @Flag int
                
As
Begin
IF (@Flag=0)
		Begin
		  If not Exists (Select * From TblUser Where UserName=@Username)
			Begin
				Insert InTo TblUser(FirstName,LastName,UserName,UserPassword,UserRoleID,MobileNo,EmailId,IsActive,CreatedDate,AddedBy) Values
				(@Firstname,@Lastname,@Username,HASHBYTES('SHA1',@password),@UserRole,@mobileno,@emailID,@chkactive,GETDATE(),@UserID)	
			End	
		End
	Else
		Begin
		     Update TblUser set FirstName=@Firstname,LastName=@Lastname,UserPassword=HASHBYTES('SHA1',@password),UserRoleID=@UserRole,MobileNo=@mobileno,
		     EmailId=@emailID,IsActive=@chkactive,LastModifiedDate=GETDATE()   where UserName=@Username
		End		
End
GO
