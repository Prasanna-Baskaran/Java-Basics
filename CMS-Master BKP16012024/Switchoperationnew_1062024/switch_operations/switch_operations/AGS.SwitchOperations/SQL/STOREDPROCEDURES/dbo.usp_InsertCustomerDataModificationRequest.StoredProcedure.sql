USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCustomerDataModificationRequest]    Script Date: 08-06-2018 16:58:18 ******/
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
CREATE PROCEDURE [dbo].[usp_InsertCustomerDataModificationRequest]
@DataTable dbo.[CustomerdataModificationType] READONLY,
@Filename VARCHAR (MAX),
@BANK INT
	
AS
BEGIN

   DECLARE @Validationsp varchar (max)
   select @Validationsp=DataValidation_SP from tblmasconfiguration where  BankId=@BANK

	INSERT INTO TblCustomerDataModification (Cifid,[Add 1],[Add 2],[Add 3],[City],[State],[Pincode],[Country],[CUSTOMER NAME],[NAME ON CARD],
	[DATE],[EMAIL],	[COUNTRY_DIAL_CODE],[CITY_DIAL_CODE],[MOBILE_NUMBER],[DOB],[PAN],[FOURTH LINE EMBOSSING],[AADHAR_NO],[PIN_MAILER],
	[FileName],Processed,[Rejected],[Updated],[Requested_Date],[Processed_Date],[Updated_Date],[BANK],PGKValue)
	(SELECT   CIFID,ADD1,ADD2,ADD3,City,State,Pincode,Country,[CUSTOMER_NAME],[NAME_ON_CARD],[DATE],[EMAILID],[COUNTRY_DIAL_CODE],[CITY_DIAL_CODE],[MOBILE_NUMBER],[DOB],[PAN],[FOURTH LINE EMBOSSING],[AADHAR_NO],
	[PIN_MAILER],@Filename,0,0,0,GETDATE(),NULL,NULL,@BANK,ISNULL(PGKValue,'') FROM @DataTable)
	
	EXEC @Validationsp 			
	UPDATE TblCustomerDataModification SET Processed=1 , Processed_Date=GETDATE() WHERE Processed=0
    SELECT Cifid,[Add 1],[Add 2],[Add 3],[City],[State],[Pincode],[Country],[CUSTOMER NAME],[NAME ON CARD],[DATE],[EMAIL],[COUNTRY_DIAL_CODE],[CITY_DIAL_CODE],[MOBILE_NUMBER],[DOB],[PAN],[FOURTH LINE EMBOSSING],[AADHAR_NO],[PIN_MAILER],[FileName],Reason,Processed_Date  FROM TblCustomerDataModification WHERE FILENAME=@Filename AND Rejected=1

	
END


GO
