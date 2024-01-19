USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_UploadCardFeesData_Utkarsh]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[USP_UploadCardFeesData_Utkarsh]
	@Type_CardFeeMaster Type_CardFeeMaster_Utkarsh readonly,
	@FileName varchar(1000),
	@issuerNO numeric(9)
as
/*  
	Description: This function provide bankwise customer details
	Author: Pratik Mhatre	  
	Create Date: 30-08-2017
	Modified Date: 
	Modification:   
*/  
begin

	--select * into Temp_29122017 from @Type_CardFeeMaster
	
	Declare @tblCard_FeeMaster table(
					id int identity(1,1),
					Bin varchar(10) ,
					FeeAmount varchar(20),
					FeeCategory varchar(10))		
				
	insert into @tblCard_FeeMaster
	select c.Bin, c.FeeAmount,c.feecategory from @Type_CardFeeMaster c 
			left join CardFeeType_Utkarsh t with(nolock) on c.FeeCategory collate SQL_Latin1_General_CP1_CI_AS=t.FeeCategory
			left join AllowedSchemeCodeCardProduction a with(nolock) on c.bin=a.CardPrefix
			where ISNUMERIC(FeeAmount)<>1 /*Checking FeeAmount is numeric or not*/ 
				  or ISNUMERIC(Bin)<>1  /*Checking BIN is numeric or not*/ 
				  or t.FeeCategory is null /*Checking is entered fee category is valid or not*/ 
				  or LEN(c.Bin)<>8  /*Checking length of BIN*/ 
				  or a.CardPrefix is null /*Checking entered cardprefix is present in AllowedSchemeCodeCardProduction or not*/ 
				  
	if ((select COUNT(1) from @tblCard_FeeMaster) =0)
	begin
			/*Archive CardFeeMaster table if any update or insert request occured*/
			Declare @ArchiveID Bigint
			select  @ArchiveID=ISNULL(MAX(FeeID),0)+1 from CardFeeMaster_Generic_archive with(nolock)
			
			insert into CardFeeMaster_Generic_archive
			select *,@ArchiveID,GETDATE() from CardFeeMaster_Generic
			
	
			insert into @tblCard_FeeMaster
			select c.Bin, c.FeeAmount,c.feecategory from @Type_CardFeeMaster c			
			Declare @min as int
			Declare @max as int
			Declare @Bin as varchar(10)
			Declare @FeeAmount as varchar(20)
			Declare @FeeCategory as varchar(5)			
			select @min=MIN(id),@max=Max(id) from @tblCard_FeeMaster
			while (@min<=@max)
			begin
				set @Bin=''
				set @FeeAmount=''
				set @FeeCategory=''				
				select @Bin=BIn,@FeeAmount=FeeAmount,@FeeCategory=FeeCategory from @tblCard_FeeMaster where id=@min				
				if exists(select top 1 1 from CardFeeMaster_Generic c with(nolock) where IssuerNo=@issuerNO and bin=@Bin and FeeCategory=@FeeCategory)
				   begin
						update CardFeeMaster_Generic set FeeAmount=@FeeAmount,ModifiedDate=GETDATE(),ModifiedBy=1 where  IssuerNo=@issuerNO and bin=@Bin and FeeCategory=@FeeCategory
				   end
				else
				   begin
						insert into CardFeeMaster_Generic (IssuerNo ,Bin,FeeAmount,FeeCategory,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,IsActive)
						values(@issuerNO,@Bin,@FeeAmount,@FeeCategory,1,1,GETDATE(),GETDATE(),1)
				   end
				set @min=@min+1		
			end			
			
	end
	else
	begin
		/*If there is any invalid data in file*/
		select Bin,FeeAmount,FeeCategory from @tblCard_FeeMaster		
	end		
end


GO
