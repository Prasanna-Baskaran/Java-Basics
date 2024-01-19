USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_AcceptRejectCardLimit]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_AcceptRejectCardLimit]	
	@CheckerID BIGINT,
	@RequestTypeID INT=0,
	@Remark VARCHAR(200)='',
	@ReqID VARCHAR(500)='',
	@FormStatusID int,	
	@BankID Varchar(200)=1,
	@SystemID Varchar(200)=1
AS
BEGIN
	Begin Transaction  
	Begin Try    
	  
	 declare @tbl table(value varchar(200),RowID int)
	 	 insert into @tbl (value,RowID)(SELECT VALUE,RowID FROM dbo.fnSplit(@ReqID,','))
		 Declare @IssuerNo Int=23

		 SELECT @IssuerNo=BankCode from TblBanks WITH(NOLOCK) Where ID=@BankID

	 IF EXISTS( Select COUNT(1) fROM @tbl)
	 BEGIN

	 --ACCEPT REQ
	    IF(@FormStatusID=1)
		BEGIN

		  --SWITCH  Card limit Update Call
			DECLARE @IntCurID int
			DECLARE @IntMaxCount int
			DECLARE @IntRowID int=1

			Declare @IntpriOutput int=1 ,@StrpriOutputDesc Varchar(500)='' 			  
			 SELECT @IntMaxCount= Count(1) from @tbl t
			 INNER JOIN TblCardLimits ct ON t.value=ct.ID
			 INNER JOIN CardRPAN CP ON ct.CardRPAN_ID=CP.ID
			 

		 IF(@IntMaxCount>0)
		 BEGIN
		  
		 While (@IntRowID<=@IntMaxCount)
			BEGIN			
			   
			  SET @IntpriOutput=1
			  --Get current ID  to  update
			 SELECT @IntCurID=value from @tbl WHERE RowID=@IntRowID			
			 
			 DECLARE	@RPAN			VarChar(Max)=''			  
			  , @GoodsNrTran	Numeric(18,0)=0
			  , @GoodsLimit		Numeric(18,2)=0
			  ,	@GoodsLimitPT	Numeric(18,2)=0
			  ,	@CashNrTran		Numeric(18,0)=0
			  ,	@CashLimit		Numeric(18,2)=0
			  ,	@CashLimitPT	Numeric(18,2)=0
			  ,	@PaymentNrTran	Numeric(18,0)=0
			  ,	@PaymentLimit	Numeric(18,2)=0
			  ,	@PaymentLimitPT	Numeric(18,2)=0
			  ,	@CNPLimit		Numeric(18,2)=0
			  ,	@CNPLimitPT		Numeric(18,2)=0
			  ,	@Login			Varchar(100) = 'AGS_App'
				 

				 SELECT 	@RPAN=EncPAN,@GoodsNrTran=CL.PurchaseNo,@GoodsLimit=cl.PurchaseDailyLimit,@GoodsLimitPT=CL.PurchasePTLimit
				  ,@CashNrTran=CL.WithDrawNO,@CashLimit=CL.WithDrawDailyLimit,@CashLimitPT=CL.WithDrawPTLimit
				  ,@PaymentNrTran=Cl.PaymentNO,@PaymentLimit=CL.PaymentDailyLimit,@PaymentLimitPT=CL.PaymentPTLimit
				  ,@CNPLimit=CL.CNPDailyLimit,@CNPLimitPT=CL.CNPPTLimit				 	
				  from TblCardLimits CL WITH(NOLOCK) INNER JOIN CARDRPAN CP WITH(NOLOCK) ON CL.CardRPAN_ID=CP.ID 
				  Where CL.ID= @IntCurID AND CL.SystemID=@SystemID AND CL.BankID=@BankID

				  
				  ---- Start  Saving old limits
					  BEGIN TRY
	        	DECLARE @tblResult TABLE
					   	(
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
						  EncryptAccount VARCHAR(800)
						)
							Declare @IntSWOutput int=1 ,@StrSWOutputDesc Varchar(500)='' 			  
							--Get Card limits from switch
						INSERT INTO @tblResult(goods_nr_trans_lim,goods_lim,tran_goods_lim,goods_offline_lim,cash_nr_trans_lim,cash_lim,tran_cash_lim,cash_offline_lim,paymnt_nr_trans_lim,paymnt_lim,tran_paymnt_lim,paymnt_offline_lim
						,tran_paymnt_offline_lim,cnp_lim,tran_cnp_lim,cnp_offline_lim,tran_cnp_offline_lim,last_updated_date,last_updated_user,card_program , card_status ,  expiry_date,HRC,customer_id,Issue_Date,  EncryptAccount )
						exec AGSS1RT.postcard.dbo.[Usp_GetCardLimit_AGS] @RPAN,@IssuerNo,@IntSWOutput output ,@StrSWOutputDesc output

						IF(@IntSWOutput=0)
						BEGIN
							UPDATE TblCardLimits SET PurchaseNo_O= TR.goods_nr_trans_lim ,PurchaseDailyLimit_O=TR.goods_lim,PurchasePTLimit_O=TR.tran_goods_lim
								,WithDrawNO_O=TR.cash_nr_trans_lim,	WithDrawDailyLimit_O=TR.cash_lim,WithDrawPTLimit_O=TR.tran_cash_lim
								, PaymentNO_O=TR.paymnt_nr_trans_lim,	PaymentDailyLimit_O=TR.paymnt_lim,	PaymentPTLimit_O =TR.tran_paymnt_lim
								FROM ( SELECT goods_nr_trans_lim,goods_lim,tran_goods_lim,cash_nr_trans_lim,cash_lim,tran_cash_lim,cash_offline_lim,
												paymnt_nr_trans_lim,paymnt_lim,tran_paymnt_lim,paymnt_offline_lim,tran_paymnt_offline_lim
												,cnp_lim,tran_cnp_lim,cnp_offline_lim,tran_cnp_offline_lim FROM  @tblResult )TR 
							 WHERE  ID=@IntCurID AND SystemID=@SystemID AND BankID=@BankID
						 END
						
	
					  END TRY
					   BEGIN CATCH
					   
					   END CATCH
				   ---- END  Saving old limits
				  --*****************  Switch Sp Limit Update Call *******************
				  BEGIN Try
				 
					exec  AGSS1RT.postcard.dbo.[Usp_SetCardLimit_AGS] @RPAN= @RPAN,@IssuerNo=@IssuerNo,@GoodsNrTran=@GoodsNrTran,@GoodsLimit=@GoodsLimit,@GoodsLimitPT=@GoodsLimitPT
				                             ,@CashNrTran=@CashNrTran ,@CashLimit=@CashLimit,@CashLimitPT=@CashLimitPT
											 ,@PaymentNrTran=@PaymentNrTran,@PaymentLimit=@PaymentLimit,@PaymentLimitPT=@PaymentLimitPT
											 ,@CNPLimit=@CNPLimit,@CNPLimitPT=@CNPLimitPT,@Login=@Login,@IntpriOutput=@IntpriOutput output,@StrpriOutputDesc =@StrpriOutputDesc OUTPUT				 				 
				  END TRY
					BEGIN CATCH				
						SET @IntpriOutput=1;
						SET @StrpriOutputDesc='Error occurs';					
			--			GOTO ExceptionErrorLog;	
					END CATCH				 
						 
				 IF(@IntpriOutput=0)
				  BEGIN
		             UPDATE TblCardLimits SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,CheckedDate=GETDATE(),IsSuccess=1,SwitchResponse=@StrpriOutputDesc WHERE  ID=@IntCurID AND SystemID=@SystemID AND BankID=@BankID
				  END
				  ELSE
				  BEGIN
				    UPDATE TblCardLimits SET CheckerID=@CheckerID ,CheckedDate=GETDATE(),IsSuccess=0,SwitchResponse=@StrpriOutputDesc WHERE  ID=@IntCurID AND SystemID=@SystemID AND BankID=@BankID
				  END
				SET @IntRowID +=1				
			END
		 END
		  
		--UPDATE TblCardLimits SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,CheckedDate=GETDATE(),IsSuccess=1 WHERE ID IN(SELECT value FROM @tbl)
		

			SELECT  Co.ID, C.bankcustID AS [CustomerID],dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],(C.FirstName +' '+ISNULL(LastName,'')) AS [CustomerName]
				,fs.FormStatus AS [Status],fs.FormStatusID ,CR.RequestType,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
				,Co.RequestTypeID
				From TblCardLimits Co WITH(NOLOCK)
				INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
				INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPAN_ID=pan.ID
				INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
				INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
				INNER JOIN @tbl temp ON temp.Value=co.ID		 
				where co.SystemID=@SystemID AND co.BankID=@BankID
		END
		--REJECT REQUEST	    
		ELSE IF(@FormStatusID=2)
		BEGIN
		  UPDATE TblCardLimits SET FormStatusID=@FormStatusID ,CheckerID=@CheckerID ,CheckedDate=GETDATE(),Remark=@Remark,IsSuccess=1,SwitchResponse='' WHERE ID IN(SELECT value FROM @tbl)  AND SystemID=@SystemID AND BankID=@BankID 
		    AND SystemID=@SystemID AND BankID=@BankID
		  SELECT  Co.ID, C.bankcustID AS [CustomerID],dbo.ufn_DecryptPAN(Pan.DecPAN) AS [CardNo],(C.FirstName +' '+ISNULL(LastName,'')) AS [CustomerName]
			,fs.FormStatus AS [Status],fs.FormStatusID ,cr.RequestType,Case WHEN ISNULL(Co.ISSuccess,0)=1 THEN 'Success' ELSE 'Failed' END AS[Response] 
			,Co.RequestTypeID
			From TblCardLimits Co WITH(NOLOCK)
			INNER JOIN TblCustomersDetails C WITH(NOLOCK)  ON C.CustomerID=Co.CustomerID
			INNER JOIN CardRPAN Pan WITH(NOLOCK) ON co.CardRPAN_ID=pan.ID
			INNER JOIN TblFormStatus fs WITH(NOLOCK) ON ISNULL(co.FormStatusID,0)=fs.FormStatusID
			INNER JOIN TblCardRequests CR WITH(NOLOCK) ON Co.RequestTypeID=Cr.ID
			INNER JOIN @tbl temp ON temp.Value=co.ID		 
			where co.SystemID=@SystemID AND co.BankID=@BankID
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
