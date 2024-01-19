USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpUpdateCourierDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpUpdateCourierDetails] 

 @CourierName  varchar(70),
 @OfficeName   varchar(100),
 @MobileNo     varchar(12),
 @Status       int ,
 @ModifiedBy   int,
 @CourierId    int,
 @SystemID Varchar(200)=1,
 @BankID Varchar(200)=1

AS  
BEGIN  
BEGIN TRANSACTION

	Begin Try  

			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)='Courier details updation failed.'
			
			
			 update  TblCourier 
	           set CourierName =@CourierName,Officeaddress =@OfficeName, ContactNo=@MobileNo,Status=@Status,
	           LastModifieddate=getdate(), ModifiedBy=@ModifiedBy
	         where ID=@CourierId AND SystemID=@SystemID AND BankID=@BankID
               
               set @StrOutput ='0'
	           		
				 Set @StrOutputDesc=  'Courier details is updated successfully.'	
	           	
	           	
			 
			 
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
