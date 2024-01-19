USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_CIFValidation_Malda_DBCC]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_CIFValidation_Malda_DBCC]
	
AS
BEGIN

	
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Records Missing column' Where Rejected=0 And Processed=0 And  (((ISNULL([PIN_MAILER],''))='Error In Record')  OR (((ISNULL(PGKValue,''))='Error In Record'))) 
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer ID' Where Rejected=0 And Processed=0 And  ((ISNUMERIC(ISNULL([CIFID],''))=0)  OR (Not (Len(LTrim(RTrim(IsNull([CIFID],'')))) Between 7 And 16))) 
			Update TblCustomerDataModification Set Rejected=1, Reason='Invalid Customer Name' where ((RTRIM(LTRIM(REPLACE(Isnull([Customer Name],'0'),' ',''))) LIKE '%[^a-zA-Z ]%')OR( ([Customer Name] like '%[^a-zA-Z0-9 ]%')) OR (LEN(RTRIM(LTRIM(Isnull([Customer Name],'0'))))>25) 
			OR ((LEN(LTRIM(RTRIM([Customer Name])))=0))) AND Rejected=0 And Processed=0 
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Date' 
			where ((ISNUMERIC(ISNULL([DATE],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DATE],'')))))<>8) OR (dbo.FunCheckIsDate([DATE])=0)) AND Rejected=0 And Processed=0  
            UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid EMAIL' where (Len(RTRIM(LTRIM(ISNULL([EMAIL],''))))>0) AND ((PatIndex('%;%',[EMAIL])>0) Or (PatIndex('%,%',[EMAIL])>0) Or (PatIndex('%@%.%',[EMAIL])=0) OR (LEN(RTRIM(LTRIM([EMAIL])))>50) )AND Rejected=0 And Processed=0   --mail
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Mobile No' where ((RTRIM(LTRIM(ISNULL([Mobile_number],''))) Like '%[^0-9]%') Or (Len(LTrim(RTrim(ISNULL([Mobile_number],''))))<>10))AND Rejected=0 And Processed=0   
			UPDATE TblCustomerDataModification Set  Rejected=1, Reason='Invalid Date of Birth' where
			  ((ISNUMERIC(ISNULL([DOB],''))=0)  OR ((Len(LTrim(RTrim(IsNull([DOB],'')))))<>8) OR (dbo.FunCheckIsDate([DOB])=0)) AND Rejected=0 And Processed=0    -- date of birth
  
END


GO
