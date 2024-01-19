USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_CIFValidation_Utkarsh]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_CIFValidation_Utkarsh]
AS
BEGIN

	
	
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Records Missing column' Where Rejected=0 And Processed=0 And  (((ISNULL([PIN_MAILER],''))='Error In Record')  OR (((ISNULL(PGKValue,''))='Error In Record'))) 
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer ID' Where Rejected=0 And Processed=0 And  ((ISNUMERIC(ISNULL([CIFID],''))=0)  OR (Not (Len(LTrim(RTrim(IsNull([CIFID],'')))) Between 7 And 16))) 
            Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 1' Where Rejected=0 And Processed=0   And ((RTRIM(LTRIM(Replace([Add 1],' ','')))  like '%[^a-zA-Z0-9 /\-]%') OR (LEN(RTRIM(LTRIM(Replace([Add 1],' ',''))))=0) OR (LEN(RTRIM(LTRIM([Add 1])))>51))   
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 2' Where Rejected=0 And Processed=0   And ((RTRIM(LTRIM(Replace([Add 2],' ','')))  like '%[^a-zA-Z0-9 /\-]%') OR (LEN(RTRIM(LTRIM(Replace([Add 2],' ',''))))=0)OR (LEN(RTRIM(LTRIM([Add 2])))>51))   
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 3' Where Rejected=0 And Processed=0   And ((RTRIM(LTRIM(Replace([Add 3],' ','')))  like '%[^a-zA-Z0-9 /\-]%') OR (LEN(RTRIM(LTRIM([Add 3])))>51))  
			

				
			Update TblCustomerDataModification Set Rejected=1, Reason='Address 2 & 3 Length Exceeds Limit' Where Rejected=0 And Processed=0  And Len([Add 2] + [Add 3])>100  
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid City' where ((LEN(LTRIM(RTRIM(City)))=0)OR (RTRIM(LTRIM(Isnull(replace([City],' ',''),'0'))) LIKE '%[^a-zA-Z ]%') OR ([City] like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([City],'0'))))>40))AND Rejected=0 And Processed=0
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid State'   where ((LEN(LTRIM(RTRIM(State)))=0)OR(RTRIM(LTRIM(Isnull([State],'0'))) LIKE '%[^a-zA-Z ]%') OR([State] Like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([State],'0'))))>20))AND Rejected=0 And Processed=0   --state
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid PinCode'  where   ((ISNUMERIC(ISNULL([PinCode],''))=0)  OR ((Len(LTrim(RTrim(IsNull([PinCode],'')))))>6)) AND Rejected=0 And Processed=0    --pincode	
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Country'  where ((LEN(RTRIM(LTRIM(Isnull([Country],''))))=0)OR(RTRIM(LTRIM(Isnull([Country],'0'))) LIKE '%[^a-zA-Z ]%') OR (LEN(RTRIM(LTRIM(Isnull([Country],'0'))))>10)) AND Rejected=0 And Processed=0  
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer Name' where ((RTRIM(LTRIM(REPLACE(Isnull([Customer Name],'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( ([Customer Name] like '%[^a-zA-Z0-9 ]%')) OR (LEN(RTRIM(LTRIM(Isnull([Customer Name],'0'))))>25) 
			OR ((LEN(LTRIM(RTRIM([Customer Name])))=0))) AND Rejected=0 And Processed=0 
		    Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Name on Card' Where Rejected=0 And Processed=0  And (Not (Len(LTrim(RTrim(IsNull([NAME ON CARD],'')))) Between 3 And 26) Or([NAME ON CARD] Like '%[^a-zA-Z ]%'))  
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Date' 
			where ((ISNUMERIC(ISNULL([DATE],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DATE],'')))))<>8) OR (dbo.FunCheckIsDate([DATE])=0)) AND Rejected=0 And Processed=0   
		    --UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Adhar Number' where ((ISNULL(DATE,'')<>'') AND (Len(LTrim(RTrim(IsNull(DATE,''))))>8) OR(RTRIM(LTRIM(ISNULL(DATE,''))) Like '%[^0-9]%')) AND Rejected=0 And Processed=0  
            UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid EMAIL' where (Len(RTRIM(LTRIM(ISNULL([EMAIL],''))))>0) AND ((PatIndex('%;%',[EMAIL])>0) Or (PatIndex('%,%',[EMAIL])>0) Or (PatIndex('%@%.%',[EMAIL])=0) OR (LEN(RTRIM(LTRIM([EMAIL])))>50) )AND Rejected=0 And Processed=0   --mail
     		UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Country Dial Code' 
			where --(ISNULL([Country_Dial_code],'')<>'') AND 
			((ISNUMERIC(ISNULL([Country_Dial_code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Country_Dial_code],'')))))>4))  AND Rejected=0 And Processed=0     -- country  code
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid City dial Code'
			where --(ISNULL([City_Dial_code],'')<>'') AND 
			((ISNUMERIC(ISNULL([City_Dial_code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([City_Dial_code],'')))))>4)) AND Rejected=0 And Processed=0   	 
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Mobile No' where ((RTRIM(LTRIM(ISNULL([Mobile_number],''))) Like '%[^0-9]%') Or (Len(LTrim(RTrim(ISNULL([Mobile_number],''))))<>10))AND Rejected=0 And Processed=0   
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Date of Birth' where
			  ((ISNUMERIC(ISNULL([DOB],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DOB],'')))))<>8) OR (dbo.FunCheckIsDate([DOB])=0)) AND Rejected=0 And Processed=0    -- date of birth
   --         UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid PAN' 
			--where --(ISNULL([PAN],'')<>'') AND 
			--( (Len(LTrim(RTrim(IsNull([PAN],''))))>12) or (Len(LTrim(RTrim(IsNull([PAN],''))))=0)) AND Rejected=0 And Processed=0    --pan
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Fourth line embossing ' where
			---- (ISNULL([Fourth Line Embossing],'')<>'') AND
			--(
			--( (LTrim(RTrim(IsNull([Fourth Line Embossing],''))) like '%[&.()\/@$#^<>?!`~_+=|%]%') 
			--OR (Len(LTrim(RTrim(IsNull([Fourth Line Embossing],''))))>20) 
			--OR (Len(LTrim(RTrim(IsNull([Fourth Line Embossing],''))))=0) 
			--)) AND Rejected=0 And Processed=0    -- 4 th line embos
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Adhar Number' 
			--where-- ((ISNULL(AADHAR_NO,'')<>'') AND 
			--((Len(LTrim(RTrim(IsNull([AADHAR_NO],''))))>19)or Len(LTrim(RTrim(IsNull([AADHAR_NO],''))))=0 OR(RTRIM(LTRIM(ISNULL([AADHAR_NO],''))) Like '%[^0-9]%')) AND Rejected=0 And Processed=0  
			--/*PGKValue Change*/

			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Pin mailer'  where 
			----  (( ((Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))<>0) and (ISNUMERIC(ISNULL(PIN_MAILER,''))=0))  OR not (((Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))=0) or (Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))=2))
			-- not IsNull(PIN_MAILER,'') in ('03','02','01')
			--   AND Rejected=0 And Processed=0    --pincode	

	
END



GO
