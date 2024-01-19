USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetFeeMaster]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpSetFeeMaster] 
 @FeeNameCode int,       
 @FeeAmount numeric(18,2),
 @CreatedBy  int,
 @AmountPercentageFlag int

 
 

AS  
BEGIN  
BEGIN TRANSACTION
BEGIN TRY
     
    
			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)=''
			declare  @FeeMasterCode numeric(18,0)
			
Declare @FeeName varchar(150)
select @FeeName= FeeName from TblFeeMaster WITH(NOLOCK) where FeeCode=@FeeNameCode

  
	
	        if Exists(select FeeCode from dbo.TblFeeDetails with(nolock) where FeeCode=@FeeNameCode )
	        begin
	        Set @StrOutput='1'
						
						Set @StrOutputDesc= @FeeName +' '+' '+ 'Amount already set for selected FeeType.'
	        
	        end
	        else
	        begin
	         if (@AmountPercentageFlag = 1)
	         begin
	           insert into dbo.TblFeeDetails (FeeCode,InterestRate,CreatedDate,CreatedBy,IspercAmount) values(@FeeNameCode,@FeeAmount,getdate(),@CreatedBy,@AmountPercentageFlag)
	           	end
	           	else
	           	begin
	           	insert into dbo.TblFeeDetails (FeeCode,Amount,CreatedDate,CreatedBy,IspercAmount) values(@FeeNameCode,@FeeAmount,getdate(),@CreatedBy,@AmountPercentageFlag)
	           	end
				 Set @StrOutputDesc= @FeeName +' '+' '+  'Amount set successfully.'
			 end
				Select @StrOutput As Code,@StrOutputDesc As [OutputDescription]

	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH

ROLLBACK TRANSACTION;

SELECT 1  As Code,'Error occurs.' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
END CATCH;  	
   
END  

GO
