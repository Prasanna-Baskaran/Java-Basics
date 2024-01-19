USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteUserCalendarDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[USP_DeleteUserCalendarDetails]--[dbo].[USP_InsertInstallment]  2,6
@Date date,   
@HolidayName varchar(150)
 


As    
Begin  
 BEGIN TRANSACTION
 BEGIN TRY
Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Deletion Of `Holiday Date Failed.'

if exists(select Code from dbo.Tbl_UserCalendar where Userdate=@Date and HolidayName=@HolidayName)
begin
 
 delete from dbo.Tbl_UserCalendar  where Userdate=@Date and HolidayName=@HolidayName


Set @StrPriOutput='2'
Set @StrPriOutputDesc=' Holiday Date Deleted Successfully'

end
else

				Begin
					Set @StrPriOutput='0'
					Set @StrPriOutputDesc='Holiday Date is not Added'
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
