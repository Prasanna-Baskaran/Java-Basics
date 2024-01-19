USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCardAutomationReport]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[USP_GetCardAutomationReport](
@varbankname varchar(50),
@fromdate datetime,
@todate datetime)
AS 
--*******************************************************
--	NAME	:	USP_GetCardAutomationReport
--	PURPOSE	:	GET REPORT FOR CARD Automation FORMAT 
--	DATE	:	17-05-2017
--	BY		:	NISHIGANDHA A PADHYE
--	Ex		:	EXEC  USP_GetCardAutomationReport 'BhartiBank','2017-05-12 13:43:37.940','2017-05-16 21:32:03.287'
--******************************************************
BEGIN
	SELECT bank.BankName  AS BankName ,filedata.CIF_FileName  as CIF_FILE_NAME,filedata.CreatedDate AS CreatedDate,CIF_ID AS CIF_ID
	from TblCif_Filedata filedata
	inner join TblBanks bank 
	on filedata.BankID = bank.ID

	WHERE bank.BankName=@varbankname and filedata.CreatedDate between @fromdate and @todate
END

--EXEC  USP_GetCardAutomationReport 'BhartiBank','2017-05-12 13:43:37.940','2017-05-16 21:32:03.287'
GO
