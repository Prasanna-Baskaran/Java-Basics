USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[SpSetCourierDetails]    Script Date: 08-06-2018 16:58:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpSetCourierDetails] 
 
 @CourierName  varchar(70),
 @OfficeName   varchar(100),
 @MobileNo     varchar(12),
 @Status       int ,
 @CreatedBy    int,
 @SystemID Varchar(200)=1,
 @BankID Varchar(200)=1


AS  
BEGIN  
BEGIN TRANSACTION

	Begin Try  
	
	
			Declare @StrOutput varchar(1)='1'		
			Declare @StrOutputDesc varchar(200)=''
			
	        if Exists(select ID from dbo.tblcourier with(nolock) where CourierName=@CourierName AND SystemID=@SystemID AND BankID=@BankID  )
	        begin
	        
	                    Set @StrOutput='1'
						
						Set @StrOutputDesc='Courier Name is already exists.'
	        
	        end
	        
	       
	        else
	          begin
	       
	           insert into Tblcourier (CourierName,OfficeAddress,ContactNo,status,CreatedDate,CreatedBy,SystemID,BankID) 
                  values(@CourierName,@OfficeName,@MobileNo,@Status,getdate(),@CreatedBy,@SystemID,@BankID)
	           	 if(@@rowcount>0)
	           	 begin
	           	  set @StrOutput ='0'
	           		
				 Set @StrOutputDesc=  'Courier Name is added successfully.'
	           	 end
	           	 
	           	 else
	           	 begin
	           	  set @StrOutput ='1'
	           		
				 Set @StrOutputDesc=  'Courier Name is  not added.'
	           	 end
	           	
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
