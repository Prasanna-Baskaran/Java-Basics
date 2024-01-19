USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertCardRePINRequest]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================
-- Author:		<Author,,Gufran KHAN>
-- Create date: <Create Date,,08-01-2018>
-- Description:	<Description,,To Insert The REPIN Request>
-- =============================================
Create PROCEDURE [dbo].[usp_InsertCardRePINRequest]
@DataTable dbo.[CardRePIN] READONLY,
@Filename VARCHAR (MAX),
@BANK INT
	
AS
BEGIN
	INSERT INTO CardRePINRequest (cardNo,CIFID,AccountNo,bankid,fileName,RequestedDate,rejected,processed,reason,rejectedDate,ProcessedDate,updated,updateddate)
	(SELECT dbo.ufn_EncryptPAN(CardNO),CIFID,AccountNO,@BANK,@Filename,GETDATE(),0,0,null,null,NULL,0,null FROM @DataTable)
	/*Validation*/
	/*START*/
	UPDATE  CardRePINRequest Set Rejected=1, Reason='Invalid Records Missing column' Where Rejected=0 And Processed=0 And  (((ISNULL(CIFID,''))='Error In Record')  OR (((ISNULL(AccountNo,''))='Error In Record')) OR (((ISNULL(dbo.ufn_DecryptPAN(cardNo),''))='Error In Record')) ) 
	UPDATE  CardRePINRequest Set Rejected=1, Reason='InValid CIF ID' Where Rejected=0 And Processed=0 and ISNULL(CIFID,'')='' AND ISNULL(dbo.ufn_DecryptPAN(cardNo),'')=''
	UPDATE  CardRePINRequest Set Rejected=1, Reason='InValid Account NO' Where Rejected=0 And Processed=0 and ISNULL(AccountNo,'')='' AND ISNULL(dbo.ufn_DecryptPAN(cardNo),'')=''
	
	
	/*END*/
	
	
         	--Update CardRePINRequest Set Rejected=1, Reason='No Card Found' Where Rejected=0 And Processed=0 
		    UPDATE CardRePINRequest SET Processed=1 , ProcessedDate=GETDATE() WHERE Processed=0
		    --SELECT dbo.ufn_DecryptPAN(cardNo),[FileName],Reason  FROM CardRePINRequest WHERE FILENAME=@Filename AND Rejected=1

	
END

GO
