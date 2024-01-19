USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[usp_ProcessCardFeeData_Utkarsh]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[usp_ProcessCardFeeData_Utkarsh]
@param char(1),
@Date datetime,
@CustId varchar(20)=null
as
/*  
	Description: Processing Calculated Card Fee charged details.
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 
	Modification:   
*/  
begin

Declare @CardFeeDateWiseData table(
	RowNo [bigint]  NOT NULL,
	[Id] [bigint]   NULL,
	[Pan] [varchar](100) NULL,
	[AccountNo] [varchar](20) NULL,
	[IssuedDate] [datetime] NULL,
	[CustomerId] [varchar](20) NULL,
	[CustomerName] [varchar](1000) NULL,
	[hold_rsp_code] [varchar](2) NULL,
	[FeeAmount] [numeric](10, 2) NULL,
	[FeeType] [varchar](2) NULL,
	[BranchCode] [varchar](20) NULL,
	[SchemeCode] [varchar](20) NULL,
	[Remarks] [varchar](1000) NULL,
	[CardPrefix] [varchar](10) NULL,
	[NewReIssueCardPrefix] [varchar](8) NULL,
	[expiry_date] [varchar](8) NULL,
	[IsCardIssued] [bit] NULL,
	[IsCardexpired] [bit] NULL,
	[IssuerNo] [numeric](9, 0) NULL,
	[CreatedOn] [datetime] NULL,
	ReissuedRequestDate datetime
)

Declare @CardFeeAllData table(
	RowNo [bigint]  NOT NULL,
	[Id] [bigint]   NULL,
	[Pan] [varchar](100) NULL,
	[AccountNo] [varchar](20) NULL,
	[IssuedDate] [datetime] NULL,
	[CustomerId] [varchar](20) NULL,
	[CustomerName] [varchar](1000) NULL,
	[hold_rsp_code] [varchar](2) NULL,
	[FeeAmount] [numeric](10, 2) NULL,
	[FeeType] [varchar](2) NULL,
	[BranchCode] [varchar](20) NULL,
	[SchemeCode] [varchar](20) NULL,
	[Remarks] [varchar](1000) NULL,
	[CardPrefix] [varchar](10) NULL,
	[NewReIssueCardPrefix] [varchar](8) NULL,
	[expiry_date] [varchar](8) NULL,
	[IsCardIssued] [bit] NULL,
	[IsCardexpired] [bit] NULL,
	[IssuerNo] [numeric](9, 0) NULL,
	[CreatedOn] [datetime] NULL,
	ReissuedRequestDate datetime
)

/*Inserting Only matched data into Temp table*/
insert into @CardFeeDateWiseData
select ROW_NUMBER() over (order by CustomerId,IssuedDate) [RowNo], Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
	SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn ,ReissuedRequestDate
from CardFeeCalculation_Utkarsh with(nolock) where 
FeeType=@param and 
(
	cast(IssuedDate as date)=cast(@Date as date) 
	and @param in ('I','R')
) or
(
	DATEPART(MM,IssuedDate)=DATEPART(MM,@Date) and 
	DATEPART(DD,IssuedDate)=DATEPART(DD,@Date) and
	--DATEPART(YYYY,IssuedDate) not between  DATEPART(YYYY,DATEADD(YYYY, -1,@Date))and DATEPART(YYYY,@Date)
	Datepart(YYYY, IssuedDate)<>datepart(YYYY,@Date)
	and @param in ('A')
)

--and CustomerId=@CustId
--order  by CustomerId
--select '@CardFeeDateWiseData',* from @CardFeeDateWiseData 


/*Getting all card data which are matched with date condition*/
insert into @CardFeeAllData
select  ROW_NUMBER() over (partition by C.customerid order by C.customerid,C.issueddate) [RowNo], Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,
    hold_rsp_code,FeeAmount,FeeType,BranchCode,
	SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn   ,ReIssuedRequestDate
from CardFeeCalculation_Utkarsh c with(readpast) 
where CustomerId in (select distinct CustomerId from @CardFeeDateWiseData) and FeeType=@param
	--and CustomerId=@CustId

--select '@CardFeeAllData',* from @CardFeeAllData
--select '@CardFeeDateWiseData',* from @CardFeeDateWiseData
--select '@CardFeeAllData',* from @CardFeeAllData

/*Temporary Cards table to store cards which are matched with scenarios*/
DECLARE @CardFeeTable table(RowNo int, cardno varchar(200))

/*Variable Declaration*/
Declare @HoldRspCode varchar(20)
DECLARE @CustomerID varchar(20) ,@IssuedDate datetime, @FeeAmount numeric(10,2),@AccountNo varchar(50), @CardPrefix varchar(10)
Declare @Cardno varchar(200)
Declare @NewReIssueCardPrefix varchar(10)
declare @InnerMinIndx int,@InnerMaxIndx int 
Declare @IsCardIssued bit=0 
Declare @BaseCard varchar(200)
Declare @LastCard varchar(200)
declare @MinIndx int,@MaxIndx int 
Declare @IsCardReIssued bit=0
Declare @FirstLinkedCard varchar(200)
Declare @LastLinkedCard varchar(200) 
Declare @ReissuedRequestDate Datetime

select @MinIndx=min([RowNo]),@MaxIndx=max([RowNo]) from @CardFeeDateWiseData  
--select @MinIndx ,@MaxIndx

--select '@CardFeeDateWiseData',* from @CardFeeDateWiseData 
--select '@CardFeeAllData',* from @CardFeeAllData
		
if @param='I'
 		begin
			while(@MinIndx<=@MaxIndx) --Looping on @CardFeeDateWiseData Table 
			begin
				set @CustomerID=''
				set @AccountNo=''
				set @CardPrefix=''
				set @IssuedDate=null;
				set @FeeAmount=0;
				set @HoldRspCode=null;
				set @NewReIssueCardPrefix=null;
							
				/*Getting needed details in variable from matched records*/
				SELECT @HoldRspCode=hold_rsp_code, @Cardno=Pan,@CardPrefix=Cardprefix, @NewReIssueCardPrefix=NewReIssueCardPrefix,
					   @CustomerID=customerID ,@IssuedDate=IssuedDate ,@AccountNo=AccountNo,@IsCardIssued=IsCardIssued ,@IsCardReIssued=IsCardIssued 
				from @CardFeeDateWiseData  where RowNo=@MinIndx
				
				--SELECT @HoldRspCode , @Cardno ,@CardPrefix , @NewReIssueCardPrefix , @CustomerID ,@IssuedDate ,@AccountNo ,@IsCardIssued 
				
				truncate table CardFeesDataCustWise_Utkarsh									
				Insert into CardFeesDataCustWise_Utkarsh(Rw,RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
					SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate)
				Select  ROW_NUMBER() over (order by IssuedDate desc ) [Rw],RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
					SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate  
				from @CardFeeAllData where CustomerId=@CustomerID 
				--and accountno=@AccountNo 
				order by IssuedDate desc  --order by IssuedDate desc
				
				--select 'CardFeesDataCustWise_Utkarsh',* from CardFeesDataCustWise_Utkarsh
				IF (isnull(@IsCardReIssued,0) = 0 and  isnull(@HoldRspCode,0) not in ('41','43','54')) 
				Begin
					/*Checking is there any card linked with this card if @FirstLinkedCard is null it means this is first card or this card
					Not having any reisssued card*/
					--select @IssuedDate '@IssuedDate',@CardPrefix '@CardPrefix'
					select @FirstLinkedCard = dbo.[Udf_GetFirstLinkedCard_Utkarsh](@IssuedDate,@CardPrefix) 
					--select @FirstLinkedCard 'FirstLinkCard'
					if (@FirstLinkedCard is null)
					begin				
						if(not exists(select 1 from @CardFeeTable where cardno=@Cardno ))
							Begin
								insert into @CardFeeTable values(@MinIndx ,@Cardno)
							End	
					end
				end								
				SET @MinIndx+=1				
			end				
			--select '@CardFeeTable',* from @CardFeeTable 
			insert into ProcessedCardFeesData_Utkarsh (Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,IssuerNo,CreatedOn)
			select c.Pan,c.AccountNo,c.IssuedDate,c.CustomerId,c.CustomerName,c.hold_rsp_code,c.FeeAmount,c.FeeType,c.IssuerNo,GETDATE ()
			from @CardFeeAllData c  
			inner join @CardFeeTable t on c.Pan=t.cardno 
			where isnull(c.hold_rsp_code,'') not in ('41','43','54') and
			c.Pan not in (select distinct Pan from ProcessedCardFeesData_Utkarsh with(nolock) where FeeType in ('I','R'))
				
		end
else if @param='A'
		begin	
			while(@MinIndx<=@MaxIndx) --Looping on @CardFeeDateWiseData Table 
				begin
					set @BaseCard=null
					set @LastCard=null			
					set @CustomerID=''
					set @AccountNo=''
					set @CardPrefix=''
					set @IssuedDate=null;
					set @FeeAmount=0;
					set @HoldRspCode=null;
					set @NewReIssueCardPrefix=null;
					set @ReissuedRequestDate=null;
								
					/*Getting needed details in variable from matched records*/
					SELECT @HoldRspCode=hold_rsp_code, @Cardno=Pan,@CardPrefix=Cardprefix, @NewReIssueCardPrefix=NewReIssueCardPrefix,
						   @CustomerID=customerID ,@IssuedDate=IssuedDate ,@AccountNo=AccountNo,@IsCardIssued=IsCardIssued ,@ReissuedRequestDate=ReissuedRequestDate
					from @CardFeeDateWiseData  where RowNo=@MinIndx
					
					--SELECT @HoldRspCode , @Cardno ,@CardPrefix , @NewReIssueCardPrefix ,@CustomerID ,@IssuedDate ,@AccountNo ,@IsCardIssued 

					truncate table CardFeesDataCustWise_Utkarsh		
					insert into CardFeesDataCustWise_Utkarsh(Rw,RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
						SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate)
					select  ROW_NUMBER() over (order by IssuedDate desc ) [Rw],RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
						SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate  
					from @CardFeeAllData where CustomerId=@CustomerID 
					--and accountno=@AccountNo 
					order by IssuedDate desc  --order by IssuedDate desc
					Begin
					/*Checking is there any card linked with this card if @FirstLinkedCard is null it means this is first card or this card
					Not having any reisssued card*/
					select @FirstLinkedCard = dbo.[Udf_GetFirstLinkedCard_Utkarsh](@IssuedDate,@CardPrefix) 
					--select @IssuedDate,@CardPrefix
					--select @FirstLinkedCard 'FirstLinkCard'
					
					if @FirstLinkedCard is null /*If @FirstLinkedCard is null it means current matched card is first card then check its last card to apply fees*/
					begin
						select @LastLinkedCard=[dbo].[Udf_GetLastLinkedCard_Utkarsh](@IssuedDate,@ReissuedRequestDate,@NewReIssueCardPrefix) 
						--select @LastLinkedCard '@LastLinkedCard'
						--select @IssuedDate '@LastLinkedCardDT',@ReissuedRequestDate 'RissueDT',@NewReIssueCardPrefix
						if (@LastLinkedCard is not null) /*If @LastLinkedCard is not null it means this is last card so apply fees on last card active card*/
						begin	
							set @Cardno=@LastLinkedCard
						end							
						if(not exists(select 1 from @CardFeeTable where cardno=@Cardno ))
						Begin
							insert into @CardFeeTable values(@MinIndx ,@Cardno)
						End
					end					
				end
				SET @MinIndx+=1				
				end				
				
				insert into ProcessedCardFeesData_Utkarsh (Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,IssuerNo,CreatedOn)
				select c.Pan,c.AccountNo,c.IssuedDate,c.CustomerId,c.CustomerName,c.hold_rsp_code,c.FeeAmount,c.FeeType,c.IssuerNo,GETDATE ()
				from @CardFeeAllData c  inner join @CardFeeTable t 	on c.Pan=t.cardno 
				where 
				isnull(c.hold_rsp_code,'') not in ('41','54','43') and
				c.Pan not in (select distinct Pan from ProcessedCardFeesData_Utkarsh  with(nolock) where FeeType=@param)
		end
else 
	begin
		while(@MinIndx<=@MaxIndx) --Looping on @CardFeeDateWiseData Table 
		begin
			set @BaseCard=null
			set @LastCard=null	
			set @CustomerID=''
			set @AccountNo=''
			set @CardPrefix=''
			set @IssuedDate=null;
			set @FeeAmount=0;
			set @HoldRspCode=null;
			set @NewReIssueCardPrefix=null;
						
			/*Getting needed details in variable from matched records*/
			SELECT @HoldRspCode=hold_rsp_code, @Cardno=Pan,@CardPrefix=Cardprefix, @NewReIssueCardPrefix=NewReIssueCardPrefix,
				   @CustomerID=customerID ,@IssuedDate=IssuedDate ,@AccountNo=AccountNo,@IsCardIssued=IsCardIssued 
			from @CardFeeDateWiseData  where RowNo=@MinIndx
			
			--SELECT @HoldRspCode , @Cardno ,@CardPrefix , @NewReIssueCardPrefix , @CustomerID ,@IssuedDate ,@AccountNo ,@IsCardIssued 
			
			truncate table CardFeesDataCustWise_Utkarsh					
			insert into CardFeesDataCustWise_Utkarsh(Rw,RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
				SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate)
			select  ROW_NUMBER() over (order by IssuedDate desc ) [Rw],RowNo,Id,Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,BranchCode,
				SchemeCode,Remarks,CardPrefix,NewReIssueCardPrefix,expiry_date,IsCardIssued,IsCardexpired,IssuerNo,CreatedOn,ReIssuedRequestDate  
			from @CardFeeAllData where CustomerId=@CustomerID 
			--and accountno=@AccountNo 
			order by IssuedDate desc  --order by IssuedDate desc
			
		IF (isnull(@IsCardReIssued,0) = 0 and  isnull(@HoldRspCode,0) not in ('41','43','54')) 
			Begin
				/*Checking is there any card linked with this card if @FirstLinkedCard is null it means this is first card or this card
				Not having any reisssued card. If @FirstLinkedCard is not null it mean this card is reissued card */
				--select @IssuedDate,@CardPrefix
				select @FirstLinkedCard = dbo.[Udf_GetFirstLinkedCard_Utkarsh](@IssuedDate,@CardPrefix) 
				--select @FirstLinkedCard 'FirstLinkCard'
				if (@FirstLinkedCard is not null and @NewReIssueCardPrefix is null) /*@NewReIssueCardPrefix is null means this is the last card*/
				begin				
					if(not exists(select 1 from @CardFeeTable where cardno=@Cardno ))
						Begin
							--select 'done'
							insert into @CardFeeTable values(@MinIndx ,@Cardno)
						End	
				end
			end		
			SET @MinIndx+=1
		end			
		--select '@CardFeeAllData',* 	from @CardFeeAllData 
			--select * from #CardFeeDateWiseData where RowNo=@MinIndx			
			insert into ProcessedCardFeesData_Utkarsh (Pan,AccountNo,IssuedDate,CustomerId,CustomerName,hold_rsp_code,FeeAmount,FeeType,IssuerNo,CreatedOn)			
			select c.Pan,c.AccountNo,c.IssuedDate,c.CustomerId,c.CustomerName,c.hold_rsp_code,c.FeeAmount,c.FeeType,c.IssuerNo,GETDATE ()
			from @CardFeeAllData c  inner join @CardFeeTable t 	on c.Pan=t.cardno and c.IsCardexpired=0 
			where   isnull(hold_rsp_code,'') not in ('41','43','54') and
			c.Pan not in (select distinct Pan from ProcessedCardFeesData_Utkarsh  with(nolock) where FeeType in ('I','R'))	
	end
end
