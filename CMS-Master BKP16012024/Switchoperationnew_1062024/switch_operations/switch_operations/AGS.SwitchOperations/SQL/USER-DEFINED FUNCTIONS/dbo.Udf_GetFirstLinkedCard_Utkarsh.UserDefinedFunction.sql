
CREATE FUNCTION [dbo].[Udf_GetFirstLinkedCard_Utkarsh](@IssuedDate datetime,@NewReIssueCardPrefix varchar(10))
RETURNS varchar(400)
AS
BEGIN
	--Declare @IssuedDate datetime,@NewReIssueCardPrefix varchar(10)
	--set @IssuedDate='2016-12-15 13:08:57.000'
	--set @NewReIssueCardPrefix='51229351'
--select* from CardFeesDataCustWise_Utkarsh with(nolock) 
	DECLARE @Cardno varchar(400)
	Declare @NextExist int=1

	while(@NextExist>0)
	BEGIN	
		/*Checking is there any previous cards is available on the basis of issue date of current card and cardprefix*/
		select @NextExist= COUNT(1)
		from CardFeesDataCustWise_Utkarsh with(nolock) where IssuedDate < @IssuedDate and ReIssuedRequestDate < @IssuedDate 
		and NewReIssueCardPrefix = isnull(@NewReIssueCardPrefix,'')	
		
		if(@NextExist =0)
		Begin
			break;
		End
		else
		Begin
			select @Cardno=Pan,@NewReIssueCardPrefix=CardPrefix,@IssuedDate=IssuedDate 
			from CardFeesDataCustWise_Utkarsh with(nolock) where IssuedDate < @IssuedDate and 
			isnull(NewReIssueCardPrefix,'') =  isnull(@NewReIssueCardPrefix,'')	
		End
	End	
	return @Cardno
END
GO


