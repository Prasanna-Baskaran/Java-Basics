USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CABulk_SaveCardProdData_20180514]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Sp_CABulk_SaveCardProdData_20180514]
	@CustBulkData CustBulkDataType READONLY,
	@IssuerNo int =0,
	@FileName varchar(800)=''
AS
BEGIN
 Begin Transaction  
 Begin Try    
 DECLARE @strCode int=1,@strOutputDesc VARCHAR(800)='Error',@OutputCode Varchar(50)=''
	declare @Batchno varchar(20)= 'BATCH' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')
	DECLARE @Bank VARCHAR(200)='',@SystemID VARCHAR(200)='',@AddValidationSP Varchar(500)=''

SELECT @Bank=ID,@AddValidationSP=Validate_Add_SP,@SystemID=SystemID From TblBanks WITH(NOLOCK) where BankCode=@IssuerNo
--SELECT Distinct TOP 1 @SystemID=SystemID From @CustBulkData

IF Exists(SELECT Top 1 1 FROM @CustBulkData )
BEGIN
		if(@Bank <> '')
		BEGIN
		----- Save CIF file Data
		INSERT INTO TblCIF_FileData (CIF_FileName,BatchNo,BankID,CreatedDate,CIF_ID,CustomerName,NameOnCard
		,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
		,Address1,Address2,Address3,City,State,PinCode,Country,Mothers_Name,DOB
		,CountryCode,STDCode,MobileNo,EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,
		Verified_Date,PAN_No,Mode_Of_Operation,Fourth_Line_Embossing,Aadhar_No,
		Issue_DebitCard,Pin_Mailer,Account_Type,SystemID,PGKValue) /*Added PGKValue Column Change on 02 Nov 2017*/
		(SELECT @FileName,@Batchno,@Bank,GETDATE(),CIF_ID,CustomerName,NameOnCard
		,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
		,Address1,Address2,Address3,City,State,PinCode,Country,Mothers_Name,DOB
		,CountryCode,STDCode,MobileNo,EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,
		Verified_Date,PAN_No,Mode_Of_Operation,Fourth_Line_Embossing,Aadhar_No,
		Issue_DebitCard,Pin_Mailer,'',SystemID,PGKValue From @CustBulkData) /*Added PGKValue. Change on 02 Nov 2017*/


		---for previous rejected cards process again
	IF EXISTS(Select top 1 1 from tblCardProduction Cp  WITH(NOLOCK) 
	INNER JOIN @CustBulkData c ON Cp.[CIF ID]=c.CIF_ID where  cp.Bank=@Bank AND Rejected=1)
			BEGIN
		 Insert INTO tblCardProcessFailedLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
	   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
		,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
		,[Fourth Line Embossing],[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
		,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
		,ResidentCustomer,IsAuthorised,AuthorisedBy,[Date],SystemID,IsBulkUpload,CIF_FileName,Account_Type,Pin_Mailer,SwitchCardProgram,PGKValue)/*Added PGKValue Column Change on 02 Nov 2017*/
		(Select cp.Code,cp.[CIF ID],cp.[Customer Name],cp.[Customer Preferred name],cp.[Card Type and Subtype],cp.[AC ID],cp.[AC open date],cp.[CIF Creation Date]
				   ,cp.[Address Line 1],cp.[Address Line 2],cp.[Address Line 3],cp.City,cp.[State],cp.[Pin Code],cp.[Country code],cp.[Mothers Maiden Name],cp.DOB,cp.[Country Dial code]
			,cp.[City Dial code],LTRIM(RTRIM(cp.[Mobile phone number])),cp.[Email id],cp.[Scheme code],cp.[Branch code],cp.[Entered date],cp.[Verified Date],cp.[PAN Number],cp.[Mode Of Operation]
			,cp.[Fourth Line Embossing],cp.[Debit Card Linkage Flag],cp.[Uploaded On],cp.Rejected,cp.Reason,cp.Processed,cp.Downloaded,cp.[Login],cp.AccountLinkage,cp.ExistingCustomer
			,cp.[Skip],cp.BatchNo,cp.AccountLinkageSMSSent,cp.AccountLinkageSMSGUID,cp.Aadhaar,cp.AddOnCards,cp.Bank,cp.ProcessedOn,cp.[Bc Branch Code],cp.[Center Name],cp.[Orig Card Type and Subtype]
			,cp.ResidentCustomer,cp.IsAuthorised,cp.AuthorisedBy,GETDATE(),cp.SystemID,cp.IsBulkUpload,CIF_FileName,'',cp.Pin_Mailer,cp.SwitchCardProgram,isnull(B.PGKValue,'') /*Added PGKValue. Change on 02 Nov 2017*/
			 from tblCardProduction cp  WITH(NOLOCK) 		
			 INNER JOIN @CustBulkData B ON cp.[CIF ID]=B.CIF_ID	
			 where cp.Bank=@Bank AND Rejected=1 )
       
			   DELETE cp FROM tblCardProduction cp   INNER JOIN @CustBulkData c ON Cp.[CIF ID]=c.CIF_ID where cp.Bank=@Bank AND cp.Rejected=1
			END
		

		-- Insert data for card prosessing
			Insert into tblCardProduction ([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
	   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
		,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
		,[Fourth Line Embossing],[Debit Card Linkage Flag],Aadhaar,[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
		,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
		,ResidentCustomer,SystemID,IsBulkUpload,CIF_FileName,Account_Type,SwitchCardProgram,Pin_Mailer,PGKValue,BankID) 
		(SELECT CIF_ID,CustomerName,NameOnCard,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
		,Replace(Replace([Address1],'-',' '),',',' '),Replace(Replace(Address2,'-',' '),',',' '),Replace(Replace(Address3,'-',' '),',',' ') ,City,[State],PinCode,Country,Mothers_Name,DOB,Replace(Replace(IsNull(CountryCode,''),'+',''),'-','') 
		,STDCode,Replace(Replace(Replace(Replace(MobileNO,'+91(0)',''),'+91(00)',''),'+',''),'-',''),EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,Verified_Date,PAN_No,Mode_Of_Operation,
		Fourth_Line_Embossing,Issue_DebitCard,Aadhar_No,GETDATE(),0,'',0,0,0,0,0,0,@Batchno,0,'',0,@Bank,NULL,'','',Bin_Prefix,1,@SystemID,1,@FileName
		--,Account_Type   
		,dbo.FunGetAccountTypeCardProgram(0,SCHEME_Code,Bin_Prefix,@Bank)    --- Get AccountType 
		--,ISNULL(Pin_Mailer,'N')
		,dbo.FunGetAccountTypeCardProgram(1,SCHEME_Code,Bin_Prefix,@Bank)  --- to save cardProgram
		,Isnull(Pin_Mailer,'')
		,ISNULL(PGKValue,'') /*Getting Account_Type as PGKValue. Change on 02 Nov 2017*/
		,@Bank
		from @CustBulkData)
				---------------------------------------------

  ---- ************************ Validation for Card Processing  *************************

  ------ When Additional  validation required
    IF(ISNULL(@AddValidationSP,'')<>'')
	BEGIN
	   EXEC @AddValidationSP @Bank ,@Batchno ,@IssuerNo 
	END
	--ELSE
	--BEGIN  
		
		Update tblCardProduction Set  Rejected=1, Reason='Invalid Bin Prefix/Scheme Code'  Where 
		((ISNULL(account_type,'')='')  OR(ISNULL(SwitchCardProgram,'')='') OR
		((account_type Not in ( Select Value from dbo.fnSplit((Select FixedValue from TblCardAutoCIFConfig WITH(NOLOCK) 
		 Where FieldName='AccountType' And CardProgram=SwitchCardProgram),'|') ))
		))AND Rejected=0 And Processed=0  And Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 

		Update cp SET cp.Rejected=1, cp.Reason='Invalid CIF ID' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='CIFID'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[CIF ID],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND (ISNUMERIC(ISNULL(cp.[CIF ID],''))=0)) OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[CIF ID])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[CIF ID])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[CIF ID],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[CIF ID],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[CIF ID]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))		
			) AND (Len(RTRIM(LTRIM(cp.[CIF ID])))<>0))
			 )

		Update cp SET cp.Rejected=1, cp.Reason='Invalid CustomerName' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='CustomerName'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Customer Name],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND(ISNUMERIC(ISNULL(cp.[Customer Name],''))=0)) OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Customer Name])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Customer Name])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Customer Name],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Customer Name],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Customer Name]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Customer Name])))<>0) )
			 )

	Update cp SET cp.Rejected=1, cp.Reason='Invalid Name On Card' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='NameOnCard'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Customer Preferred name],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND (ISNUMERIC(ISNULL(cp.[Customer Preferred name],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Customer Preferred name])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Customer Preferred name])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Customer Preferred name],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Customer Preferred name],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Customer Preferred name]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Customer Preferred name])))<>0) )
			 )

			 
	Update cp SET cp.Rejected=1, cp.Reason='Invalid AccountNo' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='AccountNo'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[AC ID],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND(ISNUMERIC(ISNULL(cp.[AC ID],''))=0))
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[AC ID])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[AC ID])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[AC ID],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[AC ID],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[AC ID]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[AC ID])))<>0) )
			 )

			 Update cp SET cp.Rejected=1, cp.Reason='Invalid Account opening date' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='AccountOpenDate'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[AC open date],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND(ISNUMERIC(ISNULL(cp.[AC open date],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[AC open date])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[AC open date])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[AC open date],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[AC open date],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[AC open date]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			OR ((dbo.FunCheckIsDate(cp.[AC open date])=0))
			) AND (Len(RTRIM(LTRIM(cp.[AC open date])))<>0))
			 )

	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Account CIF Creation Date' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='CIFCreationDate'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(IsNull(cp.[CIF Creation Date],''))))=0))
			OR(((((ISNUMERIC(ISNULL(cp.[CIF Creation Date],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[CIF Creation Date])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[CIF Creation Date])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[CIF Creation Date],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[CIF Creation Date],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[CIF Creation Date]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
				OR ((dbo.FunCheckIsDate(cp.[CIF Creation Date])=0))
			) AND (Len(RTRIM(LTRIM(cp.[CIF Creation Date])))<>0) )
			 )

			 
	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Address 1' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Address1'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Address Line 1],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1)AND(ISNUMERIC(ISNULL(cp.[Address Line 1],''))=0) )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 1])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 1])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Address Line 1],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Address Line 1],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Address Line 1]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Address Line 1])))<>0) )
			 )

		Update cp SET cp.Rejected=1, cp.Reason='Invalid Address 2' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Address2'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Address Line 2],''))))=0))
			OR(((((ISNULL(c.IsNum,0)=1)AND(ISNUMERIC(ISNULL(cp.[Address Line 2],''))=0) )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 2])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 2])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Address Line 2],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Address Line 2],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Address Line 2]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			)AND (Len(RTRIM(LTRIM(cp.[Address Line 2])))<>0) )
			 )

			 	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Address 3' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Address3'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Address Line 3],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Address Line 3],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 3])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Address Line 3])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Address Line 3],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Address Line 3],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Address Line 3]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Address Line 3])))<>0) )
			 )
		
		 	 Update cp SET cp.Rejected=1, cp.Reason='Invalid City' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='City'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[City],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[City],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[City])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[City])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[City],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[City],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[City]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[City])))<>0) )
			 )

			 	 	 Update cp SET cp.Rejected=1, cp.Reason='Invalid State' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='State'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[State],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[State],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[State])) like '%[^a-zA-Z ]%'))
			OR ((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[State])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[State],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[State],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[State]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[State])))<>0))
			 )

	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Pin code' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='PinCode'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Pin Code],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Pin Code],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Pin Code])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Pin Code])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Pin Code],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Pin Code],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Pin Code]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			)AND (Len(RTRIM(LTRIM(cp.[Pin Code])))<>0) )
			 )

			 
	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Country' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Country'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Country code],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Country code],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Country code])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Country code])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Country code],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Country code],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Country code]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			)AND (Len(RTRIM(LTRIM(cp.[Country code])))<>0))
			 )

			 


			 
	 Update cp SET cp.Rejected=1, cp.Reason='Invalid Mothers Name' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='MotherName'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Mothers Maiden Name],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Mothers Maiden Name],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Mothers Maiden Name])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Mothers Maiden Name])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Mothers Maiden Name],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Mothers Maiden Name],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Mothers Maiden Name]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Mothers Maiden Name])))<>0))
			 )

		Update cp SET cp.Rejected=1, cp.Reason='Invalid Date of Birth' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='DOB'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[DOB],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[DOB],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[DOB])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[DOB])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[DOB],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[DOB],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[DOB]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 OR ((dbo.FunCheckIsDate(cp.[DOB])=0))
			)AND (Len(RTRIM(LTRIM(cp.[DOB])))<>0) )
			 )

			 
		Update cp SET cp.Rejected=1, cp.Reason='Invalid Country Code' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='CountryCode'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Country Dial code],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Country Dial code],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Country Dial code])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Country Dial code])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Country Dial code],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Country Dial code],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Country Dial code]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Country Dial code])))<>0))
			 )


		Update cp SET cp.Rejected=1, cp.Reason='Invalid STD Code' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='STDCode'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[City Dial code],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[City Dial code],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[City Dial code])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[City Dial code])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[City Dial code],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[City Dial code],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[City Dial code]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 ) AND (Len(RTRIM(LTRIM(cp.[City Dial code])))<>0)))

	Update cp SET cp.Rejected=1, cp.Reason='Invalid Mobile No' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='MobileNo'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Mobile phone number],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Mobile phone number],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Mobile phone number])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Mobile phone number])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Mobile phone number],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Mobile phone number],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Mobile phone number]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 ) AND(Len(RTRIM(LTRIM(cp.[Mobile phone number])))<>0)))

  -- 	Update cp SET cp.Rejected=1, cp.Reason='Invalid EmailID' from tblCardProduction cp WITH(NOLOCK) 
		--left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Email'
		--Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		--And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Email id],''))))=0))
		--	OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Email id],''))=0)  )
			 
		--	 --OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Email id])) like '%[^a-zA-Z ]%'))
		--	 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Email id])) like '%[^a-zA-Z.@_ ]%'))/*Start Sheetal to email allow @ ,.,_*/
		--	OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Email id])) like '%[^a-zA-Z0-9.@_ ]%')))   --- for emailID allow @ and ,
		--	OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Email id],'')))) Between c.MinLen And c.MaxLen)))	  
		--	OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Email id],'')))))>c.MaxLen))
		--	OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Email id]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
		--	 ) AND (Len(RTRIM(LTRIM(cp.[Email id])))<>0) ))

		Update cp SET cp.Rejected=1, cp.Reason='Invalid EmailID' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Email'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Email id],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Email id],''))=0)  )
			 --OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Email id])) like '%[^a-zA-Z ]%'))
			 OR ((ISNULL(c.IsAlpha,0)=1) AND  (dbo.ValidateEmail(RTRIM(LTRIM(cp.[Email id]))) = 0))/*Start Sheetal to email allow @ ,.,_*/
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND ( (dbo.ValidateEmail(RTRIM(LTRIM(cp.[Email id]))) = 0))))   --- for emailID allow @ and ,
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Email id],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Email id],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Email id]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 ) AND (Len(RTRIM(LTRIM(cp.[Email id])))<>0) ))

   Update cp SET cp.Rejected=1, cp.Reason='Invalid Branch Code' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='BranchCode'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Branch code],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Branch code],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Branch code])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Branch code])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Branch code],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Branch code],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Branch code]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 ) AND (Len(RTRIM(LTRIM(cp.[Branch code])))<>0)))


   Update cp SET cp.Rejected=1, cp.Reason='Invalid Entered Date' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='Entereddate'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Entered date],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Entered date],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Entered date])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Entered date])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Entered date],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Entered date],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Entered date]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 OR ((dbo.FunCheckIsDate(cp.[Entered date])=0))
			 )AND(Len(RTRIM(LTRIM(cp.[Entered date])))<>0) ))

 Update cp SET cp.Rejected=1, cp.Reason='Invalid Verified Date' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='VerifiedDate'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Verified Date],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Verified Date],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Verified Date])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Verified Date])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Verified Date],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Verified Date],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Verified Date]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 OR ((dbo.FunCheckIsDate(cp.[Verified Date])=0))
			 ) AND (Len(RTRIM(LTRIM(cp.[Verified Date])))<>0) ))



 Update cp SET cp.Rejected=1, cp.Reason='Invalid PAN Number' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='PAN'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[PAN Number],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[PAN Number],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[PAN Number])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[PAN Number])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[PAN Number],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[PAN Number],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[PAN Number]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			 ) AND (Len(RTRIM(LTRIM(cp.[PAN Number])))<>0)))

 Update cp SET cp.Rejected=1, cp.Reason='Invalid Mode of Operation' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='ModeOfOperation'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Mode Of Operation],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Mode Of Operation],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Mode Of Operation])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Mode Of Operation])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Mode Of Operation],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Mode Of Operation],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Mode Of Operation]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Mode Of Operation])))<>0) )
			 )

 

 Update cp SET cp.Rejected=1, cp.Reason='Invalid Fourth line embossing' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='ForthLineEmboss'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Fourth Line Embossing],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Fourth Line Embossing],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Fourth Line Embossing])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Fourth Line Embossing])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Fourth Line Embossing],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Fourth Line Embossing],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Fourth Line Embossing]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			)AND (Len(RTRIM(LTRIM(cp.[Fourth Line Embossing])))<>0))
			 )


 Update cp SET cp.Rejected=1, cp.Reason='Invalid AdharNo' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='AdharNo'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Aadhaar],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Aadhaar],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Aadhaar])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Aadhaar])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Aadhaar],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Aadhaar],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Aadhaar]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[Aadhaar])))<>0))
			 )


 Update cp SET cp.Rejected=1, cp.Reason='Invalid issue debit card flag' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='IssueDebitCard'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Debit Card Linkage Flag],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Debit Card Linkage Flag],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Debit Card Linkage Flag])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Debit Card Linkage Flag])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Debit Card Linkage Flag],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Debit Card Linkage Flag],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Debit Card Linkage Flag]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			)AND (Len(RTRIM(LTRIM(cp.[Debit Card Linkage Flag])))<>0) )
			 )

Update TblcardProduction   Set Rejected=1, Reason='Age Below 18yrs'  
 Where rejected=0 And Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0  And Processed=0 AND ((DATEPART(YEAR,GETDATE()-CONVERT(DateTime,SUBSTRING(DOB,3,2)+'/'+LEFT(DOB,2)+'/'+RIGHT(DOB,4)))-1900)<18)

 Update cp SET cp.Rejected=1, cp.Reason='Invalid Pin mailer' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='PinMailer'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[Pin_Mailer],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[Pin_Mailer],''))=0)  )
			 OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[Pin_Mailer])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[Pin_Mailer])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[Pin_Mailer],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[Pin_Mailer],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[Pin_Mailer]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND(Len(RTRIM(LTRIM(cp.[Pin_Mailer])))<>0))
			 )

/*Validation PGKValue as datatype alpha numeric Change on 02 Nov 2017*/
Update cp SET cp.Rejected=1, cp.Reason='Invalid PGKValue' from tblCardProduction cp WITH(NOLOCK) 
		left join TblCardAutoCIFConfig c WITH(NOLOCK) ON  c.BankID=cp.Bank and c.CardProgram=cp.SwitchCardProgram and c.FieldName='PGKValue'
		Where cp.Rejected=0 And cp.Processed=0  And cp.Bank=@Bank And BatchNo=@Batchno  And AddOnCards=0 
		And (((ISNULL(c.IsMandatory,0)=1) AND (Len(RTRIM(LTRIM(ISNULL(cp.[PGKValue],''))))=0))
			OR(((( (ISNULL(c.IsNum,0)=1) AND(ISNUMERIC(ISNULL(cp.[PGKValue],''))=0)  )
			OR ((ISNULL(c.IsAlpha,0)=1) AND (RTRIM(LTRIM(cp.[PGKValue])) like '%[^a-zA-Z ]%'))
			OR((ISNULL(c.IsAlphanumeric,0)=1) AND (RTRIM(LTRIM(cp.[PGKValue])) like '%[^a-zA-Z0-9 ]%')))
			OR((ISNULL(c.MinLen,0)<>0) AND (Not (Len(LTrim(RTrim(IsNull(cp.[PGKValue],'')))) Between c.MinLen And c.MaxLen)))	  
			OR((ISNULL(c.MaxLen,0)<>0) AND ((Len(LTrim(RTrim(IsNull(cp.[PGKValue],'')))))>c.MaxLen))
			OR((ISnULL(c.FixedValue,'')<>'') AND (UPPER(RTRIM(LTRIM(cp.[PGKValue]))) Not in(Select Upper(Value) from dbo.fnSplit(c.FixedValue,'|')) ))
			) AND (Len(RTRIM(LTRIM(cp.[PGKValue])))<>0))
			 )

	--Update TblCardProduction Set Rejected=1, Reason='Duplicate Card Request' 
	--Where Rejected=0 And Processed=0 And AddOnCards=0 And (([CIF ID]+[AC ID])
	-- In (Select c.[CIF ID]+c.[AC ID] From TblCardProduction c WITH(NOLOCK)
	-- Where c.Rejected=0 And c.Processed=0 And c.Bank=@Bank And c.SwitchCardProgram=SwitchCardProgram 
	--	Group By [CIF ID],[AC ID] Having Count(1)>1)) And Bank=@Bank --AND  BatchNo=@Batchno

	Update cp Set cp.Rejected=1, cp.Reason='Duplicate Card Request' 
	FROM tblCardProduction cp WITH(NOLOCK)
	LEFT JOIN (Select Row_number() over (partition by [CIF ID] order by Code)RW ,*
			from tblCardProduction With(NOLOCK) where Rejected=0  ) A  On  A.RW=1 AND CP.[CIF ID]=A.[CIF ID] And CP.SwitchCardProgram=A.SwitchCardProgram AND CP.Bank=A.Bank 
		Where CP.[AC ID]=A.[AC ID] AND CP.Processed=0 AND cp.Rejected=0 AND  CP.Code>A.Code AND cp.AddOnCards=0 AND  CP.Bank=@Bank


	--Update TblCardProduction Set Rejected=1, Reason='Duplicate Card Request or Card Already Issued' Where Rejected=0 And Processed=0  And AddOnCards=0 And ([CIF ID]+[AC ID]) In (Select [CIF ID]+[AC ID] From TblAuthorizedCardLog WITH(NOLOCK) Where Processed=1 AND IsAuthorised=1 And Bank=@Bank) And Bank=@Bank AND  BatchNo=@Batchno
	Update cp Set cp.Rejected=1, cp.Reason='Duplicate Card Request or Card Already Issued' 
	FROM tblCardProduction cp WITH(NOLOCK)
	Left JOIN TblCardProduction p WITH(NOLOCK) ON cp.[CIF ID]+cp.[AC ID]=P.[CIF ID]+P.[AC ID] AND CP.SwitchCardProgram=P.SwitchCardProgram AND p.Processed=1 AND p.Rejected=0 AND cp.Bank=p.Bank
	Left JOIN TblAuthorizedCardLog Au WITH(NOLOCK) ON cp.[CIF ID]+cp.[AC ID]=Au.[CIF ID]+Au.[AC ID] AND CP.SwitchCardProgram=Au.SwitchCardProgram AND Au.Processed=1 AND Au.IsAuthorised=1 and au.Rejected=0 AND cp.Bank=Au.Bank
	where cp.Bank=@Bank AND ((ISNULL(p.Code,0)<>0) OR (ISNULL(Au.Code,0)<>0)) -- to check already processing is done
	AND cp.Processed=0 and cp.Rejected=0 AND cp.AddOnCards=0 --AND  cp.BatchNo=@Batchno


	------ AccountLinkage From TblCardProduction Table
	Update CP Set CP.AccountLinkage=1
		from tblCardProduction CP With(NOLOCK)
		LEFT JOIN (Select Row_number() over (partition by [CIF ID] order by Code)RW ,*
			from tblCardProduction With(NOLOCK) where Rejected=0) A  On  A.RW=1 AND CP.[CIF ID]=A.[CIF ID] And CP.SwitchCardProgram=A.SwitchCardProgram AND CP.Bank=A.Bank 
		Where CP.[AC ID]<>A.[AC ID] AND CP.Processed=0 AND cp.Rejected=0 AND CP.AccountLinkage=0 AND  CP.Code>A.Code AND cp.AddOnCards=0 AND  CP.Bank=@Bank

	---- AccountLinkage From TblAuthorizedCardLog Table

	Update CP Set CP.AccountLinkage=1
	from tblCardProduction CP With(NOLOCK)
	LEFT JOIN (Select Row_number() over (partition by [CIF ID] order by Code)RW ,*
		from TblAuthorizedCardLog With(NOLOCK)) A  On A.RW=1 AND  CP.[CIF ID]=A.[CIF ID] And CP.SwitchCardProgram=A.SwitchCardProgram AND CP.Bank=A.Bank
	Where CP.[AC ID]<>A.[AC ID] AND CP.Processed=0 AND cp.Rejected=0 AND CP.AccountLinkage=0 AND cp.AddOnCards=0 AND  CP.Bank=@Bank



	Update tblCardProduction Set Processed=1, ProcessedOn=GETDATE(),IsAuthorised=1,AuthorisedBy=0  Where Rejected=0 AND  Processed=0 And BatchNo=@Batchno  And Bank=@Bank 

--END
			--SELECT 0 AS [Code],'success' AS [OutputDescription],@Batchno AS [OutPutCode]



			SET @strCode=0
			SET @strOutputDesc='Success'
			SET @OutputCode=''
END
END

	

COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@FileName='+@FileName,@IssuerNo
		  
		   
	END CATCH;  	

	SELECT @strCode AS [Code],@strOutputDesc AS [OutputDescription],@OutputCode AS [OutPutCode]

	--- for failed customer data
IF(@strCode=0)
BEGIN
	/* PGKValue  change for utkarsh start*/
       if @IssuerNo='52'
		   begin
		   		SELECT CIF_FileName,ISNULL([CIF ID],'')+'|'+ISNULL([Customer Name],'')+'|'+ISNULL([Customer Preferred name],'')+'|'+ISNULL([Card Type and Subtype],'')+'|'+ISNULL([AC ID],'')
				+'|'+ISNULL([AC open date],'')+'|'+ISNULL([CIF Creation Date],'')+'|'+ISNULL([Address Line 1],'')+'|'+ISNULL([Address Line 2],'')+'|'+ISNULL([Address Line 3],'')+'|'+ISNULL(City,'')
				+'|'+ISNULL([State],'')+'|'+ISNULL([Pin Code],'')+'|'+ISNULL([Country code],'')+'|'+ISNULL([Mothers Maiden Name],'')+'|'+ISNULL(DOB,'')
				+'|'+ISNULL([Country Dial code],'')+'|'+ISNULL([City Dial code],'')+'|'+ISNULL([Mobile phone number],'')+'|'+ISNULL([Email id],'')+'|'+ISNULL([Scheme code],'')
				+'|'+ISNULL([Branch code],'')+'|'+ISNULL([Entered date],'')+'|'+isnull([Verified Date],'')+'|'+ISNULL([PAN Number],'')+'|'+ISNULL([Mode Of Operation],'')
				+'|'+ISNULL([Fourth Line Embossing],'')+'|'+ISNULL(Aadhaar,'')+'|'+ISNULL([Debit Card Linkage Flag],'')+'|'+iSNULL([Pin_Mailer],'') +'|'+iSNULL([PGKValue],'') 
				+'|'+ISNULL(Reason,'')  AS [Result] 
				 from tblCardProduction WITH(NOLOCK) WHERE Rejected=1	AND BatchNo=@Batchno AND Bank=@Bank AND CIF_FileName=@FileName
		   end
	   else
		   begin
				SELECT CIF_FileName,ISNULL([CIF ID],'')+'|'+ISNULL([Customer Name],'')+'|'+ISNULL([Customer Preferred name],'')+'|'+ISNULL([Card Type and Subtype],'')+'|'+ISNULL([AC ID],'')
				+'|'+ISNULL([AC open date],'')+'|'+ISNULL([CIF Creation Date],'')+'|'+ISNULL([Address Line 1],'')+'|'+ISNULL([Address Line 2],'')+'|'+ISNULL([Address Line 3],'')+'|'+ISNULL(City,'')
				+'|'+ISNULL([State],'')+'|'+ISNULL([Pin Code],'')+'|'+ISNULL([Country code],'')+'|'+ISNULL([Mothers Maiden Name],'')+'|'+ISNULL(DOB,'')
				+'|'+ISNULL([Country Dial code],'')+'|'+ISNULL([City Dial code],'')+'|'+ISNULL([Mobile phone number],'')+'|'+ISNULL([Email id],'')+'|'+ISNULL([Scheme code],'')
				+'|'+ISNULL([Branch code],'')+'|'+ISNULL([Entered date],'')+'|'+isnull([Verified Date],'')+'|'+ISNULL([PAN Number],'')+'|'+ISNULL([Mode Of Operation],'')
				+'|'+ISNULL([Fourth Line Embossing],'')+'|'+ISNULL(Aadhaar,'')+'|'+ISNULL([Debit Card Linkage Flag],'')+'|'+iSNULL([Pin_Mailer],'') +'|'+ISNULL(Reason,'')  AS [Result] 
				 from tblCardProduction WITH(NOLOCK) WHERE Rejected=1	AND BatchNo=@Batchno AND Bank=@Bank AND CIF_FileName=@FileName
		   end	
	 END
END




GO
