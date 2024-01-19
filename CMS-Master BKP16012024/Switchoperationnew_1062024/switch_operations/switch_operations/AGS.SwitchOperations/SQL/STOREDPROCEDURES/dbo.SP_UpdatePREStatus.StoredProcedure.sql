USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePREStatus]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_UpdatePREStatus]
		@PREData PRERecordsType READONLY,
		@IssuerNo Varchar(200),
		@CardProgram VARCHAR(200)
AS
BEGIN
 Begin Transaction  
 Begin Try   

	CREATE Table #TempPREData
	(
	   [CustID] [varchar](200) NULL,
		[CardProgram] [varchar](200) NULL,
		[AccountNo] [varchar](200) NULL	
	)

	INSERT INTO #TempPREData (CustID,CardProgram,AccountNo) SELECT CustID,CardProgram,AccountNo FROM @PREData

	UPDATE AC SET AC.PREStatus=1
	 FROM TblAuthorizedCardLog AC  WITH(NOLOCK)
		 INNER JOIN #TempPREData t ON((SUBSTRING(AC.[CIF ID], PATINDEX('%[^0]%', AC.[CIF ID]+'.'), LEN(AC.[CIF ID]))) collate Latin1_General_CI_AI=(SUBSTRING(t.CustID , PATINDEX('%[^0]%', t.CustID +'.'), LEN(t.CustID )) collate Latin1_General_CI_AI
		 ))
		 AND (SUBSTRING(AC.[AC ID], PATINDEX('%[^0]%', AC.[AC ID]+'.'), LEN(AC.[AC ID]))) collate Latin1_General_CI_AI=(SUBSTRING(t.AccountNo , PATINDEX('%[^0]%', t.AccountNo +'.'), LEN(t.AccountNo))) collate Latin1_General_CI_AI
		 INNER JOIN TblBIN  B WITH(NOLOCK) ON B.CardProgram collate Latin1_General_CI_AI=t.CardProgram collate Latin1_General_CI_AI AND AC.[Card Type and Subtype] collate Latin1_General_CI_AI =B.CardPrefix 
		 INNER JOIN TblBanks BA WITH(NOLOCK) ON AC.Bank=BA.ID
    where B.CardProgram=@CardProgram  AND BA.BankCode=@IssuerNo AND ISNULL(AC.cardGenStatus,0)=1

	--To Intimate Card production team on mail
	Begin Try
		Declare @StrOutPutCode Varchar(3) ,@StrOutputDesc Varchar(2000),@StrbankName varchar(50),@RCVREmailID Nvarchar(1000),@RCVOutputPath Nvarchar(1000),@RCVOutputFileNM Nvarchar(1000)
		Select @StrbankName=Bank,@RCVREmailID=RCVREmailID From TblCardautomation with(Nolock) Where IssuerNo=@IssuerNo
		If(@IssuerNo=1)
		Begin
			Set @RCVOutputPath='\LiveCardAutomation\RBL\Output\BackUp';
			Set @RCVOutputFileNM='Output.txt';
		END
		exec [Sp_CardAutomationSendEmail] @StrbankName,@CardProgram,@RCVOutputFileNM,@RCVOutputPath,@RCVREmailID,@StrOutPutCode Output,@StrOutputDesc OutPut
		--Select @StrOutPutCode,@StrOutputDesc
	END Try
	Begin Catch
	End Catch

	--To Set the CardStatus
	Update Cust Set Cust.ISCardSuccess=1
	From TblCustomersDetails Cust With(nolock)
	Inner Join TblAuthorizedCardlog AuthCard With(Nolock) On Cust.BankCustID collate Latin1_General_CI_AI =AuthCard.[CIF ID] collate Latin1_General_CI_AI
	Inner Join TblBanks Banks With(Nolock) On  AuthCard.Bank=Banks.[ID]
	where ISnull(Cust.ISCardSuccess,0)=0 And Isnull(AuthCard.CardGenStatus,0)=1 And Isnull(AuthCard.PREStatus,0)=1	And BankCode=@IssuerNo 

	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@CardProgram='+@CardProgram,@IssuerNo	  
		   
	END CATCH;  	

	Drop table #TempPREData
END

GO
