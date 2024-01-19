USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertInstallment]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[USP_InsertInstallment]--[dbo].[USP_InsertInstallment]  2,6
@InMonths int,   
@InIntrestRate numeric(18,2) ,
@CreatedBy int 



AS    
BEGIN   

BEGIN  TRANSACTION

BEGIN  TRY

Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Addition of interest rate failed.'

if exists(select instprgcode from dbo.TblInstallmentPrg with(nolock) where InMonths=@InMonths)
BEGIN
Set @StrPriOutput='2'
Set @StrPriOutputDesc=' Installment interest rate already set for selected month'
END


ELSE
BEGIN
if(@InMonths>12)
BEGIN
Set @StrPriOutput='0'
Set @StrPriOutputDesc='Entered month is not valid'
END

ELSE
BEGIN
 insert into dbo.TblInstallmentPrg (InMonths,InIntrestRate,Created_By,Created) values(@InMonths,@InIntrestRate,@CreatedBy,getdate())
 If(@@ROWCOUNT=1)
				Begin
					Set @StrPriOutput='0'
					Set @StrPriOutputDesc='Interest rate for selected month is set successfully.'
				End
				Else
				Begin
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc='Interest rate for selected month is failed.'
				End	
				
	
End  
END

Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]

COMMIT TRANSACTION;

END TRY

BEGIN CATCH

ROLLBACK TRANSACTION;

SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
END CATCH;  

END  



GO
