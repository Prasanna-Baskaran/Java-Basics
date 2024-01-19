USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCardAutomationDetail]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[USP_GetCardAutomationDetail](
@varciffilename varchar(50)
)
AS 
--*******************************************************
--	NAME	:	USP_GetCardAutomationDetail
--	PURPOSE	:	GET REPORT FOR CARD Automation FORMAT 
--	DATE	:	17-05-2017
--	BY		:	NISHIGANDHA A PADHYE
--	Ex		:	EXEC  USP_GetCardAutomationDetail 
--******************************************************
BEGIN
	SELECT CIF_FileName AS CIF_FileName,CustomerName as Customer_Name,NameOnCard as NameOnCard,AccountNo As AccountNo,Address1 as Address,PinCode as Pincode,Country As CoutryName
	from TblCif_Filedata filedata
	inner join TblBanks bank 
	on filedata.BankID = bank.ID

	WHERE filedata.CIF_FileName=@varciffilename
END


GO
