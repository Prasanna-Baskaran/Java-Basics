USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCardOpsReqByID]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetCardOpsReqByID]
	  @ID BIGINT=0,
	  @ReqTypeID Int=0	,
	  @CardNo VARCHAR(20)='',
	  @CustomerID BIGINT=0,
	  @IntPara int=0,
	  @SystemID int=0
AS
BEGIN
	IF(@ID <>0 AND @ReqTypeID<>0)
	BEGIN
	   --Card Limit Uppdate Req
		 IF(@ReqTypeID=1)
		  BEGIN
			SELECT  C.BankCustID AS [CustomerID],(C.FirstName + ' '+ISNULL(C.LastName,'')) AS [CustomerName] ,C.MobileNo,Convert(varchar(10),C.DOB_AD,103) AS [DOB],
			Cd.PO_Box_P+'  '+Cd.HouseNo_P +'  '+Cd.StreetName_P+'  '+Cd.WardNo_P +'  '+Cd.City_P+'  '+Cd.District_P AS [Address] ,Cd.Email_P As [Email]	
			,Co.RequestTypeID,dbo.ufn_DecryptPAN(cp.DecPAN) AS [CardNo]
			,CO.PurchaseNo,co.PurchaseDailyLimit,co.PurchasePTLimit
			,CO.WithDrawNO,CO.WithDrawDailyLimit,co.WithDrawPTLimit
			,co.PaymentNO,co.PaymentDailyLimit,co.PaymentPTLimit
			,co.CNPDailyLimit,CO.CNPPTLimit,co.ID
			,ISNULL(co.FormStatusID,0) AS FormStatusID
			from TblCardLimits CO WITH(NOLOCK) 
			INNER JOIN TblCustomersDetails C WITH(NOLOCK) ON  Co.CustomerID=C.CustomerID 
			INNER JOIN TblCustomerAddress  Cd WITH(NOLOCK) ON C.CustomerID=Cd.CustomerID
			LEFT JOIN CardRPAN CP WITH(NOLOCK) ON CO.CardRPAN_ID=Cp.ID
			WHERE CO.ID=@ID
			AND CO.SystemID=@SystemID
		  END	    
		  --Other Card Ops req
		  ELSE
		  BEGIN
			SELECT  C.bankcustID AS [CustomerID] ,(C.FirstName + ' '+ISNULL(C.LastName,'')) AS [CustomerName] ,C.MobileNo,Convert(varchar(10),C.DOB_AD,103) AS [DOB],
			Cd.PO_Box_P+'  '+Cd.HouseNo_P +'  '+Cd.StreetName_P+'  '+Cd.WardNo_P +'  '+Cd.City_P+'  '+Cd.District_P AS [Address] ,Cd.Email_P As [Email]	
			,Co.RequestTypeID,dbo.ufn_DecryptPAN(cp.DecPAN) AS [CardNo],co.ID
			,ISNULL(co.FormStatusID,0) AS FormStatusID
			from TblCardOpsRequestLog CO WITH(NOLOCK) 
			INNER JOIN TblCustomersDetails C WITH(NOLOCK) ON  Co.CustomerID=C.CustomerID 
			INNER JOIN TblCustomerAddress  Cd WITH(NOLOCK) ON C.CustomerID=Cd.CustomerID
			LEFT JOIN CardRPAN CP WITH(NOLOCK) ON CO.CardRPANID=Cp.ID
			WHERE CO.ID=@ID
			AND CO.SystemID=@SystemID
		  END
	END
END


GO
