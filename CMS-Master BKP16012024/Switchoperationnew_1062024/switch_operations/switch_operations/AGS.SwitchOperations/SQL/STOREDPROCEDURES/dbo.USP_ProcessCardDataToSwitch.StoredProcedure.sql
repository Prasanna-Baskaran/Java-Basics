
ALTER Procedure [dbo].[USP_ProcessCardDataToSwitch]  --[USP_ProcessCardDataToSwitch] '54','40026',5
	@IssuerNo numeric=null,
	@FileId varchar(50)=null,
	@ProcessId bigint=null
as
/************************************************************************
Jira Id: ATPCM-620 
Purpose: Processing card details to switch.
Created History
Date        Created By				Reason
31/08/2018  Pratik Mhatre			Newly Developed
*************************************************************************/
begin	
	if @ProcessId=3 /*Issueance*/
	begin
		
		
		/*GET THE SUCESSFULL CARD INTO TEMP TABLE */
		/*START*/
		SELECT B.CODE,B.[CIF ID],B.[CUSTOMER NAME],B.[CUSTOMER PREFERRED NAME],B.[CARD TYPE AND SUBTYPE],B.[AC ID],B.[AC OPEN DATE],B.[CIF CREATION DATE],B.[ADDRESS LINE 1],B.[ADDRESS LINE 2],B.[ADDRESS LINE 3],B.[CITY],B.[STATE],B.[PIN CODE]
		,B.[COUNTRY CODE],B.[MOTHERS MAIDEN NAME],B.[DOB],B.[COUNTRY DIAL CODE],B.[CITY DIAL CODE],B.[MOBILE PHONE NUMBER],B.[EMAIL ID],B.[SCHEME CODE],B.[BRANCH CODE],
		B.[ENTERED DATE],B.[VERIFIED DATE],B.[PAN NUMBER],B.[MODE OF OPERATION],B.[FOURTH LINE EMBOSSING],B.[DEBIT CARD LINKAGE FLAG],B.[UPLOADED ON],B.[REJECTED],B.[REASON]
		,B.[PROCESSED],B.[DOWNLOADED],B.[LOGIN],B.[ACCOUNTLINKAGE],B.[EXISTINGCUSTOMER],B.[SKIP],B.[BATCHNO],B.[ACCOUNTLINKAGESMSSENT],B.[ACCOUNTLINKAGESMSGUID]
		,B.[AADHAAR],B.[ADDONCARDS],B.[BANK],B.[PROCESSEDON],B.[BC BRANCH CODE],B.[CENTER NAME],B.[ORIG CARD TYPE AND SUBTYPE],B.[RESIDENTCUSTOMER],B.[ISAUTHORISED],B.[AUTHORISEDBY]
		,B.[SYSTEMID],B.BANKID,B.CIF_FILENAME CIF_FILENAME, B.ACCOUNT_TYPE,PIN_MAILER,B.PGKVALUE
		INTO #TBLCARDPRODUCTION
		FROM TBLCARDPRODUCTION B WITH(NOLOCK)
		INNER JOIN TBLBANKS BA WITH(NOLOCK) ON B.BANK=BA.ID
		WHERE REJECTED=0 AND PROCESSED=1 AND ISAUTHORISED =1 AND 
		BA.BANKCODE=@ISSUERNO AND B.fileid=@FileId
		
		/*DATA USED TO CREATE THE ISO MSG*/
		SELECT Code , [CIF ID] as CifId,[CUSTOMER NAME] as CustomerName,[CUSTOMER PREFERRED NAME] as NameOnCard,[CARD TYPE AND SUBTYPE] as BinPrefix
		,[AC ID] AccountNo,[ADDRESS LINE 1] as address1,[ADDRESS LINE 2] as address2,[ADDRESS LINE 3] as address1
		,CITY,[STATE],[PIN CODE] as Pincode,[COUNTRY CODE] as Country,[MOTHERS MAIDEN NAME] as MothersMaidenName,DOB as DateOfBirth
		,[COUNTRY DIAL CODE] as CountryCode,[MOBILE PHONE NUMBER] as MobileNo,[EMAIL ID] as EmailID,[SCHEME CODE] as SchemeCode ,[BRANCH CODE] as BranchCode
		,[PAN NUMBER] asPanNumber,[MODE OF OPERATION],[FOURTH LINE EMBOSSING] as FourthLineEmbossing
		,[DEBIT CARD LINKAGE FLAG] as IssueDebitCard,BATCHNO 
		,AADHAAR,CIF_FILENAME CIF_FILENAME,ACCOUNT_TYPE as AccountType,PGKVALUE PGKVALUE,accountlinkage as LinkingFlag
		,CODE as CIFDBID, @ISSUERNO as IssuerNo
		FROM #TBLCARDPRODUCTION
		/*DATA USED TO CREATE THE ISO MSG*/
	end
	else if @ProcessId=4 /*Renewal*/
		begin
			select c.Code Code, dbo.ufn_DecryptPAN(C.pan) CardNo,c.Bin as BinPrefix ,[Account 1] as AccountNo,[Account 2],[Account 3],[Account 4],[Account 5],
				  schemecode [AccountType]
			from CardProductionRenewal c with(nolock) 
			where IssuerNumber=@IssuerNo and Rejected=0 and Processed=1 and 
				  Process_Type='Renewal' and FileID=@FileId 
		end
	else if @ProcessId=5 /*Upgrade*/
		begin
			select c.Code Code, dbo.ufn_DecryptPAN(C.pan) CardNo,c.Bin as BinPrefix ,CASE WHEN ISNULL([Account 1],'')='' THEN '' ELSE [Account 1]+'|'+ ISNULL(schemecode,'')+'|1' END as AccountNo,[Account 2],[Account 3],[Account 4],[Account 5],
				  schemecode [AccountType]
			from CardProductionRenewal c with(nolock) 
			where IssuerNumber=@IssuerNo and Rejected=0 and Processed=1 and 
				  Process_Type='Upgrade' and FileID=@FileId 
		end
	else if @ProcessId=6 /*Reissue*/
		begin
			Select A.ID Code, dbo.ufn_DecryptPAN(A.OldDecPan) CardNo,A.HoldRSPCode as Status ,a.NewBinPrefix as BinPrefix,BankCode
			From TblCardGenRequest A With (NoLock)
				 INNER JOIN TblBanks B WITH(NOLOCK) ON A.BankID=B.ID
			--inner join cardrpan c with(nolock) on c.IssuerNo=@IssuerNo and a.OldPan=c.EncPAN
			WHERE B.BankCode=@IssuerNo AND A.IsAuthorized=1 AND a.FIleId=@FileId
		end	
end