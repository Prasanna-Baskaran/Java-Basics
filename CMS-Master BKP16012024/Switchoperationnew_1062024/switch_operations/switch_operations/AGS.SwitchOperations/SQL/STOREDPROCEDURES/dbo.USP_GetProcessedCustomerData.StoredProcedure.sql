USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetProcessedCustomerData]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from [temp_po-old].dbo.cardproductionrenewal
--select * from  TblCardGenRequest
CREATE procedure [dbo].[USP_GetProcessedCustomerData]  --exec [USP_GetProcessedCustomerData] 'CIF UPDATION .txt','2018-03-31'
(
	@FileName varchar (max),
	@Datetime Datetime
	
)
as
BEGIN
SELECT Cifid,[Add 1],[Add 2],[Add 3],City,State,Pincode,Country,[CUSTOMER NAME],[NAME ON CARD],DATE,EMAIL,COUNTRY_DIAL_CODE,CITY_DIAL_CODE,MOBILE_NUMBER,DOB,PAN,[FOURTH LINE EMBOSSING],AADHAR_NO,PIN_MAILER,FileName,Updated_Date,'Processed'[ReportType] 
FROM TblCustomerDataModification where fileName=@FileName and updated=1 and processed=1 and rejected=0 and Requested_Date>@Datetime and switchrespCode='00'


SELECT Cifid,[Add 1],[Add 2],[Add 3],City,State,Pincode,Country,[CUSTOMER NAME],[NAME ON CARD],DATE,EMAIL,COUNTRY_DIAL_CODE,CITY_DIAL_CODE,MOBILE_NUMBER,DOB,PAN,[FOURTH LINE EMBOSSING],AADHAR_NO,PIN_MAILER,FileName,b.Description,Processed_Date,'Rejected'[ReportType] 
FROM TblCustomerDataModification a with (nolock)
left join TblResponse b  with (nolock) on a.switchrespCode=b.Code
 where fileName=@FileName and updated=1 and processed=1 and rejected=0 and Requested_Date>@Datetime and switchrespCode!='00'

Union all

	    SELECT case when Cifid='Error In Record' then '' else Cifid end,
	    case when [Add 1]='Error In Record' then '' else [Add 1] end [Add 1],
	    case when [Add 2]='Error In Record' then '' else [Add 2] end [Add 2],
	    case when [Add 3]='Error In Record' then '' else [Add 3] end [Add 3],
	    case when [city]='Error In Record' then '' else  [city]  end [City],
	    case when [State]='Error In Record' then '' else [State] end [State],
	    case when [Pincode]='Error In Record' then '' else [Pincode] end[Pincode],
	    case when [Country]='Error In Record' then '' else [Country] end[Country],
	    case when [CUSTOMER NAME]='Error In Record' then '' else [CUSTOMER NAME] end[CUSTOMER NAME],
	    case when [NAME ON CARD]='Error In Record' then '' else [NAME ON CARD] end[NAME ON CARD],
	    case when [DATE]='Error In Record' then '' else [DATE] end[DATE],
	    case when [EMAIL]='Error In Record' then '' else [EMAIL] end[EMAIL],
	    case when [COUNTRY_DIAL_CODE]='Error In Record' then '' else [COUNTRY_DIAL_CODE] end[COUNTRY_DIAL_CODE],
	    case when [CITY_DIAL_CODE]='Error In Record' then '' else [CITY_DIAL_CODE] end[CITY_DIAL_CODE],
	    case when [MOBILE_NUMBER]='Error In Record' then '' else [MOBILE_NUMBER] end[MOBILE_NUMBER],
	    case when [DOB]='Error In Record' then '' else [DOB] end[DOB],
	    case when [PAN]='Error In Record' then '' else [PAN] end[PAN],
	    case when [FOURTH LINE EMBOSSING]='Error In Record' then '' else [FOURTH LINE EMBOSSING] end[FOURTH LINE EMBOSSING],
	    case when [AADHAR_NO]='Error In Record' then '' else [AADHAR_NO] end[AADHAR_NO],
	    case when [PIN_MAILER]='Error In Record' then '' else [PIN_MAILER] end[PIN_MAILER],[FileName],Reason,Processed_Date  ,'Rejected'[ReportType]
	    FROM TblCustomerDataModification WHERE FILENAME=@Filename AND Rejected=1

END


GO
