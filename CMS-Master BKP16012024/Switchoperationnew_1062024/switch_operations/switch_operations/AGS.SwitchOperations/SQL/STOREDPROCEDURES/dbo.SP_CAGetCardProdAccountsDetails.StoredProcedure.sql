USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_CAGetCardProdAccountsDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CAGetCardProdAccountsDetails]-- [SP_CAGetCardProdAccountsDetails] 1,'',54
	@Redownload Bit=0,
	@BatchNo	VarChar(20)='',
	@IssuerNo VARCHAR(20)=''
As
/************************************************************************
Object Name: Get Card,Accounts file for CardGeneration
Purpose: Get Card,Accounts file for CardGeneration
Change History
Date         Changed By				Reason
23/04/2017  Diksha Walunj			Newly Developed
26/09/2017  Diksha Walunj			Modified Multiple AccountLink to Same Card

Change History
Date         Changed By				Reason
03/11/2017  Pratik Mhatre			Added PGKValue in card file for utkarsh bank(Issuer no. 18)

Change History
Date         Changed By				Reason
01/02/2018  Pratik Mhatre			ATPBF-124 Removed account id zero padding condition
*************************************************************************/

Begin
declare @code int = 0, @description as varchar(500) = 'SUCCESS'
begin try
--start sheetal
--Create AccountBalances file for Prepaid cards
--declare @CardType varchar(50)
--declare @Bankid int
--select @Bankid=id from tblbanks where bankcode=@IssuerNo
--select @CardType=CardType from tblbin where bankid=@Bankid
--end sheetal
--start sheetal change for account balance file for prepaid card and for insta
declare @IsAccountBalanceFile bit
declare @SyncCustomerData bit
select @IsAccountBalanceFile=IsAccountBalanceFile ,@SyncCustomerData=SyncCustomerData from tblbanks where bankcode=@IssuerNo

--select @IsInsta=IsInsta from tblbanks where bankcode=@IssuerNo
--end sheetal

	declare @DownloadBatch varchar(20)= 'D' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')

	Select B.Code,B.[CIF ID],b.[Customer Name],b.[Customer Preferred name],b.[Card Type and Subtype],b.[AC ID],b.[AC open date],b.[CIF Creation Date],b.[Address Line 1],b.[Address Line 2],b.[Address Line 3],b.[City],b.[State],b.[Pin Code]
	,b.[Country code],b.[Mothers Maiden Name],b.[DOB],b.[Country Dial code],b.[City Dial code],b.[Mobile phone number],b.[Email id],b.[Scheme code],b.[Branch code],
	b.[Entered date],b.[Verified Date],b.[PAN Number],b.[Mode Of Operation],b.[Fourth Line Embossing],b.[Debit Card Linkage Flag],b.[Uploaded On],b.[Rejected],b.[Reason]
	,b.[Processed],b.[Downloaded],b.[Login],b.[AccountLinkage],b.[ExistingCustomer],b.[Skip],b.[BatchNo],b.[AccountLinkageSMSSent],b.[AccountLinkageSMSGUID]
	,b.[Aadhaar],b.[AddOnCards],b.[Bank],b.[ProcessedOn],b.[Bc Branch Code],b.[Center Name],b.[Orig Card Type and Subtype],b.[ResidentCustomer],b.[IsAuthorised],b.[AuthorisedBy]
	,b.[SystemID],b.BankID,B.Account_Type,Pin_Mailer,B.PGKValue
	Into #tblCardProduction 
	From tblCardProduction B With (NoLock) 
	INNER JOIN TblBanks  BA WITH(NOLOCK) ON B.bank=BA.ID
	Where Rejected=0 And Processed=1  and IsAuthorised =1 AND ((@IssuerNo='')OR (BA.BankCode=@IssuerNo)) AND ((@BatchNo='') OR (B.BatchNo=@BatchNo))
	
	--Start sheetal
	--add new file name as AccountBalance_Default
	
	If Exists(SELECT top 1 1 from #tblCardProduction)
	 BEGIN

		--if @IssuerNo='9' /*9 issuer no for prapaid card*/
		if(@IsAccountBalanceFile=1)
		begin
			SELECT @code AS [Code], @description [Description], 'Accounts_Default,CustomerAccounts_Default,Customers_Default,Cards_Default,AccountBalances_Default,AccountLinkage_Default' [FileNames]
		end
		else
		begin
			SELECT @code AS [Code], @description [Description], 'Accounts_Default,CustomerAccounts_Default,Customers_Default,Cards_Default,AccountLinkage_Default' [FileNames]		
		end
	--end sheetal
	Select Distinct 
	[Scheme code],
	[CIF ID],
	--[AC ID] = SubString((Select '|' + LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) +':'+ISNULL(A.Account_Type,'')+':' + 
	[AC ID] = SubString((Select '|' + LTrim(RTrim(Convert(VarChar(Max), [AC ID] ))) +':'+ISNULL(A.Account_Type,'')+':' +  --Removed numeric condition from AC ID field ATPCM-124
				convert(varchar(50),A.RowNum) As [text()]  --Multiple AccountLink to Same Card
				From (Select Distinct [CIF ID],[AC ID],[Scheme code],AccountLinkage,Account_Type,(Row_number() over (partition by [CIF ID],C.[Card Type and SubType] order by Code))RowNum 
						 From #tblCardProduction C With (NoLock) 
					--	Left Join tblBin D With (NoLock) On C.[Card Type and SubType]=D.CardPrefix 
						Where Rejected=0 And Processed=1 ) A Where A.[CIF ID] = B.[CIF ID] Order By AccountLinkage For XML Path(''), Elements), 2, 99999) 
	InTo #CFIDACIDLinkage
	From #tblCardProduction B With (NoLock) 
	Where Rejected=0 And Processed=1 and IsAuthorised =1 --And BatchNo=@BatchNo			

	--select * from #tblCardProduction
	--select * from #CFIDACIDLinkage

	-- Accounts
			Select  Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], 'U,'+
			--LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) 
			LTrim(RTrim(Convert(VarChar(Max), [AC ID] )))  --Removed numeric condition from AC ID field ATPCM-124
			+','+isnull(A.Account_Type,'')+','+ISNULL(B.Currency,'356')+',,,,' AS [Result] From #tblCardProduction A With (NoLock)
			Left Join tblBin B With (NoLock) On A.[Card Type and SubType]=B.CardPrefix 
			INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo

	--CustomerAccounts
		Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], 'U,'+LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+
		--LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID]))))
		LTrim(RTrim(Convert(VarChar(Max), [AC ID])))  --Removed numeric condition from AC ID field ATPCM-124
		+','+ ISNULL(A.Account_Type,'') AS [Result]
		 From #tblCardProduction A With (NoLock) 
		Left Join tblBin B With (NoLock) On A.[Card Type and SubType]=B.CardPrefix 
		INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo

	 -- Cards
       /* PGKValue  change for utkarsh start*/
       if @IssuerNo='27'--change for Utkarsh on UAT issuer=52
       begin
			--Customers		
			Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName],'U,'+LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+
			--LTrim(RTrim(
			--	Case When Left([Card Type and Subtype],1)<>'6' 
			--		Then LTrim(RTrim([PAN Number])) 
			--	Else 
			--		Case When LTrim(RTrim(IsNull(Aadhaar,'')))='' 
			--			Then LTrim(RTrim([PAN Number])) 
			--		Else LTrim(RTrim(IsNull(Aadhaar,''))) End End))
			LTrim(RTrim(IsNull(Aadhaar,'')))
			  +',,'+Upper(LTrim(RTrim([Customer Name])))+',,,'+Upper(LTrim(RTrim([Customer Preferred name])))+',,,,,,,,,,,0,'+Replace(IsNull([Mobile phone number],''),'+','') +',,'+LTrim(RTrim([Email id]))+','+LTrim(RTrim([Address Line 1]))+','+LTrim(RTrim([Address Line 2]))+' '+LTrim(RTrim([Address Line 3]))+','+LTrim(RTrim(City))+','+State+','+[Pin Code]+',,,,,,,,'+Right(DOB,4)+SUBSTRING(DOB,3,2)+Left(DOB,2)+',,,0,,211Card_Prefix12'+Right([Card Type and Subtype],2) AS [Result]
			From #tblCardProduction A With (NoLock) 
			INNER JOIN TblBanks BA WITH(NOLOCK) ON A.Bank=BA.ID 
			Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1 --And BatchNo=@BatchNo


		 	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName]
			, LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+ISNULL(a.[Branch code],'')+','+ ISNULL(c.CardProgram,'')+','+isnull(A.Account_Type,'')+','+
			B.[AC ID] 
			+','+ A.PGKValue
			AS [Result]
			 From #tblCardProduction A With (NoLock) 
			 Left Join #CFIDACIDLinkage B With (NoLock) On A.[CIF ID]=B.[CIF ID] 
			 Left Join tblBin C With (NoLock) On A.[Card Type and SubType]=C.CardPrefix 
			 INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID 
			 Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1 --AND ((ISNULL(A.[Scheme code],''))=(ISNULL(C.Switch_SchemeCode,'')))--And BatchNo=@BatchNo --Order By [Branch code], A.[CIF ID]
       end /* PGKValue  change for utkarsh End*/
       else
       begin
	   --Customers		
		Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName],'U,'+LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+LTrim(RTrim(Case When Left([Card Type and Subtype],1)<>'6' Then LTrim(RTrim([PAN Number])) Else Case When LTrim(RTrim(IsNull(Aadhaar,'')))='' Then LTrim(RTrim([PAN Number])) Else LTrim(RTrim(IsNull(Aadhaar,''))) End End))+',,'+Upper(LTrim(RTrim([Customer Name])))+',,,'+Upper(LTrim(RTrim([Customer Preferred name])))+',,,,,,,,,,,0,'+Replace(IsNull([Mobile phone number],''),'+','') +',,'+LTrim(RTrim([Email id]))+','+LTrim(RTrim([Address Line 1]))+','+LTrim(RTrim([Address Line 2]))+' '+LTrim(RTrim([Address Line 3]))+','+LTrim(RTrim(City))+','+State+','+[Pin Code]+',,,,,,,,'+Right(DOB,4)+SUBSTRING(DOB,3,2)+Left(DOB,2)+',,,0,,211Card_Prefix12'+Right([Card Type and Subtype],2) AS [Result]
		 From #tblCardProduction A With (NoLock) 
		 INNER JOIN TblBanks BA WITH(NOLOCK) ON A.Bank=BA.ID 
		 Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1 --And BatchNo=@BatchNo

		 --Cards
			 Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName]
			, LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+ISNULL(a.[Branch code],'')+','+ ISNULL(c.CardProgram,'')
			+','+isnull(A.Account_Type,'')+','+B.[AC ID] 
			 AS [Result]
			 From #tblCardProduction A With (NoLock) 
			 Left Join #CFIDACIDLinkage B With (NoLock) On A.[CIF ID]=B.[CIF ID] 
			 Left Join tblBin C With (NoLock) On A.[Card Type and SubType]=C.CardPrefix 
			 INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID 
			 Where Rejected=0 And Processed=1 And AccountLinkage=0  and IsAuthorised =1 --AND ((ISNULL(A.[Scheme code],''))=(ISNULL(C.Switch_SchemeCode,'')))--And BatchNo=@BatchNo --Order By [Branch code], A.[CIF ID]
       end

--Start sheetal
--for Account Balance file record
if(@IsAccountBalanceFile=1)
		begin
			Select  Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName], 'U,'+
			--LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) 
			LTrim(RTrim(Convert(VarChar(Max), [AC ID] )))+','+B.[Ledger Account Balance]+','+B.[Available Account Balance]  --Removed numeric condition from AC ID field ATPCM-124
			+','+isnull(A.Account_Type,'') AS [Result] From #tblCardProduction A With (NoLock)
			Left Join tblBin B With (NoLock) On A.[Card Type and SubType]=B.CardPrefix 
			INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID Where Rejected=0 And Processed=1  and IsAuthorised =1 --And BatchNo=@BatchNo
		end
	--end sheetal
		
--AccountLinkage
	Select Distinct convert(Varchar,Bank) As [Bank],Replace(RTRIM(LTRIM(BA.BankName)),' ','') AS [BankName]
	, LTrim(RTrim(Convert(VarChar(Max),dbo.FunRemoveLeftZero(A.[CIF ID])))) +','+
	--LTrim(RTrim(Convert(VarChar(Max),Convert(Numeric(28,0),[AC ID])))) 
	LTrim(RTrim(Convert(VarChar(Max),[AC ID]))) --Removed numeric condition from AC ID field ATPCM-124 
	AS [Result] 
	From #tblCardProduction A With (NoLock) 
	INNER JOIN TblBanks BA WITH(NOLOCK) ON a.Bank=BA.ID 
	Where Rejected=0 And Processed=1  And AccountLinkage=1 and IsAuthorised =1--And BatchNo= @BatchNo --And ExistingCustomer=1 

	
	--Update tblCardProduction Set Downloaded=1 Where Rejected=0 And Processed=1 and IsAuthorised =1--And BatchNo=@BatchNo
	--Update tblCardProductionReissue Set Generated=1, GeneratedOn=GetDate() Where Processed=1 And Generated=0 And Convert(DateTime,Convert(VarChar(11),ProcessedOn,113))=@Date And Bank=@Bank
	
		SELECT CP.Code,CP.[CIF ID] [CIF ID],CP.[Customer Name] [Customer Name],CP.[Customer Preferred name] [Customer Preferred name],CP.[Card Type and Subtype] [Card Type and Subtype]
		,CP.[AC ID] [AC ID],CP.[AC open date] [AC open date],CP.[CIF Creation Date] [CIF Creation Date],CP.[Address Line 1] [Address Line 1], CP.[Address Line 2] [Address Line 2],CP.[Address Line 3] [Address Line 3]
		,CP.City City,CP.[State] [State], CP.[Pin Code] [Pin Code],CP.[Country code] [Country code],CP.[Mothers Maiden Name] [Mothers Maiden Name],CP.DOB DOB,CP.[Country Dial code] [Country Dial code]
		,CP.[City Dial code],CP.[Mobile phone number],CP.[Email id],CP.[Scheme code],CP.[Branch code]
		,CP.[Entered date] [Entered date],CP.[Verified Date] [Verified Date],CP.[PAN Number] [PAN Number],CP.[Mode Of Operation] [Mode Of Operation],CP.[Fourth Line Embossing] [Fourth Line Embossing]
		,CP.[Debit Card Linkage Flag] [Debit Card Linkage Flag],CP.[Uploaded On] [Uploaded On],CP.Rejected Rejected,CP.Reason Reason,CP.Processed Processed,CP.Downloaded Downloaded,CP.[Login] [Login]
		,CP.AccountLinkage AccountLinkage,CP.ExistingCustomer ExistingCustomer,CP.[Skip] [Skip],CP.BatchNo BatchNo,CP.AccountLinkageSMSSent AccountLinkageSMSSent,CP.AccountLinkageSMSGUID AccountLinkageSMSGUID
		,CP.Aadhaar Aadhaar,CP.AddOnCards AddOnCards,CP.Bank Bank,CP.ProcessedOn ProcessedOn,CP.[Bc Branch Code] [Bc Branch Code],CP.[Center Name] [Center Name],CP.[Orig Card Type and Subtype] [Orig Card Type and Subtype]
		,CP.ResidentCustomer ResidentCustomer,CP.IsAuthorised IsAuthorised,CP.AuthorisedBy AuthorisedBy,CP.SystemID SystemID,CP.BankID BankID ,GETDATE() [Date],
		CP.CIF_FileName CIF_FileName,CP.Account_Type Account_Type,CP.Pin_Mailer Pin_Mailer,@DownloadBatch ProcessBatch,SwitchCardProgram ,cp.PGKValue PGKValue
		into #finalCardprodData	from tblCardProduction CP  WITH(NOLOCK) 
		INNER JOIN #tblCardProduction C ON Cp.Code=c.Code

	--Card Files generated record moved to autherized log
	 INSERT INTO TblAuthorizedCardLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype]
	,[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3]
	,City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
	,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code]
	,[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing]
	,[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login]
	,AccountLinkage,ExistingCustomer,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID
	,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
	,ResidentCustomer,IsAuthorised,AuthorisedBy,SystemID,BankID,[Date],CIF_FileName,Account_Type,Pin_Mailer,ProcessBatch,SwitchCardProgram,PGKValue)
	select * from #finalCardprodData	
	
	/*This is added for insta change*/
	--if @IssuerNo='54'--prabhu bank
	if (@SyncCustomerData=1)
	begin
		select 1 IsCardSuccess,f.SystemID SystemID,f.BankID BankID, 
		Case When ISNULL([AC ID] ,'')='' THEN NULL ELSE dbo.ufn_EncryptPAN([AC ID])END AccNo,
		f.[CIF ID] [CIF ID] ,GETDATE() [Date],
		 Case WHen ISNULL(AccountTypeID,1)=0 THEn 1 ELSE AccountTypeID END AccType,
		 [Address Line 1] StreetName,
		 City, [Mobile phone number], [Email id],
		 [Customer Preferred name],
		  convert(datetime,stuff(stuff(DOB,5,0,'/'),3,0,'/')  , 103)  DOB  ,[Mothers Maiden Name]
		 into #CustomersDetails
		 from #finalCardprodData f 
		 left join TblCustomersDetails c with(nolock) on f.[CIF ID]=c.BankCustID 
		 left join TblAccounttype t With(NOLOCK) on f.[Scheme code]=t.AccountTypeCode
		 where c.BankCustID is null
		 
		 --select '#finalCardprodData',* from #finalCardprodData
		 --select '#CustomersDetails',* from #CustomersDetails

		INSERT INTO TblCustomersDetails (IsCardSuccess,SystemID,BankID,AccNo,BankCustID,Maker_Date_IND ,AccType,
		FirstName,MobileNo,DOB_AD,MotherName,NameOnCard,FormStatusID)
		select IsCardSuccess,							SystemID,BankID,AccNo,[CIF ID] ,[date],  AccType, 
		[Customer Preferred name],[Mobile phone number],DOB,[Mothers Maiden Name],[Customer Preferred name],1 [FormStatusID]	from #CustomersDetails
		 
		--select '#CustomersDetails',* from #CustomersDetails
		
		UPDATE TblCustomersDetails SET  ApplicationFormNo=dbo.FunGetApplicationNo(t.customerid)  
		--select dbo.FunGetApplicationNo(t.customerid)  ,*
		from TblCustomersDetails t
		inner join #CustomersDetails c on t.BankCustID=c.[CIF ID]
		
		INSERT INTO TblCustomerAddress(
			   t.Customerid, StreetName_P,City_P,District_P,Phone1_P,           Email_P,IsSameAsPermAddr,StreetName_C,City_C,District_C,Phone1_C,Email_C)	
		select CustomerID,f.StreetName,f.City,  f.City, f.[Mobile phone number],f.[Email id],1,f.StreetName,f.City,f.City,f.[Mobile phone number],f.[Email id]
		from #CustomersDetails f 
			  inner join TblCustomersDetails t on  f.[CIF ID]=t.BankCustID 

		 INSERT INTO TblCustOccupationDtl (CustomerID)
		 select CustomerID  
		 from #CustomersDetails f inner join
			  TblCustomersDetails t on f.[CIF ID]=t.BankCustID 
			  
		drop table #CustomersDetails
	end 

	  DELETE CP From tblCardProduction CP 
  		INNER JOIN #tblCardProduction C ON Cp.Code=c.Code

		DROP TABLE #tblCardProduction 
		DROP TABLE #CFIDACIDLinkage
		drop table #finalCardprodData
	ENd
	------
	ELSE
	BEGIN
		SELECT 1 AS [Code], 'No record' [Description], '' [FileNames]
	END
--END
end try
begin catch
	rollback tran
	SELECT 1 AS [Code], ERROR_MESSAGE() [Description], '' [FileNames]

	INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@IssuerNo='=@IssuerNo,@IssuerNo
end catch

end



GO
