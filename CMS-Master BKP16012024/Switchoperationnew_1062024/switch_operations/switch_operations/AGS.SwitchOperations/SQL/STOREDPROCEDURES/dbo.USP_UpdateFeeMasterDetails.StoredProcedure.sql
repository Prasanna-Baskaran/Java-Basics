USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateFeeMasterDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[USP_UpdateFeeMasterDetails] --[dbo].[USP_UpdateFeeMasterDetails] 101,1,2

@FeeAmount numeric(18,2),
@FeeTypeCode int  ,
@ModifiedBy int

As    
Begin 
BEGIN TRANSACTION
BEGIN TRY
 
Declare @StrPriOutput varchar(1)='1'		
Declare @StrPriOutputDesc varchar(200)='' 
Declare @Feecode int
Declare @FeeName varchar(max)
Declare  @IsPercAmount int
select @FeeName= FeeName from TblFeeMaster WITH(NOLOCK) where FeeCode=@FeeTypeCode

select @IsPercAmount=isPercAmount from TblFeeDetails where FeeCode=@FeeTypeCode

print(@IsPercAmount)

if(@IsPercAmount=1)

begin 
if(@FeeAmount>0 and @FeeAmount<=100)
begin
update  dbo.TblFeeDetails set InterestRate =@FeeAmount ,ModifiedBy=@ModifiedBy , ModifiedDate=getdate()  where Feecode=@FeeTypeCode
Set @StrPriOutput='0'
Set @StrPriOutputDesc=@FeeName+  '  '+ ' '+('Amount Set Successfully.')
print(2)
end

else
begin
Set @StrPriOutput='1'
Set @StrPriOutputDesc=@FeeName+  '  '+ ' '+('Invalid Percentage')
print(1)
end
end
else
begin
update  dbo.TblFeeDetails set Amount =@FeeAmount ,ModifiedBy=@ModifiedBy , ModifiedDate=getdate()  where Feecode=@FeeTypeCode
 If(@@ROWCOUNT=1)
				Begin
					Set @StrPriOutput='0'
					Set @StrPriOutputDesc=@FeeName+  '  '+ ' '+('Amount Set Successfully.')
				End
				Else
				Begin
					Set @StrPriOutput='1'
					Set @StrPriOutputDesc=@FeeName+  '  '+ ' '+ 'Amount Set  Failed.'
				End
				
end					
				Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
				
	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH

ROLLBACK TRANSACTION;

SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
END CATCH;  			
	
End    


GO
