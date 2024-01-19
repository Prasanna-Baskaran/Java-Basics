USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_CardAutomationErrorSendEmail]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[Sp_CardAutomationErrorSendEmail] 
@BankName varchar(50)='',
@IssuerNum Varchar(400)='',
@StatusCode varchar(5)='',
@StatusDesc varchar(1000)='',
@RcvEmailID varchar(1000)='',
@StrStatusCode Varchar(3) Output,
@StrStatusDesc Varchar(2000) Output
AS
/*Change Managment
created by : Prerna Patil
Created date: 26/08/2017
Created Reason: Card Automation mail intimation for output file
Execution
Declare @StrOutPutCode Varchar(3) ,@StrOutputDesc Varchar(2000) 
exec [Sp_CardAutomationSendEmail] 'Prabhu Bank','','','','',@StrOutPutCode Output,@StrOutputDesc OutPut
Select @StrOutPutCode,@StrOutputDesc

*/ 
BEGIN
	Begin Try  
-- Check the Card Count on server
----------Email Creation
declare @xml1 nvarchar (max)
declare @body nvarchar (max)
declare @body1 nvarchar (max)
declare @subject nvarchar(max)
--------Common Email Subject

select @subject = 'Card Production Error Alert ' + @@SERVERNAME + '  ' + CONVERT (nvarchar, GETDATE())
-------xml creation for Hard Drive (how data going to be entered into table)
set @xml1 = 
CAST 
(
 (
--select @BankName as 'td','', @StrCardProgram as 'td','', @OutputFileName as 'td','', @OutputFilePath as 'td','' for XML path ('tr'), elements
select @BankName as 'td','', @IssuerNum as 'td','', @StatusCode as 'td','', @StatusDesc as 'td','' for XML path ('tr'), elements

 )
as nvarchar(max)
)

-------Body for Hard Drive (table creation)

set @body1 = 
'<html><body>
<H5>
Dear Team,
<br/><br/>
Please find below output file details for Pin mailer
<br/>
<br/>
Output file details are :
<br/>
</H5>
<table border = 1>
<tr>
<th bgcolor = "#66CCFF"> 
Bank Name
</th>
<th bgcolor = "#66CCFF"> 
Issuer Number
</th>
<th bgcolor = "#66CCFF"> 
Status Code
</th>
<th bgcolor = "#66CCFF"> 
Status Description
</th>
</tr> '
set @body1 = @body1 + @xml1 +'</table>'

Select @body = @body1 +'<br/><H4>Thanks & Regards, <br/> SQL Server </H4>'

---------Email sp

--exec msdb.dbo.sp_send_dbmail
--@profile_name = 'DBA_Notifications',
--@body= @body,
--@body_format= 'HTML',
--@recipients= @RcvEmailID,
--@subject = @subject;

 End Try  
	 BEGIN CATCH 
	 --RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
			INSERT INTO TblCardAutomationErrorLog(Function_Name,Error_Desc,Error_Date,ParameterList,IssuerNo)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'BankName='+ convert(varchar(50),@BankName)+' StatusCode= '+@StatusCode,@BankName
		     SET @strStatusCode='999'        
			 SET @StrStatusDesc='Unexpected error occurred' 
	END CATCH; 
	SELECT @strStatusCode AS [STATUSCODE] ,@strStatusDesc  AS [STATUSDESC]
END

GO
