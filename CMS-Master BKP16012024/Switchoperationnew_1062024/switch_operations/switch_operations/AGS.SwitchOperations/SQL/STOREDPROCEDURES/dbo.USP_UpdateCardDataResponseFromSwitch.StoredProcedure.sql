
--exec USP_UpdateCardDataResponseFromSwitch 54,111,3,181005,'000','','encpan','12345678912','encacc','',''
ALTER procedure [dbo].[USP_UpdateCardDataResponseFromSwitch]
	@IssuerNo numeric=null,
	@FileId varchar(100)=null,
	@ProcessId bigint=null,
	@Code numeric=null,
	@ISORspCode varchar(5)=null,
	@ISORspDesc varchar(500)=null,
	@NewEncPan varchar(200)=null,
	@CardNo varchar(50)=null,
	@NewEncAcc varchar(200)=null,
	@CustomerId varchar(50)=null,
	@AccountId varchar(50)=null
as

/************************************************************************
Jira Id: ATPCM-620 
Purpose: This Store Procedure used for Log ISO response from Switch
Created History
Date        Created By				Reason
31/08/2018  Pratik Mhatre			Newly Developed
*************************************************************************/

begin
	Declare @BinPrefix varchar(20)
	Declare @CardProgram varchar(100)
	Declare @BankId varchar(100)
		
	if @ISORspCode='000' or @ISORspCode='108'
	begin
		/*INSERT CardNo. in CardRpan*/
		IF (ISNULL(@NewEncPan,'')<>'') and (ISNULL(@CardNo,'')<>'') and NOT EXISTS (select top 1 1 from  CardRpan with (Nolock) where IssuerNo=@IssuerNo and EncPAN=@NewEncPan )
		BEGIN
			INSERT INTO CardRpan (EncPAN,DecPAN,IssuerNo,customer_id,MKSP) 
			VALUES (@NewEncPan,dbo.ufn_encryptpan(@CardNo),@IssuerNo,@CustomerId,LEFT(@CardNo,6)+replicate('*',len(CONVERT(varchar (20), @CardNo))-10)+RIGHT(CONVERT(varchar (20), @CardNo),4))
		END
		
		/*INSERT CardNo. in CardRAccounts*/
		IF (ISNULL(@NewEncAcc,'')<>'') and (ISNULL(@AccountId,'')<>'') and NOT EXISTS (select top 1 1 from CardRAccounts with(Nolock) where IssuerNo=@IssuerNo and EncAcc=@NewEncAcc)
		BEGIN
			INSERT INTO CardRAccounts (EncAcc,DecAcc,IssuerNo,SchemeCode,DefaultCurrency) 
			VALUES (@NewEncAcc,dbo.ufn_encryptpan(@AccountId),@IssuerNo,'','')
		END

		 select top 1 @BankId=ID from tblbanks nolock where BankCode=@IssuerNo
		 	 
		 select @BinPrefix=CardPrefix,@CardProgram=CardProgram from tblbin nolock 
		 where BankID=@BankId and CardPrefix=left(@CardNo,LEN(Cardprefix))
		 
		 if not exists(select top 1 1 from CardProgramsForStandard4 (nolock) where IssuerNo=@IssuerNo and FileId=@FileId and BinPrefix=@BinPrefix)
		 begin	 
			 insert into CardProgramsForStandard4(IssuerNo,FileId,ProcessId,BinPrefix,CardProgram,InsertedOn)
			 values(@IssuerNo,@FileId,@ProcessId,@BinPrefix,@CardProgram,GETDATE())	
		 end	  
	end
		
	if @ProcessId=3 /*This is for Issuance*/
		begin
			update tblcardproduction set SwitchRspCode=@ISORspCode,SwitchRspDesc=@ISORspDesc where Code=@Code 	
			if @ISORspCode='000'
			begin
				update tblcardproduction set NewEncPan=@NewEncPan,NewEncAcc=@NewEncAcc,
				MaskCardNo= LEFT(@CardNo,6)+replicate('*',len(CONVERT(varchar (20), @CardNo))-10)+RIGHT(CONVERT(varchar (20), @CardNo),4)
				where Code=@Code 	
			
					SELECT   Code, [CIF ID] [CIF ID], [Customer Name] [Customer Name], [Customer Preferred name] [Customer Preferred name], [Card Type and Subtype] [Card Type and Subtype]
						, [AC ID] [AC ID], [AC open date] [AC open date], [CIF Creation Date] [CIF Creation Date], [Address Line 1] [Address Line 1],  [Address Line 2] [Address Line 2], [Address Line 3] [Address Line 3]
						, City City, [State] [State],  [Pin Code] [Pin Code], [Country code] [Country code], [Mothers Maiden Name] [Mothers Maiden Name], DOB DOB, [Country Dial code] [Country Dial code]
						, [City Dial code], [Mobile phone number], [Email id], [Scheme code], [Branch code]
						, [Entered date] [Entered date], [Verified Date] [Verified Date], [PAN Number] [PAN Number], [Mode Of Operation] [Mode Of Operation], [Fourth Line Embossing] [Fourth Line Embossing]
						, [Debit Card Linkage Flag] [Debit Card Linkage Flag], [Uploaded On] [Uploaded On], Rejected Rejected, Reason Reason, Processed Processed, Downloaded Downloaded, [Login] [Login]
						, AccountLinkage AccountLinkage, ExistingCustomer ExistingCustomer, [Skip] [Skip], BatchNo BatchNo, AccountLinkageSMSSent AccountLinkageSMSSent, AccountLinkageSMSGUID AccountLinkageSMSGUID
						, Aadhaar Aadhaar, AddOnCards AddOnCards, Bank Bank, ProcessedOn ProcessedOn, [Bc Branch Code] [Bc Branch Code], [Center Name] [Center Name], [Orig Card Type and Subtype] [Orig Card Type and Subtype]
						, ResidentCustomer ResidentCustomer, IsAuthorised IsAuthorised, AuthorisedBy AuthorisedBy, SystemID SystemID, BankID BankID ,GETDATE() [Date]
						, CIF_FileName CIF_FileName, Account_Type Account_Type, Pin_Mailer Pin_Mailer,'' ProcessBatch,SwitchCardProgram , PGKValue PGKValue
						, NewEncPan,MaskCardNo,SwitchRspCode,SwitchRspDesc,FIleId,NewEncAcc
				into #FINALCARDPRODDATA
				from tblCardProduction CP WITH(NOLOCK) where Code=@Code
			
			
				INSERT INTO TblAuthorizedCardLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype]
				,[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3]
				,City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
				,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code]
				,[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing]
				,[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login]
				,AccountLinkage,ExistingCustomer,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID
				,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
				,ResidentCustomer,IsAuthorised,AuthorisedBy,SystemID,BankID,[Date],CIF_FileName,Account_Type,Pin_Mailer
				,ProcessBatch,SwitchCardProgram,PGKValue,NewEncPan,MaskCardNo,SwitchRspCode,SwitchRspDesc,FIleId,NewEncAcc)
				select * from #FINALCARDPRODDATA
				
				DECLARE @ISArchive BIT
				SELECT @ISArchive=ISINSTA FROM TBLBANKS with(nolock) WHERE BANKCODE=@ISSUERNO
				
			   /*THIS IS USED FOR PORTAL VIEW ie. archive data in TBLCUSTOMERSDETAILS,TBLCUSTOMERADDRESS and TBLCUSTOCCUPATIONDTL */
				/*START*/
				IF (@ISArchive=1)
				BEGIN
				SELECT 1 ISCARDSUCCESS,F.SYSTEMID SYSTEMID,F.BANKID BANKID,
					CASE WHEN ISNULL(F.[AC ID] ,'')='' THEN NULL ELSE DBO.UFN_ENCRYPTPAN(F.[AC ID])END ACCNO,
					F.[CIF ID] [CIF ID] ,GETDATE() [DATE],
					CASE WHEN ISNULL(T.ACCOUNTTYPEID,1)=0 THEN 1 ELSE T.ACCOUNTTYPEID END ACCTYPE,
					F.[ADDRESS LINE 1] STREETNAME,
					F.CITY, F.[MOBILE PHONE NUMBER], F.[EMAIL ID],
					F.[CUSTOMER PREFERRED NAME],F.DOB,F.[MOTHERS MAIDEN NAME]
					INTO #CUSTOMERSDETAILS
					FROM #FINALCARDPRODDATA F
					LEFT JOIN TBLCUSTOMERSDETAILS C WITH(NOLOCK) ON F.[CIF ID]=C.BANKCUSTID
					--LEFT JOIN TBLAUTHORIZEDCARDLOG AUTH WITH(NOLOCK) ON F.[CIF ID]=AUTH.[CIF ID]
					LEFT JOIN TBLACCOUNTTYPE T WITH(NOLOCK) ON F.[SCHEME CODE]=T.ACCOUNTTYPECODE
					WHERE C.BANKCUSTID IS NULL

					/*INSERT INTO RECORD TBLCUSTOMERSDETAILS FOR PORTAL VIEW */
					/*START*/
					INSERT INTO TBLCUSTOMERSDETAILS (ISCARDSUCCESS,SYSTEMID,BANKID,ACCNO,BANKCUSTID,MAKER_DATE_IND ,ACCTYPE,
					FIRSTNAME,MOBILENO,DOB_AD,MOTHERNAME,NAMEONCARD)
					SELECT ISCARDSUCCESS,SYSTEMID,BANKID,ACCNO,[CIF ID] ,[DATE], ACCTYPE,
					[CUSTOMER PREFERRED NAME],[MOBILE PHONE NUMBER],CAST(RIGHT(DOB, 4)+SUBSTRING(DOB, 3, 2)+LEFT(DOB, 2) AS DATETIME) AS DOB,
					[MOTHERS MAIDEN NAME],[CUSTOMER PREFERRED NAME]
					FROM #CUSTOMERSDETAILS

					INSERT INTO TBLCUSTOMERADDRESS(T.CUSTOMERID, STREETNAME_P,CITY_P,DISTRICT_P,PHONE1_P, EMAIL_P,ISSAMEASPERMADDR,STREETNAME_C,CITY_C,DISTRICT_C,PHONE1_C,EMAIL_C)
					SELECT CUSTOMERID,F.STREETNAME,F.CITY, F.CITY, F.[MOBILE PHONE NUMBER],F.[EMAIL ID],1,F.STREETNAME,F.CITY,F.CITY,F.[MOBILE PHONE NUMBER],F.[EMAIL ID]
					FROM #CUSTOMERSDETAILS F
					INNER JOIN TBLCUSTOMERSDETAILS T ON F.[CIF ID]=T.BANKCUSTID

					INSERT INTO TBLCUSTOCCUPATIONDTL (CUSTOMERID)
					SELECT CUSTOMERID
					FROM #CUSTOMERSDETAILS F INNER JOIN
					TBLCUSTOMERSDETAILS T ON F.[CIF ID]=T.BANKCUSTID
					
					DROP TABLE #CUSTOMERSDETAILS

				END
				/*END*/
				
				drop table #FINALCARDPRODDATA
				delete from tblCardProduction where Code=@Code				
			end
		end
	else if @ProcessId in(4,5)/*4 For Renewal and 5 For Upgrad*/
		begin
			update CardProductionRenewal set SwitchRspCode=@ISORspCode,SwitchRspDesc=@ISORspDesc where Code=@Code 
			if @ISORspCode='000'
			begin			
				update CardProductionRenewal set Processed=1,Rejected=0,ProcessedOn=GETDATE(),NewEncPan=@NewEncPan,NewEncAcc=@NewEncAcc,
				MaskCardNo=LEFT( @CardNo,6)+replicate('*',len(CONVERT(varchar (20), @CardNo))-10)+RIGHT(CONVERT(varchar (20), @CardNo),4) 
				where Code=@Code 
			end
		end
	else if @ProcessId=6 /*Reissue*/
		begin
			update TblCardGenRequest set SwitchRspCode=@ISORspCode,SwitchRspDesc=@ISORspDesc where ID=@Code 
			if @ISORspCode = '000'
			begin
				update TblCardGenRequest set  NewEncPan=@NewEncPan ,NewEncAcc=@NewEncAcc,
					   MaskCardNo=LEFT(@CardNo,6)+ replicate ('*',len(CONVERT(varchar (20), @CardNo))-10)+ RIGHT(CONVERT(varchar (20), @CardNo),4) 
				where ID=@Code 
				
			    INSERT INTO TblCardGenRequest_History (ID,CustomerID,OldCardRPANID,NewBinPrefix,HoldRSPCode
										,RSPCode,SwitchResponse,STAN,RRN,AuthID,Remark,FormStatusID
										,IsRejected,RejectReason,MakerID,CreatedDate,CheckerID,CheckedDate
										,IsAuthorized,UploadFileName,BankID,SystemID,ProcessID,schemecode
										,Account1,Account2,Account3,Account4,Account5,Reserved1,Reserved2
										,Reserved3,Reserved4,Reserved5,Branch_Code,ExpiryDate,New_Card,Customer_Name
										,New_Card_Activation_Date,AccountLinkage,Downloaded,Date,UploadID,OldPan
										,NewEncPan,MaskCardNo,SwitchRspCode,SwitchRspDesc,FIleId,NewEncAcc)
					 Select  ID, CustomerID, OldCardRPANID, NewBinPrefix, HoldRSPCode
										, RSPCode, SwitchResponse, STAN, RRN, AuthID, Remark, FormStatusID
										, IsRejected, RejectReason, MakerID, CreatedDate, CheckerID, CheckedDate
										, IsAuthorized, UploadFileName, BankID, SystemID, ProcessID, schemecode
										, Account1, Account2, Account3, Account4, Account5, Reserved1, Reserved2
										, Reserved3, Reserved4, Reserved5, Branch_Code, ExpiryDate, New_Card, Customer_Name
										, New_Card_Activation_Date,'' AccountID,1,GETDATE(),UploadID, OldPan
										,NewEncPan,MaskCardNo,SwitchRspCode,SwitchRspDesc,FIleId,NewEncAcc
					 From TblCardGenRequest A WITH(NOLOCK)
					 where ID=@Code
				Delete from TblCardGenRequest where ID=@Code
			end
		end
end


