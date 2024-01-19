USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetUserDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetUserDetails]
 @MobileNo VARCHAR(12)='',
 @UserName VARCHAR(50)=''
AS
BEGIN
IF EXISTS (SELECT 1 FROM TblUser WITH(NOLOCK) WHERE((upper(LTRIM(RTRIM(UserName))))=(upper(LTRIM(RTRIM(@UserName))))))
  BEGIN
     SELECT UserID,FirstName,LastName,MobileNo,EmailId,IsActive,CONVERT(VARCHAR(10),CreatedDate,103 ) AS [Created Date]
	  FROM TblUser WITH(NOLOCK) 
	  WHERE (((upper(LTRIM(RTRIM(UserName))))=(upper(LTRIM(RTRIM(@UserName))))))
  END
END

GO
