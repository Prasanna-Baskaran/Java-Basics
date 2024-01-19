USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_getCardProdAccountsDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[SP_getCardProdAccountsDetails]-- [SP_getCardProdAccountsDetails] 1,'BATCH20170207131837'
	@Redownload Bit,
	@BatchNo	VarChar(20)
As
Begin
declare @code int = 0, @description as varchar(500) = 'SUCCESS'
begin try
	Select * 
	Into #tblCardProduction 
	From tblCardProduction B With (NoLock) 
	Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo

	SELECT @code AS [Code], @description [Description], 'Accounts,CustomerAccounts,Customers,Cards,AccountLinkage' [FileNames]

	Select Distinct 
	[Scheme code],
	[CIF ID],
	[AC ID] = SubString((Select '|' + LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) +':'+AccountType+':' + Case When AccountLinkage=0 Then '1' Else '2' End As [text()] From (Select Distinct [CIF ID],[AC ID],[Scheme code],AccountLinkage,AccountType From #tblCardProduction C With (NoLock) Left Join tblBin D With (NoLock) On C.[Card Type and SubType]=D.CardPrefix Where Rejected=0 And Processed=1 ) A Where A.[CIF ID] = B.[CIF ID] Order By AccountLinkage For XML Path(''), Elements), 2, 99999) 
	InTo #CFIDACIDLinkage
	From #tblCardProduction B With (NoLock) 
	Where Rejected=0 And Processed=1 and IsAuthorised =1 --And BatchNo=@BatchNo			

--select * from #tblCardProduction
--select * from #CFIDACIDLinkage


	Select  Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], 'U,'+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) +','+isnull(AccountType,'')+',356,,,,' AS [Result] From #tblCardProduction A With (NoLock) Left Join tblBin B With (NoLock) On A.[Card Type and SubType]=B.CardPrefix INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo
	--Select Distinct 'U,'+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) +','+isnull(AccountType,'')+',356,,,,' From #tblCardProduction A With (NoLock) Left Join tblBin B With (NoLock) On A.[Scheme code]=B.Switch_SchemeCode And A.[Card Type and SubType]=B.CardPrefix Where Rejected=0 And Processed=1 And Downloaded<=@Redownload and IsAuthorised =1 --And BatchNo=@BatchNo
	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], 'U,'+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[CIF ID])))) +','+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) +','+ ISNULL(AccountType,'') AS [Result] From #tblCardProduction A With (NoLock) Left Join tblBin B With (NoLock) On A.[Card Type and SubType]=B.CardPrefix INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo
	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName],'U,'+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[CIF ID])))) +','+LTrim(RTrim(Case When Left([Card Type and Subtype],1)<>'6' Then LTrim(RTrim([PAN Number])) Else Case When LTrim(RTrim(IsNull(Aadhaar,'')))='' Then LTrim(RTrim([PAN Number])) Else LTrim(RTrim(IsNull(Aadhaar,''))) End End))+',,'+Upper(LTrim(RTrim([Customer Preferred name])))+',,,'+Upper(LTrim(RTrim([Customer Preferred name])))+',,,,,,,,,,,0,'+ Replace(IsNull([Country Dial code],''),'+','')+Case When IsNull([City Dial code],'0')!=0 And Replace(IsNull([Country Dial code],''),'+','')!='91' Then IsNull([City Dial code],'') Else '' End+Replace(IsNull([Mobile phone number],''),'+','') +',,'+LTrim(RTrim([Email id]))+','+LTrim(RTrim([Address Line 1]))+','+LTrim(RTrim([Address Line 2]))+' '+LTrim(RTrim([Address Line 3]))+','+LTrim(RTrim(City))+','+State+','+[Pin Code]+',,,,,,,,'+Right(DOB,4)+SUBSTRING(DOB,3,2)+Left(DOB,2)+',,,0,,211Card_Prefix12'+Right([Card Type and Subtype],2) AS [Result] From #tblCardProduction A With (NoLock) INNER JOIN TblBanks BA WITH(NOLOCK) ON A.Bank=BA.ID Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1 --And BatchNo=@BatchNo
	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),A.[CIF ID])))) +','+[Branch code]+','+ ISNULL(CardProgram,'')+','+isnull(AccountType,'')+','+B.[AC ID] AS [Result] From #tblCardProduction A With (NoLock) Left Join #CFIDACIDLinkage B With (NoLock) On A.[CIF ID]=B.[CIF ID] Left Join tblBin C With (NoLock) On A.[Card Type and SubType]=C.CardPrefix INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1--And BatchNo=@BatchNo --Order By [Branch code], A.[CIF ID]
	--Select Distinct LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),A.[CIF ID])))) +','+[Branch code]+','+ ISNULL(CardProgram,'')+','+isnull(AccountType,'')+','+B.[AC ID] From #tblCardProduction A With (NoLock) Left Join #CFIDACIDLinkage B With (NoLock) On A.[CIF ID]=B.[CIF ID] And A.[Scheme code]=B.[Scheme code] Left Join tblBin C With (NoLock) On A.[Scheme code]=C.Switch_SchemeCode And A.[Card Type and SubType]=C.CardPrefix Where Rejected=0 And Processed=1 And AccountLinkage=0 And Downloaded<=@Redownload and IsAuthorised =1--And BatchNo=@BatchNo --Order By [Branch code], A.[CIF ID]
	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),A.[CIF ID])))) +','+LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) AS [Result] From #tblCardProduction A With (NoLock) INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  And AccountLinkage=1 and IsAuthorised =1--And BatchNo= @BatchNo --And ExistingCustomer=1 


	--Update tblCardProduction Set Downloaded=1 Where Rejected=0 And Processed=1 and IsAuthorised =1--And BatchNo=@BatchNo
	--Update tblCardProductionReissue Set Generated=1, GeneratedOn=GetDate() Where Processed=1 And Generated=0 And Convert(DateTime,Convert(VarChar(11),ProcessedOn,113))=@Date And Bank=@Bank

	--Card Files generated record moved to autherized log
	INSERT INTO TblAuthorizedCardLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype]
	,[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3]
	,City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
	,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code]
	,[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing]
	,[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login]
	,AccountLinkage,ExistingCustomer,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID
	,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
	,ResidentCustomer,IsAuthorised,AuthorisedBy,SystemID,BankID,[Date] )
		SELECT CP.Code,CP.[CIF ID],CP.[Customer Name],CP.[Customer Preferred name],CP.[Card Type and Subtype]
			,CP.[AC ID],CP.[AC open date],CP.[CIF Creation Date],CP.[Address Line 1],CP.[Address Line 2],CP.[Address Line 3]
			,CP.City,CP.[State],CP.[Pin Code],CP.[Country code],CP.[Mothers Maiden Name],CP.DOB,CP.[Country Dial code]
			,CP.[City Dial code],CP.[Mobile phone number],CP.[Email id],CP.[Scheme code],CP.[Branch code]
			,CP.[Entered date],CP.[Verified Date],CP.[PAN Number],CP.[Mode Of Operation],CP.[Fourth Line Embossing]
			,CP.[Debit Card Linkage Flag],CP.[Uploaded On],CP.Rejected,CP.Reason,CP.Processed,CP.Downloaded,CP.[Login]
			,CP.AccountLinkage,CP.ExistingCustomer,CP.[Skip],CP.BatchNo,CP.AccountLinkageSMSSent,CP.AccountLinkageSMSGUID
			,CP.Aadhaar,CP.AddOnCards,CP.Bank,CP.ProcessedOn,CP.[Bc Branch Code],CP.[Center Name],CP.[Orig Card Type and Subtype]
			,CP.ResidentCustomer,CP.IsAuthorised,CP.AuthorisedBy,CP.SystemID,CP.BankID ,GETDATE()
				from tblCardProduction CP  WITH(NOLOCK) 
				INNER JOIN #tblCardProduction C ON Cp.[CIF ID]=c.[CIF ID]

  DELETE CP From tblCardProduction CP 
  	INNER JOIN #tblCardProduction C ON Cp.[CIF ID]=c.[CIF ID]

	DROP TABLE #tblCardProduction 
	DROP TABLE #CFIDACIDLinkage
end try
begin catch
	rollback tran
	SELECT 1 AS [Code], ERROR_MESSAGE() [Description], '' [FileNames]

	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
	SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+' Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
end catch

end
GO
