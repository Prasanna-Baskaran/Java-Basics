USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetBankDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[USP_GetBankDetails]
AS
BEGIN

     SELECT  ID,BankName  FROM TblBanks 
	
	     
END
GO
