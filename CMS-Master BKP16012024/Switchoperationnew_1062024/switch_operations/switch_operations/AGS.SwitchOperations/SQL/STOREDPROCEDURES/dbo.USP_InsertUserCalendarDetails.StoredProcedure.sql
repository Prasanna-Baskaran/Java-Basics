USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertUserCalendarDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[USP_InsertUserCalendarDetails]--[dbo].[USP_InsertInstallment]  2,6
@Date date,   
@HolidayName varchar(150) ,
@CreatedBy int 


As    
Begin  

BEGIN TRANSACTION
BEGIN TRY 

Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Addition of `user holiday date failed.'

if exists(select Code from dbo.Tbl_UserCalendar with(nolock) where Userdate=@Date)
begin
Set @StrPriOutput='2'
Set @StrPriOutputDesc='User holiday date already added.'

end
else
begin

 insert into dbo.Tbl_UserCalendar (UserDate,HolidayName,CreatedOn,CreatedBy) values(@Date,@HolidayName,GETDATE(),@CreatedBy)
 If(@@ROWCOUNT=1)
				Begin
					Set @StrPriOutput='0'
					Set @StrPriOutputDesc='Holiday date details  added successfully.'
				End
				Else
				Begin
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc='Holiday date details addition failed.'
				End	
				
	
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

end 

 


GO
