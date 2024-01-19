USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[CustomerDataValidation_Prabhu]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--[dbo].[usp_InsertCustomerDataModificationRequest] 
Create PROCEDURE [dbo].[CustomerDataValidation_Prabhu]
	
AS
BEGIN
		
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Records Missing column'		Where Rejected=0 And Processed=0 And  (((ISNULL([PIN_MAILER],''))='Error In Record')  OR (((ISNULL(PGKValue,''))='Error In Record'))) 
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer ID'					Where Rejected=0 And Processed=0 And  (((ISNULL([CIFID],''))='')  OR (Not (Len(LTrim(RTrim(IsNull([CIFID],'')))) Between 7 And 20))) 
            Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 1'						Where Rejected=0 And Processed=0 And ((RTRIM(LTRIM(Replace([Add 1],' ','')))  like '%[&.,()\/@$#^<>?!`~_+=|% ]%') OR (LEN(RTRIM(LTRIM(Replace([Add 1],' ',''))))=0) OR (LEN(RTRIM(LTRIM([Add 1])))>50))   
            Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 2'						Where Rejected=0 And Processed=0 And ((RTRIM(LTRIM(Replace([Add 2],' ','')))  like '%[&.,()\/@$#^<>?!`~_+=|% ]%') OR (LEN(RTRIM(LTRIM(Replace([Add 2],' ',''))))=0)OR (LEN(RTRIM(LTRIM([Add 2])))>50))   
		    Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Add 3'						Where Rejected=0 And Processed=0 And ((RTRIM(LTRIM(Replace([Add 3],' ','')))  like '%[&.,()\/@$#^<>?!`~_+=|% ]%') OR (LEN(RTRIM(LTRIM([Add 3])))>50))  
			Update TblCustomerDataModification Set Rejected=1, Reason='Address 2 & 3 Length Exceeds Limit'	Where Rejected=0 And Processed=0 And Len([Add 2] + [Add 3])>100  
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid City'						Where Rejected=0 And Processed=0 And ((LEN(LTRIM(RTRIM(City)))=0)OR (RTRIM(LTRIM(Isnull(replace([City],' ',''),'0'))) LIKE '%[^a-zA-Z ]%') OR ([City] like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([City],'0'))))>40))
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid State'						Where Rejected=0 And Processed=0 And ((LEN(LTRIM(RTRIM(State)))=0)OR(RTRIM(LTRIM(Isnull([State],'0'))) LIKE '%[^a-zA-Z ]%') OR([State] Like '%[^a-zA-Z0-9 ]%') OR (LEN(RTRIM(LTRIM(Isnull([State],'0'))))>20))
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid PinCode'						Where Rejected=0 And Processed=0 And ((ISNUMERIC(ISNULL([PinCode],''))=0)  OR ((Len(LTrim(RTrim(IsNull([PinCode],'')))))>6)) 
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid Country'						Where Rejected=0 And Processed=0 And((LEN(RTRIM(LTRIM(Isnull([Country],''))))=0)OR(RTRIM(LTRIM(Isnull([Country],'0'))) LIKE '%[^a-zA-Z ]%') OR (LEN(RTRIM(LTRIM(Isnull([Country],'0'))))>10)) 
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer Name'				Where Rejected=0 And Processed=0 And((RTRIM(LTRIM(REPLACE(Isnull([Customer Name],'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( ([Customer Name] like '%[^a-zA-Z0-9 ]%')) OR (LEN(RTRIM(LTRIM(Isnull([Customer Name],'0'))))>25) OR ((LEN(LTRIM(RTRIM([Customer Name])))=0)))
		    Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Name on Card'				Where Rejected=0 And Processed=0 And (Not (Len(LTrim(RTrim(IsNull([NAME ON CARD],'')))) Between 3 And 26) Or([NAME ON CARD] Like '%[^a-zA-Z ]%'))  
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid Date'						Where Rejected=0 And Processed=0 And((ISNUMERIC(ISNULL([DATE],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DATE],'')))))<>8) OR (dbo.FunCheckIsDate([DATE])=0)) 
		    UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid EMAIL'						Where Rejected=0 And Processed=0 And(Len(RTRIM(LTRIM(ISNULL([EMAIL],''))))>0) AND ((PatIndex('%;%',[EMAIL])>0) Or (PatIndex('%,%',[EMAIL])>0) Or (PatIndex('%@%.%',[EMAIL])=0) OR (LEN(RTRIM(LTRIM([EMAIL])))>50))
     		UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid Mobile No'					Where Rejected=0 And Processed=0 And((RTRIM(LTRIM(ISNULL([Mobile_number],''))) Like '%[^0-9]%') Or (Len(LTrim(RTrim(ISNULL([Mobile_number],''))))<>10))
			UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid Date of Birth'				Where Rejected=0 And Processed=0 And((ISNUMERIC(ISNULL([DOB],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DOB],'')))))<>8) OR (dbo.FunCheckIsDate([DOB])=0)) 
            UPDATE TblCustomerDataModification Set Rejected=1, Reason='Invalid PAN'						Where Rejected=0 And Processed=0 And(ISNULL([PAN],'')<>'') AND 	((Len(LTrim(RTrim(IsNull([PAN],''))))>20) or (Len(LTrim(RTrim(IsNull([PAN],''))))=0)) 
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Fourth line embossing ' where (ISNULL([Fourth Line Embossing],'')<>'') AND (( (LTrim(RTrim(IsNull([Fourth Line Embossing],''))) like '%[&.()\/@$#^<>?!`~_+=|%]%') OR (Len(LTrim(RTrim(IsNull([Fourth Line Embossing],''))))>20) OR (Len(LTrim(RTrim(IsNull([Fourth Line Embossing],''))))=0) )) AND Rejected=0 And Processed=0    -- 4 th line embos
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Adhar Number' where-- ((ISNULL(AADHAR_NO,'')<>'') AND ((Len(LTrim(RTrim(IsNull([AADHAR_NO],''))))>19)or Len(LTrim(RTrim(IsNull([AADHAR_NO],''))))=0 OR(RTRIM(LTRIM(ISNULL([AADHAR_NO],''))) Like '%[^0-9]%')) AND Rejected=0 And Processed=0  
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Pin mailer'  where    ((((Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))<>0) and (ISNUMERIC(ISNULL(PIN_MAILER,''))=0))  OR not (((Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))=0) or (Len(LTrim(RTrim(IsNull(PIN_MAILER,'')))))=2)) not IsNull(PIN_MAILER,'') in ('03','02','01') AND Rejected=0 And Processed=0
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Country Dial Code' where (ISNULL([Country_Dial_code],'')<>'') AND ((ISNUMERIC(ISNULL([Country_Dial_code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([Country_Dial_code],'')))))>4))  AND Rejected=0 And Processed=0
			--UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid City dial Code'    where (ISNULL([City_Dial_code],'')<>'') AND    ((ISNUMERIC(ISNULL([City_Dial_code],''))=0)  OR ((Len(LTrim(RTrim(IsNull([City_Dial_code],'')))))>4)) AND Rejected=0 And Processed=0   	 
            UPDATE TblCustomerDataModification SET Processed=1 , Processed_Date=GETDATE() WHERE Processed=0
				
			
			
		    
		    

	
END


GO
