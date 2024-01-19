
ALTER proc [dbo].[USP_ExecuteStandard4OnCardProgram]--exec [USP_ExecuteStandard4OnCardProgram] '54','40034','3','VISA DEBIT DOM 5','',''
(
	@IssuerNo Numeric,
	@FileId varchar(20),
	@ProcessId int,
	@CardProgram varchar(100),
	@StrOutPutCode Varchar(3) Output,
	@StrOutputDesc Varchar(2000) Output 
)
as
/************************************************************************
Jira Id: ATPCM-620 
Purpose:Execute stanard 4 job on the basis of card program.
Created History
Date        Created By				Reason
31/08/2018  Pratik Mhatre			Newly Developed
*************************************************************************/
begin

insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'**Standard4 Execution Start**','usp_ExecuteStandard4OnCardProgram',getdate())

declare @Standard4Batch varchar(100),@Standard4ExePath varchar(Max),@Standard4Output  varchar(Max),@BatchFilePath VARCHAR(MAX),@LogFilePath VARCHAR(800),@BankName Varchar(100)
Declare @Standard4LogFileName varchar(500),@Standard4LogFIlePath varchar(500), @MoveFileBatchPath varchar(500)
declare @GetFileSizeBatchPath Varchar(2000)
declare @GetCardOutputBatchPath VARCHAR(max),@OutCardFile_InputPath Varchar(800),@OutCardFile_DestPath Varchar(800)
		,@WinSCPExePath VARCHAR(800),@WinSCP_User VARCHAR(200),@WinSCP_PWD VARCHAR(200),@WinSCP_IP VARCHAR(100)
		,@WinSCP_Port VARCHAR(20),@WinSCP_LogPath VARCHAR(800),@OutCardFile_BackUp_Path VARCHAR(800),@Standard3Batch varchar(100)--new

Declare @OutputFileName varchar(30)='',@Std2ProcessType char(2)='1'
--Bank_Output
	SELECT 
		   @Standard4Batch=[Standard4Batch],@Standard4ExePath=[Standard4ExePath],@Standard4Output=Standard4Output
		  ,@BatchFilePath=RTRIM(LTRIM(BatchFilesPath))		  
		  ,@LogFilePath=RTRIM(LTRIM(WinSCP_LogPath)),@BankName=RTRIM(LTRIM(Bank))
		  ,@GetFileSizeBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(GetFileSizeBatch))--to check file size
		  ,@GetCardOutputBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(GetCardOutputBatch)),@OutCardFile_InputPath=RTRIM(LTRIM(Standard4Output))+'\output.txt'--Bank_Output column name change and / remove from output
		  ,@OutCardFile_DestPath=RTRIM(LTRIM(Bank_Output))+'output_'+ Case When ISNULL(@OutputFileName,'')<>'' Then @OutputFileName+'_'  Else '' END +CONVERT(varchar(10), GETDATE(), 112)+'_'+REPLACE((CONVERT(varchar(10),GETDATE(),8)),':','_')+'.txt'
		  ,@WinSCPExePath=RTRIM(LTRIM(WinSCPExePath)) ,@WinSCP_User=RTRIM(LTRIM(AGS_SFTPUser)) ,@WinSCP_PWD=(RTRIM(LTRIM(ISNULL(dbo.ufn_decryptpan(AGS_SFTPPassword),'')))) ,@WinSCP_IP=RTRIM(LTRIM(AGS_SFTPServer))
		  ,@WinSCP_Port=RTRIM(LTRIM(AGS_SFTPPort)) ,@WinSCP_LogPath=RTRIM(LTRIM(WinSCP_LogPath))-- 
		  ,@OutCardFile_BackUp_Path=RTRIM(LTRIM(OutCardFile_BackUpPath))
		  ,@Standard3Batch=[Standard3Batch] 
		  ,@MoveFileBatchPath=RTRIM(LTRIM(BatchFilesPath))+RTRIM(LTRIM(MoveFIleBatchName))
		  ,@Standard4LogFIlePath=ltrim(rtrim(standard4LogFilePath))
		 --From [TblCardAutomation] with(nolock)
		 From CardAutomationPath with(nolock)
	     where  issuerno=@IssuerNo and EnableState=1  AND ProcessId=@ProcessId
		--print('-1')
		 IF Exists(Select top 1 1 From TblBin Bin With(Nolock) 
					Inner Join TblBanks Bank With(Nolock) On Bin.Bankid=Bank.ID 
					Where Isnull(IsMagstrip,0)=1 AND BankCode=@IssuerNo AND CardProgram=LTRIM(RTRIM(@CardProgram)))
				Begin
					--Standard 3					
					Set @Standard4Batch=@Standard3Batch
					insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
					values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Checking whether need to call Standard3 or standard4','Standard4Batch: '+@Standard3Batch,getdate())			
				End
				Else
				Begin
					--Standard 4
					SET @Standard4Batch=@Standard4Batch					
					insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
					values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Checking whether need to call Standard3 or standard4', 'Standard4Batch: '+@Standard4Batch,getdate())
				END
	
	--select @Standard4Batch '@Standard4Batch'
	--print('0')
	
	/*Execute Standard 4 Call Switch Batch File*/
	insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
	values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Calling Postcard SP:SP_CACardAutomationStd2Std4Job_AGS', 'Param>>: @IntType:1,@Standard4Batch:'+@Standard4Batch+',@Standard4ExePath:'+@Standard4ExePath+ ',@Standard4Output:'+@Standard4Output +',@Standard2Archive:'+''+',@BatchFilePath:'+@BatchFilePath+',@LogFilePath:'+@LogFilePath+',@IssuerNo:'+cast(@IssuerNo as varchar(5))+',@BankName:'+@BankName+',@CardProgram:'+@CardProgram,GETDATE())
	
	--print('1')
	
	--select '' 'Before_SP_CANewCardAutomationStd2Std4Job_AGS'
	--select @Standard4Batch,@Standard4ExePath ,@Standard4Output ,'',@BatchFilePath,@LogFilePath,@IssuerNo,@BankName,@CardProgram
	/*Log File Name to download STD4 Log file*/
	Declare @BankNameTemp varchar(100),@NewFileName varchar(100)
	DECLARE	@TodayDate as varchar(40),@TodayHour as varchar(40),@TodayMin as varchar(40),@TodaySec as varchar(40) 
	SET @TodayDate = CONVERT(varchar(10), GETDATE(), 112)
	SET @TodayHour = DATEPART(hh,GETDATE())
	SET @TodayMin = DATEPART(mi,GETDATE())
	SET @TodaySec = DATEPART(MS,GETDATE())
	set @BankNameTemp = Replace( @BankName,' ','#')
	SELECT @NewFileName = 'Logs' + '_' + @TodayDate + '_' + @TodayHour + '_' + @TodayMin + '_' + @TodaySec + '.txt'
	set @Standard4LogFileName=@BankNameTemp+'_Standard4_'+ @NewFileName

	exec [AGSS1RT].postcard.dbo.[SP_CANewCardAutomationStd2Std4Job_AGS] 1,@Standard4Batch,@Standard4ExePath ,@Standard4Output ,'',@BatchFilePath,@LogFilePath,@IssuerNo,@BankName,@CardProgram,@FileId,@ProcessId,@StrOutPutCode OutPut,@StrOutputDesc OutPut,@Standard4LogFileName 
	--select '' 'After_SP_CANewCardAutomationStd2Std4Job_AGS'
	
	insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
	values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response from SP_CACardAutomationStd2Std4Job_AGS', '@StrOutPutCode: '+isnull(@StrOutPutCode,'') + ',@StrOutputDesc: '+ isnull(@StrOutputDesc,'')  + ',@Standard4LogFileName: '+ isnull(@Standard4LogFileName,''),getdate())
	
	if @Standard4LogFileName is not null and ltrim(RTRIM( @Standard4LogFileName)) <>''
	begin
		/*Update Log File Name in TblCardAutomation to download from Switch*/	
		--update TblCardAutomation set Standard4LogFIleName =@Standard4LogFileName where IssuerNo=@IssuerNo
				
		set @standard4LogFilePath=@standard4LogFilePath+ @Standard4LogFileName
		set @LogFilePath=@LogFilePath+'\'+@Standard4LogFileName
	end

/*If Rsp Code is '00' then move all file from switch server to AGS sftp*/
IF(@StrOutPutCode='00')--p2
	begin
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Waiting for'+'00:05', '',getdate())
		WaitFor Delay '00:02'
	
		Declare @ProcessOutputFileCnt As SmallINT=0
		
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response Is 00 then Check output File is created, checking file Count on server', 'SP_CACheckAllStandFile_AGS',getdate())
		
		--select '' 'Before_ SP_CACheckAllStandFile_AGS'		
		Exec  AGSS1RT.postcard.dbo.SP_CACheckAllStandFile_AGS 2,@Standard4Output,@ProcessOutputFileCnt OutPut
		print @ProcessOutputFileCnt
		--select '' 'After_ SP_CACheckAllStandFile_AGS'
		
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response From SP:SP_CACheckAllStandFile_AGS ie. @ProcessOutputFileCnt:'+ cast(@ProcessOutputFileCnt as varchar(50)), 'SP_CACheckAllStandFile_AGS',getdate())
		
		/*IF files count is 1 then its successfully then move all files*/
		IF(@ProcessOutputFileCnt=1)--p1
		begin			
			insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)
			values(@IssuerNo,@CardProgram,'','Check file size get file count=1',getdate(),'[SP_CACheckAllStandFile_AGS]')
			
			Declare @ProcessOutputFileSize As Varchar(100)=0
			
			insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
			values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'If @ProcessOutputFileCnt=1 then check File Size.', 'SP_CACheckFileSize_AGS',getdate())
			/*Check File Size, is output file contain any data or not*/
			
			--select '' 'Before_ SP_CACheckFileSize_AGS'
			Exec  [AGSS1RT].postcard.dbo.[SP_CACheckFileSize_AGS] 0,@GetFileSizeBatchPath,@Standard4Output,@ProcessOutputFileSize OutPut
			--select '' 'After_ SP_CACheckFileSize_AGS'
			
			insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
			values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response From SP:SP_CACheckFileSize_AGS ie. @ProcessOutputFileSize:'+cast(@ProcessOutputFileSize as varchar(100)), 'SP_CACheckFileSize_AGS',getdate())
			
			PRINT @ProcessOutputFileSize
			IF(@ProcessOutputFileSize>0) --P0
				Begin
				
					--'Param:SP_CAFileUploadCardAuto_AGS @IntPara:2'+','+'@CustAcctFileUpldBatchPath:'+',@CardFileUpldBatchPath:'+''+',@GetCardOutputBatchPath:'+@GetCardOutputBatchPath+',@BatchFilePath:'+''+',@CustAcctfileInputPath:'+''+',@CustAcctfileDestPath:'+''+',@CardfileInputPath:'+''+',@CardfileDestPath:'+''+',@OutCardFile_InputPath:'+@OutCardFile_InputPath+',@OutCardFile_DestPath:'+@OutCardFile_DestPath +',@WinSCPExePath:'+@WinSCPExePath+',@WinSCP_User:'+@WinSCP_User+',@WinSCP_PWD:'+@WinSCP_PWD+',@WinSCP_IP:'+@WinSCP_IP+',@WinSCP_Port:'+@WinSCP_Port+',@WinSCP_LogPath:'+@WinSCP_LogPath +',@OutCardFileBackUpPath:'+@OutCardFile_BackUp_Path+',@Std2ProcessType:'
					insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
					values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'If @ProcessOutputFileSize>0 then Calling WINSCP Proc to copy Output file to SFTP folder', 'Souce FilePath:'+@OutCardFile_InputPath+ ' , Destination FilePath:'+@OutCardFile_DestPath ,getdate())		
					
					set @WinSCP_LogPath = @WinSCP_LogPath +'\'+@BankName+'.txt' --'\PrabhuDebit.txt'
					
					--select '' 'Before_ SP_CAFileUploadCardAuto_AGS'
					/*Calling WINSCP Proc to copy Output file to SFTP folder*/ 
					 Exec AGSS1RT.postcard.dbo.SP_CAFileUploadCardAuto_AGS 2,'','',@GetCardOutputBatchPath,'',
						'',	'','','',@OutCardFile_InputPath,@OutCardFile_DestPath ,@WinSCPExePath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,
						@WinSCP_LogPath ,@OutCardFile_BackUp_Path,'',@StrOutPutCode OutPut,@StrOutputDesc OutPut   
					
					--select '' 'After_ SP_CAFileUploadCardAuto_AGS'	
					insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
					values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response From SP:SP_CAFileUploadCardAuto_AGS' , '@StrOutPutCode: '+@StrOutPutCode + ',@StrOutputDesc: '+@StrOutputDesc,getdate())
				 END
			 Else
				 BEGIN
					if @Standard4LogFileName is not null and ltrim(RTRIM( @Standard4LogFileName)) <>''
					 begin
						--select @MoveFileBatchPath '@MoveFileBatchPath',@WinSCPExePath '@WinSCPExePath',@WinSCP_LogPath '@WinSCP_LogPath',@WinSCP_User '@WinSCP_User',
						--@WinSCP_PWD '@WinSCP_PWD',@WinSCP_IP '@WinSCP_IP',@WinSCP_Port '@WinSCP_Port',@LogFilePath '@LogFilePath',@standard4LogFilePath '@standard4LogFilePath', 
						--@IssuerNo '@IssuerNo',@CardProgram '@CardProgram',@FileId '@FileId',@ProcessId '@ProcessId'
						
						/*Update Log File Name in TblCardautomationStatus to download from Switch*/	
						update TblCardautomationStatus set Std4FailedLogFileName= @Standard4LogFileName where id=@FileId			
						insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
						values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Moving Standard 4 file on local SFTP becouse file size is less than 0', 'USP_MoveFileFromSwitchTOApplication',getdate())
						exec [AGSS1RT].postcard.dbo.USP_MoveFileFromSwitchTOApplication @MoveFileBatchPath,@WinSCPExePath,@WinSCP_LogPath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@LogFilePath,@standard4LogFilePath,@IssuerNo,@CardProgram,@FileId,@ProcessId						
						--select '' 'After_ USP_MoveFileFromSwitchTOApplication First Else'			
					 end	
				 
					SET @StrOutPutCode='915'                    
					SET @StrOutputDesc='Output File genrated but with size '+cast( @ProcessOutputFileSize as varchar(100)) + ' bytes.'		
				 END--P0 end
		END--@ProcessOutputFileCnt Success
		Else
		Begin
			if @Standard4LogFileName is not null and ltrim(RTRIM( @Standard4LogFileName)) <>''
			 begin	
				/*Update Log File Name in TblCardautomationStatus to download from Switch*/	
				update TblCardautomationStatus set Std4FailedLogFileName= @Standard4LogFileName where id=@FileId
				insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
				values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Moving Standard 4 file on local SFTP because file is not created on switch', 'USP_MoveFileFromSwitchTOApplication',getdate())
				exec [AGSS1RT].postcard.dbo.USP_MoveFileFromSwitchTOApplication @MoveFileBatchPath,@WinSCPExePath,@WinSCP_LogPath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@LogFilePath,@standard4LogFilePath,@IssuerNo,@CardProgram,@FileId,@ProcessId				
				--select '' 'After_ USP_MoveFileFromSwitchTOApplication Last Else'			
			 end	
			SET @StrOutPutCode='914'                    
			SET @StrOutputDesc='Output File not generated. Decline With Msg :'+@StrOutputDesc 						
		END		--P1 End												
	END-- G. Success
Else
	Begin
	if @Standard4LogFileName is not null and ltrim(RTRIM( @Standard4LogFileName)) <>''
		begin	 
		/*Update Log File Name in TblCardautomationStatus to download from Switch*/	
		update TblCardautomationStatus set Std4FailedLogFileName= @Standard4LogFileName where id=@FileId			
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Moving Standard 4 file on local SFTP becouse Standard4 Job Execution failed.', 'USP_MoveFileFromSwitchTOApplication',getdate())
		exec [AGSS1RT].postcard.dbo.USP_MoveFileFromSwitchTOApplication @MoveFileBatchPath,@WinSCPExePath,@WinSCP_LogPath,@WinSCP_User,@WinSCP_PWD,@WinSCP_IP,@WinSCP_Port,@LogFilePath,@standard4LogFilePath,@IssuerNo,@CardProgram,@FileId,@ProcessId						 
	
		SET @StrOutPutCode='913'                    
		SET @StrOutputDesc='Standard4 Job Execution failed. Decline With Msg :'+ @StrOutputDesc 	
		end
	END 
end
