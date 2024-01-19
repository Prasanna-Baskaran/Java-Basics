USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CardAutomation]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_CardAutomation] 
@IntType SmallInt=0
AS
BEGIN
	Begin Try  
	If(@IntType=0)
	Begin
		Declare @Standard2Batch varchar(100),@Standard2ExePath varchar(Max),@Standard2Input varchar(Max),@Standard2Archive varchar(Max)

		Select @Standard2Batch=[Standard2Batch] ,	@Standard2ExePath=[Standard2ExePath],	@Standard2Input=[Standard2Input] ,	@Standard2Archive=[Standard2Archive]
		From [TblCardAutomation]
	--	select @Standard2Batch + @Standard2ExePath + @Standard2Input +@Standard2Archive

		exec TEST.postcard.dbo.[usp_CardAutomation_AGS] 0,@Standard2Batch,@Standard2ExePath ,@Standard2Input ,@Standard2Archive
	END
	Else If(@IntType=1)
	Begin
		Declare @Standard4Batch varchar(100),@Standard4ExePath varchar(Max),@Standard4Output  varchar(Max)

		Select @Standard4Batch=[Standard4Batch] ,	@Standard4ExePath=[Standard4ExePath],	@Standard4Output=Standard4Output 
		From [TblCardAutomation]
		select @Standard4Batch + @Standard4ExePath + @Standard4Output 

		exec TEST.postcard.dbo.[usp_CardAutomation_AGS] 1,@Standard4Batch,@Standard4ExePath ,@Standard4Output ,''
	END
	--COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH; 
END
GO
