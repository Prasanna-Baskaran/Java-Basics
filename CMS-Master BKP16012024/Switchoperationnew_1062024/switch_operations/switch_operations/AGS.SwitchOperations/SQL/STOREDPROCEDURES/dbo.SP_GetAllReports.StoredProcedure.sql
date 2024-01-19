USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllReports]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_GetAllReports]
	@IntPara int=0,
	@BankID VARCHAR(200),
	@SystemID VARCHAR(200),
	@FromDate VARCHAR(200)='',
	@ToDate VARCHAR(200)='',
	@ProductType VARCHAR(200)='',
	@Status VARCHAR(20)='',
	@CardNo Varchar(200)='',
	@CustID Varchar(200)=''
AS
BEGIN
----- **************** Personalized Card Report ***********
	IF(@IntPara=0)
	BEGIN
	Select P.ProductType,dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],cu.bankCustID AS[CustomerID],Cu.FirstName+' '+Cu.Lastname AS [Full Name],AC.[Customer Preferred name] AS[NameOnCard],dbo.FunRemoveLeftZero(AC.[AC ID]) AS [AccountNo],convert(varchar(12),ac.[date],103) AS [Date],
		 case WHEN cardfs.FormStatusID=1 THEN 'Success' ELSE cardfs.formstatus END AS [CardStatus],Case when PREFs.FormStatusID=1 THEN 'Success' ELSE PREFs.formstatus END[PREStatus]
		from TblAuthorizedCardLog AC WITH(NOLOCK)
		INNER JOIN tblformstatus Cardfs WITH(NOLOCK) ON ISNULL(AC.CardGenStatus,0)=Cardfs.FormStatusID
		INNER JOIN tblformstatus PREFs WITH(NOLOCK) ON ISNULL(AC.PREStatus,0)=PREFs.FormStatusID
		INNER JOIN TblCustomersdetails Cu WITH(NOLOCK) ON AC.[CIF ID]=Cu.BankCustID
		INNER JOIN TblProductType P WITH(NOLOCK) ON Cu.ProductType_ID=P.ID
		INNER JOIN TblBanks BA WITH(NOLOCK) ON BA.ID=AC.BANK
		LEFT JOIN CardRpan Pan WITH(NOLOCK) ON Pan.customer_id=cu.bankcustID AND Pan.IssuerNo=BA.BankCode	
		where(( (@FromDate='') AND (@ToDate='')) OR ((convert(date,ac.[date],103)) between convert(date ,@FromDate,103) and  convert(date ,@ToDate,103)))
		AND AC.Bank=@BankID and ISNULL(AC.SystemID,2)=@SystemID
		AND ((@ProductType='') OR (cu.ProductType_ID=@ProductType))
		AND ((@Status='') OR (ISNULL(AC.CardGenStatus,0)=@Status) OR(ISNULL(Ac.PREStatus,0)=@Status))
	END
----********************* Txn Report *****************
	IF(@IntPara=1)
	BEGIN
	 -- EXEC SP_GetTxnReport @BankID=@BankID, @SystemID=@SystemID, @FromDate=@FromDate, @ToDate=@ToDate, @CardNo=@CardNo, @CustID=@CustID
	 print 1
	END
END

GO
