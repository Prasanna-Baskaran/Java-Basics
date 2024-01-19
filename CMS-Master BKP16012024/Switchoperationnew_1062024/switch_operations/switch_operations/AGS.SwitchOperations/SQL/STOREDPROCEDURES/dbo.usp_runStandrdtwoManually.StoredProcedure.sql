USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_runStandrdtwoManually]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_runStandrdtwoManually] --exec [usp_runStandrdtwoManually] 55,'VisaDebitIntl'
@IntIssuerNo int,
@CACardProgram varchar(50)
as
begin

Declare @StrOutPutCode Varchar(3) ,@StrOutputDesc Varchar(2000) 
	Declare	@SwitchInstitutionID varchar(11), @Standard2Batch varchar(100),@Standard2ExePath varchar(Max),@Standard2Input varchar(Max),@Standard2Archive varchar(Max),@Standard4Batch varchar(100),@Standard4ExePath varchar(Max),@Standard4Output  varchar(Max),@Standard3Batch varchar(100)
	
	DECLARE @CustAcctFileUpldBatchPath VARCHAR(max),@CardFileUpldBatchPath VARCHAR(max),@GetCardOutputBatchPath VARCHAR(max),@BatchFilePath VARCHAR(MAX),@CustAcctfileInputPath VARCHAR(MAX),@CustAcctfileDestPath VARCHAR(MAX)	
		 ,@CardfileInputPath VARCHAR(MAX),@CardfileDestPath VARCHAR(MAX),@OutCardFile_InputPath Varchar(800),@OutCardFile_DestPath Varchar(800),@OutCardFile_DestPathBasicPath Varchar(800),
		 @WinSCPExePath VARCHAR(800),@WinSCP_User VARCHAR(200),@WinSCP_PWD VARCHAR(200),@WinSCP_IP VARCHAR(100),@WinSCP_Port VARCHAR(20),
		 @WinSCP_LogPath VARCHAR(800),@OutCardFile_BackUp_Path VARCHAR(800),@CardOutputpath VARCHAR(800),@CardErrorpath VARCHAR(800),
		 @LogFilePath VARCHAR(800),@BankName Varchar(100),@GetFileSizeBatchPath Varchar(2000)
	--Assigning Value
	--Start Sheetal
	declare @IsAccountBalanceFile bit
	select @IsAccountBalanceFile=IsAccountBalanceFile from tblbanks where Bankcode=55
	--end sheetal
			Declare @OutputFileName varchar(30)='',@Std2ProcessType char(2)='1'

	SELECT @SwitchInstitutionID=SwitchInstitutionID,@Standard2Batch=[Standard2Batch],@Standard2ExePath=[Standard2ExePath],@Standard2Input=Standard2Input,@Standard2Archive=Standard2Archive,
		   @Standard4Batch=[Standard4Batch],@Standard4ExePath=[Standard4ExePath],@Standard4Output=Standard4Output,
		   @CustAcctFileUpldBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(CustAcctFileUploadBatch)),
		   @CardFileUpldBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(CardFileUploadBatch)),
		   @BatchFilePath=RTRIM(LTRIM(BatchFilesPath)),@GetCardOutputBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(GetCardOutputBatch))
		  ,@CustAcctfileInputPath=RTRIM(LTRIM(CustAcctfileInputPath)),@CustAcctfileDestPath=(RTRIM(LTRIM(Standard2Input))+'\')
		 ,@CardfileInputPath=RTRIM(LTRIM(CardfileInputPath)),@CardfileDestPath=RTRIM(LTRIM(CardfileDestPath))
		 ,@OutCardFile_InputPath=RTRIM(LTRIM(Standard4Output))+'\output.txt',
	--	@OutCardFile_DestPath=RTRIM(LTRIM(OutCardFile_DestPath))+'/output_'+ Case When ISNULL(@OutputFileName,'')<>'' Then @OutputFileName+'_'  Else '' END +CONVERT(varchar(10), GETDATE(), 112)+'_'+REPLACE((CONVERT(varchar(10),GETDATE(),8)),':','_')+'.txt'
		 @OutCardFile_DestPathBasicPath=RTRIM(LTRIM(OutCardFile_DestPath))
		 ,@WinSCPExePath=RTRIM(LTRIM(WinSCPExePath)) ,@WinSCP_User=RTRIM(LTRIM(WinSCP_User)) ,@WinSCP_PWD=RTRIM(LTRIM(WinSCP_PWD)) ,@WinSCP_IP=RTRIM(LTRIM(WinSCP_IP))
		 ,@WinSCP_Port=RTRIM(LTRIM(WinSCP_Port)) ,@WinSCP_LogPath=RTRIM(LTRIM(WinSCP_LogPath)) 
		 ,@OutCardFile_BackUp_Path=RTRIM(LTRIM(OutCardFile_BackUpPath))
		 ,@CardOutputpath=RTRIM(LTRIM(CardOutput)),@CardErrorpath=RTRIM(LTRIM(CardError))
		 ,@WinSCP_LogPath=WinSCP_LogPath+'\'+Replace(Bank,' ','')+'_'+ CONVERT(varchar(10), GETDATE(), 112)+'.txt'  --new change
		 ,@LogFilePath=RTRIM(LTRIM(WinSCP_LogPath)),@BankName=RTRIM(LTRIM(Bank))
		 ,@Standard3Batch=[Standard3Batch] --- standard3 changes	
		 ,@GetFileSizeBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(GetFileSizeBatch))
	From [TblCardAutomation] with(nolock)
	Where IssuerNo=@IntIssuerNo And EnableState=1

	IF Exists(Select top 1 1 From TblBin Bin With(Nolock) Inner Join TblBanks Bank With(Nolock) On Bin.Bankid=Bank.ID Where Isnull(IsMagstrip,0)=1 AND BankCode=@IntIssuerNo AND CardProgram=LTRIM(RTRIM(@CACardProgram)))
				Begin
					--Standard 3
					
					Set @Standard4Batch=@Standard3Batch
				End
				Else
				Begin
										--Standard 4
					SET @Standard4Batch=@Standard4Batch
				END
 --select @Standard2Batch,@Standard2ExePath ,@Standard2Input ,@Standard2Archive,@BatchFilePath,@LogFilePath,27,@BankName,'' 
--exec  [AGSS1RT].postcard.dbo.[SP_CACardAutomationJobs_AGS] 0,@Standard2Batch,@Standard2ExePath ,@Standard2Input ,@Standard2Archive,@BatchFilePath,@LogFilePath,55,@BankName,'', @StrOutPutCode OutPut,@StrOutputDesc OutPut

exec  [AGSS1RT].postcard.dbo.[SP_CACardAutomationJobs_AGS] 1,@Standard4Batch,@Standard4ExePath ,@Standard4Output ,'',@BatchFilePath,@LogFilePath,@IntIssuerNo,@BankName,@CACardProgram,@StrOutPutCode OutPut,@StrOutputDesc OutPut


--select @Standard4Batch,@Standard4ExePath ,@Standard4Output ,'',@BatchFilePath,@LogFilePath,@IntIssuerNo,@BankName,@CACardProgram --'VISA DEBIT DOM 3'
select @StrOutPutCode  ,@StrOutputDesc
end
GO
