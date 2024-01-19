USE [SwitchOperations]
GO
/****** Object:  UserDefinedFunction [dbo].[Udf_GetBaseOrLastCard]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[Udf_GetBaseOrLastCard](@CustId varchar(20),@AccountId varchar(20),@type char(1))
RETURNS varchar(200)
AS
/*  
	Description: Give Base and last card.
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 
	Modification:   
	
	Type 'B' for base card
	Type 'L' for base card
*/  
BEGIN
DECLARE @Cardno varchar(200)

if @type='B'
begin
	select top 1 @Cardno=Pan from CardFeesDataCustWise_Utkarsh with(nolock) 
	where CustomerId=@CustId and AccountNo=@AccountId
	order by IssuedDate
end
else
begin
	select top 1 @Cardno=Pan from CardFeesDataCustWise_Utkarsh with(nolock) 
	where CustomerId=@CustId and AccountNo=@AccountId and isnull(IsCardIssued,0) = 0
		and isnull(hold_rsp_code,0) not in ('41','43','54') 
	order by IssuedDate desc
end
RETURN @Cardno
END


GO
