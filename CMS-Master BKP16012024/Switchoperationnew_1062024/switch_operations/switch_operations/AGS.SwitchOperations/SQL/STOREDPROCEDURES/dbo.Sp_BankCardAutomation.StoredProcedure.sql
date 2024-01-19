USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_BankCardAutomation]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--update tblcardautomation set status=null ,statusremark=null where issuerno=9
------------------------
CREATE PROCEDURE [dbo].[Sp_BankCardAutomation]---[Sp_BankCardAutomation] 9,'AGSWallet',3,'',''
@IntIssuerNo Int=0,
@StrCardProgram Varchar(4000)='',
@StrProcessID Varchar(10)='0',
@StrStatusCode Varchar(3) Output,
@StrStatusDesc Varchar(2000) Output
AS
/*Change Managment
created by : Prerna Patil
Created date: 26/04/2017
Created Reason: Card Automation SP Bank Wise
@IntType- 0 For Standard 2 Job
@IntType- 1 For Standard 4 Job

----------------------------------------------------
09/10/2017  Diksha Walunj          ATPCM224: @StrProcessID i/p para  added for reissue card gen status update

Execution
Declare @StrOutPutCode Varchar(3) ,@StrOutputDesc Varchar(2000) 
exec Sp_BankCardAutomation '1','60739306,RBMC55974670',0,@StrOutPutCode Output,@StrOutputDesc OutPut
Select @StrOutPutCode,@StrOutputDesc
[AGSS1RT]
--Dev linked server  [AGSS1RT]->  [10.10.0.21]
[AGSS1RT]

*/ 
BEGIN
	Begin Try  

	--Status Variables
	Set @StrStatusCode='900'
	Set @StrStatusDesc='Failed' 

	--Variables declarations
	insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'start*************************************Sp_BankCardAutomation execution started',getdate(),'')
	Declare	@SwitchInstitutionID varchar(11), @Standard2Batch varchar(100),@Standard2ExePath varchar(Max),@Standard2Input varchar(Max),@Standard2Archive varchar(Max),@Standard4Batch varchar(100),@Standard4ExePath varchar(Max),@Standard4Output  varchar(Max),@Standard3Batch varchar(100)
	
	DECLARE @CustAcctFileUpldBatchPath VARCHAR(max),@CardFileUpldBatchPath VARCHAR(max),@GetCardOutputBatchPath VARCHAR(max),@BatchFilePath VARCHAR(MAX),@CustAcctfileInputPath VARCHAR(MAX),@CustAcctfileDestPath VARCHAR(MAX)	
		 ,@CardfileInputPath VARCHAR(MAX),@CardfileDestPath VARCHAR(MAX),@OutCardFile_InputPath Varchar(800),@OutCardFile_DestPath Varchar(800),@OutCardFile_DestPathBasicPath Varchar(800),
		 @WinSCPExePath VARCHAR(800),@WinSCP_User VARCHAR(200),@WinSCP_PWD VARCHAR(200),@WinSCP_IP VARCHAR(100),@WinSCP_Port VARCHAR(20),
		 @WinSCP_LogPath VARCHAR(800),@OutCardFile_BackUp_Path VARCHAR(800),@CardOutputpath VARCHAR(800),@CardErrorpath VARCHAR(800),
		 @LogFilePath VARCHAR(800),@BankName Varchar(100),@GetFileSizeBatchPath Varchar(2000)
	--Assigning Value
	--Start Sheetal
	declare @IsAccountBalanceFile bit
	select @IsAccountBalanceFile=IsAccountBalanceFile from tblbanks where Bankcode=@IntIssuerNo
	--end sheetal
			Declare @OutputFileName varchar(30)='',@Std2ProcessType char(2)='1'
			If (Isnull(@StrProcessID,'0')<>'0')
			Begin
				Set @OutputFileName=(Select Top 1 [FileName] From TblCACardGenProcessTypes With(Nolock) Where ProcessID=@StrProcessID)
				insert into CardAutodebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Get FileName from TblCACardGenProcessTypes='+@OutputFileName,getdate(),'')
				Set @Std2ProcessType=(Case When @StrProcessID=4 then '2' When @IsAccountBalanceFile=1 then '3' Else '1' END   )
				--Start sheetal another case added for account balance file creation for prepaid cards
			END

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
	Where IssuerNo=@IntIssuerNo  And EnableState=1
	insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Select All path from TblCardAutomation',getdate(),'')

	--Failuer Conditions
	IF((isnull(@Standard2Input,'')='') OR (isnull(@Standard2Archive,'')='') OR (isnull(@Standard4Output,'')=''))          
	BEGIN          
		 SET @StrStatusCode='901'                    
		 SET @StrStatusDesc='Path not exists.'              
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'901 Path not exist',getdate(),'')
		 GOTO LblResult                  
	END
	IF((isnull(@Standard2Batch,'')='') OR (isnull(@Standard4Batch,'')=''))          
	BEGIN          
		 SET @StrStatusCode='902'                    
		 SET @StrStatusDesc='Batch file not exists.'              
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'902 batch file not exist',getdate(),'')
		 GOTO LblResult                  
	END
	IF((isnull(@StrCardProgram,'')='') )          
	BEGIN          
		 SET @StrStatusCode='998'                    
		 SET @StrStatusDesc='Card Program can not be blank.'              
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'998 Card Program can not be blank.',getdate(),'')
		 GOTO LblResult                  
	END
	--*****************Card Automation Process Starts***********************--
	--select * from tblcardautomation where issuerno=9
	--To check the 
	Declare @LastStatus As SmallInt=(Select [Status] From [TblCardAutomation] with(nolock)	Where IssuerNo=@IntIssuerNo  And EnableState=1)
	If(@LastStatus>907 and @LastStatus<=910)
		GOTO LBLCardProcess
	If(@LastStatus>=911 and @LastStatus<=915)
		GOTO LBLSTANDARD4		
	
	--To run the 
	If (Isnull(@StrProcessID,'0') in(5,6))
		GOTO LBLCardProcess


		--A. Calling Card Clearall Proc to clear all prev files from Standard2 Location 
		Declare @StrOutPutCode Varchar(3) ,@StrOutputDesc Varchar(2000) 
		EXEC Sp_PreStandard 0,@IntIssuerNo,@StrOutPutCode OutPut,@StrOutputDesc OutPut
		print @StrOutPutCode
		 
		--B. Calling WINSCP Proc to copy accounts,customer,customersaccounts file on mentioned location
		IF(@StrOutPutCode='00')--P11
		Begin
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Calling WINSCP Proc to copy accounts,customer,customersaccounts file on mentioned location',getdate(),'@StrOutPutCode == 0')
		 print 'B'
		--Exec 10.10.0.21.postcard.dbo.SP_CAFileUploadCardAuto_AGS 0,@StrOutPutCode OutPut,@StrOutputDesc OutPut
			Exec  [AGSS1RT].postcard.dbo.SP_CAFileUploadCardAuto_AGS 0,@CustAcctFileUpldBatchPath,'','','',
			@CustAcctfileInputPath,	@CustAcctfileDestPath,'','','','',@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@WinSCP_LogPath,'',@Std2ProcessType,@StrOutPutCode OutPut,@StrOutputDesc OutPut  --Diksha Newly added

		Declare @MoveFileCnt As SmallINT=0
		Exec  [AGSS1RT].postcard.dbo.SP_CACheckAllStandFile_AGS 0,@Standard2Input,@MoveFileCnt OutPut
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'get file count from SP_CACheckAllStandFile_AGS',getdate(), 'processid:' +cast(@StrProcessID as varchar(100)) + ', count:' +cast(@MoveFileCnt as varchar(50)))

		print @MoveFileCnt
		--IF files count is 3 then its successfully move all files
		IF((@StrProcessID=4 AND @MoveFileCnt>=2) OR (@StrProcessID<>4 AND @MoveFileCnt>=3))--P10
		Begin
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'IF files count is 3 then its successfully move all files',getdate(), 'processid:' +cast(@StrProcessID as varchar(100)) + ', count:' +cast(@MoveFileCnt as varchar(50)))
		--C. Calling Statndard2 POSjava JOB to process accounts files
		IF(@StrOutPutCode='00')--P9
		Begin
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'C. Calling Statndard2 POSjava JOB to process accounts files',getdate(),'@StrOutPutCode == 0')
		exec  [AGSS1RT].postcard.dbo.[SP_CACardAutomationJobs_AGS] 0,@Standard2Batch,@Standard2ExePath ,@Standard2Input ,@Standard2Archive,@BatchFilePath,@LogFilePath,@IntIssuerNo,@BankName,'', @StrOutPutCode OutPut,@StrOutputDesc OutPut

		--D. Checking wheter files are properly moved into archive or not
		IF(@StrOutPutCode='00')--P8
		Begin
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'D. Checking wheter files are properly moved into archive or not',getdate(),'@StrOutPutCode == 0')
		Declare @ProcessFileCnt As SmallINT=0
		--Set @ProcessFileCnt =(Select 10.10.0.21.postcard.dbo.FunCACheckAllStand2FileAGS(@Standard2Archive))
		Exec  [AGSS1RT].postcard.dbo.SP_CACheckAllStandFile_AGS 0,@Standard2Archive,@ProcessFileCnt OutPut

		print @ProcessFileCnt
		--IF files count is 3 then its successfully move all files
		--IF(@ProcessFileCnt=3)--P7
		IF((@StrProcessID=4 AND @ProcessFileCnt>=2) OR (@StrProcessID<>4 AND @ProcessFileCnt>=3))--P7
		Begin


		LBLCardProcess:
		--*1. Clear All Path output and error
		EXEC Sp_PreStandard 2,@IntIssuerNo,@StrOutPutCode OutPut,@StrOutputDesc OutPut
         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'LBLCardprocess started',getdate(),'Clear All Path output and error, stroutputcode:' + @StrOutPutCode)
		PRINT 'TEST'
		PRINT @StrOutPutCode
		If(@StrOutPutCode='00')--P6
		Begin
		--E. Calling WINSCP Proc to copy Cards file
		--Exec 10.10.0.21.postcard.dbo.SP_CAFileUploadCardAuto_AGS 1,@StrOutPutCode OutPut,@StrOutputDesc OutPut
			--Exec 10.10.0.21.postcard.dbo.SP_CAFileUploadCardAuto_AGS 1,@CardFileUpldBatchPath,@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@CardfileInputPath,@CardfileDestPath,@StrOutPutCode OutPut,@StrOutputDesc OutPut  --Diksha Newly added
			Exec  [AGSS1RT].postcard.dbo.SP_CAFileUploadCardAuto_AGS 1,'',@CardFileUpldBatchPath,'','',
			'',	'',@CardfileInputPath,@CardfileDestPath,'','' ,@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@WinSCP_LogPath,'','',@StrOutPutCode OutPut,@StrOutputDesc OutPut  --Diksha Newly added
			
		IF(@StrOutPutCode='00')--P5
		Begin
		--Waiting for 120 sec to process the card file and to check the card file is in output or in error folder
			Declare @IntPriCntr As BigInt=1
			Declare @IntPriMaxCntr As BigInt=30

			--Loop for 30 times each 30 sec=15 min
			WHILE(@IntPriMaxCntr>@IntPriCntr)
			 BEGIN
	         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'wait 30 seconds',getdate(),'Loop for 30 times each 30 sec=15 min')
				 WAITFOR DELAY '00:00:30:00'
				--Check in card folder (File Gayab)
					Declare @CardFile As SmallINT=0
					Declare @CardfileNewDestPath As Varchar(100)=(Replace(@CardfileDestPath,'Cards.txt',''));
					Exec  [AGSS1RT].postcard.dbo.SP_CACheckAllStandFile_AGS 1,@CardfileNewDestPath,@CardFile OutPut
						If(@CardFile=0)
						Begin
							Break;
						END
				--Check in Support Log			

		       SET @IntPriCntr=@IntPriCntr+1
			 END

		--WAITFOR DELAY '00:02'

		----Read the file and save in folder
		--Declare @mytable As Table ( AccountNum varchar(50), AccountInfo varchar(10), CardStatus varchar(100) )
		--Insert into @mytable
		--Exec  [AGSS1RT].postcard.dbo.[SP_CAPartialFileExists_AGS] 0,@CardOutputpath,@CardErrorPath,@StrOutPutCode OutPut,@StrOutputDesc OutPut

			--Read the file and save in folder
--new
		Declare @mytable As Table ( [CardCustomerID] varchar(50), [CardSequenceNum] varchar(10), CardStatus varchar(200) )
		Declare @CardCustomerID varchar(50),@CardSequenceNum varchar(10),@CardStatus varchar(200)
		Insert into @mytable
		Exec  [AGSS1RT].postcard.dbo.[SP_CAPartialFileExists_AGS] 0,@CardOutputpath,@CardErrorPath,@StrOutPutCode OutPut,@StrOutputDesc OutPut
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)
		values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'After execution of SP_CAPartialFileExists_AGS got result StrOutPutCode: '+@StrOutPutCode+'StrOutputDesc: '+@StrOutputDesc,getdate(),'@mytable data :CardCustomerID'+@CardCustomerID+'CardSequenceNum :'+@CardSequenceNum+'@CardStatus :'+@CardStatus)
		--Unquieid
		
		Declare @UniqueID as Varchar(50)=NewID()
		Begin TRY
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'@UniqueID'+@UniqueID,getdate(),'')
			
			Insert INTO [TblCAProcessedCard](UniqueID,[CardCustomerID],[CardSequenceNum],[CardStatus],[CardStatusRemark],[IssuerNum])
			(Select @UniqueID,[CardCustomerID],[CardSequenceNum],substring(CardStatus,len(CardStatus)-1,2),substring(CardStatus,0,len(CardStatus)-1),@IntIssuerNo FROM @mytable)
			--Insert into TblCAProcessCard( UniqueID,[AccountNum],[AccountInfo],[CardStatus],IssuerNum)		
			--(Select @UniqueID,AccountNum,AccountInfo,CardStatus,@IntIssuerNo from @mytable)
			insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Update card status',getdate(),'')
			Exec dbo.[Sp_CAUpdateCardStatus] @UniqueID,@IntIssuerNo,@StrProcessID,@StrOutPutCode OutPut,@StrOutputDesc OutPut  --ATPCM224: @StrProcessID i/p para  added for reissue card gen status update

		END TRY
		Begin Catch
		END Catch
		--select * from TblCAProcessedCard
		--F. Calling Card Clearall Proc to clear all prev files from Standard4 Location 
		--IF(@StrOutPutCode='00')--P4
		If Exists (Select top 1 1 from TblCAProcessedCard with(Nolock) Where UniqueID=@UniqueID AND Ltrim(RTRIM(CardStatus)) = '00' and IssuerNum=@IntIssuerNo)
		Begin
			insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'wait for 5 min for standard 4 job',getdate(),'')
		 --Delay in standard 4 job maximum 5 min
		 WaitFor Delay '00:15'

		LBLSTANDARD4:
		--Loop for Card Program
		If Exists(Select top 1 1 From fnSplit (@StrCardProgram,','))--P3.1
		Begin		
		
			Declare @IntCardPrgCnt As Int=(Select count(RowID) From dbo.fnSplit (@StrCardProgram,','))
			Declare @IntCardIncr As Int =1
			insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Loop on all card programs',getdate(),'Total count' + cast(@IntCardPrgCnt as varchar(50)))
			While(@IntCardIncr<=@IntCardPrgCnt)
			Begin

			Declare @CACardProgram As varchar(100)=(Select Value From dbo.fnSplit (@StrCardProgram,',') Where RowID=@IntCardIncr)
			Set @OutCardFile_DestPath='';
			Set @OutCardFile_DestPath=RTRIM(LTRIM(@OutCardFile_DestPathBasicPath))+'/output_'+ Case When ISNULL(@OutputFileName,'')<>'' Then @OutputFileName+'_'  Else '' END +CONVERT(varchar(10), GETDATE(), 112)+'_'+REPLACE((CONVERT(varchar(10),GETDATE(),8)),':','_')+'.txt'

			EXEC Sp_PreStandard 1,@IntIssuerNo,@StrOutPutCode OutPut,@StrOutputDesc OutPut
			insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'pre standard called',getdate(),'outputcode desc:' + @StrOutputDesc + 'ca card program:' + @CACardProgram)

			IF(@StrOutPutCode='00')--P3
			Begin
				---- start for standard 3 job 
				--To Check the Standard 3 or 4 job to be run
				IF Exists(Select top 1 1 From TblBin Bin With(Nolock) Inner Join TblBanks Bank With(Nolock) On Bin.Bankid=Bank.ID Where Isnull(IsMagstrip,0)=1 AND BankCode=@IntIssuerNo AND CardProgram=LTRIM(RTRIM(@CACardProgram)))
				Begin
					--Standard 3
					insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'standard 3 job will run',getdate(),'')
					Set @Standard4Batch=@Standard3Batch
				End
				Else
				Begin
					insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'standard 4 job will run',getdate(),'')
					--Standard 4
					SET @Standard4Batch=@Standard4Batch
				END
			 insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'G. Calling Statndard4 POSjava JOB to process accounts files and then wait for 15 mmin',getdate(),'[SP_CACardAutomationJobs_AGS]')
			--G. Calling Statndard4 POSjava JOB to process accounts files
			exec  [AGSS1RT].postcard.dbo.[SP_CACardAutomationJobs_AGS] 1,@Standard4Batch,@Standard4ExePath ,@Standard4Output ,'',@BatchFilePath,@LogFilePath,@IntIssuerNo,@BankName,@CACardProgram,@StrOutPutCode OutPut,@StrOutputDesc OutPut
			IF(@StrOutPutCode='00')--P2
			Begin
			 --Delay after standard 4 job maximum 15 min to be set
			 insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Waiting for 15 min',getdate(),'SP_CACheckAllStandFile_AGS')
			 WaitFor Delay '00:05'
			 insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Wait over for 15 min',getdate(),'SP_CACheckAllStandFile_AGS')
			Declare @ProcessOutputFileCnt As SmallINT=0
			Exec  [AGSS1RT].postcard.dbo.SP_CACheckAllStandFile_AGS 2,@Standard4Output,@ProcessOutputFileCnt OutPut
				print @ProcessOutputFileCnt
				--IF files count is 1 then its successfully move all files
				IF(@ProcessOutputFileCnt=1) --P1
				Begin
					--To check the file size
					 insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'Check file size',getdate(),'[SP_CACheckFileSize_AGS]')
					Declare @ProcessOutputFileSize As Varchar(100)=0
					Exec  [AGSS1RT].postcard.dbo.[SP_CACheckFileSize_AGS] 0,@GetFileSizeBatchPath,@Standard4Output,@ProcessOutputFileSize OutPut
					IF(@ProcessOutputFileSize>0) --P0
					Begin
					
					--H. Calling WINSCP Proc to copy Output file to SFTP folder
					 insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'H. Calling WINSCP Proc to copy Output file to SFTP folder',getdate(),'called SP_CAFileUploadCardAuto_AGS')

						--Exec 10.10.0.21.postcard.dbo.SP_CAFileUploadCardAuto_AGS 2,@StrOutPutCode OutPut,@StrOutputDesc OutPut	
						--Exec 10.10.0.21.postcard.dbo.SP_CAFileUploadCardAuto_AGS 2,@GetCardOutputBatchPath,@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@OutCardFile_InputPath,@OutCardFile_DestPath,@StrOutPutCode OutPut,@StrOutputDesc OutPut  --Diksha Newly added												
						Exec  [AGSS1RT].postcard.dbo.SP_CAFileUploadCardAuto_AGS 2,'','',@GetCardOutputBatchPath,'',
						'',	'','','',@OutCardFile_InputPath,@OutCardFile_DestPath ,@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@WinSCP_LogPath,@OutCardFile_BackUp_Path,'',@StrOutPutCode OutPut,@StrOutputDesc OutPut  --Diksha Newly added	
					END
					Else
					BEgin
						SET @StrStatusCode='915'                    
						SET @StrStatusDesc='Output File genrated but with size '+@ProcessOutputFileSize + ' bytes.'
						GOTO LblResult 
					END--P0 end
				END--@ProcessOutputFileCnt Success
				Else
				Begin  
					SET @StrStatusCode='914'                    
					SET @StrStatusDesc='Output File not genrated. Decline With Msg '+@StrOutputDesc 
					GOTO LblResult 
				END		--P1 End												
			END-- G. Success
			Else
			Begin  
				SET @StrStatusCode='913'                    
				SET @StrStatusDesc='Standard4 Job Execution failed. Decline With Msg '+@StrOutputDesc 
				GOTO LblResult 
			END--P2 END
			--G. END
			END
			Else
			Begin  
				SET @StrStatusCode='912'                    
				SET @StrStatusDesc='Not able to clear previous files from Standard4 job. Decline With Msg '+@StrOutputDesc 
				GOTO LblResult 
			END--P3 END
			--Counter increment
			 Set @IntCardIncr=@IntCardIncr+1
		 END
		END
		Else
			Begin  
				SET @StrStatusCode='911'                    
				SET @StrStatusDesc='Not able to clear previous files from Standard4 job. Decline With Msg '+@StrOutputDesc 
				GOTO LblResult 
			END--P3.1 END

		ENd--F. Success
		Else
		Begin  
			Declare @StrCardStatus As varchar(50) =(Select top 1 CardStatus from TblCAProcessedCard Where UniqueID=@UniqueID AND Ltrim(RTRIM(CardStatus)) ='00' )
			SET @StrStatusCode='910'                    
			SET @StrStatusDesc='Card File Processing. Decline With Msg '+@StrOutputDesc + ' OR Card file declined with '+Isnull(@StrCardStatus,'File not prcoessed')
			GOTO LblResult 
		END --P4 END
		--F. END
		END--E. Success
		Else
		Begin  
			SET @StrStatusCode='909'                    
			SET @StrStatusDesc='Copying card file is failed. Decline With Msg '+@StrOutputDesc 
			GOTO LblResult 
		END --P5 END
		--E. END
		End--*1 Success
		Else
		Begin  
			SET @StrStatusCode='908'                    
			SET @StrStatusDesc='Card files not able to clear from output and error folder. Declined with message ' + @StrOutputDesc 
			GOTO LblResult 
		END --P6 END		
		END
		Else
		Begin  
			SET @StrStatusCode='907'                    
			SET @StrStatusDesc='Not all files process for Standard2 JOB. Process files count '+convert(varchar(5),@ProcessFileCnt)
			GOTO LblResult 
		END --P7 END
		END --D. Success
		Else
		Begin  
			SET @StrStatusCode='906'                    
			SET @StrStatusDesc='Standard2 Job Execution failed. Decline With Msg '+@StrOutputDesc 
			GOTO LblResult 
		END --P8 End
		--D. END
		END--C. Success
		Else
		Begin  
			SET @StrStatusCode='905'                    
			SET @StrStatusDesc='Not able to place the standard2 file through WINSCP. Decline With Msg '+@StrOutputDesc 
			GOTO LblResult 
		END --P9 End
		--C. END

		END-- Move files
		Else
		Begin  
			SET @StrStatusCode='904'                    
			SET @StrStatusDesc='Moved files count is not proper. Count is '+convert(Varchar(4),@MoveFileCnt) 
			GOTO LblResult 
		END--P10 End

		END--B.Success
		Else
		Begin  
			SET @StrStatusCode='903'                    
			SET @StrStatusDesc='Not able to clear backup standard2 files. Decline With Msg '+@StrOutputDesc 
			GOTO LblResult 
		END --P11
		--B. END
	
		SET @strStatusCode='00'        
        SET @StrStatusDesc='success.'  
	         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'done card automation',getdate(),'status code: '+ @strStatusCode + ', status description:' + @StrStatusDesc)
		 
LblResult: 
	
		Update  [TblCardAutomation] SET [Status]=Case When (@strStatusCode='00') Then 1 Else @strStatusCode END, StatusRemark=@StrStatusDesc,LastRunTime=GETDATE()
		Where IssuerNo=@IntIssuerNo 

		--In case of failuer the status is updated 
		If (Isnull(@StrProcessID,'0')<>'0')
		Begin
			--Update the status
			Update TblCACardGenStatusLog SET [Status]=Case When (@strStatusCode='00') Then 1 Else @strStatusCode END,ErrorDesc=@StrStatusDesc,Modifieddate=Getdate()
			Where IssuerNo=@IntIssuerNo AND ProcessID=@StrProcessID
		END
	         insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'LblResult:done card automation',getdate(),'status code: '+ @strStatusCode + ', status description:' + @StrStatusDesc)

		--In case of Error message the mail sent to stakeholders
		If(@strStatusCode<>'00')
		BEGIN
			Begin Try
				Declare @StrbankName varchar(50),@RCVREmailID Nvarchar(1000)
				Select @StrbankName=Bank,@RCVREmailID=NotificationEmailID From TblCardautomation with(Nolock) Where IssuerNo=@IntIssuerNo
				exec [Sp_CardAutomationErrorSendEmail] @StrbankName,@IntIssuerNo,@strStatusCode,@StrStatusDesc,@RCVREmailID,@StrOutPutCode Output,@StrOutputDesc OutPut
			END Try
			Begin Catch
			End Catch
		END

	insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,@StrCardProgram,@StrProcessID,'End*************************************Sp_BankCardAutomation execution End',getdate(),'')
    End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'IssuerNumber='+ convert(varchar(5),@IntIssuerNo)+' CardProgram= '+@StrCardProgram,@IntIssuerNo
		     SET @strStatusCode='999'        
			 SET @StrStatusDesc='Unexpected error occurred' 
	END CATCH; 
	SELECT @strStatusCode AS [STATUSCODE] ,@strStatusDesc  AS [STATUSDESC]
END

GO
