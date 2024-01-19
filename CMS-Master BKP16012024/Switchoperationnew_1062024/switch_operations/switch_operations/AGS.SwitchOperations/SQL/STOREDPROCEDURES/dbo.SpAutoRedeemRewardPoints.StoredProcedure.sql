USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpAutoRedeemRewardPoints]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpAutoRedeemRewardPoints]
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
Declare @StrPriOutputDesc varchar(200)='Reward Redeem failed.'
BEGIN TRANSACTION
BEGIN TRY

	--Card type wise timit to be fetch
	--Fetch cards having reward points greater than rewards points in config
	--Update to switch and change
	--deduct points for those cards

	-- Fetch Card type wise limit
	-- Fetch Card wise monthly consolidated reward points
	-- Update card amount/reward point (This is affect in switch)
	-- From where amount will be deduct.... how to find out transaction of which card type
	
	set @StrPriOutput ='0'		
	set @StrPriOutputDesc ='Reward update successful.'

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
