USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_FetchCardAndAccountEncFromSwtich]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[USP_FetchCardAndAccountEncFromSwtich]
as
begin 	
	--EXEC master..xp_CMDShell 'D:\CardAccountUtility\build\classes\AccountRUtility.bat'
	exec  [AGSS1RT].postcard.dbo.USP_PostCardAndAccountEncTOSwitchOps	
end
GO
