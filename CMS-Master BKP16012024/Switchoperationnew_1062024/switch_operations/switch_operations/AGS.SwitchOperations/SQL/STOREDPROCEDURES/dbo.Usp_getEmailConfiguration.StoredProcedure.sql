USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Usp_getEmailConfiguration]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name- Gufran Khan>
-- Create date: <Create Date,23-10-2017,>
-- Description:	<Description,To get the Email configuration,>
-- =============================================
Create PROCEDURE [dbo].[Usp_getEmailConfiguration]
	
AS
BEGIN
	SELECT SMTPCLIENT,EmailFrom,EmailPort,EmailUserName,dbo.ufn_DecryptPAN(EmailPassWord)[EmailPassword],EmailTo,EmailBCC,EmailMsg
	FROM EmailAlert
END

GO
