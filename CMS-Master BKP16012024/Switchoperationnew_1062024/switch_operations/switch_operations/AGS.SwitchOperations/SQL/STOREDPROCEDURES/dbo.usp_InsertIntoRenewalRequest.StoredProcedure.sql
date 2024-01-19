USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertIntoRenewalRequest]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_InsertIntoRenewalRequest]
	@DataTable dbo.CardRenewal READONLY,
	@Filename VARCHAR (MAX),
	@Issuerno varchar(5)
AS
BEGIN
	
	/*INSERT INTO DATA INTO TABLE*/
	---START----
	INSERT INTO CardProductionReNEWAL (IssuerNumber,[CIF ID],[PAN],[Remarks],schemecode,[BIN],[Account 1],[Account 2],[Account 3],[Account 4],[Account 5],
	[Reserved 1],[Reserved 2],[Reserved 3],[Reserved 4],[Reserved 5],[RequestedOn],[Processed],[ProcessedOn],Rejected,uploadfilename,process_type)
	(
	
	
	SELECT @Issuerno, DT.[CIF ID],Dbo.ufn_EncryptPAN(DT.CARDNO),DT.Remarks,DT.SchemeCode,DT.BIN,DT.[Account 1],DT.[Account 2],DT.[Account 3],DT.[Account 4],DT.[Account 5],
	DT.[Reserved1],DT.[Reserved2],DT.[Reserved3],DT.[Reserved4],DT.[Reserved5],GETDATE(),0,NULL,0,@Filename,'Renewal'
	FROM @DataTable DT )

	--SELECT @Issuerno, DT.[CIF ID],Convert(varbinary(max),DT.CARDNO),DT.Remarks,DT.SchemeCode,DT.BIN,DT.[Account 1],DT.[Account 2],DT.[Account 3],DT.[Account 4],DT.[Account 5],
	--DT.[Reserved1],DT.[Reserved2],DT.[Reserved3],DT.[Reserved4],DT.[Reserved5],GETDATE(),0,NULL,0,@Filename,'Renewal'
	--FROM @DataTable DT )
	END
    ---END----
    /*DATA VALIDATION*/
    
    
    /*START*/
 --   UPDATE  CRP SET CRP.Rejected=1,CRP.RejectedReason='Invalid Data',CRP.RejectedDate=GETDATE() 
	--From    CardProductionReNewal CRP
	--inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber
	--WHERE Rejected=0 AND Processed=0 And convert(varchar(20),Pan) not like '%'+CardNoValidation+'%' and [BIN]  not like '%'+BINValidation+'%' and [CIF ID] not like '%'+CIFIDValidation+'%' 
	--AND  CRP.IssuerNumber=@Issuerno




    /*BUG FIX*/--commented by Himanshu On the 26022018
    UPDATE  CardProductionReNewal SET Rejected=1,RejectedReason='FileName Already Exists',RejectedDate=GETDATE()     
    WHERE Rejected=0 AND Processed=0 AND UploadFileName in (select UploadFileName from CardProductionReNewal with (Nolock) where Processed=1  group by UploadFileName)
        
    /*CIF,CardNo,AccountNo validation*/        
    UPDATE CPR SET Rejected=1,RejectedReason='Invalid CIF And CardNo Combination',RejectedDate=GETDATE() 
    FROM CardProductionReNewal CPR    
    WHERE Processed=0 and Rejected=0 and not exists (SELECT 1 FROM CardRPAN CP WITH (NOLOCK)  WHERE IssuerNo=@Issuerno AND CPR.[Cif Id]=CP.customer_id
    --AND LEFT((CONVERT(varchar (20),PAN)),6)+replicate('*',len(CONVERT(varchar (20),pan))-10)+RIGHT(CONVERT(varchar (20),pan),4)=MKSP 
	AND dbo.ufn_DecryptPAN(pan)=DBO.ufn_DecryptPAN(DecPAN))
    
    
   
   
   
    UPDATE  CRP SET Rejected=1,RejectedReason='Invalid Card No',RejectedDate=GETDATE() From CardProductionReNewal CRP          
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber
	WHERE  (dbo.ufn_DecryptPAN(pan)='' or PatIndex(CardNoValidation,dbo.ufn_DecryptPAN(pan))!=0 or LEN(dbo.ufn_DecryptPAN(pan))>CardLength)
	And Rejected=0 AND Processed=0 
   
    
	UPDATE  CRP SET Rejected=1,RejectedReason='Invalid CIF ID' ,RejectedDate=GETDATE()  From CardProductionReNewal CRP  
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber           
	WHERE Rejected=0 AND Processed=0 AND ([CIF ID]=''  or  PatIndex(CIFIDValidation,[CIF ID])!=0 or LEN([CIF ID])>CIFLength)
    
	UPDATE  CRP SET Rejected=1,RejectedReason='Invalid ACCOUNT 1',RejectedDate=GETDATE()  From CardProductionReNewal CRP  
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber            
	WHERE Rejected=0 AND Processed=0 AND ([Account 1]='' or PatIndex(AccountValidation,[Account 1])!=0 or LEN([Account 1])>AccountLength)
    
	UPDATE  CRP SET Rejected=1,RejectedReason='Invalid BIN+PRFIX',RejectedDate=GETDATE() From CardProductionReNewal CRP 
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber         
	WHERE Rejected=0 AND Processed=0 AND (BIN ='' or PatIndex(BINValidation,BIN)!=0 or LEN(BIN)>BinLength)
	
	/*code commented for Prabhu Bankny Himanshu schemecode is blank 
    UPDATE  CardProductionReNewal SET Rejected=1,RejectedReason='Invalid SCHEMECODE',RejectedDate=GETDATE()          WHERE Rejected=0 AND Processed=0 AND schemecode='' 
	UPDATE 	CardProductionReNewal SET Rejected=1,RejectedReason='Invalid SCHEMECODE AND CARDPREFIX COMBINATION',RejectedDate=GETDATE() WHERE Rejected=0 
	AND Processed=0 AND IsNull([Schemecode],'')+IsNull(BIN,'') Not In (Select IsNull(SchemeCode,'')+IsNull(CardPrefix,'') From AllowedSchemeCodeCardProduction) 
	And Bank=@Issuerno*/
    ----END------

 


    /* <Jira ID> :ATPBF-56  <Changed By>:Gufran Khan <Date>:2017-11-10 Purpose NEW CR **/
    /*START
    UPDATE T SET Rejected=1,RejectedReason='Card Is Already upgraded',RejectedDate=GETDATE() 
    FROM CardProductionReNewal T WITH (NOLOCK)
    WHERE Rejected=0 AND Processed=0 AND 
    EXISTS (SELECT 1 FROM CardProductionReNewal A WITH (NOLOCK) WHERE  
	CONVERT(varchar(20),T.Pan)=dbo.ufn_DecryptPAN(a.Pan) AND Rejected=0 AND Processed=1 AND A.Process_Type='REISSUE'  AND ProcessedOn<GETDATE())
    /*END*/*/
    
    
    /*PAN ENCRYPTED*/
    ----START------
     
     UPDATE CPR SET CPR.SWITCHPAN=cp.EncPAN ,cpr.Pan=cp.DecPAN
     FROM CardProductionReNewal CPR
     INNER JOIN CardRPAN CP ON IssuerNo= @Issuerno--AND LEFT(CONVERT(varchar (20),pan),6)+replicate('*',len(CONVERT(varchar (20),pan))-10)+RIGHT(CONVERT(varchar (20),pan),4)=MKSP 
	 AND    dbo.ufn_DecryptPAN(pan)=DBO.ufn_DecryptPAN(DecPAN)
     WHERE  Processed=0
     -----END-----
     /*To UPDATE THE BRANCH CODE,EXPIRY_DATE,customer_Name*/
     UPDATE CPR SET CPR.Branch_code=RIGHT('0000'+CAST( b.Branch_code AS VARCHAR(4)),4),cpr.expiry_date=b.expiry_date,
	        cpr.customer_Name=b.Customer_Name,CPR.CurrencyCode=B.CurrencyCode,CPR.AccountType=b.AccountType
     FROM CardProductionReNewal CPR
     INNER JOIN SyncCardDetails B With (NoLock) On CPR.SWITCHPAN Collate SQL_Latin1_General_CP1_CI_AS=B.pan_encrypted   
     WHERE Rejected=0 AND Processed=0 and B.IssuerNo=@Issuerno
     UPDATE  CardProductionReNewal SET Processed=1,ProcessedOn=GETDATE() WHERE  UploadFileName=@Filename 
     /*TO GET THE REJECTED RECORDS*/
     SELECT [CIF ID],ISNULL(dbo.ufn_DecryptPAN(PAN),CONVERT(varchar(20),pan))[card No],BIN,schemecode,[Account 1],RejectedReason  FROM CardProductionReNewal 
	 WITH (NOLOCK) WHERE Rejected=1 AND UploadFileName=@Filename and RejectedDate between DATEADD(DAY,-1,GETDATE()) And getdate()


GO
