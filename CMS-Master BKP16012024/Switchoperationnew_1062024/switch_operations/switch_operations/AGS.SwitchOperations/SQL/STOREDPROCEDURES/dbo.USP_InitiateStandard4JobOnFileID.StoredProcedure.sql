
--exec USP_InitiateStandard4JobOnFileID 54,30015
ALTER Procedure [dbo].[USP_InitiateStandard4JobOnFileID]
	@IssuerNo numeric,
	@FileId varchar(50)
as
/************************************************************************
Jira Id: ATPCM-620 
Purpose: Initiating standard 4 job on the basis of FIleID.
Created History
Date        Created By				Reason
31/08/2018  Pratik Mhatre			Newly Developed
*************************************************************************/
begin
	Declare @Status char(2)	='1'
	insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
	values(@IssuerNo,@FileId,null,null,'**Standard4 Process Initiated**','',getdate())
	
	select ROW_NUMBER() over(order by binprefix) RwId,BinPrefix,CardProgram,IssuerNo,FileId,ProcessId into #CardprogramForSTD4 
	from CardProgramsForStandard4 nolock 
	where  IssuerNo=@IssuerNo and FileId=@FileId and isnull(processed,'0')='0' --and id<>10002
	
	insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
	values(@IssuerNo,@FileId,null,null,'Card Program Count:'+cast( (select COUNT(1) from #CardprogramForSTD4) as varchar(10)),'',getdate())
		
	Declare @min int 
	Declare @max int 
	Declare @BinPrefix varchar(20),@CardProgram varchar(100) ,@ProcessId bigint
	Declare @BankId bigint
	select @BankId=ID from tblbanks nolock where BankCode=@IssuerNo
	select @min=MIN(RwId),@max=MAX(RwId) from #CardprogramForSTD4

	/*Loop on FileId>>CardProgram to run standard 4*/
	while @min<=@max
	begin
		set @Status='0'
		Declare @StrOutPutCode Varchar(3) 
		Declare @StrOutputDesc Varchar(2000)
		set @BinPrefix=''
		set @CardProgram=''
		set @ProcessId=null
		select @BinPrefix=BinPrefix,@CardProgram=CardProgram,@ProcessId=ProcessId from #CardprogramForSTD4 where RwId=@min
		
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Calling SP:usp_ExecuteStandard4OnCardProgram ','usp_ExecuteStandard4OnCardProgram ',getdate())
		
		/*Execute Standard for on CardProgram*/
		exec dbo.usp_ExecuteStandard4OnCardProgram @IssuerNo,@FileId,@ProcessId,@CardProgram,@StrOutPutCode output,@StrOutputDesc output		
		
		insert into CardAutomationStandard4Log(IssuerNo,FileId,ProcessId,CardProgram,Message,MessageData,InsertedOn)
		values(@IssuerNo,@FileId,@ProcessId,@CardProgram,'Response from SP:usp_ExecuteStandard4OnCardProgram ','@StrOutPutCode:'+ @StrOutPutCode +',''@StrOutputDesc:'+ @StrOutputDesc,getdate())	
		
		--select @StrOutPutCode 'lastCode',@StrOutputDesc 'lastDesc'
		/*Check is standard 4 is ran successfully or not against perticular card program*/
		IF (@StrOutPutCode='00')
			begin
				set @Status='1'
				--select @IssuerNo '@IssuerNo',@FileId '@FileId',@BinPrefix '@BinPrefix' ,@Status '@Status1'
				--select * from CardAutomationStandard4 where IssuerNo=@IssuerNo and FileId=@FileId and BinPrefix=@BinPrefix
				update CardProgramsForStandard4 set Processed='1',RspCode=@StrOutPutCode,RspDesc=@StrOutputDesc , UpdatedOn=GETDATE()
				where IssuerNo=@IssuerNo and FileId=@FileId and BinPrefix=@BinPrefix
			end
		else
			begin			
				set @Status='0'
				--select @IssuerNo '@IssuerNo',@FileId '@FileId',@BinPrefix '@BinPrefix' ,@Status '@Status0'
				update CardProgramsForStandard4 set Processed='0',RspCode=@StrOutPutCode,RspDesc=@StrOutputDesc , UpdatedOn=GETDATE()
				where IssuerNo=@IssuerNo and FileId=@FileId and BinPrefix=@BinPrefix
				break; /*Break Loop in Case any failuer in standard4*/
			end		
		--select @BinPrefix,@CardProgram		
		set @min=@min+1
	end
	drop table #CardprogramForSTD4
	select @Status 'Standard4LastStatus'
end