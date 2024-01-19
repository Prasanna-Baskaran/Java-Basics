USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_CardFeeCalculation_Utkarsh]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[USP_CardFeeCalculation_Utkarsh]
@Param char(1),
@IssuerNo numeric(9),
@FromDate datetime,
@TODate datetime
as
/*  
	Description: Calculating Card Fee charging for utkarsh bank
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 
	Modification:   
*/  
/* Param 'I' for NTB ie. after 30 days 
   Param 'R' for NTB ie. T+1 
   Param 'A' for Annual charge
*/
begin

	/*Archive CardFeeCalculation table*/
	set nocount on 

		Declare @FeeID Bigint
		Declare @Cnt Bigint
		select  @FeeID=ISNULL(MAX(FeeID),0)+1 from CardFeeCalculation_Utkarsh_archive with(nolock)
		select  @Cnt =count(1) from CardFeeCalculation_Utkarsh with(nolock)
		
		insert into CardFeeCalculation_Utkarsh_archive (Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,
					BranchCode,SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,
					FeeID,ArchiveDate,Cnt,ReIssuedRequestDate)
		select  Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,
					BranchCode,SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,
					@FeeID,GETDATE(),@cnt,ReIssuedRequestDate from CardFeeCalculation_Utkarsh with(nolock)
		truncate table CardFeeCalculation_Utkarsh

	/*Getting Card Details from switch*/	
	select * into #CardDetails from dbo.Udf_GetCardDetailsToCharge(@FromDate,@TODate,@IssuerNo,@Param)
	 
		--select * from #CardDetails 
		/*Getting Card Reissue details*/
		select * into #CardReissueDetails from 
					(select ROW_NUMBER()over(partition by OldPan order by [Date] desc)rw,
						OldPan Pan,Remark Remarks,NewBinPrefix BIN, Date [ReIssuedRequestDate] from TblCardGenRequest_History with(nolock) 
						where OldPan in (select distinct EncPan from #CardDetails)					
					)a where rw=1
		
		--select *  from TblCardGenRequest_History with(nolock) 
		insert into CardFeeCalculation_Utkarsh(pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,SchemeCode,Remarks,
		CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate)		
		select Encpan,AccountNo, date_issued,customer_id,Customer_Name, hold_rsp_code,m.FeeAmount,@Param FeeType,''Branchcode,'' schemecode,'' remarks,
		LEFT(cardno,8) CardPrefix,
		case when r.BIN is not null then r.BIN  else null end NewReIssueCardPrefix,
		expiry_date,	
		case when r.BIN is not null then 1 else 0 end IsCardIssued,
		case when cast(expiry_date+'01' as datetime)> @FromDate then 0 else 1 end IsCardexpired,
		@IssuerNo IssuerNo, GETDATE() CreatedOn	,
		r.ReIssuedRequestDate
		from 
		#CardDetails C with(nolock) 
		left join CardFeeMaster_Generic m with(nolock) on LEFT(c.cardno,8)=m.Bin and m.feecategory=@Param
		left join #CardReissueDetails r with(nolock) on r.pan=C.Encpan
		
		drop table #CardReissueDetails
		drop table #CardDetails		
		exec usp_ProcessCardFeeData_Utkarsh @Param,@FromDate--,@TODate 
end
