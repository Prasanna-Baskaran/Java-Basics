USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpCalculateInterest]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpCalculateInterest]
@TranDateTime datetime = null
as
begin

/*
//Description	: Where is customer details? Datewise card wise consolidate update redeem points?
//Queries		: 
*/

if(isnull(@TranDateTime,'') = '')
	SET @TranDateTime = DATEADD(M,-1,getdate())

declare @FromDate as Date = (SELECT DATEADD(MONTH, DATEDIFF(MONTH,0,@TranDateTime),0))
declare @ToDate as Date = (SELECT EOMONTH(@TranDateTime))

Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='Interest Rate Calculation failed.'
BEGIN TRANSACTION
BEGIN TRY

	--
	
	set @StrPriOutput ='0'		
	set @StrPriOutputDesc ='Interest Rate Calculation successful.'

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	set @StrPriOutput ='1'		
	set @StrPriOutputDesc = ERROR_MESSAGE()
END CATCH

Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]

end 
GO
