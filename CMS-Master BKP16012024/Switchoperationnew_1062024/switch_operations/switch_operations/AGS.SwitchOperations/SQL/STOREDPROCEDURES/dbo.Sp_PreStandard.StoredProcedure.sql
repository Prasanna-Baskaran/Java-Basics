USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_PreStandard]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_PreStandard] 
@IntType SmallInt=0,
@IntIssuerNo Int=0,
@StrOutPutCode Varchar(3) Output,
@StrOutputDesc Varchar(2000) Output

AS
/*Change Managment
created by : Prerna Patil
Created date: 25/04/2017
Created Reason: TO Check ProperPath and Move/Rname Old Files
@IntType- 0 For Standard 2 Job
@IntType- 1 For Standard 4 Job

*/ 
BEGIN
	Begin Try  
	Set @StrOutPutCode='900'
	Set @StrOutputDesc='Failed' 	 
	 --select * from tblcardautomation where issuerno=9
	insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,'','','Sp_PreStandard execution started',getdate(),'')
	
	Declare	 @Standard2Input varchar(Max),@Standard2Archive varchar(Max),@Standard4Output  varchar(Max),@StrCardOutput  varchar(Max),@StrCardError  varchar(Max)
	SELECT @Standard2Input=Standard2Input,@Standard2Archive=Standard2Archive,@Standard4Output=Standard4Output ,
	@StrCardOutput=CardOutput,@StrCardError=CardError
	From [TblCardAutomation] with(nolock) where IssuerNo=@IntIssuerNo  

	IF((isnull(@Standard2Input,'')='') OR (isnull(@Standard2Archive,'')='') OR (isnull(@Standard4Output,'')='')OR (isnull(@StrCardOutput,'')='')OR (isnull(@StrCardError,'')=''))          
	BEGIN          
	 SET @StrOutPutCode='901'                    
	 SET @StrOutputDesc='Path not exists.'              
	 GOTO LblResult                  
	END

	If(@IntType=0)
	Begin
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,'','','Sp_CAPreStandard_AGS called,Standard2 job uses IntType='+cast(@IntType as varchar(100)) ,getdate(),@Standard2Input)
		exec  [AGSS1RT].postcard.dbo.[Sp_CAPreStandard_AGS] 0,@Standard2Input,@Standard2Archive ,'','','' ,@StrOutPutCode OutPut,@StrOutputDesc OutPut
	END
	Else If(@IntType=1)
	Begin
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,'','','Sp_CAPreStandard_AGS called',getdate(),@Standard4Output)		
		 exec  [AGSS1RT].postcard.dbo.[Sp_CAPreStandard_AGS] 1,'','' ,@Standard4Output,'','' ,@StrOutPutCode OutPut,@StrOutputDesc OutPut
	END
	Else If(@IntType=2)
	Begin
		insert into CardAutoDebugLog (IssuerNo,CardProgram,ProcessID,Message,InsertedOn,MessageData)values(@IntIssuerNo,'','','Sp_CAPreStandard_AGS called',getdate(),@StrCardOutput)		
		exec  [AGSS1RT].postcard.dbo.[Sp_CAPreStandard_AGS] 2,'','' ,'',@StrCardOutput,@StrCardError,@StrOutPutCode OutPut,@StrOutputDesc OutPut
	END
LblResult: 
   
    End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)          
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'IssuerNumber='+ convert(varchar(5),@IntIssuerNo)+ ',@IntType='+convert(varchar(2),@IntType),@IntIssuerNo
		      SET @StrOutPutCode='999'        
			 SET @StrOutputDesc='Unexpected error occurred [Sp_PreStandard]' 
	END CATCH; 
END
GO
