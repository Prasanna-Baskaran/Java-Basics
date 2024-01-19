USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetSetCardLimit_20180604]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec Sp_GetSetCardLimit @IntPara=0,@CardNo='4660460000000165',@SystemID=2,@BankID=1
CREATE PROCEDURE [dbo].[Sp_GetSetCardLimit_20180604]
	@CustomerID bigint=0,
	@IntPara Smallint=NULL,
	
	@CreatedByID bigint=0,		
	@CardNo VARCHAR(30)='',
	@PurchaseNo int=0,
	@PurchaseDailyLimit  numeric(12,2)=0,
	@PurchasePTLimit NUMERIC(12,2)=0,
	@WithDrawNO int=0,
	@WithDrawDailyLimit  numeric(12,2)=0,
	@WithDrawPTLimit NUMERIC(12,2)=0,
    @PaymentNO int=0,
	@PaymentDailyLimit  numeric(12,2)=0,
	@PaymentPTLimit NUMERIC(12,2)=0,
	@CNPPTLimit NUMERIC(12,2)=0,
	@CNPDailyLimit  NUMERIC(12,2)=0,
	@MakerID	bigint=0,
	@UpdateRemark VARCHAR(100)='',

	@ReqTypeID int=0,
	@SystemID varchar(200)=1,
	@BankID varchar(200)=1,
	@BankCustID VARCHAR(800)=''	
		


AS
BEGIN
Begin Transaction  
	Begin Try    
	--********************************** Search Customer ************************************************
	IF(@IntPara=0)
	BEGIN
	  IF exists(Select Top 1 1 from Tblcustomersdetails c WITH(NOLOCK)INNER JOIN cardrpan P WITH(NOLOCK) ON RTRIM(LTRIM(C.BankCustID))=RTRIM(LTRIM(P.customer_id))
				Where dbo.ufn_decryptpan(P.decpan)=@CardNo AND ((@BankCustID='') OR(P.customer_id=@BankCustID) OR(c.CustomerID=@BankCustID))
				AND c.BankID=@BankID AND c.SystemID=@SystemID
				)
				BEGIN
						EXEC Sp_GetSwitchCardDetails  @CardNO=@CardNo,@SystemID=@SystemID,@BankID=@BankID

--select 'MUNU001000196101'	customer_id
--,'BISHWA KHANAL' CustomerName
--,''MobileNo
--,	'01/01/1900'	DOB
--,'SANEPA-2    NO_DATA ' Address
--,''Email
--,	'5'PurchaseNo
--,	'80000'PurchaseDailyLimit
--,	'30000'	 PurchasePTLimit
--,'5'	WithDrawNO
--,'60000' WithDrawDailyLimit
--,'30000'	WithDrawPTLimit
--,'5'	PaymentNO
--,'80000'	PaymentDailyLimit
--,'30000' PaymentPTLimit
--,'80000'	CNPDailyLimit
--,'30000'	CNPPTLimit
--,'4660460000000165'	CardNo
--,'1804'	ExpiryDate
--,'Activate Card'	CardStatus
--,'10/04/2012'	CardIssueddate
--,'0 '	HRC
--,'1'	CardStatusID
--,'490969'	PAN_ID
--,'00111900015779000001'	AccountNo
--,'551207' CustomerId


	            END
	END
	------**************************** Card Limit Insert ******************************
	  ELSE	IF(@IntPara=1)
		BEGIN	
				Declare @StrPriOutput varchar(1)='1'		
				Declare @StrPriOutputDesc varchar(200)='Card limit is not save '
				Declare @CardRPANID BIGINT=0
				IF(@CustomerID <> 0 AND @CardNo <> '')
			     BEGIN
		    	--Modify Limit
				  IF EXISTS(Select 1 From TblCustomersDetails Cu WITH(NOLOCK)
				      INNER JOIN CardRPAN PAN WITH(NOLOCK) ON Cu.BankCustID=PAN.customer_id 
				    WHERE Cu.CustomerID=@CustomerID AND
					    dbo.ufn_DecryptPAN(PAN.DecPAN) =@CardNo
				         AND  ISNULL(IsCardSuccess,0)=1
						 AND ISNULL(FormStatusID,0)=1
						 AND cu.SystemID=@SystemID
						 AND cu.BankID=@BankID

				  )
				  BEGIN
				  --customer logic change
							--SELECT @CardRPANID=ID FROM CardRPAN WITH(NOLOCK) WHERE @CardNo=dbo.ufn_DecryptPAN(DecPAN) AND customer_id=@CustomerID				   
								SELECT @CardRPANID=P.ID  FROM TblCustomersDetails Cu WITH(NOLOCK) 
							INNER JOIN CardRPAN P WITH(NOLOCK) ON Cu.BankCustID=P.customer_id 
							INNER JOIN TblBanks B WITH(NOLOCK) ON P.issuerNo=B.BankCode
							 WHERE @CardNo=dbo.ufn_DecryptPAN(DecPAN) AND B.ID=@BankID


							IF EXISTS(SELECT 1 FROM TblCardLimits WITH(NOLOCK) WHERE CardRPAN_ID=@CardRPANID  AND SystemID=@SystemID AND BankID=@BankID)
							BEGIN
							--maintain history
							INSERT INTO TblCardLimits_History  (PurchaseNo,PurchaseDailyLimit,PurchasePTLimit,WithDrawNO,WithDrawDailyLimit,WithDrawPTLimit
                                         ,PaymentNO,PaymentDailyLimit,PaymentPTLimit,MakerID,CreatedDate,ModifiedByID,ModifiedDate,CheckerID,CheckedDate
											,FormStatusID,Remark,CustomerID,CardRPAN_ID,UpdateRemark
											,PurchaseNo_O,PurchaseDailyLimit_O,PurchasePTLimit_O,WithDrawNO_O,WithDrawDailyLimit_O
											,WithDrawPTLimit_O,PaymentNO_O,PaymentDailyLimit_O,PaymentPTLimit_O,CNPDailyLimit
											,CNPPTLimit,CNPDailyLimit_O,CNPPTLimit_O,Delete_Date,RequestTypeID,SwitchResponse,SystemID,BankID)
										SELECT PurchaseNo,PurchaseDailyLimit,PurchasePTLimit,WithDrawNO,WithDrawDailyLimit,WithDrawPTLimit
                                         ,PaymentNO,PaymentDailyLimit,PaymentPTLimit,MakerID,CreatedDate,ModifiedByID,ModifiedDate,CheckerID,CheckedDate
											,FormStatusID,Remark,CustomerID,CardRPAN_ID,UpdateRemark
											,PurchaseNo_O,PurchaseDailyLimit_O,PurchasePTLimit_O,WithDrawNO_O,WithDrawDailyLimit_O
											,WithDrawPTLimit_O,PaymentNO_O,PaymentDailyLimit_O,PaymentPTLimit_O,CNPDailyLimit
											,CNPPTLimit,CNPDailyLimit_O,CNPPTLimit_O,GETDATE(),RequestTypeID ,SwitchResponse,SystemID,BankID FROM  TblCardLimits WITH(NOLOCK)
											 WHERE CardRPAN_ID=@CardRPANID AND CustomerID=@CustomerID AND SystemID=@SystemID And BankID=@BankID
							   DELETE TblCardLimits  WHERE CardRPAN_ID=@CardRPANID AND CustomerID=@CustomerID AND SystemID=@SystemID AND BankID=@BankID
							END

						   INSERT INTO TblCardLimits (PurchaseNo,PurchaseDailyLimit,PurchasePTLimit,WithDrawNO,WithDrawDailyLimit,WithDrawPTLimit
									 ,PaymentNO,PaymentDailyLimit,PaymentPTLimit,MakerID,CreatedDate,FormStatusID,UpdateRemark,CardRPAN_ID
									 ,CNPDailyLimit,CNPPTLimit,CustomerID,RequestTypeID,SystemID,BankID)
									 VALUES(@PurchaseNo,@PurchaseDailyLimit,@PurchasePTLimit,@WithDrawNO,@WithDrawDailyLimit,@WithDrawPTLimit
									 ,@PaymentNO,@PaymentDailyLimit,@PaymentPTLimit,@MakerID,GETDATE(),0,@UpdateRemark,@CardRPANID,@CNPDailyLimit,@CNPPTLimit,@CustomerID,1,@SystemID						 
									 ,@BankID
									 )
						   IF(@@ROWCOUNT>0)
						   BEGIN				      
								SET @StrPriOutput='0'
								SET @StrPriOutputDesc='Card limit is save successfully '
						   END
						   ELSE
						   BEGIN
							 SET @StrPriOutput='1'
							 SET @StrPriOutputDesc='Card limit is not save'
						   END	
				  END
				  ELSE
				  BEGIN
							 SET @StrPriOutput='1'
							 SET @StrPriOutputDesc='Card limit is not save'
				  END

			
			  END
				ELSE
				BEGIN
				 Set @StrPriOutput ='1'		
				 Set @StrPriOutputDesc ='Card limit is not save '
				END

				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
	   END

	  

		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
	 IF(@IntPara=1)
	  BEGIN
		SELECT 1  As Code,'Card limit is not save' As [OutputDescription]
	 END
	 --else IF(@IntPara=2)
	 --BEGIN
	 -- SELECT 1  As Code,'Card request is not save' As [OutputDescription]
	 --END
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	
END


GO
