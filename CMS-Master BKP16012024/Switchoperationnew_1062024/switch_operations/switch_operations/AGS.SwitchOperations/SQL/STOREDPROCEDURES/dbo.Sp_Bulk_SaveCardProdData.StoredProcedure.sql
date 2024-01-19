USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_Bulk_SaveCardProdData]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sp_Bulk_SaveCardProdData]
	@CustBulkData CustBulkDataType READONLY,
	@IssuerNo int =0,
	@FileName varchar(800)=''
AS
/************************************************************************
Object Name: 
Purpose: Save CIF FileData for CardGeneration
Change History
Date			Changed By				Reason
23/04/2017		Diksha Walunj			Newly Developed
29/09/2017     Diksha Walunj           [AGSCM-32] :AccountType comes in SchemeCode Field 


*************************************************************************/

BEGIN
 Begin Transaction  
 Begin Try    
 DECLARE @strCode int=1,@strOutputDesc VARCHAR(800)='Error',@OutputCode Varchar(50)=''
declare @Batchno varchar(20)= 'BATCH' + CONVERT(VARCHAR,GETDATE(),112) + REPLACE(CONVERT(VARCHAR(8),GETDATE(),114),':','')
DECLARE @Bank VARCHAR(200)='',@SystemID VARCHAR(200)=''

SELECT @Bank=ID From TblBanks WITH(NOLOCK) where BankCode=@IssuerNo
SELECT Distinct TOP 1 @SystemID=SystemID From @CustBulkData

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
		Issue_DebitCard,Pin_Mailer,Account_Type,SystemID) 
		(SELECT @FileName,@Batchno,@Bank,GETDATE(),CIF_ID,CustomerName,NameOnCard
		,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
		,Address1,Address2,Address3,City,State,PinCode,Country,Mothers_Name,DOB
		,CountryCode,STDCode,MobileNo,EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,
		Verified_Date,PAN_No,Mode_Of_Operation,Fourth_Line_Embossing,Aadhar_No,
		Issue_DebitCard,Pin_Mailer,'',SystemID From @CustBulkData)


		If(@IssuerNo=1)
		Begin
					-- Insert data for card prosessing
				Insert into [Temp_PO-old].dbo.CardProduction ([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
		   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
			,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
			,[Fourth Line Embossing],[Debit Card Linkage Flag],Aadhaar,[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
			,[Skip],UploadFileName,AccountLinkageSMSSent,AccountLinkageSMSGUID,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
			,ResidentCustomer)
			--,SystemID,IsBulkUpload,CIF_FileName,Account_Type,Pin_Mailer) 
			(SELECT CIF_ID,CustomerName,NameOnCard,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
			,Address1,Address2,Address3 ,City,[State],PinCode,Country,Mothers_Name,DOB,CountryCode 
			,STDCode,MobileNo,EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,Verified_Date,PAN_No,Mode_Of_Operation,
			Fourth_Line_Embossing,Issue_DebitCard,Aadhar_No,GETDATE(),0,'',0,0,0,0,0,0,@FileName,0,'',0,1,GETDATE(),'','','',1
			--,SystemID,1,@FileName,Account_Type,Pin_Mailer
			from @CustBulkData)

			exec [Temp_PO-old].dbo.[usp_processCardProductionData]

		END
	Else 
		Begin
		---for previous rejected cards process again
			IF EXISTS(Select 1 from tblCardProduction Cp  WITH(NOLOCK) INNER JOIN @CustBulkData c ON Cp.[CIF ID]=c.CIF_ID where  cp.Bank=@Bank)
			BEGIN
	 Insert INTO tblCardProcessFailedLog (Code,[CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
	   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
		,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
		,[Fourth Line Embossing],[Debit Card Linkage Flag],[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
		,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,Aadhaar,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
		,ResidentCustomer,IsAuthorised,AuthorisedBy,[Date],SystemID,IsBulkUpload,CIF_FileName,Account_Type,Pin_Mailer)
		(Select cp.Code,cp.[CIF ID],cp.[Customer Name],cp.[Customer Preferred name],cp.[Card Type and Subtype],cp.[AC ID],cp.[AC open date],cp.[CIF Creation Date]
				   ,cp.[Address Line 1],cp.[Address Line 2],cp.[Address Line 3],cp.City,cp.[State],cp.[Pin Code],cp.[Country code],cp.[Mothers Maiden Name],cp.DOB,cp.[Country Dial code]
			,cp.[City Dial code],LTRIM(RTRIM(cp.[Mobile phone number])),cp.[Email id],cp.[Scheme code],cp.[Branch code],cp.[Entered date],cp.[Verified Date],cp.[PAN Number],cp.[Mode Of Operation]
			,cp.[Fourth Line Embossing],cp.[Debit Card Linkage Flag],cp.[Uploaded On],cp.Rejected,cp.Reason,cp.Processed,cp.Downloaded,cp.[Login],cp.AccountLinkage,cp.ExistingCustomer
			,cp.[Skip],cp.BatchNo,cp.AccountLinkageSMSSent,cp.AccountLinkageSMSGUID,cp.Aadhaar,cp.AddOnCards,cp.Bank,cp.ProcessedOn,cp.[Bc Branch Code],cp.[Center Name],cp.[Orig Card Type and Subtype]
			,cp.ResidentCustomer,cp.IsAuthorised,cp.AuthorisedBy,GETDATE(),cp.SystemID,cp.IsBulkUpload,CIF_FileName,cp.Account_Type,cp.Pin_Mailer
			 from tblCardProduction cp  WITH(NOLOCK) 		
			 INNER JOIN @CustBulkData B ON cp.[CIF ID]=B.CIF_ID	
			 where cp.Bank=@Bank )
       
			   DELETE cp FROM tblCardProduction cp   INNER JOIN @CustBulkData c ON Cp.[CIF ID]=c.CIF_ID where    Bank=@Bank

			END
		

		-- Insert data for card prosessing
			Insert into tblCardProduction ([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date]
	   ,[Address Line 1],[Address Line 2],[Address Line 3],City,[State],[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code]
		,[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation]
		,[Fourth Line Embossing],[Debit Card Linkage Flag],Aadhaar,[Uploaded On],Rejected,Reason,Processed,Downloaded,[Login],AccountLinkage,ExistingCustomer
		,[Skip],BatchNo,AccountLinkageSMSSent,AccountLinkageSMSGUID,AddOnCards,Bank,ProcessedOn,[Bc Branch Code],[Center Name],[Orig Card Type and Subtype]
		,ResidentCustomer,SystemID,IsBulkUpload,CIF_FileName,Account_Type,Pin_Mailer) 
		(SELECT CIF_ID,CustomerName,NameOnCard,Bin_Prefix,AccountNo,AccountOpeningDate,CIF_Creation_Date
		,Address1,Address2,Address3 ,City,[State],PinCode,Country,Mothers_Name,DOB,CountryCode 
		,STDCode,MobileNo,EmailID,SCHEME_Code,BRANCH_Code,Entered_Date,Verified_Date,PAN_No,Mode_Of_Operation,
		Fourth_Line_Embossing,Issue_DebitCard,Aadhar_No,GETDATE(),0,'',0,0,0,0,0,0,@Batchno,0,'',0,@Bank,GETDATE(),'','','',1,SystemID,1,@FileName
		     /* Start [AGSCM-32] : Diksha Walunj: 29/09/2017 :Accounttype comes in SchemeCode Field   */
			-- ,Account_Type 
			,SCHEME_Code
			 /* End [AGSCM-32] : Diksha Walunj: 29/09/2017 :Accounttype comes in SchemeCode Field   */
		,Pin_Mailer
		from @CustBulkData)
				---------------------------------------------
	
		---- Validation for Card Processing
			Update tblCardProduction Set Rejected=1, Reason='Invalid Customer ID' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And  ((ISNUMERIC(ISNULL([CIF ID],''))=0)  OR (Not (Len(LTrim(RTrim(IsNull([CIF ID],'')))) Between 7 And 16))) And  Bank=@Bank 

			Update tblCardProduction Set Rejected=1, Reason='Invalid Customer Name' where ((RTRIM(LTRIM(REPLACE(Isnull([Customer Name],'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( ([Customer Name] like '%[^a-zA-Z0-9 ]%')) OR (LEN(RTRIM(LTRIM(Isnull([Customer Name],'0'))))>25) OR ((LEN(LTRIM(RTRIM([Customer Name])))=0))) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And  Bank=@Bank 

			Update tblCardProduction Set Rejected=1, Reason='Invalid Name on Card' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And (Not (Len(LTrim(RTrim(IsNull([Customer Preferred name],'')))) Between 3 And 26) Or([Customer Preferred name] Like '%[^a-zA-Z ]%')) And Bank=@Bank 

			Update C Set C.Rejected=1, C.Reason='Invalid Bin Prefix'
			FROM tblCardProduction C 
			LEFT JOIN TblBIN B ON C.[Card Type and Subtype]=B.CardPrefix AND C.Bank=B.BankID
			Where C.Bank=@Bank AND C.Rejected=0 And C.Processed=0 And C.[Skip]=0 AND (ISNULL(B.CardProgram,'')='')	
			AND ((ISNULL(C.[Scheme code],''))=(ISNULL(b.Switch_SchemeCode,'')))	

			Update tblCardProduction Set Rejected=1, Reason='Invalid Account No' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And  ((ISNUMERIC(ISNULL([AC ID],''))=0)  OR (Not (Len(LTrim(RTrim(IsNull([AC ID],'')))) Between 7 And 16))) And Bank=@Bank 

			update 	tblCardProduction Set Rejected=1, Reason='Invalid Account opening date' where (RTRIM(LTRIM(ISNULL([AC open date],'')))<>'')  AND ((ISNUMERIC(ISNULL([AC open date],''))=0)  OR ((Len(LTrim(RTrim(IsNull([AC open date],'')))))<>8)) And Bank=@Bank AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 -- account open date

	        update tblCardProduction  Set Rejected=1, Reason='Invalid CIF creation date' where (RTRIM(LTRIM(ISNULL([CIF Creation Date],'')))<>'')AND ((ISNUMERIC(ISNULL([CIF Creation Date],''))=0)  OR ((Len(LTrim(RTrim(IsNull([CIF Creation Date],'')))))<>8)) And Bank=@Bank AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0

			Update tblCardProduction Set Rejected=1, Reason='Invalid Address Line 1' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0  And ((RTRIM(LTRIM(Replace([Address Line 1],' ','')))  like '%[&.()\/@$#^<>?!`~_+=|% ]%')OR (LEN(RTRIM(LTRIM(Replace([Address Line 1],' ',''))))=0) OR (LEN(RTRIM(LTRIM([Address Line 1])))>50))  And Bank=@Bank 
	
			Update tblCardProduction Set Rejected=1, Reason='Invalid Address Line 2' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0  And ((RTRIM(LTRIM(Replace([Address Line 2],' ','')))  like '%[&.()\/@$#^<>?!`~_+=|% ]%') OR (LEN(RTRIM(LTRIM(Replace([Address Line 2],' ',''))))=0)OR (LEN(RTRIM(LTRIM([Address Line 2])))>50))  And Bank=@Bank 
	
			Update tblCardProduction Set Rejected=1, Reason='Invalid Address Line 3' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0  And ((RTRIM(LTRIM(Replace([Address Line 3],' ','')))  like '%[&.()\/@$#^<>?!`~_+=|% ]%') OR (LEN(RTRIM(LTRIM(Replace([Address Line 3],' ',''))))=0) OR (LEN(RTRIM(LTRIM([Address Line 3])))>50))  And Bank=@Bank 

			Update tblCardProduction Set Rejected=1, Reason='Address 2 & 3 Length Exceeds Limit' Where Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Len([Address Line 2] + [Address Line 3])>100 And Bank=@Bank 

			UPDATE tblCardProduction Set Rejected=1, Reason='Invalid City' where ((LEN(LTRIM(RTRIM(City)))=0)OR (RTRIM(LTRIM(Isnull(replace([City],' ',''),'0'))) LIKE '%[^a-zA-Z ]%') OR ([City] like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([City],'0'))))>40))AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank   --city

			UPDATE tblCardProduction Set Rejected=1, Reason='Invalid State'   where ((LEN(LTRIM(RTRIM(State)))=0)OR(RTRIM(LTRIM(Isnull([State],'0'))) LIKE '%[^a-zA-Z ]%') OR([State] Like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([State],'0'))))>20))AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank --state


			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Pin code'  where   ((ISNUMERIC(ISNULL([Pin Code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Pin Code],'')))))>6)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --pincode	
			
			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Country'  where ((LEN(RTRIM(LTRIM(Isnull([Country code],''))))=0)OR(RTRIM(LTRIM(Isnull([Country code],'0'))) LIKE '%[^a-zA-Z ]%') OR (LEN(RTRIM(LTRIM(Isnull([Country code],'0'))))>10)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank


			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Mothers Name'where (isnull([Mothers Maiden Name],'')<>'') AND ((RTRIM(LTRIM(Isnull([Mothers Maiden Name],'0'))) LIKE '%[^a-zA-Z ]%') OR (LEN(RTRIM(LTRIM(Isnull([Mothers Maiden Name],'0'))))>25)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank --mothers name

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Date of Birth' where  ((ISNUMERIC(ISNULL([DOB],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DOB],'')))))<>8) OR (dbo.FunCheckIsDate([DOB])=0)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- date of birth

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Country Code' where (ISNULL([Country Dial code],'')<>'') AND ((ISNUMERIC(ISNULL([Country Dial code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Country Dial code],'')))))>4))  AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank   -- country  code

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid STD Code'where (ISNULL([City Dial code],'')<>'') AND ((ISNUMERIC(ISNULL([City Dial code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([City Dial code],'')))))>4)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank 
			 
			 UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Mobile No' where ((RTRIM(LTRIM(ISNULL([Mobile phone number],''))) Like '%[^0-9]%') Or (Len(LTrim(RTrim(ISNULL([Mobile phone number],''))))<>10))AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank 

			 UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Email' where (Len(RTRIM(LTRIM(ISNULL([Email id],''))))>0) AND ((PatIndex('%;%',[Email id])>0) Or (PatIndex('%,%',[Email id])>0) Or (PatIndex('%@%.%',[Email id])=0) OR (LEN(RTRIM(LTRIM([Email id])))>50))AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank --mail

			  --UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Scheme Code' where (ISNULL([Scheme code],'')<>'')AND(Len(RTRIM(ltrim([Scheme code])))>8) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank   --scheme code
			Update C Set C.Rejected=1, C.Reason='Invalid Scheme Code'  FROM tblCardProduction C 
				LEFT JOIN TblBIN B ON ISNULL(C.[Scheme code],'')=ISNULL(B.Switch_SchemeCode,'') AND C.Bank=B.BankID AND RTRIM(LTRIM(c.[Card Type and Subtype]))=RTRIM(LTRIM(B.CardPrefix))
				Where C.Bank=@Bank AND C.Rejected=0 And C.Processed=0 And C.[Skip]=0 AND 
				 (ISNULL(B.CardProgram,'')='')	


			  UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Branch Code' where (Len(LTrim(RTrim(IsNull([Branch code],''))))<>4) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --branch code

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Entered Date' where (ISNULL([Entered date],'')<>'')AND (ISNULL([Entered date],'')<>'') AND((ISNUMERIC(ISNULL([Entered date],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Entered date],'')))))<>8)  OR ((dbo.FunCheckIsDate([Entered date])=0))) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- entered date

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Verified Date' where (ISNULL([Verified Date],'')<>'') AND ((ISNUMERIC(ISNULL([Verified Date],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Verified Date],'')))))<>8)  OR ( (dbo.FunCheckIsDate([Verified Date])=0))) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- verified date

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid PAN Number' where (ISNULL([PAN Number],'')<>'') AND (Len(LTrim(RTrim(IsNull([PAN Number],''))))>12) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --pan

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Mode of Operation' where (ISNULL([Mode Of Operation],'')<>'') AND ((ISNUMERIC(ISNULL([Mode Of Operation],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Mode Of Operation],'')))))<>2)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- mode of operation

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Fourth line embossing ' where (ISNULL([Fourth Line Embossing],'')<>'') AND(( (LTrim(RTrim(IsNull([Fourth Line Embossing],''))) like '%[&.()\/@$#^<>?!`~_+=|% ]%') OR (Len(LTrim(RTrim(IsNull([Fourth Line Embossing],''))))>20))) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- 4 th line embos

			 UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Debit card linkage flag' where ((Len(LTrim(RTrim(IsNull([Debit Card Linkage Flag],''))))<>1) OR (RTRIM(LTRIM(Isnull([Debit Card Linkage Flag],'0'))) LIKE '%[^a-zA-Z ]%') OR (RTRIM(LTRIM([Debit Card Linkage Flag]))not in(UPPER('Y'),UPPER('N')) ))  AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --debit card flag
 
			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Pin mailer' where  (ISNULL(Pin_Mailer,'')<>'') AND ((ISNUMERIC(ISNULL([Pin_Mailer],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Pin_Mailer],'')))))<>2)) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  -- pin mailer 

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Account Type' where  ((ISNUMERIC(ISNULL([Account_Type],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Account_Type],'')))))<>2))  AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --account type

			UPDATE tblCardProduction Set  Rejected=1, Reason='Invalid Adhar Number' where (ISNULL(Aadhaar,'')<>'') AND (Len(LTrim(RTrim(IsNull([Aadhaar],''))))>19) AND Rejected=0 And Processed=0 And [Skip]=0 And AddOnCards=0 And Bank=@Bank  --adhar
		
				---- for already customer is processed for card generation
			Update C Set C.Rejected=1, C.Reason='Duplicate card request'
			FROM tblCardProduction C 
			INNER JOIN TblAuthorizedCardLog  AC ON C.[CIF ID]=AC.[CIF ID] AND C.[AC ID]=AC.[AC ID]
			Where C.Rejected=0 And C.Processed=0 And C.[Skip]=0 And C.AddOnCards=0 And C.Bank=@Bank				

			Update tblCardProduction Set Processed=1, ProcessedOn=GETDATE(),IsAuthorised=1,AuthorisedBy=0  Where Rejected=0 AND  Processed=0 And BatchNo=@Batchno  And Bank=@Bank 

			END
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
	SELECT CIF_FileName,ISNULL([CIF ID],'')+'|'+ISNULL([Customer Name],'')+'|'+ISNULL([Customer Preferred name],'')+'|'+ISNULL([Card Type and Subtype],'')+'|'+ISNULL([AC ID],'')
	+'|'+ISNULL([AC open date],'')+'|'+ISNULL([CIF Creation Date],'')+'|'+ISNULL([Address Line 1],'')+'|'+ISNULL([Address Line 2],'')+'|'+ISNULL([Address Line 3],'')+'|'+ISNULL(City,'')
	+'|'+ISNULL([State],'')+'|'+ISNULL([Pin Code],'')+'|'+ISNULL([Country code],'')+'|'+ISNULL([Mothers Maiden Name],'')+'|'+ISNULL(DOB,'')
	+'|'+ISNULL([Country Dial code],'')+'|'+ISNULL([City Dial code],'')+'|'+ISNULL([Mobile phone number],'')+'|'+ISNULL([Email id],'')+'|'+ISNULL([Scheme code],'')
	+'|'+ISNULL([Branch code],'')+'|'+ISNULL([Entered date],'')+'|'+isnull([Verified Date],'')+'|'+ISNULL([PAN Number],'')+'|'+ISNULL([Mode Of Operation],'')
	+'|'+ISNULL([Fourth Line Embossing],'')+'|'+ISNULL(Aadhaar,'')+'|'+ISNULL([Debit Card Linkage Flag],'')+'|'+ISNULL(Pin_Mailer,'')+'|'+ISNULL(Reason,'') AS [Result]
	 from tblCardProduction WITH(NOLOCK) WHERE Rejected=1	AND BatchNo=@Batchno AND Bank=@Bank AND CIF_FileName=@FileName
	 END
END



GO
