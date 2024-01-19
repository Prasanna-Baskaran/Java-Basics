USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCustomerToProcess]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_GetCustomerToProcess]
	@ID Bigint
AS
BEGIN
Begin Transaction 
	 DECLARE @CustomerID Bigint 
 Begin Try  

	DECLARE @SystemID VARCHAR(200)
	DECLARE @BankID VARCHAR(200)
	DECLARE @ProductTypeID VARCHAR(200)

	DECLARE @Authorized smallint=4

	Select TOP 1 @SystemID=Bi.SystemID,@BankID=Bi.BankID ,@ProductTypeID=P.ID
	from TblBulkCustomerInfo B WITH(NOLOCK)
	INNER JOIN TblBin Bi WITH(NOLOCK) ON RTRIM(LTRIM(B.Cardprefix))=RTRIM(LTRIM(Bi.CardPrefix))
	INNER JOIN TblProductType P WITH(NOLOCK) ON P.BIN_ID=Bi.ID

	------ save into cust details table
	INSERT INTO TblCustomersDetails (FirstName,LastName,MobileNo,DOB_AD,Nationality,GenderID,MaritalStatusID,PassportNo_CitizenShipNo,IssueDate_District,FormStatusID,Maker_Date_IND,MakerID,ProductType_ID,NameOnCard,BulkUpload,BankID,SystemID,cardtypeID,INST_ID,AccNo,AccType)
	SELECT FirstName,LastName,MobileNo,cast(right(DOB, 4)+substring(DOB, 3, 2)+left(DOB, 2) as datetime),Nationality,Gender,0,Passport_IdentiNo,LEFT(RTRIM(LTRIM(IssueDate)),2)+'/'+Substring(RTRIM(LTRIM(IssueDate)),3,2)+'/'+RIGHT(IssueDate,4),0,GETDATE(),0,@ProductTypeID,NameOnCard ,1,@BankID,@SystemID,0,0
	,Case When ISNULL(RTRIM(LTRIM(AccountNo)),'')<>'' Then dbo.ufn_encryptPAN(accountNo) ELSE NULL END ,AccountType from TblBulkCustomerInfo
	where ID=@ID

	SET @CustomerID=SCOPE_IDENTITY()
	IF(@CustomerID>0)
	BEGIN
	 --start Diksha Acc logic change
	 UPDATE TblCustomersDetails SET ApplicationFormNo=dbo.FunGetApplicationNo(@CustomerID),AccNo=dbo.FunGetAccountNo(@CustomerID),bankcustID=dbo.FunGetBankCustID(@CustomerID)  WHERE CustomerID=@CustomerID
		------ save into cust address table
	  INSERT INTO TblCustomerAddress (CustomerID,HouseNo_P,StreetName_P,City_P,District_P,IsSameAsPermAddr,HouseNo_C,StreetName_C,City_C,District_C,Email_P,Email_C )
	  SELECT @CustomerID,HouseNo,StreetName,City,District,1,HouseNo,StreetName,City,District,Email,Email From TblBulkCustomerInfo WITH(NOLOCK) where ID=@ID

	  DECLARE @IsCollect Varchar(20)
	  DECLARE @IsEmail Varchar(20)
	   DECLARE @StatementDel Varchar(20)
	  Select @StatementDel=StatementDelivery From TblBulkCustomerInfo WITH(NOLOCK) where ID=@ID  
	  IF(@StatementDel='1')
		  BEGIN
		  SET @IsCollect=1
		  SET @IsEmail=0
		  END
	  ELSE IF (@StatementDel='2')
		  BEGIN
		   SET @IsCollect=0
		  SET @IsEmail=1
		  END
	  ELSE IF (@StatementDel='3')
		  BEGIN
		   SET @IsCollect=1
		   SET @IsEmail=1
		  END  
		  ----- save into cust occupation details
	  INSERT INTO TblCustOccupationDtl (CustomerID,IsCollectStatement ,IsEmailStatemnt,EmailForStatement,ProfessionTypeID,OrganizationTypeID,IsOtherCreditCard,IsPrabhuBankAcnt,ProfessionType)
						 SELECT @CustomerID,@IsCollect,@IsEmail,Email,0,0,0,0,'' From TblBulkCustomerInfo WITH(NOLOCK) where ID=@ID

		SET @Authorized=1	  

	END
	COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 		
	  ExceptionErrorLog:
	  SET @Authorized=2
		INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date,ParameterList)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE(),'@ID='+@ID
		   
	END CATCH; 

		Update TblBulkCustomerInfo SET Authorized=@Authorized ,CustomerID=@CustomerID  WHERE ID=@ID  
END

GO
