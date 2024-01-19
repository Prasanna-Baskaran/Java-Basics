
CREATE FUNCTION [dbo].[Udf_GetLastLinkedCard_utkarsh](@IssuedDate datetime,@ReissuedRequestDate datetime,@NewCardPrefix varchar(10))
RETURNS varchar(400)
AS
BEGIN
DECLARE @Cardno varchar(400)
Declare @NextExist int=1
while(@NextExist>0)
BEGIN
		
	select @NextExist= COUNT(1)
	from CardFeesDataCustWise_Utkarsh with(nolock) where IssuedDate > @IssuedDate and  IssuedDate > @ReissuedRequestDate
	and CardPrefix = isnull(@NewCardPrefix,'')	
	if(@NextExist =0 )
	Begin
		break;
	End
	else
	Begin
		select @Cardno=Pan,@NewCardPrefix=NewReIssueCardPrefix,@IssuedDate=IssuedDate ,@ReissuedRequestDate=ReIssuedRequestDate
		from CardFeesDataCustWise_Utkarsh with(nolock) where IssuedDate > @IssuedDate and CardPrefix = isnull(@NewCardPrefix,'')			
	End
End	
RETURN @Cardno
END



GO


