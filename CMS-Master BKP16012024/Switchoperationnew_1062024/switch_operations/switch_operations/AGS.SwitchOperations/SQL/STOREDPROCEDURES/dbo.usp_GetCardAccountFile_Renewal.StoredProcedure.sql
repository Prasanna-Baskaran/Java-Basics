USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCardAccountFile_Renewal]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetCardAccountFile_Renewal]--[usp_GetCardAccountFile_Renewal] 'CIF_RENEW16042018109.txt','55'
	@FileName varchar (50),
	@IssuerNo varchar(5)
AS
BEGIN
     DECLARE @SwicthAccountDetails  varchar(1000)
	 DECLARE @Query  varchar(max)
	 Select @SwicthAccountDetails =AccountTable From SwitchMasterTable nolock where IssuerNo=@IssuerNo

	--Select @SwicthAccountDetails =SwicthAccountDetails From SFTP_Details nolock where IssuerNo=@IssuerNo

    DECLARE @ACCOUNT VARCHAR (MAX)
   	DECLARE @COUNT varchar(60) =1 
	Create Table #FINALACCOUNTS(ACCOUNT VARCHAR (MAX))
    --DECLARE @FINALACCOUNT TABLE ()


  SELECT * INTO #_Renewal FROM CardProductionRenewal WITH (NOLOCK) WHERE UploadFileName=@FileName AND Rejected=0 AND Processed=1 and   [Account 1]!=''
  SELECT  ISNULL([Account 1],'')+','+ ISNULL([Account 2],'')+','+ISNULL([Account 3],'')+','+ISNULL([Account 4],'')+','+ISNULL([ACCOUNT 5],'') [ACCOUNTS],
  ROW_NUMBER() OVER  (ORDER BY SWITCHPAN) [#ROW]
  INTO #ACCOUNTS  FROM #_Renewal
  
   WHILE (@COUNT<=(SELECT COUNT(*) FROM #ACCOUNTS ))
  BEGIN
  SELECT @ACCOUNT=ACCOUNTS FROM #ACCOUNTS WHERE #ROW=@COUNT
   INSERT INTO #FINALACCOUNTS (ACCOUNT)
  Select item From dbo.[udf_split](@ACCOUNT,',')
  SET @COUNT=@COUNT+1
 END
 Create table #FINALACCOUNT(ACCOUNT varchar(max),ACCOUNT_TYPE varchar(max))

 Set @Query =' Insert Into #FINALACCOUNT 
               SELECT DISTINCT  FA.ACCOUNT,RA.ACCOUNT_TYPE  FROM #FINALACCOUNTS FA
INNER JOIN CardRAccounts CA WITH (NOLOCK) ON IssuerNo='+@IssuerNo+' AND FA.ACCOUNT=DBO.ufn_DecryptPAN(CA.DecAcc)
INNER JOIN [AGSS1RT].postcard.'+@SwicthAccountDetails+' RA WITH (NOLOCK) ON RA.Issuer_nr='+@IssuerNo+' 
and CA.EncAcc COLLATE SQL_Latin1_General_CP1_CI_AS=RA.account_id_encrypted
WHERE DATE_DELETED IS NULL'
Exec(@Query)
--Select * from #FINALACCOUNT

SELECT  LTrim(RTrim(Convert(VarChar(Max),A.[CIF ID]))) +','+ISNULL(A.Branch_code,'')+','+ISNULL(C.CardProgram,'')+','+ISNULL(C.AccountType,'')+','
+[Account 1]+':'+C.AccountType+':1'+case when [Account 2]!='' then'|'+[Account 2]+':'+(SELECT Top 1 ACCOUNT_TYPE collate SQL_Latin1_General_CP1_CI_AS FROM #FINALACCOUNT 
WHERE ACCOUNT collate SQL_Latin1_General_CP1_CI_AS=A.[Account 2] )+':2'else''end+case when [Account 3]!='' 
then'|'+[Account 3]+':'+(SELECT Top 1 ISNULL(ACCOUNT_TYPE,'') collate SQL_Latin1_General_CP1_CI_AS FROM #FINALACCOUNT WHERE ACCOUNT collate SQL_Latin1_General_CP1_CI_AS=A.[Account 3] )+':3'else''end
From #_Renewal A With (NoLock) 
Left Join Tblbin C With (NoLock) On  A.BIN collate SQL_Latin1_General_CP1_CI_AS =C.CardPrefix
Where Rejected=0 And Processed=1  And UploadFileName=@FileName

 
Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),A.[Account 1])))+','+B.AccountType+','+B.Currency+',,,,' From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where Rejected=0 And Processed=1  And UploadFileName=@FileName     			
UNION ALL
Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),A.[Account 2])))+','+B.AccountType+','+B.Currency+',,,,' From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where Rejected=0 And Processed=1 and [Account 2]!='' And 
UploadFileName=@FileName     			
UNION ALL
Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),A.[Account 3])))+','+B.AccountType+','+B.Currency+',,,,' From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where Rejected=0 And Processed=1 and [Account 3]!='' And 
UploadFileName=@FileName     			
UNION ALL
Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),A.[Account 4])))+','+B.AccountType+','+B.Currency+',,,,' From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where Rejected=0 And Processed=1  and [Account 4]!=''  And UploadFileName=@FileName     			
UNION ALL
Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),A.[Account 5])))+','+B.AccountType+','+B.Currency+',,,,' From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where Rejected=0 And Processed=1  and [Account 5]!='' And
 UploadFileName=@FileName     			
/*CustomerAccount FILE*/
Select distinct  'U,'+ LTrim(RTrim(Convert(VarChar(Max),[CIF ID])))+','+ LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[Account 1])))) +','+B.AccountType From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where
 Rejected=0 And Processed=1  And UploadFileName=@FileName     			
UNION ALL
Select distinct  'U,'+ LTrim(RTrim(Convert(VarChar(Max),[CIF ID])))+','+ LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[Account 2])))) +','+B.AccountType From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where
 Rejected=0 And Processed=1  AND [Account 2]!='' And UploadFileName=@FileName     			
UNION ALL
Select distinct  'U,'+ LTrim(RTrim(Convert(VarChar(Max),[CIF ID])))+','+ LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[Account 3])))) +','+B.AccountType From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where
 Rejected=0 And Processed=1  AND [Account 3]!='' And UploadFileName=@FileName   			
UNION ALL
Select distinct  'U,'+ LTrim(RTrim(Convert(VarChar(Max),[CIF ID])))+','+ LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[Account 4])))) +','+B.AccountType From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where
 Rejected=0 And Processed=1  AND [Account 4]!='' And UploadFileName=@FileName    			
UNION ALL
Select distinct  'U,'+ LTrim(RTrim(Convert(VarChar(Max),[CIF ID])))+','+ LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[Account 5])))) +','+B.AccountType From #_Renewal A With (NoLock) Left Join Tblbin B With (NoLock) On  A.BIN=B.CardPrefix Where
 Rejected=0 And Processed=1  AND [Account 5]!=''  And UploadFileName=@FileName  			


/*BIN SUMMARY*/

Select BIN, Convert(VarChar(10),Count(*)) [Counter]
From #_Renewal A With (NoLock)
Where Rejected=0 And Processed=1  And UploadFileName=@FileName group by BIN
DROP TABLE #ACCOUNTS
DROP TABLE #_Renewal
DROP TABLE #FINALACCOUNT


END
GO
