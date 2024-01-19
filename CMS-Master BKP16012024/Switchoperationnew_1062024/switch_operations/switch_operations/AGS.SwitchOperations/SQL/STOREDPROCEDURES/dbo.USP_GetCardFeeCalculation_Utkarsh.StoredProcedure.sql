USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCardFeeCalculation_Utkarsh]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--USP_GetCardFeeCalculation_Utkarsh 'I'
CREATE procedure [dbo].[USP_GetCardFeeCalculation_Utkarsh]
@Param char(1)
as
/*  
	Description: This store procedure gives calculated debit card fees.
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 
	Modification:  
*/  
/*
	Description: This store procedure gives calculated debit card fees.
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 15-12-2017
	Modification: Added GST Change.  
*/  

begin
	
	DECLARE @SERTAX Numeric (18,2)
	SELECT @SERTAX = TaxInPercent FROM TaxDetails where TaxType = 'GST'

	select c.AccountNo [Account No] , left(dbo.ufn_DecryptPAN(DecPAN),6)+'******'+ RIGHT( dbo.ufn_DecryptPAN(DecPAN),4) [Card No],
		   t.FeeDesc [Fee Type],ISNULL(c.FeeAmount,0) FeeAmount ,
		   CAST(CAST(isnull(cast(ISNULL(c.FeeAmount,0) as numeric(9,2))* @SERTAX ,0) as decimal(10,2)) / 100 as decimal(10,2))GST
	from ProcessedCardFeesData_Utkarsh c with(nolock) 
		   inner join CardRPAN p with(nolock) on c.Pan=p.EncPAN
		   inner join CardFeeType_Utkarsh t with(nolock) on c.FeeType=t.FeeCategory
	where cast(CONVERT(VARCHAR(10),CreatedOn,110) as datetime)= cast(CONVERT(VARCHAR(10),GETDATE() ,110) as datetime) /*Date condition*/
		   and FeeType in (@Param) order by FeeType,IssuedDate
end

GO
