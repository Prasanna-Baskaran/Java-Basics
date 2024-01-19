USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCustCardByCustomerId_test]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetCustCardByCustomerId_test]--[Sp_GetCustCardByCustomerId_test] '01015955','1038851','1'
	@CustomerID VARCHAR(200),
	@CustIdentity Varchar(20)	          
	,@BankId int
AS
BEGIN
 Begin Transaction  
	Begin Try 
	Declare @IntpriOutput int ,@StrpriOutputDesc Varchar(500) 
	DECLARE	@IssuerNo int, @EncPAN VARCHAR(1000)='' ,@EncAcccount VARCHAR(800)=''  
	DECLARE @R_CustomerID VARCHAR(200)='',@R_CustomerName VARCHAR(200),@R_MobileNo VARCHAR(20),@R_DOB VARCHAR(50),@R_Address VARCHAR(800),@R_Email VARCHAR(200),@R_PAN_ID VARCHAR(200),@ID VARCHAR(800),@R_CardNO VARCHAR(20)
    Declare @RW int, @CustID varchar(max) 
	DECLARE	 @PanId int
	select * into #tempTblcustomerdetails from TblCustomersDetails C WITH(NOLOCK)
	where C.BankCustID = @CustomerID
	SELECT @IssuerNo=BankCode FROM TblBanks WITH(NOLOCK) WHERE ID=@BankID
				DECLARE @tblResult TABLE
				(
				Id int identity(1,1),
				  goods_nr_trans_lim numeric(18,0),
				  goods_lim numeric(18,0),
				  tran_goods_lim numeric(18,0),
				  goods_offline_lim numeric(18,0),
				  cash_nr_trans_lim numeric(18,0),
				  cash_lim numeric(18,0),
				  tran_cash_lim numeric(18,0),
				  cash_offline_lim numeric(18,0),
				  paymnt_nr_trans_lim numeric(18,0),
				  paymnt_lim numeric(18,0),
				  tran_paymnt_lim numeric(18,0),
				  paymnt_offline_lim numeric(18,0),
				  tran_paymnt_offline_lim numeric(18,0),
				  cnp_lim numeric(18,0),
				  tran_cnp_lim numeric(18,0),
				  cnp_offline_lim numeric(18,0),
				  tran_cnp_offline_lim numeric(18,0),
				  last_updated_date DATETIME,
				  last_updated_user VARCHAR(200),
				  card_program VARCHAR(100),
				  card_status smallint,
				  expiry_date VARCHAR(20) ,
				  HRC VARCHAR(5) ,
				  customer_id VARCHAR(50),
				  Issue_Date Varchar(50),
				  EncryptAccount VARCHAR(800),
				  EncryptPan VARCHAR(800)
				)

				Declare @AccountNo varchar(20)
				Declare @EncAcc varchar(500)
				
				select @AccountNo =dbo.ufn_decryptpan(accno) from tblcustomersdetails with(nolock) where customerid= @CustIdentity -- 461586
				select @EncAcc=EncAcc from cardraccounts with(nolock) where issuerno=@IssuerNo and dbo.ufn_decryptpan(decacc)= @AccountNo 
				
				select @EncAcc '@EncAcc'
				set @CustomerID=dbo.[LtrimZero](@CustomerID)
				PRINT @EncPAN
				INSERT INTO @tblResult(goods_nr_trans_lim,goods_lim,tran_goods_lim,goods_offline_lim,cash_nr_trans_lim,cash_lim,tran_cash_lim,cash_offline_lim,
				paymnt_nr_trans_lim,paymnt_lim,tran_paymnt_lim,paymnt_offline_lim,tran_paymnt_offline_lim,cnp_lim,tran_cnp_lim,cnp_offline_lim,tran_cnp_offline_lim,
				last_updated_date,last_updated_user,card_program , card_status ,  expiry_date,HRC,customer_id,Issue_Date,EncryptAccount,EncryptPan)
				exec  [AGSS1RT].postcard.dbo.[Usp_GetCardDetailsByCustomerID_AGS] @CustomerID ,@IssuerNo,@IntpriOutput output ,@StrpriOutputDesc output ,@EncAcc
		

	select '@tblResult',* from @tblResult
	

	Select  t.customer_id  , TCD.FirstName +' '+ISNULL(TCD.LastName,'')  AS [CustomerName] ,TCD.MobileNo AS MobileNo ,
	   cd.Email_p As [Email]				
			
			, dbo.ufn_DecryptPAN(cp.DecPAN) AS [CardNo],expiry_date AS ExpiryDate
			
		    ,t.Issue_Date CardIssueddate				
			,Case when t.HRC='0' THEN '' ELSE CASE WHEN T.HRC IN ('06,75') THEN 'Temporary Block' else 'Permanent Block' END END As BlockStatus
			,case when cr.SwitchCode=2 then 'Repin' else CR.RequestType end CardStatus 
			
			,dbo.ufn_decryptPAN(A.DecAcc) AS [AccountNo]
			--, ID AS [ID]		
			from @tblResult  t 
			inner JOIN TblCustomersDetails TCD with (nolock) on t.customer_id=dbo.[LtrimZero](tcd.BankCustID )and TCD.customerID=@CustIdentity
			LEFT JOIN TblCustomerAddress  Cd WITH(NOLOCK) ON TCD.CustomerID=Cd.CustomerID
			LEFT JOIN TblCardRequests CR WITH(NOLOCK) ON ISNULL(t.HRC,0)=ISNULL(CR.SwitchCode,0) AND CR.Flag='C'
			LEFT JOIN CardRpan CP with (nolock) on t.EncryptPan=cp.encpan
			LEFT JOIN CardRAccounts A with(NOLOCK) ON t.EncryptAccount=A.EncAcc AND A.IssuerNo=@IssuerNo --and dbo.ufn_DecryptPAN( TCD.AccNo)=dbo.ufn_DecryptPAN( a.DecAcc)

  COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	
END

GO
