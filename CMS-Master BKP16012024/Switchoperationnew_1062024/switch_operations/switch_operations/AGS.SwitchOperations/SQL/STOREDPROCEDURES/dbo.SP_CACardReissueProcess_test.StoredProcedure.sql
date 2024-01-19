USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_CACardReissueProcess_test]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------
CREATE PROCEDURE [dbo].[SP_CACardReissueProcess_test] 
		@Redownload Bit=0,
		@BatchNo	VarChar(20)='',
		@IssuerNo VARCHAR(20)=''
AS
/************************************************************************
Object Name: 
Purpose: Get Card Reissue file For Card Generation  
Change History
Date         Changed By				Reason
03/10/2017 Diksha Walunj			Newly Developed
DECLARE @IssuerNo VARCHAR(20)='18',@Redownload Bit=0,
		@BatchNo	VarChar(20)=''
Exec [SP_CACardReissueProcess] @Redownload,@BatchNo,@IssuerNo
*************************************************************************/
BEGIN
	declare @code int = 0, @description as varchar(500) = 'SUCCESS'
begin try
	
	Select A.* INTO #CardReissuetemp 
	From TblCardGenRequest_history A With (NoLock)
	INNER JOIN TblBanks B WITH(NOLOCK)  ON A.BankID=B.ID
	WHERE B.BankCode=@IssuerNo AND A.IsAuthorized=1 --AND ISNULL(A.IsRejected,0)=0
	and a.createddate >'2018-05-29'
	
			If Exists(Select top 1 1 from #CardReissuetemp WITH(NOLOCK) )
			BEGIN
				SELECT @code AS [Code], @description [Description], 'Cards_Reissue' As [FileNames]

				Declare @IntpriOutput int ,@StrpriOutputDesc Varchar(500) 
				DECLARE  @TblCardAccount AS Table(
				customer_id VARCHAR(800)
				,pan_encrypted VARCHAR(800)
				,account_id_encrypted VARCHAR(800)
				,account_type VARCHAR(800)
				,account_type_qualifier VARCHAR(800)
				,issuer_nr VARCHAR(200)
				,DefaultAccType VARCHAR(20)
				)

	

		 DECLARE @PANIssuer AS PANIssuerType
		
		INSERT INTO @PANIssuer (EncPAN,IssuerNo) 
		 Select P.EncPAN,P.IssuerNo from  #CardReissuetemp Ct WITH(NOLOCK)
						 INNER JOIN CardRPAN P With(NOLOCK) ON Ct.OldCardRPANID=P.ID  
						 Where P.IssuerNo=@IssuerNo
 
    -- Select * from @PANIssuer
		INSERT INTO @TblCardAccount (customer_id,	pan_encrypted,account_id_encrypted,account_type,account_type_qualifier,issuer_nr,DefaultAccType)
		exec [SP_CAGetSwitchCardAccountLinkage_AGS] @IssuerNo,@PANIssuer,@IntpriOutput output ,@StrpriOutputDesc output
		------**************************** Account Linkage ********************
		Select Distinct 
			 [CustomerID]=C.CustomerID ,
			 [DefaultAccType]=CA.DefaultAccType,
			 [AccountID]=
			SubString((Select  '|' + LTrim(RTrim(Convert(VarChar(Max), dbo.ufn_DecryptPAN(B.DecAcc)))) +':'+ISNULL(B.AccountType,'')+':' + 
							--Case When AccountLinkage=0 Then '1' Else '2' End As [text()] 
							B.AccountQualifier  As [text()]
			  from (SELECT  CA.account_type  AS[AccountType] ,CA.customer_id AS [CustID] ,A.DecAcc AS[DecAcc],account_type_qualifier AS[AccountQualifier] 
				FROM @TblCardAccount CA
				INNER JOIN CardRAccounts  A WITH(NOLOCK) ON CA.account_id_encrypted=A.EncAcc AND CA.issuer_nr=A.IssuerNo where  C.CustomerID=CA.customer_id) B
				Order By B.AccountQualifier For XML Path(''), Elements), 2, 99999) 
				INTO #CAAccountLinkage
			From #CardReissuetemp C WITH(NOLOCK) 
			INNER JOIN @TblCardAccount CA ON C.CustomerID=CA.customer_id

			------************************* Card File ****************************
		if @IssuerNo='27'
			begin			
				Select Distinct  
							 LTrim(RTrim(Convert(VarChar(Max),A.CustomerID))) +','+ISNULL(a.Branch_Code,'')+','+ ISNULL(c.CardProgram,'')+','+isnull(B.DefaultAccType,'')+','+ ISNULL( B.AccountID,'') +','   AS [Result]
							 From #CardReissuetemp A With (NoLock) 
							 Left Join #CAAccountLinkage B With (NoLock) On A.CustomerID=B.CustomerID 
							 Left Join tblBin C With (NoLock) On A.NewBinPrefix=C.CardPrefix AND A.BankID=C.BankID
			end
		else
			begin
				Select Distinct 
						 LTrim(RTrim(Convert(VarChar(Max),A.CustomerID))) +','+ISNULL(a.Branch_Code,'')+','+ ISNULL(c.CardProgram,'')+','+isnull(B.DefaultAccType,'')+','+B.AccountID AS [Result]
						 From #CardReissuetemp A With (NoLock) 
						 Left Join #CAAccountLinkage B With (NoLock) On A.CustomerID=B.CustomerID 
						 Left Join tblBin C With (NoLock) On A.NewBinPrefix=C.CardPrefix AND A.BankID=C.BankID
			end

					 ------ INSERT into Authorized card requests table
					 --INSERT INTO TblCardGenRequest_History (ID,CustomerID,OldCardRPANID,NewBinPrefix,HoldRSPCode
						--				,RSPCode,SwitchResponse,STAN,RRN,AuthID,Remark,FormStatusID
						--				,IsRejected,RejectReason,MakerID,CreatedDate,CheckerID,CheckedDate
						--				,IsAuthorized,UploadFileName,BankID,SystemID,ProcessID,schemecode
						--				,Account1,Account2,Account3,Account4,Account5,Reserved1,Reserved2
						--				,Reserved3,Reserved4,Reserved5,Branch_Code,ExpiryDate,New_Card,Customer_Name
						--				,New_Card_Activation_Date,AccountLinkage,Downloaded,Date,UploadID)
					 --Select A.ID,A.CustomerID,A.OldCardRPANID,A.NewBinPrefix,A.HoldRSPCode
						--				,A.RSPCode,A.SwitchResponse,A.STAN,A.RRN,A.AuthID,A.Remark,A.FormStatusID
						--				,A.IsRejected,A.RejectReason,A.MakerID,A.CreatedDate,A.CheckerID,A.CheckedDate
						--				,A.IsAuthorized,A.UploadFileName,A.BankID,A.SystemID,A.ProcessID,A.schemecode
						--				,A.Account1,A.Account2,A.Account3,A.Account4,A.Account5,A.Reserved1,A.Reserved2
						--				,A.Reserved3,A.Reserved4,A.Reserved5,A.Branch_Code,A.ExpiryDate,A.New_Card,A.Customer_Name
						--				,A.New_Card_Activation_Date,B.AccountID,1,GETDATE(),UploadID
					 --From #CardReissuetemp A  WITH(NOLOCK)
					 --Left Join #CAAccountLinkage B WITH(NOLOCK)  On A.CustomerID=B.CustomerID 

					 --DELETE CR
					 --FROM  TblCardGenRequest CR WITH(NOLOCK)
					 --INNER join  #CardReissuetemp T WITH(NOLOCK)  ON CR.CustomerID=T.CustomerID

			 drop table #CAAccountLinkage
			END
			ELSE
			BEGIN
				SELECT 1 AS [Code], 'No record' [Description], '' [FileNames]
				--DROP TABLE #CardReissuetemp 
			END
		
			DROP TABLE #CardReissuetemp 
--END
end try
begin catch
	rollback tran
	SELECT 1 AS [Code], ERROR_MESSAGE() [Description], '' [FileNames]
	
	INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@IssuerNo='+@IssuerNo,@IssuerNo
end catch

END

GO
