USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_getUserDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_getUserDetails]
	@User		VarChar(100)
	
AS
BEGIN
If @User <>''
	Begin		
	select FirstName,LastName,UserName,RoleName,MobileNo,EmailId,IsActive from TblUser
		Left join TblUserRole on TblUserRole.UserRoleID=TblUser.UserRoleID
	  where UserName=@User		
	End
END

GO
