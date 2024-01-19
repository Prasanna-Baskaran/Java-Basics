USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SaveCustomerDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_SaveCustomerDetails]
@FirstName		varchar(200)='',
@MiddleName		varchar(200)='',
@LastName		varchar(200)='',
@MobileNo Varchar(50)='',		
@DOB_BS		datetime=NULL,
@DOB_AD		datetime=NULL,
@Nationality		varchar(100)='',
@GenderID		varchar(10)=NULL,
@MaritalStatusID		varchar(10)=NULL,
@PassportNo_CitizenShipNo		varchar(200)='',
@IssueDate_District		varchar(200)='',
@ResidenceTypeID		tinyint=NULL,
@ResidenceDesc		varchar(200)='',
@VehicleTypeID		tinyint=NULL,
@VehicleType		varchar(200)='',
@VehicleNo		varchar(200)='',
@MakerID		BIGINT=0,
@CardTypeID		int=0,
@SpouseName		varchar(200)='',
@MotherName		varchar(200)='',
@FatherName		varchar(200)='',
@GrandFatherName		varchar(200)='',
@ProductType_ID int=0,
@INST_ID int=0,

@PO_Box_P		varchar(50)='',
@HouseNo_P		varchar(200)='',
@StreetName_P		varchar(200)='',
@Tole_P		varchar(200)='',
@WardNo_P		varchar(200)='',
@City_P		varchar(200)='',
@District_P		varchar(200)='',
@Phone1_P varchar(20)='',		
@Phone2_P		varchar(200)='',
@FAX_P		varchar(200)='',
@Mobile_P		varchar(200)='',
@Email_P		varchar(200)='',
@IsSameAsPermAddr		bit=0,
@PO_Box_C		varchar(50)='',
@HouseNo_C		varchar(200)='',
@StreetName_C		varchar(200)='',
@Tole_C		varchar(200)='',
@WardNo_C		varchar(200)='',
@City_C		varchar(200)='',
@District_C		varchar(200)='',
@Phone1_C		varchar(200)='',
@Phone2_C		varchar(200)='',
@FAX_C		varchar(200)='',
@Mobile_C		varchar(200)='',
@Email_C		varchar(200)='',


@PO_Box_O		varchar(50)='',
@StreetName_O		varchar(200)='',
@City_O		varchar(200)='',
@District_O		varchar(200)='',
@Phone1_O		varchar(200)='',
@Phone2_O		varchar(200)='',
@FAX_O		varchar(200)='',
@Mobile_O		varchar(200)='',
@Email_O		varchar(200)='',


@ProfessionTypeID		smallint=0,
@OrganizationTypeID		smallint=0,
@OrganizationTypeDesc		varchar(50)='',
@ProfessionType		varchar(50)='',
@PreviousEmployment		varchar(50)='',
@CompanyName VARCHAR(200)='',
@Designation		varchar(50)='',
@BusinessType		varchar(100)='',
@WorkSince		varchar(50)='',


@AnnualSalary		varchar(200)='',
@AnnualIncentive		varchar(200)='',
@AnnualBuisnessIncome		varchar(200)='',
@RentalIncome		varchar(200)='',
@Agriculture		varchar(200)='',
@Income		varchar(200)='',
@TotalAnnualIncome		varchar(200)='',
@PrincipalBankName		varchar(200)='',
@AccountTypeID		int=0,
@AccountTypeDesc		varchar(200)='',
@IsPrabhuBankAcnt		bit=0,
@PrabhuBankAccountNo		varchar(200)='',
@PrabhuBankBranch		varchar(200)='',
@IsCollectStatement		bit=0,
@IsEmailStatemnt		bit=0,
@EmailForStatement		varchar(100)='',

@ReffName1		varchar(100)='',
@ReffDesignation1		varchar(100)='',
@ReffPhoneNo1		varchar(100)='',
@ReffName2		varchar(100)='',
@ReffDesignation2		varchar(100)='',
@ReffPhoneNo2		varchar(100)='',

@IsOtherCreditCard		bit=0,
@FormStatusID int=0,
@Maker_Date_NE datetime=NULL ,
@NameOnCard VARCHAR(25)=''
,@OtherCreditCardDtl  TblOtherCreditCardDtlType READONLY
,@SystemID int,
@BankID int=1

AS
BEGIN
	Begin Transaction  
	Begin Try  
  
			Declare @StrPriOutput varchar(1)='1'		
			Declare @StrPriOutputDesc varchar(200)='Customer details are not saved.'
			DECLARE @CustomerID BIGINT=0

			IF((@FirstName <> ''  AND @LastName <> '' AND @MobileNo <> ''))
				BEGIN
			   
			  IF Not Exists(SELECT 1 FROM TblCustomersDetails WITH(NOLOCK) WHERE  SystemID=@SystemID AND BankID=@BankID AND PassportNo_CitizenShipNo=@PassportNo_CitizenShipNo  )
			   BEGIN
			   
					INSERT INTO TblCustomersDetails (FirstName,MiddleName,LastName,MobileNo,DOB_BS,DOB_AD,Nationality,GenderID
				,MaritalStatusID,PassportNo_CitizenShipNo,IssueDate_District,ResidenceTypeID,ResidenceDesc,VehicleTypeID
				,VehicleType,VehicleNo,CardTypeID,SpouseName,MotherName,FatherName,GrandFatherName
				,FormStatusID,Maker_Date_NE,Maker_Date_IND,MakerID,ProductType_ID,INST_ID,NameOnCard,SystemID,BankID,AccNo,AccType) VALUES (@FirstName,@MiddleName,@LastName,@MobileNo,@DOB_BS,@DOB_AD,@Nationality,@GenderID
				,@MaritalStatusID,@PassportNo_CitizenShipNo,@IssueDate_District,@ResidenceTypeID,@ResidenceDesc,@VehicleTypeID
				,@VehicleType,@VehicleNo,@CardTypeID,@SpouseName,@MotherName,@FatherName,@GrandFatherName
				,@FormStatusID,@Maker_Date_NE,GETDATE(),@MakerID,@ProductType_ID,@INST_ID,@NameOnCard,@SystemID,@BankID
				 ,Case When ISNULL(@PrabhuBankAccountNo,'')='' THEN NULL ELSE dbo.ufn_EncryptPAN(@PrabhuBankAccountNo) END,Case WHen ISNULL(@AccountTypeID,1)=0 THEn 1 ELSE @AccountTypeID END  --start Diksha Acc logic change
				)		
						SELECT @CustomerID=SCOPE_IDENTITY()

					If(@CustomerID>0)
                    BEGIN
					--start Diksha Acc logic/ customerID  change
					  UPDATE TblCustomersDetails SET ApplicationFormNo=dbo.FunGetApplicationNo(@CustomerID),AccNo=dbo.FunGetAccountNo(@CustomerID),bankcustID=dbo.FunGetBankCustID(@CustomerID)  WHERE CustomerID=@CustomerID
					 --insert into customeraddress table
					 INSERT INTO TblCustomeraddress (CustomerID,PO_Box_P,HouseNo_P,StreetName_P,Tole_P,WardNo_P,City_P
													  ,District_P,Phone1_P,Phone2_P,FAX_P,Mobile_P,Email_P,IsSameAsPermAddr,PO_Box_C,HouseNo_C
													  ,StreetName_C,Tole_C,WardNo_C,City_C,District_C,Phone1_C,Phone2_C,FAX_C,Mobile_C,Email_C
													  ,PO_Box_O,StreetName_O,City_O,District_O,Phone1_O,Phone2_O,FAX_O,Mobile_O,Email_O)
											  VALUES(@CustomerID,@PO_Box_P,@HouseNo_P,@StreetName_P,@Tole_P,@WardNo_P,@City_P
													 ,@District_P,@Phone1_P,@Phone2_P,@FAX_P,@Mobile_P,@Email_P,@IsSameAsPermAddr,@PO_Box_C,@HouseNo_C
													 ,@StreetName_C,@Tole_C,@WardNo_C,@City_C,@District_C,@Phone1_C,@Phone2_C,@FAX_C,@Mobile_C,@Email_C
													 ,@PO_Box_O,@StreetName_O,@City_O,@District_O,@Phone1_O,@Phone2_O,@FAX_O,@Mobile_O,@Email_O)
						  IF(@@ROWCOUNT>0)
					       BEGIN
					     --insert into Occupation table
						 INSERT INTO TblCustOccupationDtl (CustomerID,ProfessionTypeID,ProfessionType,OrganizationTypeID,OrganizationTypeDesc,PreviousEmployment,Designation,CompanyName
						 ,BusinessType,WorkSince,AnnualSalary,AnnualIncentive,AnnualBuisnessIncome,RentalIncome,Agriculture,Income
						 ,TotalAnnualIncome,IsOtherCreditCard,PrincipalBankName,AccountTypeDesc,IsPrabhuBankAcnt,IsCollectStatement
						 ,IsEmailStatemnt,EmailForStatement,ReffName1,ReffDesignation1,ReffPhoneNo1,ReffName2,ReffDesignation2,ReffPhoneNo2)
						   VALUES(@CustomerID,@ProfessionTypeID,@ProfessionType,@OrganizationTypeID,@OrganizationTypeDesc,@PreviousEmployment,@Designation,@CompanyName
						 ,@BusinessType,@WorkSince,@AnnualSalary,@AnnualIncentive,@AnnualBuisnessIncome,@RentalIncome,@Agriculture,@Income
						 ,@TotalAnnualIncome,@IsOtherCreditCard,@PrincipalBankName,@AccountTypeDesc,@IsPrabhuBankAcnt,@IsCollectStatement
						 ,@IsEmailStatemnt,@EmailForStatement,@ReffName1,@ReffDesignation1,@ReffPhoneNo1,@ReffName2,@ReffDesignation2,@ReffPhoneNo2)

						  IF(@@ROWCOUNT>0)
						   BEGIN

						   --Other Credit Card details
						  IF EXISTS( SELECT COUNT(1) from @OtherCreditCardDtl)
						   BEGIN
						      INSERT INTO TblOtherCreditCardDtl (CustomerID,CardType,IssuedBy,IssuedDate,Limit,Overdue,ExpiryDate) 
							  SELECT @CustomerID,CardType,IssuedBy,IssuedDate,Limit,Overdue,ExpiryDate from @OtherCreditCardDtl
						   END

							 SET @StrPriOutput=0
							 SET @StrPriOutputDesc='Customer details are saved .'

						   END
						  ELSE
						   BEGIN
							RollBACK TRANSACTION; 
							 SET @StrPriOutput=1
							 SET @StrPriOutputDesc='Customer details are not saved.'
						   END
					  END 
					  ELSE
					  BEGIN
					        RollBACK TRANSACTION; 
							 SET @StrPriOutput=1
							 SET @StrPriOutputDesc='Customer details are not saved.'
					  END			
				END
					ELSE
					BEGIN
					 SET @StrPriOutput=1
					 SET @StrPriOutputDesc='Customer details are not saved.'
					END
               END
			   ELSE
			   BEGIN
			   
			      SET @StrPriOutput=1
				  SET @StrPriOutputDesc='Passport No/Identification No is already  exists.'
			   END
			END
			ELSE
			BEGIN
			 SET @StrPriOutput=1
			 SET @StrPriOutputDesc='Customer details are not proper.'
			END

					Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription] ,@CustomerID  AS[OutPutCode]

		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT 1  As Code,'Error occurs.' As [OutputDescription],0 AS [OutPutCode]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  

END

GO
