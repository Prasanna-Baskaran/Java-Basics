USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_processCustomerDetailsForCard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------==============================================
CREATE procedure [dbo].[Sp_processCustomerDetailsForCard] 
@CardNos varchar(max),
@UserId varchar(5),
@Bank varchar(200)=1,
@SystemID varchar(200)=1

as
begin
declare @code int = 1, @description as varchar(500) = 'Failed to process'

begin tran
begin try

	select * into #customer from dbo.TblCustomersDetails with(nolock) where customerid in (select Value from fnSplit(@CardNos,',')) And SystemID=@SystemID AND BankID=@Bank
	declare @Batchno varchar(20)= 'BATCH' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')

	---for previous rejected cards process again
	IF EXISTS(Select 1 from tblCardProduction Cp  WITH(NOLOCK) INNER JOIN #customer c ON Cp.[CIF ID]=c.BankCustID where cp.SystemID=@SystemID AND cp.Bank=@Bank AND ISNULL(Rejected,0)=1)
	BEGIN
	 Insert INTO tblCardProcessFailedLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
	   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
		,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
		,[Fourth Line Embossing],[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
		,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
		,ResidentCustomer,IsAuthorised,AuthorisedBy,[Date],SystemID,Account_Type)
		(Select Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
				   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
			,[City Dial code],LTRIM(RTRIM([Mobile phone number])),[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
			,[Fourth Line Embossing],[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
			,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
			,ResidentCustomer,IsAuthorised,AuthorisedBy,GETDATE(),cp.SystemID,cp.Account_Type
			 from tblCardProduction cp  WITH(NOLOCK) 
			 INNER JOIN #customer c ON Cp.[CIF ID]=c.BankCustID 
			 where cp.Bank=@Bank AND cp.SystemID=@SystemID  AND ISNULL(Rejected,0)=1)
       
	   DELETE cp FROM tblCardProduction cp   INNER JOIN #customer c ON Cp.[CIF ID]=c.BankCustID where cp.SystemID=@SystemID and Bank=@Bank AND ISNULL(Rejected,0)=1

	END

	select BankCustID, FirstName + ' '+ LastName [Customer Name], SUBSTRING(NameOnCard,0,24)   [Preferred name]
	, isnull(tb.CardPrefix,'') [CardPrefix]
	--, RIGHT(CONCAT('00000000000000000000', dbo.ufn_decryptPAN(AccNo)), 16)[AccountID]   --start Diksha Account Logic Change
	,dbo.ufn_decryptPAN(AccNo) [AccountID]
	, REPLACE(CONVERT(varchar,Checker_Date_IND,103),'/','') [Checker_Date_IND], REPLACE(CONVERT(varchar,GETDATE(),103),'/','') [CIFCreationdate]
	, Replace(Replace('HouseNo ' + HouseNo_P+' StreetName' + StreetName_P ,'-',' '),',',' ') [Address1]
	, Replace(Replace( ',City '+City_P+' District '+ca.District_P,'-',' '),',',' ') [Address2]
	, Replace(Replace('WardNo'+ ISNULL(WardNo_P,''),'-',' '),',',' ')  [Address3] --address to be fill
	, ca.City_P, '' [State], '' [PINCODE], '' [COUNTRYCODE], MotherName, REPLACE(CONVERT(varchar, CP.DOB_AD,103),'/','') [DOB], '' [DIALCODE], '' [CITYDIALCODE]
	, (LTRIM(RTRIM(cp.MobileNo))) [MobileNo], ca.Email_P, ISNULL(tb.Switch_SchemeCode,'') [schemecode], tp.INST_ID [branchcode], REPLACE(CONVERT(varchar,cp.Maker_Date_IND,103),'/','') [entereddate]
	, REPLACE(CONVERT(varchar,cp.Checker_Date_IND,103),'/','') [verifieddate], SUBSTRING(cp.PassportNo_CitizenShipNo,0,12) [pannumber], '02' [modeofoperation], '' [forthlineembossing]
	, 'Y' [Debit Card Linkage Flag], GETDATE() [uploadedon], 0 [Rejected], '' [reason], 0 [Processed], 0 [Downloaded], @UserId [Login], 0 [AccountLinkage]
	, 0 [existingcustomer], 0 [Skip], @Batchno [batchno], 0 [AccountLinkageSMSSent], '' [AccountLinkageSMSGUID], '' [Aadhar], 0 [AddOnCards]
	, @Bank [Bank], getdate() [processedon], '' [Bc Branch Code], '' [centername], '' [originalcardtype], 1 [ResidentCustomer],cp.SystemID,Case WHEN ISNULL(cp.AccType,0)=0 Then 10 Else At.AccountTypeCode END AS[AccountType]
	into #CardProduction
	from #customer cp
	left outer join TblCustomerAddress ca with(nolock) on ca.CustomerID = cp.CustomerID
	left outer join TblProductType tp with(nolock) on tp.id = cp.ProductType_ID
	left outer join TblBIN tb with(nolock) on tb.ID=tp.BIN_ID
	left JOIN TblAccountType At WITH(NOLOCK) on ISNULL(cp.AccType,1)=At.AccountTypeID

	

	insert into tblCardProduction([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],
	[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3],[City],[State],[Pin Code],
	[Country code],[Mothers Maiden Name],[DOB],[Country Dial code],[City Dial code],[Mobile phone number],[Email id],
	[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing],
	[Debit Card Linkage Flag],[Uploaded On],[Rejected],[Reason],[Processed],[Downloaded],[Login],[AccountLinkage],
	[ExistingCustomer],[Skip],[BatchNo],[AccountLinkageSMSSent],[AccountLinkageSMSGUID],[Aadhaar],[AddOnCards],[Bank],
	[ProcessedOn],[Bc Branch Code],[Center Name],[Orig Card Type and Subtype],[ResidentCustomer],SystemID,Account_Type)
	select * from #CardProduction


	--Update tblCardProduction Set Rejected=1, Reason='Invalid Customer ID' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Not (Len(LTrim(RTrim(IsNull([CIF ID],'')))) Between 0 And 17) And Bank=@Bank AND SystemID=@SystemID

	--Update tblCardProduction Set Rejected=1, Reason='Invalid Account No' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Not (Len(LTrim(RTrim(IsNull([AC ID],'')))) Between 12 And 16)  And Bank=@Bank AND SystemID=@SystemID

	--Update tblCardProduction Set Rejected=1, Reason='Invalid Card Type and Subtype' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Len(LTrim(RTrim(IsNull([Card Type and Subtype],''))))<>12 And Bank=@Bank AND SystemID=@SystemID

	Update tblCardProduction Set Rejected=1, Reason='Invalid Email Id' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Len([Email id])>0 And ((PatIndex('%;%',[Email id])>0) Or (PatIndex('%,%',[Email id])>0) Or (PatIndex('%@%.%',[Email id])=0))  And Bank=@Bank AND SystemID=@SystemID

	Update tblCardProduction Set Rejected=1, Reason='Invalid Mobile No' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And (((LTRIM(RTRIM([Mobile phone number] )))Like '%[^0-9]%') Or ((Len(LTRIM(RTRIM([Mobile phone number] ))))>10) Or ((Len(LTRIM(RTRIM([Mobile phone number] ))))<10))  And Bank=@Bank AND SystemID=@SystemID

	--Update tblCardProduction Set Rejected=1, Reason='Duplicate Card Request' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And ([CIF ID]+[AC ID]) In (Select [CIF ID]+[AC ID] From tblCardProduction Where Rejected=0 And Processed=0 And Bank=1 Group By [CIF ID],[AC ID] Having Count(*)>1) And Bank=1 

	--Update tblCardProduction Set Rejected=1, Reason='Debit Card Not requested by Customer' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And [Debit Card Linkage Flag]<>'Y' And Bank=1 

	Update tblCardProduction Set Processed=1, ProcessedOn=GETDATE()  Where Processed=0  And Bank=@Bank AND SystemID=@SystemID --Rejected=0 And Processed=0 And [Skip]=0 And Bank=1

	SELECT 0 AS [Code], 'Success' [Description]
	
	select [CIF ID],[Customer Name],[Card Type and Subtype] as [CardPrefix],[AC ID] AS [AccountNo],
	[AC open date] AS [Date],[City],[Mobile phone number] AS [MobileNo],[Email id] AS [Email],
	Case When [Rejected]=1 THEN 'Card request rejected' ELSE 'Card request processed successfully' END AS [Status],ISNULL([Reason],'') AS [Remark]
	from tblCardProduction where BatchNo=@Batchno
    	AND ((SystemID=@SystemID)) 
		AND ((Bank=@Bank)) 
		AND (BatchNo=@Batchno)
		order by ISNULL(Rejected ,0) desc

	



commit tran
end try
begin catch
	rollback tran
	SELECT 1 AS [Code], ERROR_MESSAGE() [Description]

	INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
	SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+' Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
end catch
end

GO
