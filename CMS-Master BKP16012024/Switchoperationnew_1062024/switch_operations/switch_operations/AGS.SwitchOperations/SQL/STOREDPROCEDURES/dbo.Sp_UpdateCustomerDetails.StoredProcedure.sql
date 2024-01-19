USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_UpdateCustomerDetails]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_UpdateCustomerDetails]
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
@Maker_Date_NE datetime=NULL

,@CustomerID BIGINT=0
,@NameOnCard VARCHAR(25)=''
,@OtherCreditCardDtl  TblOtherCreditCardDtlType READONLY

,@BankID int=1

AS
BEGIN
	Begin Transaction  
	Begin Try  
  
			Declare @StrPriOutput varchar(1)='1'		
			Declare @StrPriOutputDesc varchar(200)='Customer details are not updated.'
			DECLARE @IntResult tinyint=1
			DECLARE @IssuerNo INT=23


			
			----update cust details on switch
			IF EXISTS(SELECT TOP 1 1 FROM TblCustomersDetails WITH(NOLOCK) WHERE (ISNULL(IsCardSuccess,0)=1) AND( CustomerID=@CustomerID) AND (FormStatusID=1))
			BEGIN
			  Declare @IntSwitchOutput int=1 ,@StrSwitchOutput Varchar(500)='' 
			  DECLARE @CustName VARCHAR(500)=UPPER(@FirstName+' '+@MiddleName+' '+@LastName)	  
			  			
				DECLARE @Login VARCHAR(100)='AGS_App'
				---- customerID logic change
				DECLARE @BankCustID VARCHAR(800)=''
				----Select @IssuerNo=BankCode from TblBanks WITH(NOLOCK) where ID=@BankID
				SELECT @BankCustID= c.BankCustID,@IssuerNo=BankCode from tblcustomersdetails c WITH(NOLOCK) 
				INNER JOIN TblBanks B WITH(NOLOCK)  ON c.BankID=B.ID
				where CustomerID=@CustomerID 
				
				BEGIN TRY
				  exec   [AGSS1RT].postcard.dbo.[usp_setCardDetails_AGS] @IssuerNo=@IssuerNo ,@CustomerID=@BankCustID,@CustName=@CustName,@NameOnCard=@NameOnCard,@Mobile=@MobileNo,@Email=@Email_P,@Login=@Login,@IntpriOutput=@IntSwitchOutput OUTPUT ,@StrpriOutputDesc=@StrSwitchOutput OUTPUT
				END TRY
                BEGIN CATCH
				  SET @IntSwitchOutput=1
				END CATCH

				Update TblCustomersDetails SET UpdateSWResponse=@StrSwitchOutput WHERE CustomerID=@CustomerID
					IF(@IntSwitchOutput=0)   --Successful update details on switch
					BEGIN
					 SET @IntResult=1
					 ------Maintain log of autherized customer's updated details
						INSERT INTO TblUpdateCustDetailsLog (CustomerID,OldDetails,NewDetails,ModifiedDate)
						 SELECT Cu.CustomerID,
						 'CustName ='+UPPER(Cu.FirstName+' '+Cu.MiddleName+' '+Cu.LastName)+'|NameOnCard='+Cu.NameOnCard+'|Mobile='+Cu.MobileNo+'| Email='+ cd.Email_P
						 ,'CustName ='+@CustName+'|NameOnCard='+@NameOnCard+'|Mobile='+@MobileNo+'| Email='+ @Email_P
						 ,GETDATE()
						 From TblCustomersDetails Cu WITH(NOLOCK)
						  INNER JOIN  TblCustomerAddress Cd WITH(NOLOCK) ON Cu.CustomerID=Cd.CustomerID 
						   WHERE Cu.CustomerID=@CustomerID
					END
					ELSE
					BEGIN
					 SET @IntResult=0
					END
			END
						
		 IF(@IntResult=1)
		  BEGIN
			IF((@FirstName <> ''  AND @LastName <> '' AND @MobileNo <> '' AND @CustomerID <>0))
				BEGIN

				IF EXISTS(SELECT 1 FROM TblCustomersDetails  WITH(NOLOCK)  where CustomerID=@CustomerID )
				BEGIN
				 -- IF NOT EXISTS(SELECT 1 FROM TblCustomersDetails WITH(NOLOCK) where CustomerID <>@CustomerID AND MobileNo=@MobileNo)
				  --BEGIN
				    UPDATE TblCustomersDetails SET FirstName=@FirstName,MiddleName=@MiddleName,LastName=@LastName,MobileNo=@MobileNo
				,DOB_BS=@DOB_BS,DOB_AD=@DOB_AD,Nationality=@Nationality,GenderID=@GenderID
				,MaritalStatusID=@MaritalStatusID,PassportNo_CitizenShipNo=@PassportNo_CitizenShipNo,IssueDate_District=@IssueDate_District
				,ResidenceTypeID=@ResidenceTypeID,ResidenceDesc=@ResidenceDesc,VehicleTypeID=@VehicleTypeID
				,VehicleType=@VehicleType,VehicleNo=@VehicleNo,CardTypeID=@CardTypeID,SpouseName=@SpouseName,MotherName=@MotherName,
				FatherName=@FatherName,GrandFatherName=@GrandFatherName,ModifiedByID=@MakerID,ModifiedDate_IND=GETDATE(),ProductType_ID=@ProductType_ID 
				,INST_ID=@INST_ID,NameOnCard=@NameOnCard
				,Remark=''--,AccNo=@PrabhuBankAccountNo,AccType=@AccountTypeID
				WHERE CustomerID=@CustomerID

				UPDATE TblCustomerAddress SET PO_Box_P=@PO_Box_P,HouseNo_P=@HouseNo_P,StreetName_P=@StreetName_P,Tole_P=@Tole_P,WardNo_P=@WardNo_P,City_P=@City_P
                                                  ,District_P=@District_P,Phone1_P=@Phone1_P,Phone2_P=@Phone2_P,FAX_P=@FAX_P
												  ,Mobile_P=@Mobile_P,Email_P=@Email_P,IsSameAsPermAddr=@IsSameAsPermAddr
												  ,PO_Box_C=PO_Box_C,HouseNo_C=HouseNo_C, StreetName_C=@StreetName_C,Tole_C=@Tole_C,WardNo_C=@WardNo_C
												  ,City_C=@City_C,District_C=@District_C ,Phone1_C=@Phone1_C
												  ,Phone2_C=@Phone2_C,FAX_C=@FAX_C,Mobile_C=@Mobile_C,Email_C=@Email_C
												  ,PO_Box_O=@PO_Box_O,StreetName_O=@StreetName_O,City_O=@City_O,District_O=@District_O,Phone1_O=@Phone1_O
												  ,Phone2_O=@Phone2_O,FAX_O=@FAX_O,Mobile_O=@Mobile_O,Email_O=@Email_O
									WHERE CustomerID=@CustomerID


               UPDATE TblCustOccupationDtl SET ProfessionTypeID=@ProfessionTypeID,ProfessionType=@ProfessionType,OrganizationTypeID=@OrganizationTypeID,OrganizationTypeDesc=@OrganizationTypeDesc
			   ,PreviousEmployment=@PreviousEmployment,Designation=@Designation,CompanyName=@CompanyName
						 ,BusinessType=@BusinessType,WorkSince=@WorkSince,AnnualSalary=@AnnualSalary,AnnualIncentive=@AnnualIncentive,AnnualBuisnessIncome=@AnnualBuisnessIncome
						 ,RentalIncome=@RentalIncome,Agriculture=@Agriculture,Income=@Income
						 ,TotalAnnualIncome=@TotalAnnualIncome,IsOtherCreditCard=@IsOtherCreditCard,PrincipalBankName=@PrincipalBankName,
						 AccountTypeDesc=@AccountTypeDesc,IsPrabhuBankAcnt=@IsPrabhuBankAcnt,IsCollectStatement=@IsCollectStatement
						 ,IsEmailStatemnt=@IsEmailStatemnt,EmailForStatement=@EmailForStatement,ReffName1=@ReffName1,ReffDesignation1=@ReffDesignation1,ReffPhoneNo1=@ReffPhoneNo1
						 ,ReffName2=@ReffName2,ReffDesignation2=@ReffDesignation2,ReffPhoneNo2=@ReffPhoneNo2
						 WHERE CustomerID=@CustomerID

						 SET @StrPriOutput ='0'		
			             SET @StrPriOutputDesc ='Customer details are updated.'
				  --END
				  --ELSE
				  --BEGIN
				  --  SET @StrPriOutput ='1'		
			   --     SET @StrPriOutputDesc ='Mobile no  is already exists'
				  --END
						 


				END
				ELSE
				BEGIN
				 SET @StrPriOutput ='1'		
			     SET @StrPriOutputDesc ='Customer details are not updated.'
				END 			   			  
		          		   
			   
			END
			ELSE
			BEGIN
			 SET @StrPriOutput=1
			 SET @StrPriOutputDesc='Customer details are not proper.'
			END
		  END
		  ELSE
		  BEGIN
		        SET @StrPriOutput ='1'		
			    SET @StrPriOutputDesc ='Customer details are not updated.'
		  END
					Select @StrPriOutput As Code,@StrPriOutputDesc As [OutputDescription],@CustomerID  AS[OutPutCode]

		COMMIT TRANSACTION;    
    End Try  
	 BEGIN CATCH 
	 RollBACK TRANSACTION; 
		SELECT 1  As Code,'Customer details are not updated.' As [OutputDescription],0  AS[OutPutCode]
	  
			INSERT INTO TblErrorDetail(Procedure_Name,Error_Desc,Error_Date)                 
		  SELECT ERROR_PROCEDURE(),ERROR_MESSAGE()+'Line Number:' +cast(ERROR_LINE() as varchar(50)),GETDATE()
		    
	END CATCH;  

END

GO
