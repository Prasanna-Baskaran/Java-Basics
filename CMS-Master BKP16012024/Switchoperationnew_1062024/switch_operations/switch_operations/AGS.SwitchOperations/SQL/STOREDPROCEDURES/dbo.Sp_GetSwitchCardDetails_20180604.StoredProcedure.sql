USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetSwitchCardDetails_20180604]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_GetSwitchCardDetails_20180604]--[Sp_GetSwitchCardDetails] '4660461000000262','','2','1','','',0
	@CardNO VARCHAR(20),                      --[Sp_GetSwitchCardDetails] '6376762000060000599','','1','4','','',0
	@CustomerID VARCHAR(200)='',	          --[Sp_GetSwitchCardDetails] '6376762000060000144','','1','4','','',0
	@SystemID varchar(200)=1,
	@BankID varchar(200)=1,
	@Name VARCHAR(800)='',
	@AccountNo Varchar(800)='',
	@IntPara int =0

AS
BEGIN
 Begin Transaction  
	Begin Try 
	Declare @IntpriOutput int ,@StrpriOutputDesc Varchar(500) 
	DECLARE	@IssuerNo int, @EncPAN VARCHAR(1000)='' ,@EncAcccount VARCHAR(800)=''  
	--  for gettting card limits,and details by PAN
	--SELECT top 1 * FROM CardRPAN WITH(NOLOCK) WHERE  dbo.ufn_DecryptPAN(DecPAN)=@CardNo
	IF(@IntPara=0)
	BEGIN
	 --IF EXISTS(SELECT top 1 1 FROM CardRPAN WITH(NOLOCK) WHERE  dbo.ufn_DecryptPAN(DecPAN)=@CardNo)
	 BEGIN
	    DECLARE @R_CustomerID VARCHAR(200)='',@R_CustomerName VARCHAR(200),@R_MobileNo VARCHAR(20),@R_DOB VARCHAR(50),@R_Address VARCHAR(800),@R_Email VARCHAR(200),@R_PAN_ID VARCHAR(200),@ID VARCHAR(800),@R_CardNO VARCHAR(20)
	Declare @aa varchar(max)
  --Customer details
    Declare @RW int, @CustID varchar(max) 
DECLARE	 @PanId int
--Declare @IntpriOutput int ,@StrpriOutputDesc Varchar(500) 


		Select ROW_NUMBER() over(partition by dbo.ufn_DecryptPAN(Cp.DecPAN) order by Maker_Date_IND desc) RW 
				,C.bankcustID R_CustomerID
				,C.FirstName +' '+ISNULL(C.LastName,'') R_CustomerName
				,C.MobileNo R_MobileNo
				,Convert(varchar(10),C.DOB_AD,103) R_DOB
				,ISNULL(Cd.PO_Box_P,'')+' '+ISNULL(Cd.HouseNo_P,'') +' '+ISNULL(Cd.StreetName_P,'')+' '+ISNULL(Cd.WardNo_P,'') +' '+ISNULL(Cd.City_P,'')+' '+ISNULL(Cd.District_P,'') R_Address
				,Cd.Email_P R_Email
				,CP.ID	R_PAN_ID
				,C.CustomerID CustID
				,dbo.ufn_DecryptPAN(Cp.DecPAN)  R_CardNo
				,Cp.EncPAN	EncPAN 	
				into #temp
				from TblCustomersDetails C WITH(NOLOCK)
				INNER JOIN TblCustomerAddress  Cd WITH(NOLOCK) ON C.CustomerID=Cd.CustomerID
				LEFT JOIN CardRPAN CP WITH(NOLOCK) ON C.BankCustID=Cp.customer_id		
				WHERE ISNULL(C.FormStatusID,0)=1 
				AND ((C.SystemID=@SystemID))
				AND ((isnull(@AccountNo ,'')='') OR (@AccountNo=dbo.ufn_decryptpan(c.accno)))
				AND ((isnull(@CardNo,'')='') OR (@CardNo=dbo.ufn_DecryptPAN(Cp.DecPAN)))
				AND 
				(
					(@Name='') 
					OR
					RTRIM(LTRIM(C.FirstName)) like '%'+ RTRIM(LTRIM(@Name))+'%'
					OR
					RTRIM(LTRIM(C.LastName)) like '%'+ RTRIM(LTRIM(@Name))+'%'
					OR
					((UPPER((RTRIM(LTRIM(C.FirstName)))+(RTRIM(LTRIM(C.LastName)))))like '%'+UPPER(replace(RTRIM(LTRIM(@Name)),' ',''))+'%')
				)

		--select '#temp',* from #temp
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

		SET NOCOUNT ON
		DECLARE cur_car CURSOR
		STATIC FOR 
		SELECT RW, R_CustomerID,R_CustomerName, R_MobileNo, R_DOB, R_Address , R_Email ,@R_PAN_ID , CustID , R_CardNo , EncPAN 
		 from #temp
		OPEN cur_car
		IF @@CURSOR_ROWS > 0
		 BEGIN 
		 FETCH NEXT FROM cur_car INTO 
		@RW, @R_CustomerID,@R_CustomerName, @R_MobileNo, @R_DOB, @R_Address , @R_Email ,@R_PAN_ID , @CustID , @R_CardNo , @EncPAN 
		 WHILE @@Fetch_status = 0
		 BEGIN
		 PRINT @EncPAN
		 PRINT @IssuerNo
				--Get Card details from switch
				INSERT INTO @tblResult(goods_nr_trans_lim,goods_lim,tran_goods_lim,goods_offline_lim,cash_nr_trans_lim,cash_lim,tran_cash_lim,cash_offline_lim,
				paymnt_nr_trans_lim,paymnt_lim,tran_paymnt_lim,paymnt_offline_lim,tran_paymnt_offline_lim,cnp_lim,tran_cnp_lim,cnp_offline_lim,tran_cnp_offline_lim,
				last_updated_date,last_updated_user,card_program , card_status ,  expiry_date,HRC,customer_id,Issue_Date,EncryptAccount)
				exec  [AGSS1RT].postcard.dbo.[Usp_GetCardLimit_AGS] @EncPAN ,@IssuerNo,@IntpriOutput output ,@StrpriOutputDesc output
		
				select @panid=max(isnull(id,0)) from @tblResult
				update @tblResult set encryptpan=@encpan where id=@panid
				delete from @tblResult  where id=@panid and @IntpriOutput<>0
				--select @EncPAN, @IntpriOutput  ,@StrpriOutputDesc 

		 FETCH NEXT FROM cur_car INTO @RW, @R_CustomerID,@R_CustomerName, @R_MobileNo, @R_DOB, @R_Address , @R_Email ,@R_PAN_ID , @CustID , @R_CardNo , @EncPAN 
		 END
		END
		CLOSE cur_car
		DEALLOCATE cur_car
		SET NOCOUNT OFF 

	--select * from @tblResult
	print 'inprioutput' 
	print @IntpriOutput 
	--select @EncPAN
--if card present


		Select  customer_id  , R_CustomerName AS [CustomerName] , R_MobileNo AS MobileNo, R_DOB AS [DOB],
			  R_Address AS [Address] , R_Email As [Email]				
			, goods_nr_trans_lim AS PurchaseNo,goods_lim AS PurchaseDailyLimit,tran_goods_lim AS PurchasePTLimit
			 ,cash_nr_trans_lim  AS WithDrawNO,cash_lim AS WithDrawDailyLimit,tran_cash_lim AS WithDrawPTLimit,
			paymnt_nr_trans_lim AS PaymentNO,paymnt_lim AS PaymentDailyLimit,tran_paymnt_lim AS PaymentPTLimit
			,cnp_lim AS CNPDailyLimit,tran_cnp_lim AS CNPPTLimit
			, R_CardNo AS [CardNo],expiry_date AS ExpiryDate
			,case when cr.SwitchCode=2 then 'Repin' else CR.RequestType end CardStatus 
		    ,t.Issue_Date CardIssueddate				
			,HRC
			,CASE WHEN ((ISNULL(HRC,0)='06' ) OR(ISNULL(HRC,0)=0)) THEN '1' ELSE '0' END AS [CardStatusID]  --for temporary block, show activate card option
			, R_PAN_ID AS [PAN_ID]  ---for customerID logic change
			,dbo.ufn_decryptPAN(A.DecAcc) AS [AccountNo]
			,CustID CustomerId
			from
			#temp m inner join
			 @tblResult  t on m.encpan=t.encryptpan 
			 --inner join TblCustomersDetails c with(nolock) on m.CustID
			INNER JOIN TblCardRequests CR WITH(NOLOCK) ON ISNULL(t.HRC,0)=ISNULL(CR.SwitchCode,0) AND CR.Flag='C'
			LEFT JOIN CardRAccounts A with(NOLOCK) ON t.EncryptAccount=A.EncAcc AND A.IssuerNo=@IssuerNo
     END

	END	
	 --------- for getting customers details,card details by issuerno, Pan,AccountNo
	 ELSE
	 BEGIN
			 print 'ELSE for getting customers details,card details by issuerno, Pan,AccountNo'
				---- Get encrypted PAN
								SELECT @EncPAN=EncPAN,@IssuerNo=B.bankCode FROM CardRPAN P WITH(NOLOCK)
		INNER JOIN Tblbanks B WITH(NOLOCK) ON P.IssuerNo=B.BankCode
		 WHERE( dbo.ufn_DecryptPAN(DecPAN)=@CardNo) AND B.ID=@BankID 
			---- Get encrypted Account
								SELECT @EncAcccount=EncAcc,@IssuerNo=B.bankCode FROM CardRAccounts A WITH(NOLOCK) 
		INNER JOIN Tblbanks B WITH(NOLOCK) ON A.IssuerNo=B.BankCode
		WHERE( dbo.ufn_DecryptPAN(DecAcc)=@AccountNo)  AND B.ID=@BankID
				--Select @EncPAN ,@EncAcccount
																																																																																IF((@IssuerNo<>'') OR (@EncAcccount<>'') OR(@EncPAN<>'') )
			BEGIN
    DECLARE @tblCustDetail TABLE
		(CustomerName	VARCHAR(800)	,
			mobile_nr	VARCHAR(800)	,
			email	VARCHAR(800)	,
			pan_encrypted	VARCHAR(800)	,
			account_id_encrypted	VARCHAR(800)	,
			account_type	VARCHAR(800)	,
			issuer_nr	VARCHAR(800)	,
			customer_id	VARCHAR(800)	,
			currency_code	VARCHAR(800)	,
			account_type_qualifier	VARCHAR(800)	,
			date_issued	VARCHAR(800)	,
			card_program	VARCHAR(800)	,
			card_status	VARCHAR(800)	,
			HRC	VARCHAR(800)	,
			[ExpiryDate]	VARCHAR(800)	,
			[AddressDetail]	VARCHAR(800)		
		)
		INSERT INTO @tblCustDetail(CustomerName	,mobile_nr	,email	,pan_encrypted	,account_id_encrypted	,account_type	,
					issuer_nr	,customer_id	,currency_code	,account_type_qualifier	,date_issued	,card_program	,card_status	,
					HRC	,[ExpiryDate],[AddressDetail]	)	
						exec  [AGSS1RT].postcard.dbo.Usp_GetCardDetails_AGS @IssuerNo,@EncPan,@EncAcccount,@IntpriOutput output ,@StrpriOutputDesc output
		--Select @IntpriOutput,@StrpriOutputDesc
        --if card present
		If(@IntpriOutput=0)
		BEGIN 
				--Select * from @tblCustDetail 
		   Select  --c.CustomerID,
		   c.BankCustID AS [CustomerID],CustomerName,mobile_nr AS [MobileNo],email AS [Email],dbo.ufn_decryptPAN(P.DecPAN) AS [CardNo],dbo.ufn_decryptPAN(A.DecAcc) AS [AccountNo]
		   ,t.date_issued  AS [IssuedDate]
		   from @tblCustDetail t 
		   left JOIN CardRPAN P WITH(NOLOCK)ON t.pan_encrypted=P.EncPAN
		   left JOIN CardRAccounts A WITH(NOLOCK) ON t.account_id_encrypted=A.EncAcc
			INNER JOIN TblCustomersdetails c WITH(NOLOCK) ON t.customer_id=c.bankcustID
			where c.BankID=@BankID ANd c.SystemID=@SystemID 
		END
	END
	 END 

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
