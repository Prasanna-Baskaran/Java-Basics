USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetSaveCardOpsRequest]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--search ---[Sp_GetSaveCardOpsRequest] 0,0,0,'4660461000000189',0,'',0,2,1,'','',''
--block card ---[Sp_GetSaveCardOpsRequest] 18836,1,1,'4660461000000189',0,'block card',3,2,1,'','',''
CREATE PROCEDURE [dbo].[Sp_GetSaveCardOpsRequest] --[Sp_GetSaveCardOpsRequest] 0,0,0,'4660461000000247',0,'',0,2,1,'','DEBNT000000000008827',''
	@CustomerID bigint=0,
	@IntPara Smallint=NULL,	
	@CreatedByID bigint=0,		
	@CardNo VARCHAR(30)='',	
	@MakerID	bigint=0,
	@UpdateRemark VARCHAR(100)='',
	@ReqTypeID int=0,
	@SystemID Varchar(200)=1,
	@BankID Varchar(200)=1,
	@Name VARCHAR(800)='',
	@BankCustID VARCHAR(800)=''	,
	@AccountNo VARCHAR(800)	=''
AS
BEGIN
Begin Transaction  
	Begin Try    	
	--********************************** Search Customer ************************************************
	IF(@IntPara=0)
	BEGIN
	  Declare @MaskCard varchar(30)
	  set @MaskCard= LEFT(@CardNo,6)+replicate('*',len(CONVERT(varchar (20),@CardNo))-10)+RIGHT(CONVERT(varchar (20),@CardNo),4)
	  
	  IF exists(Select Top 1 1 from Tblcustomersdetails c WITH(NOLOCK)
				INNER JOIN cardrpan P WITH(NOLOCK) ON RTRIM(LTRIM(C.BankCustID))=RTRIM(LTRIM(P.customer_id))
				Where 
				MKSP= @MaskCard and
				dbo.ufn_decryptpan(P.decpan)=@CardNo 
				AND ((@BankCustID='') OR(P.customer_id=@BankCustID) OR(c.CustomerID=@CustomerID))
				AND c.BankID=@BankID AND c.SystemID=@SystemID
				)
				BEGIN
				print 1
				  EXEC Sp_GetSwitchCardDetails @CardNO=@CardNo,@SystemID=@SystemID,@BankID=@BankID,@Name=@Name--,@CustomerID=@BankCustID
				END
	  
	END	
	   ---************************* Card Operation ********************
	ELSE	IF(@IntPara=1)
		BEGIN	
			Declare @StrPriOutput varchar(1)='1'		
						Declare @StrPriOutputDesc varchar(200)='Card request is not save'
						Declare @CardRPANID BIGINT=0
						DECLARE @SwitchHRC VARCHAR(20)=''
				IF(@CustomerID <> 0 AND @CardNo <> '')
			     BEGIN
		    	
				  IF EXISTS(Select 1 From TblCustomersDetails Cu WITH(NOLOCK)
				      INNER JOIN CardRPAN PAN WITH(NOLOCK) ON Cu.BankCustID=PAN.customer_id 
				    WHERE --Cu.CustomerID=@CustomerID  AND
					     dbo.ufn_DecryptPAN(PAN.DecPAN) =@CardNo
				        -- AND  ISNULL(IsCardSuccess,0)=1
						 AND ISNULL(FormStatusID,0)=1
						 AND cu.SystemID=@SystemID 
						 AND cu.BankID=@BankID
				  )
				  BEGIN
				  --- start diksha customerID logic change
							SELECT @CardRPANID=P.ID  FROM TblCustomersDetails Cu WITH(NOLOCK) 
							INNER JOIN CardRPAN P WITH(NOLOCK) ON Cu.BankCustID=P.customer_id 
							INNER JOIN TblBanks B WITH(NOLOCK) ON P.issuerNo=B.BankCode
							 WHERE @CardNo=dbo.ufn_DecryptPAN(DecPAN) AND B.ID=@BankID --AND customer_id=@CustomerID --change customeid logic				   

							SELECT @SwitchHRC=SwitchCode FROM TblCardRequests WITH(NOLOCK) WHERE ID=@ReqTypeID
							IF EXISTS(SELECT 1 FROM TblCardOpsRequestLog WITH(NOLOCK) WHERE CardRPANID=@CardRPANID and SystemID=@SystemID And BankID=@BankID)
							BEGIN
							--maintain history
							INSERT INTO TblCardOpsRequestLog_history  (RequestTypeID,CustomerID,CardRPANID,FormStatusID,MakerID,ModifiedByID
                              ,ModifiedDate,CheckerID,CheckedDate,CreatedDate,Remark,RejectReason,IsSuccess,SwitchHRC,SwitchResponse,SystemID)
										SELECT RequestTypeID,CustomerID,CardRPANID,FormStatusID,MakerID,ModifiedByID
                              ,ModifiedDate,CheckerID,CheckedDate,CreatedDate,Remark,RejectReason,IsSuccess,SwitchHRC,SwitchResponse,SystemID FROM  TblCardOpsRequestLog WITH(NOLOCK)
											 WHERE CardRPANID=@CardRPANID  AND CustomerID=@CustomerID
											 AND SystemID=@SystemID AND BankID=@BankID
							   DELETE TblCardOpsRequestLog  WHERE CardRPANID=@CardRPANID AND CustomerID=@CustomerID AND SystemID=@SystemID AND BankID=@BankID
							END

						   INSERT INTO TblCardOpsRequestLog (RequestTypeID,CustomerID,CardRPANID,FormStatusID,MakerID,CreatedDate,Remark,SwitchHRC,SystemID,BankID)
									 VALUES(@ReqTypeID,@CustomerID,@CardRPANID,0,@MakerID,GETDATE(),@UpdateRemark,@SwitchHRC,@SystemID ,@BankID)
						   IF(@@ROWCOUNT>0)
						   BEGIN				      
								SET @StrPriOutput='0'
								SET @StrPriOutputDesc='Card request is save successfully '
						   END
						   ELSE
						   BEGIN
							 SET @StrPriOutput='1'
							 SET @StrPriOutputDesc='Card request is not save'
						   END	
				  END
				  ELSE
				  BEGIN
							 SET @StrPriOutput='1'
							 SET @StrPriOutputDesc='Card request is not save'
				  END		
			  END
				ELSE
				BEGIN
				 Set @StrPriOutput ='1'		
				 Set @StrPriOutputDesc ='Card request is not save '
				END

				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
	   END

	   ---GET Individual card request raised
	ELSE IF(@IntPara=2)
	   BEGIN
	     	 SELECT  C.bankCustID ,(C.FirstName + ' '+C.MiddleName+' '+C.LastName) AS [CustomerName] ,C.MobileNo,Convert(varchar(10),C.DOB_AD,103) AS [DOB],
		Cd.PO_Box_P+'  '+Cd.HouseNo_P +'  '+Cd.StreetName_P+'  '+Cd.WardNo_P +'  '+Cd.City_P+'  '+Cd.District_P AS [Address] ,Cd.Email_P As [Email]	
		from TblCardOpsRequestLog CO WITH(NOLOCK) 
		INNER JOIN TblCustomersDetails C WITH(NOLOCK) ON  Co.CustomerID=C.CustomerID 
		INNER JOIN TblCustomerAddress  Cd WITH(NOLOCK) ON C.CustomerID=Cd.CustomerID
		LEFT JOIN CardRPAN CP WITH(NOLOCK) ON C.BankCustID=Cp.customer_id
		WHERE ISNULL(C.FormStatusID,0)=1 
		--AND ISNULL(C.IsCardSuccess,0)=1
		AND ((@CardNo='') OR (@CardNo=dbo.ufn_DecryptPAN(Cp.DecPAN))) 
		AND ((C.CustomerID=@CustomerID) OR(@CustomerID=0))		
		AND ((CO.SystemID=@SystemID))
		AND ((CO.BankID=@BankID))
	   END

	   ---************************ Get Details Of Cards ****************
   ELSE IF(@IntPara=3)
    BEGIN
	 -- Select Cu.FirstName+' '+cu.LastName As [Customer Name],cu.MobileNo,ca.Email_P,ISNULL(dbo.ufn_DecryptPAN(pan.DecPAN),'')AS [CardNo],Case WHEN ISNULL(dbo.ufn_DecryptPAN(pan.DecPAN),'')='' THEN '' Else convert(Varchar(500),cu.CustomerID) END AS [AccountNo] from TblCustomersDetails Cu WITH(NOLOCK)
		--Left JOIN CardRPAN pan WITH(NOLOCK) ON Cu.bankCustID=pan.customer_id
		--INNER JOIN TblCustomerAddress Ca WITH(NOLOCK) ON Cu.CustomerID=ca.CustomerID
		--where cu.FormStatusID=1  AND Cu.IsCardSuccess=1
		--AND( (@Name='') OR( Upper(RTRIM(LTRIM(Cu.FirstName))+RTRIM(LTRIM(Cu.LastName)))  like '%'+UPPER(REPLACE((RTRIM(LTRIM(@Name))),' ','')+'%')))
		--AND ((@CardNo='') OR (@CardNo=dbo.ufn_DecryptPAN(pan.DecPAN))) 		
		--AND((@BankCustID='')OR(Cu.bankCustID=@BankCustID))  ---27/07
		--AND (cu.SystemID=@SystemID)
		--AND (cu.BankID=@BankID)
		
			exec [Sp_GetSwitchCardDetails] @SystemID=@SystemID,@BankID=@BankID,@CardNO=@CardNO,@IntPara=0,@Name=@Name,@AccountNo=@AccountNo
       
	END
		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	  RollBACK TRANSACTION; 
	 IF(@IntPara=1)
	 BEGIN
		 
	  SELECT 1  As Code,'Card request is not save' As [OutputDescription] 
	  END	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  	
END

GO
