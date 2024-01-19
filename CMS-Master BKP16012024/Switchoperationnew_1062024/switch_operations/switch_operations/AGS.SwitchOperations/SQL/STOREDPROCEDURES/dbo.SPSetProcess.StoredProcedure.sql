USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SPSetProcess]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SPSetProcess]
@IntProcessID Int,
@IntState Int,
@IntpriOutputCode Int Output,
@StrpriOutputDesc Varchar(500) OutPut
AS
BEGIN
	BEGIN TRANSACTION  
	BEGIN TRY 
	
	Set @IntpriOutputCode=1;
	Set @StrpriOutputDesc='Error Occurs';

	If(@IntState=1)
	Begin
			Update TblMassModule Set  NextRunDate=(Case When FrequencyUnit='SS' Then DATEADD(SS,Frequency,isnull(NextRunDate,GETDATE()))
														When FrequencyUnit='MI' Then DATEADD(MI,Frequency,isnull(NextRunDate,GETDATE()))
														When FrequencyUnit='HH' Then DATEADD(HH,Frequency,isnull(NextRunDate,GETDATE()))
														When FrequencyUnit='DD' Then DATEADD(DD,Frequency,isnull(NextRunDate,GETDATE()))
														When FrequencyUnit='MM' Then DATEADD(MM,Frequency,isnull(NextRunDate,GETDATE()))
														When FrequencyUnit='YY' Then DATEADD(YY,Frequency,isnull(NextRunDate,GETDATE()))
													End
													),	[Status]=@IntState,		
			ModifiedBy=1,ModifiedDate=GetDate(),LastRunTime=GETDATE()
			Where MassModuleCode=@IntProcessID
			If(@@ROWCOUNT=1)
			Begin
				Set @IntpriOutputCode=0;
				Set @StrpriOutputDesc='Process details updated Sucessfully..!';
			End				
	End
	Else 
	Begin
			Update TblMassModule Set  [Status]=@IntState,ModifiedBy=1,ModifiedDate=GetDate(),LastRunTime=GETDATE()
			Where MassModuleCode=@IntProcessID
			If(@@ROWCOUNT=1)
			Begin
				Set @IntpriOutputCode=0;
				Set @StrpriOutputDesc='Process details updated Sucessfully..!';
			End				
	End
	Commit Transaction
			 End Try  
	 BEGIN CATCH  
	 RollBack Transaction
		--SELECT   
		--	ERROR_NUMBER() AS ErrorNumber  
		--	,ERROR_SEVERITY() AS ErrorSeverity  
		--	,ERROR_STATE() AS ErrorState  
		--	,ERROR_PROCEDURE() AS ErrorProcedure  
		--	,ERROR_LINE() AS ErrorLine  
		--	,ERROR_MESSAGE() AS ErrorMessage;  

	INSERT INTO TblErrorDetail([Procedure_Name],Error_Desc,Error_Date,ParameterList) 
	VALUES(ERROR_PROCEDURE(),ERROR_MESSAGE(),GETDATE(),convert(varchar(10),@IntProcessID))
			
	
	END CATCH; 		

END

GO
