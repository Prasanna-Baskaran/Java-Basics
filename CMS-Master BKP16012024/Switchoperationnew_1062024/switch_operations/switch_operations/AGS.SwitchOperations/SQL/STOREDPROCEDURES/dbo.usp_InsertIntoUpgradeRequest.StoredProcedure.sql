USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertIntoUpgradeRequest]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_InsertIntoUpgradeRequest]
	@DataTable dbo.[CardReissue] READONLY,
	@Filename VARCHAR (MAX),
	@IssuerNo varchar(Max)
AS
BEGIN
	
	/*INSERT INTO DATA INTO TABLE*/
	---START----
	
	
	INSERT INTO CardProductionReNEWAL (IssuerNumber,[CIF ID],[PAN],[BIN],[Account 1],[RequestedOn],[Processed],[ProcessedOn],Rejected,uploadfilename,Process_type)
	(
	SELECT @IssuerNo,[CustomerID],Convert(varbinary(500),DT.[CARDNO] ),DT.[BIN],DT.[Account],GETDATE(),0,NULL,0,@Filename,'Upgrade'
	FROM @DataTable DT )
	END
    ---END----


 
   /*BUG Raised By chand*/
    UPDATE  CardProductionReNewal SET Rejected=1,RejectedReason='FileName Already Exists',RejectedDate=GETDATE()     
    WHERE Rejected=0 AND Processed=0 AND UploadFileName in (select UploadFileName from CardProductionReNewal with (Nolock) where Processed=1 group by UploadFileName)

	/*DATA VALIDATION*/
	UPDATE CPR SET Rejected=1,RejectedReason='Invalid CIF And CardNo Combination',RejectedDate=GETDATE() 
    FROM CardProductionReNewal CPR    
    WHERE Processed=0 and Rejected=0 AND NOT EXISTS (SELECT 1 FROM CardRPAN CP WITH (NOLOCK)  WHERE IssuerNo=@IssuerNo AND CPR.[Cif Id]=CP.customer_id
	 AND PAN=DBO.ufn_DecryptPAN(DecPAN))
    
	
    UPDATE CardProductionReNewal SET Rejected=1,RejectedReason='No Card Found' 
    WHERE Rejected=0 AND Processed=0 AND NOT EXISTS (SELECT 1 FROM CARDRPAN C WITH (NOLOCK) WHERE
    IssuerNo=@IssuerNo AND  CONVERT(varchar (20),pan)=DBO.ufn_DecryptPAN(DecPAN)  )
    
	 UPDATE  CRP SET Rejected=1,RejectedReason='Invalid Card No',RejectedDate=GETDATE() From CardProductionReNewal CRP          
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber
	WHERE  (CONVERT(varchar (20),pan)='' or PatIndex(CardNoValidation,CONVERT(varchar (20),pan))!=0 or LEN(CONVERT(varchar (20),pan))>CardLength)
	And Rejected=0 AND Processed=0 
   
    
	UPDATE  CRP SET Rejected=1,RejectedReason='Invalid CIF ID' ,RejectedDate=GETDATE()  From CardProductionReNewal CRP  
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber           
	WHERE Rejected=0 AND Processed=0 AND ([CIF ID]=''  or  PatIndex(CIFIDValidation,[CIF ID])!=0 or LEN([CIF ID])>CIFLength)
   
   	UPDATE  CRP SET Rejected=1,RejectedReason='Invalid BIN+PRFIX',RejectedDate=GETDATE() From CardProductionReNewal CRP 
	inner join TblReissueRenewalFileValidation TRRV on  TRRV.IssuerNo=CRP.IssuerNumber         
	WHERE Rejected=0 AND Processed=0 AND (BIN ='' or PatIndex(BINValidation,BIN)!=0 or LEN(BIN)>BinLength)

	UPDATE CardProductionReNewal SET Rejected =1, RejectedReason='New Prefix Bin Can Not Be Same As Old Prefix Bin',RejectedDate=GETDATE() 
	where  LEFT(PAN,Len(Bin)) = BIN and processed=0  
	
	UPDATE CPR SET Rejected=1,RejectedReason='CARD SEND FOR PROCESSING IS ALREADY BLOCK',RejectedDate=GETDATE() 
	FROM CardProductionReNewal CPR
	INNER JOIN CardRPAN CP WITH (NOLOCK) ON IssuerNo=@IssuerNo AND CPR.[Cif Id]=CP.customer_id  AND PAN=DBO.ufn_DecryptPAN(DecPAN)
	INNER JOIN SyncCardDetails B WITH (NOLOCK) ON CP.EncPAN=B.pan_encrypted
	WHERE B.hold_rsp_code IN ('41,43')     
	
	UPDATE CPR SET Rejected=1,RejectedReason='CIF NOT FOUND IN SYSTEM',RejectedDate=GETDATE() FROM CardProductionReNewal CPR    
    WHERE Processed=0 and Rejected=0 AND NOT EXISTS (SELECT 1 FROM CardRPAN CP WITH (NOLOCK)  WHERE IssuerNo=@IssuerNo AND CPR.[Cif Id]=CP.customer_id)
    
	
        
    ----END------
    
    
    /* <Jira ID> :ATPBF-56  <Changed By>:Gufran Khan <Date>:2017-11-10 Purpose NEW CR **/
    /*START*/
    UPDATE T SET Rejected=1,RejectedReason='Card Is Already Renew',RejectedDate=GETDATE() 
    FROM CardProductionReNewal T WITH (NOLOCK)
    WHERE Rejected=0 AND Processed=0 AND 
    EXISTS (SELECT 1 FROM CardProductionReNewal A WITH (NOLOCK) 
    WHERE  CONVERT(varchar(20),T.Pan)=dbo.ufn_DecryptPAN(pan) AND Rejected=0 AND Processed=1 AND A.Process_Type='Renewal' 
    AND T.Bin=A.Bin
    AND ProcessedOn<GETDATE())
    /*END*/
    
    /*PAN ENCRYPTED*/
    ----START------
     
     UPDATE CPR SET CPR.SWITCHPAN=cp.EncPAN,cpr.Pan=cp.DecPAN
     FROM CardProductionReNewal CPR
     INNER JOIN CardRPAN CP ON IssuerNo=@IssuerNo  AND
     CONVERT(varchar (20),pan)=DBO.ufn_DecryptPAN(DecPAN)
     WHERE  Processed=0
     /*end*/ 
     
     /*START*/
   
    /*To UPDATE THE BRANCH CODE*/
     UPDATE CPR SET CPR.Branch_code=RIGHT('0000'+CAST( b.Branch_code AS VARCHAR(4)),4),cpr.expiry_date=b.expiry_date,cpr.customer_Name=b.Customer_Name
     FROM CardProductionReNewal CPR
     INNER JOIN SyncCardDetails B With (NoLock) On CPR.SWITCHPAN Collate SQL_Latin1_General_CP1_CI_AS=B.pan_encrypted   
     WHERE Rejected=0 AND Processed=0 
    
    UPDATE  CardProductionReNewal SET Processed=1,ProcessedOn=GETDATE() WHERE  UploadFileName=@Filename
    /*Rejected Record*/
    SELECT [CIF ID],ISNULL(dbo.ufn_DecryptPAN(pan),CONVERT(varchar(20),pan))[card No],BIN,schemecode,[Account 1],RejectedReason  FROM CardProductionReNewal WITH (NOLOCK) WHERE Rejected=1 AND UploadFileName=@Filename and RejectedDate between DATEADD(DAY,-1,GETDATE()) And getdate()
GO
