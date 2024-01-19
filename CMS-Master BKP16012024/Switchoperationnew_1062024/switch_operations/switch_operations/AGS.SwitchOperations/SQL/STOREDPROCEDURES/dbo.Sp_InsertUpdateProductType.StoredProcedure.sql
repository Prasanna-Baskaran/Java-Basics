USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsertUpdateProductType]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_InsertUpdateProductType]
	@ID INT =0,
	@BIN_ID int=0,
	@INST_ID int=0,
	@CardType_ID int=0,
	@ProductType VARCHAR(50)='',
	@ProductTypeDesc VARCHAR(50)='',
	@MakerID BIGINT=NULL    ,
	@SystemID VARCHAR(200)=1,
	@BankID Varchar(200)=1

AS
BEGIN
Begin Transaction  
	Begin Try
	Declare @StrPriOutput varchar(1)='1'					
	Declare @StrPriOutputDesc varchar(200)='Product type is not saved'

  IF(@INST_ID <> 0 AND @CardType_ID<>0 AND @BIN_ID<>0 AND @ProductType <>'' AND @ProductTypeDesc <>'')
  BEGIN  
    IF(@ID = 0)
	 BEGIN
	 --Add new Product type
	   --If NOT EXISTS (SELECT top 1 1 FROM TblProductType WITH(NOLOCK) WHERE ((RTRIM(LTRIM(ProductType))=RTRIM(LTRIM(@ProductType))) AND INST_ID=@INST_ID) AND SystemID=@SystemID AND BankID=@BankID )
	   If NOT EXISTS (SELECT top 1 1 FROM TblProductType WITH(NOLOCK) WHERE (BIN_ID=@BIN_ID OR (REPLACE(UPPER(RTRIM(LTRIM(ProductType))),' ','')=REPLACE(UPPER(RTRIM(LTRIM(@ProductType))),' ',''))) AND SystemID=@SystemID AND BankID=@BankID )
	   BEGIN
	
	     INSERT INTO  TblProductType (CardType_ID,BIN_ID,INST_ID,ProductType,ProductTypeDesc,MakerID,CreatedDate,SystemID,BankID) 
		 VALUES(@CardType_ID,@BIN_ID,@INST_ID,@ProductType,@ProductTypeDesc,@MakerID,GETDATE(),@SystemID,@BankID)
		 
		 IF(@@ROWCOUNT>0)
		 BEGIN
	
		    SET @StrPriOutput ='0'					
	        SET @StrPriOutputDesc ='Product type is saved'
		 END
		 ELSE
		  BEGIN
		    SET @StrPriOutput ='1'					
	        SET @StrPriOutputDesc ='Product type is not saved'
		  END
	   END
	   ELSE
	   BEGIN
	       SET @StrPriOutput ='1'					
	       SET @StrPriOutputDesc ='Product type/card prefix is already exists'
	   END
	 END
	 --Update Product type
	 ELSE IF EXISTS(SELECT 1 FROM TblProductType WITH(NOLOCK) WHERE ID=@ID AND bankID=@BankID AND SystemID=@SystemID)
	 BEGIN
	   UPDATE TblProductType SET ProductType=@ProductType,BIN_ID=@BIN_ID,INST_ID=@INST_ID,ProductTypeDesc=@ProductTypeDesc,ModifiedByID=@MakerID,ModifiedDate=GETDATE(),FormStatusID=0,Remark='' WHERE ID=@ID  AND BankID=@BankID AND SystemID=@SystemID
	     SET @StrPriOutput ='0'					
	     SET @StrPriOutputDesc ='Product type is updated successfully'
	 END
	 ELSE
	 BEGIN
	      SET @StrPriOutput ='1'					
	      SET @StrPriOutputDesc ='Product type is not saved'
	 END
  END
  ELSE
  BEGIN
     SET @StrPriOutput ='1'					
	SET @StrPriOutputDesc ='Product type is not saved'
  END

  Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription]
COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
	 		SELECT 1  As Code,'Product type is not saved' As [OutputDescription]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  



END

GO
