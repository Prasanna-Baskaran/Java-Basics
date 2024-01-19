USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCardFile_ReIssue]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetCardFile_ReIssue] --[usp_GetCardFile_ReIssue] 'PrabhuCardReissue.txt','23'
	@FileName varchar (50),
	@IssuerNo varchar(5)
AS
BEGIN
   Declare @Query Varchar(max)
   Declare @CardTable varchar(1000)
   Declare @CardAccountsTable varchar(1000)
   Declare @AccountTable varchar(1000)

   Select @CardTable=CardTable,@CardAccountsTable=CardAccountsTable,@AccountTable=AccountTable from SwitchMasterTable With (nolock)  where IssuerNo=@IssuerNo
         
			Select 
			ISNULL(a.[CIF ID],'')+
			','+Case When Len(A.Branch_code)=3 Then '0' Else '' End+ISNULL(A.Branch_code,'') +
			','+C.CardProgram+','
            [REC1], dbo.ufn_DecryptPAN(a.Pan) [Card No]			
			into #CardMaster
			From CardProductionReNewal A With (NoLock) 
			Left Join TblBin C With (NoLock) On A.BIN=C.CardPrefix
			Where UploadFileName=@FileName and Processed=1 and Rejected=0
			
		   Create Table #pc_account(account_id varchar(100),ACCOUNT_TYPE varchar(20),account_id_encrypted varchar(100))
		   Create Table #pc_card_account(pan varchar(100),account_id varchar(100),account_type_nominated varchar(20),account_type_qualifier int)
		   Create Table #card_account([Card No] varchar(100),[Account No] varchar(100),[account_type] varchar(20),account_type_qualifier int)

		   Set @Query ='Insert into #pc_account
		   Select account_id,ACCOUNT_TYPE,account_id_encrypted  from [AGSS1RT].postcard.'+@AccountTable+' with (nolock)
		   where  date_deleted is Null 
		   
		   Insert into #pc_card_account
		   select pan,account_id ,account_type_nominated,account_type_qualifier from [AGSS1RT].postcard.'+ @CardAccountsTable +' with (nolock) 
		   where date_deleted Is Null
       

		     Insert into #card_account
			 select distinct dbo.ufn_DecryptPAN(A.Pan)[Card No],dbo.ufn_DecryptPAN(e.DecAcc)[Account No],D.account_type[account_type],c.account_type_qualifier
             from CardProductionRenewal a with (nolock)
             inner Join [AGSS1RT].postcard.'+@CardTable+' B With (NoLock) On A.SwitchPan Collate SQL_Latin1_General_CP1_CI_AS=B.pan_encrypted Collate SQL_Latin1_General_CP1_CI_AS   
             inner Join #pc_card_account C With (NoLock) On B.pan=C.pan and b.default_account_type=c.account_type_nominated 
             inner Join #pc_account D With (NoLock) On C.account_id=D.account_id 
            inner Join CardRAccounts E With (NoLock) On IssuerNo='+@Issuerno+' and  D.account_id_encrypted Collate SQL_Latin1_General_CP1_CI_AS=E.EncAcc Collate SQL_Latin1_General_CP1_CI_AS 
            where UploadFileName='''+@FileName+'''
			and  Processed=1 and Rejected=0'

			    Print @Query
		        Exec(@Query)
		   	
			select [Card No],[Account No],[account_type],ROW_NUMBER() OVER(partition by [Card No] ORDER BY account_type_qualifier ,[Account No]) AS [ROWCOUNT]
			Into #card_account_dump from #card_account
			/*CR 1 CASE 1  */
			--where not exists (select  1 from CardProductionRenewal a with (nolock) where  UploadFileName=@FileName and  Processed=1 and Rejected=0 and  a.[Account 1]=[Account No])
			
            Select *
             --A.REC1 collate SQL_Latin1_General_CP1_CI_AS + ISNULL('|'+Stuff((Select '|' + IsNull([Account No],'')collate SQL_Latin1_General_CP1_CI_AS+':'+account_type collate SQL_Latin1_General_CP1_CI_AS+':'+CONVERT(varchar (10),[ROWCOUNT]) From #_card_account_dump B Where A.[Card No]=B.[Card No]  For XML Path('')),1,1,''),'') 
            From #CardMaster A 	
            left join #card_account b on a.[Card No]=b.[Card No]
            DROP TABLE #CardMaster
            DROP TABLE #card_account_dump
			Drop table #card_account
            
END


GO
