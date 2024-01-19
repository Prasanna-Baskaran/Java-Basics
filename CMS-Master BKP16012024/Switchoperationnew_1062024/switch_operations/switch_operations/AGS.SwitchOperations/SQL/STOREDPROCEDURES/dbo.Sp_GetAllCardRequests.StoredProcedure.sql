USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetAllCardRequests]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetAllCardRequests] 
	@IntPara  int =0,
	@CustomerID BIGINT=0,
	@CardNo VARCHAR(20)='',
	@RequestTypeID int,
	@ReqID VARCHAR='',
	@SystemID varchar(200)=1,
	@BankID Varchar(200)=1,
	@BankCustID Varchar(800)=''
AS
BEGIN
IF(@RequestTypeID=1)
BEGIN

  SELECT  Co.ID, C.bankCustID AS [CustomerID],dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],(C.FirstName +' '+ISNULL(LastName,'')) AS [CustomerName]
	,fs.FormStatus AS [Status],ISNULL(co.FormStatusID,0) AS FormStatusID ,cr.RequestType,Co.RequestTypeID,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
	From TblCardLimits Co WITH(NOLOCK)
	INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
	INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPAN_ID=pan.ID
	INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
	INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
	WHERE Co.RequestTypeID=@RequestTypeID
	--AND ((@CustomerID=0) OR Co.CustomerID=@CustomerID)
    AND ((@BankCustID='') OR (((C.BankCustID=@BankCustID))))
	AND ((@CardNo='')OR (@CardNo=dbo.ufn_DecryptPAN(pan.DecPAN)))
	AND ((@ReqID='') OR (Co.ID=@ReqID))
	AND(co.SystemID=@SystemID)
	AND(co.BankID=@BankID)
	ORDER BY co.ID Desc
END
ELSE
BEGIN
	SELECT co.ID,C.bankCustID AS [CustomerID],(dbo.ufn_DecryptPAN(Pan.DecPAN)) AS [CardNO],(C.FirstName +' '+ISNULL(LastName,'')) AS [CustomerName]
	,fs.FormStatus AS [Status],ISNULL(co.FormStatusID,0) AS FormStatusID,cr.RequestType,co.RequestTypeID,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
	From TblCardOpsRequestLog Co WITH(NOLOCK)
	INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
	INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPANID=pan.ID
	INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
	INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
	WHERE Co.RequestTypeID=@RequestTypeID 
	--AND ((@CustomerID=0) OR Co.CustomerID=@CustomerID)
	AND ((@BankCustID='') OR (((C.BankCustID=@BankCustID))))
	AND ((@CardNo='')OR (@CardNo=dbo.ufn_DecryptPAN(pan.DecPAN)))
	AND(co.SystemID=@SystemID)
		AND(co.BankID=@BankID)
	END
END

GO
