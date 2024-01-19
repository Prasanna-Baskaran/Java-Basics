USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateInstallment]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[USP_UpdateInstallment] --[dbo].[USP_UpdateInstallment] 7,1,0

@InIntrestRate numeric(18,2),
@Code int ,
@ModifiedBy int 

As    
Begin  

BEGIN TRANSACTION
BEGIN TRY

Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Saving failed.' 



 update  dbo.TblInstallmentPrg  set InIntrestRate =@InIntrestRate ,Modified_By=@ModifiedBy ,Modified=getdate()  where InstPrgCode=@Code
 If(@@ROWCOUNT=1)
				Begin
					Set @StrPriOutput='0'
					Set @StrPriOutputDesc='InterestRate updated successfully.'
				End
				Else
				Begin
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc='InterestRate updation failed.'
				End	
				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
				
				
	COMMIT TRANSACTION;

END TRY

BEGIN CATCH

ROLLBACK TRANSACTION;

SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
END CATCH;  

End    


GO
