USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetSwitchAccountDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_GetSwitchAccountDetails]--[Sp_GetSwitchAccountDetails] '6376762000060000649','','1','4','','',0
	@CardNO VARCHAR(20),                      --[Sp_GetSwitchAccountDetails] '6376762000060000599','','1','4','','',0
	@CustomerID VARCHAR(200)='',	          --[Sp_GetSwitchAccountDetails] '6376762000060000144','','1','4','','',0
	@SystemID varchar(200),                 --[Sp_GetSwitchAccountDetails] '6376761000120000127','','2','1','','',0
	@BankID varchar(200),
	@Name VARCHAR(800)='',
	@AccountNo Varchar(800)='',
	@IntPara int =0

AS
BEGIN
    SET NOCOUNT ON
	Declare @IntpriOutput int,@StrpriOutputDesc Varchar(500) 
	DECLARE	@IssuerNo int, @EncPAN VARCHAR(1000)='' ,@EncAcccount VARCHAR(800)='' 
 Begin Transaction  
	Begin Try 
	
	--select * from cardrpan
	--BEGIN
--	CREATE CLUSTERED INDEX [ix_CardNo] ON [dbo].CardRPAN 
--(
--            DecPAN ASC
--) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

	 --IF EXISTS(SELECT top 1 1 FROM CardRPAN WITH(NOLOCK) WHERE  dbo.ufn_DecryptPAN(DecPAN)=@CardNo)
	 BEGIN  
	 --select  dbo.ufn_DecryptPAN(DecPAN)as CardNo  into #temp from cardrpan where dbo.ufn_DecryptPAN(DecPAN)=@CardNo
	 SELECT @IssuerNo=BankCode FROM TblBanks WITH(NOLOCK) WHERE ID=@BankID
	 
		---- Get encrypted PAN
		SELECT @EncPAN=EncPAN,@IssuerNo=B.bankCode FROM CardRPAN P WITH(NOLOCK)
		INNER JOIN Tblbanks B WITH(NOLOCK) ON P.IssuerNo=B.BankCode
		 WHERE( dbo.ufn_DecryptPAN(DecPAN)=@CardNo) AND B.ID=@BankID 
	
	    DECLARE @tblCustDetail TABLE
		(  CustomerName	VARCHAR(800)	,
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
			[AddressDetail]	VARCHAR(800),
			[Linkingflag] varchar (100)		
		)
		INSERT INTO @tblCustDetail(CustomerName	,mobile_nr	,email	,pan_encrypted	,account_id_encrypted	,account_type	,
					issuer_nr	,customer_id	,currency_code	,account_type_qualifier	,date_issued	,card_program	,card_status	,
					HRC	,[ExpiryDate],[AddressDetail],[Linkingflag]	)	
						exec  [AGSS1RT].postcard.dbo.[Usp_GetCardDetails_AGS_accountlinkanddelink] @IssuerNo,@EncPan,@EncAcccount,@IntpriOutput output ,@StrpriOutputDesc output
						
						--exec  [AGSS1RT].postcard.dbo.[Usp_GetCardDetails_AGS_accountlinkanddelink] @IssuerNo,@EncPan,@EncAcccount,@IntpriOutput output ,@StrpriOutputDesc output
	--select * from @tblCustDetail
	 
	 --select  case when Linkingflag='LINK' then '01' else '02' end [Flag],dbo.ufn_decryptPAN(P.DecPAN) AS [CardNo],dbo.ufn_decryptPAN(A.DecAcc) AS [AccountNo], account_type as [AccountType],account_type_qualifier as [AccountQualifier]
	 select  [Linkingflag],dbo.ufn_decryptPAN(P.DecPAN) AS [CardNo],dbo.ufn_decryptPAN(A.DecAcc) AS [AccountNo], account_type as [AccountType],account_type_qualifier as [AccountQualifier]
	
	FROM @tblCustDetail	t
	left JOIN CardRPAN P WITH(NOLOCK)ON t.pan_encrypted=P.EncPAN
		   left JOIN CardRAccounts A WITH(NOLOCK) ON t.account_id_encrypted=A.EncAcc
			INNER JOIN TblCustomersdetails c WITH(NOLOCK) ON t.customer_id=c.bankcustID
			where c.BankID=@BankID ANd c.SystemID=@SystemID 
	--select'10' as [AccountType],'1' as [AccountQualifier],'LINK'as [Linkingflag],'6376762000060000649' as [CardNo],'524000000034' as [AccountNo]
	END
	--drop table #temp
		--end
		

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
