USE [SwitchOperations]
GO
/****** Object:  StoredProcedure [dbo].[Sp_SearchCustomer_Bkp26042018]    Script Date: 08-06-2018 16:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sp_SearchCustomer_Bkp26042018]
	@ApplicationNo VARCHAR(20)='',
	@MobileNo Varchar(12)='',
	@CustomerID Bigint =0,
	@DOB_AD Datetime=NULL,
	@DOB_BS Datetime=NULL,
	@IdentificationNo VARCHAR(50)='',
	@CustomerName VARCHAR(200)='',
	@IntPara tinyint =0,
	@CreatedDate Datetime=NULL,
	@SystemID VARCHAR(200)=1,
	@BankID VARCHAR(200)=1,
	@CardNo VARCHAR(20)='',	
	@BankCustID VARCHAR(200)=''


AS
BEGIN

--Search Customer
IF(@IntPara=0)
BEGIN

  SELECT DISTINCT Cu.CustomerID AS [CustID],Cu.BankCustID AS [CustomerID], Cu.FirstName+' '+ISNULL(Cu.LastName,'') as[Name],
  --Convert(VARCHAR(10),Cu.DOB_AD,120) AS [Date Of Birth],
  cd.PO_Box_P As Address,
  CASE WHEN (Convert(VARCHAR(10),Cu.DOB_AD,103))='1900-01-01' THEN NULL ELSE (Convert(VARCHAR(10),Cu.DOB_AD,103)) END AS [Date Of Birth],Cu.ApplicationFormNo AS[Application No],Cu.FormStatusID,s.FormStatus,ISNULL(cu.Remark,'') AS [Remark]
		, (Convert(VARCHAR(10),ISNULL(Maker_Date_IND,Maker_Date_IND),103)) AS [CreatedDate] --,(Convert(VARCHAR(10),ISNULL(ModifiedDate_IND,''),103)) AS ModifiedDate
			
		From TblCustomersDetails Cu WITH(NOLOCK)
		INNER JOIN TblFormStatus S WITH(NOLOCK) ON cu.FormStatusID=S.FormStatusID
		LEFT JOIN CardRPAN pan WITH(NOLOCK) on cu.BankCustID=pan.customer_id
		Left Join TblCustomerAddress cd with(nolock) on cu.CustomerID=cd.CustomerID --for cardNo filter
		where --((@CustomerID=0) OR (Cu.CustomerID=@CustomerID)) 
		((@BankCustID='') OR (Cu.BankCustID=@BankCustID)) 
			AND ((@IdentificationNo='') OR (Cu.PassportNo_CitizenShipNo=@IdentificationNo))
			AND ((@MobileNo='') OR (Cu.MobileNo=@MobileNo))
			AND ((@ApplicationNo='')OR(Cu.ApplicationFormNo=@ApplicationNo))
			AND ( (ISNULL(@DOB_AD,0)=0) OR (CONVERT(date,ISNULL(cu.DOB_AD,'1900-01-01'),103)=(convert(date,@DOB_AD,103))) )
		    --AND ((@DOB_BS is NULL)  OR (CONVERT(date,ISNULL(cu.DOB_BS,'1900-01-01'),103)=(convert(date,@DOB_BS,103)) ))
			AND( 
			(@CustomerName='')
			 OR
			 (Cu.FirstName like '%'+@CustomerName+'%')
			  OR
			 (Cu.LastName like '%'+@CustomerName+'%')
			 OR
			 ( Upper(Cu.FirstName+' '+Cu.LastName)  like '%'+UPPER(@CustomerName)+'%')
			 )
			AND( (ISNULL(@CreatedDate,0)=0) OR (convert(date,cu.Maker_Date_IND,103)=convert(date,@CreatedDate,103))) 
			AND ((Cu.SystemID=@SystemID))
			AND(Cu.BankID=@BankID)
			AND ((@CardNo='') OR (dbo.ufn_DecryptPAN(pan.DecPAN)=@CardNo))  --for cardNo filter			
			--AND (CONVERT(date,cu.DOB_AD,103)=(convert(date,@DOB_AD,103)) OR(@DOB_AD=NULL))
			--AND (CONVERT(date,cu.DOB_BS,103)=(convert(date,@DOB_BS,103)) OR(@DOB_BS=NULL))
END
ELSE IF(@IntPara=1)
BEGIN
	--Get All Customer Detail  
	Select DISTINCT Cu.FirstName+' '+Cu.LastName as[Name], 
	--Cu.DOB_AD AS [Date Of Birth],
	CASE WHEN (Convert(VARCHAR(10),Cu.DOB_AD,103))='1900-01-01' THEN NULL ELSE (Convert(VARCHAR(10),Cu.DOB_AD,103))END as[Date Of Birth],
		Cu.PassportNo_CitizenShipNo AS [Identification No],Cu.ApplicationFormNo AS[Application No],
		Cu.CustomerID ,Cu.FirstName,Cu.MiddleName,Cu.LastName,Cu.MobileNo
		--,(Convert(date,ISNULL(Cu.DOB_BS,''),103)) AS [DOB_BS]
		 ,CASE WHEN (Convert(VARCHAR(10),Cu.DOB_BS,103))='1900-01-01' THEN NULL ELSE (Convert(VARCHAR(10),Cu.DOB_BS,103)) END AS [DOB_BS]
		--,Convert(date,ISNULL(Cu.DOB_AD,''),103) as[DOB_AD]
		,CASE WHEN (Convert(VARCHAR(10),Cu.DOB_AD,103))='1900-01-01' THEN NULL ELSE (Convert(VARCHAR(10),Cu.DOB_AD,103))END as[DOB_AD]
		,Cu.Nationality,ISNULL(Cu.GenderID,0)
		,ISNULL(Cu.MaritalStatusID,0),Cu.IssueDate_District,ISNULL(Cu.ResidenceTypeID,0),ISNULL(Cu.VehicleTypeID,0),Cu.VehicleType
		,Cu.VehicleNo,ISNULL(Cu.CardTypeID,0),Cu.SpouseName,Cu.MotherName,Cu.FatherName,Cu.GrandFatherName,Cu.FormStatusID,Cu.Maker_Date_NE
		,Cu.Maker_Date_IND,Cu.MakerID
		,Cd.PO_Box_P,Cd.HouseNo_P,Cd.StreetName_P,Cd.Tole_P,Cd.WardNo_P,Cd.City_P,Cd.District_P,Cd.Phone1_P,Cd.Phone2_P
		,Cd.FAX_P,Cd.Mobile_P,Cd.Email_P,convert(tinyint,ISNULL(Cd.IsSameAsPermAddr,0)) AS [IsSameAsPermAddr] ,Cd.PO_Box_C,Cd.HouseNo_C,Cd.StreetName_C,Cd.Tole_C,Cd.WardNo_C
		,Cd.City_C,Cd.District_C,Cd.Phone1_C,Cd.Phone2_C,Cd.FAX_C,Cd.Mobile_C,Cd.Email_C,Cd.PO_Box_O,Cd.StreetName_O,Cd.City_O
		,Cd.Phone1_O,Cd.Phone2_O,Cd.FAX_O,Cd.Mobile_O,Cd.Email_O,Cd.District_O
		,ISNULL(Oc.ProfessionTypeID,''),ISNULL(Oc.OrganizationTypeID,0),Oc.OrganizationTypeDesc,Oc.PreviousEmployment,Oc.Designation,Oc.CompanyName
		,ISNULL(Oc.BusinessType,''),Oc.WorkSince,Oc.AnnualSalary,Oc.AnnualIncentive,Oc.AnnualBuisnessIncome,Oc.RentalIncome,Oc.Agriculture
		,Oc.Income,Oc.TotalAnnualIncome,Oc.IsOtherCreditCard,Oc.PrincipalBankName,ISNULL(cu.acctype,0),Oc.AccountTypeDesc,convert(tinyint,Oc.IsPrabhuBankAcnt) AS [IsPrabhuBankAcnt]
		,dbo.ufn_decryptPAN(cu.accNo),convert(tinyint,Oc.IsCollectStatement) AS [IsCollectStatement],convert(tinyint,Oc.IsEmailStatemnt) AS [IsEmailStatemnt],Oc.EmailForStatement,Oc.ReffName1,Oc.ReffDesignation1,Oc.ReffPhoneNo1
		,Oc.ReffName2,Oc.ReffDesignation2,Oc.ReffPhoneNo2,Oc.Documentation,Oc.ProfessionType
		,ISNULL(cu.ProductType_ID,0) AS [ProductType_ID],ISNULL(cu.INST_ID,0) AS [INST_ID]
		,KC.SignatureFileName,KC.Photo,KC.IDProof --KYC documents file name
		,cu.NameOnCard
		,Cu.BankCustID AS [CIFID]		
		from TblCustomersDetails cu WITH(NOLOCK)
		INNER JOIN TblCustomerAddress Cd  WITH(NOLOCK) ON cu.CustomerId=Cd.CustomerID
		INNER Join TblCustOccupationDtl Oc WITH(NOLOCK) ON Cu.CustomerId=Oc.CustomerID
		LEFT JOIN TblKYCDocuments KC WITH(NOLOCK) ON cu.CustomerId=KC.CustomerID
		where (Cu.CustomerID=@CustomerID)
		AND ((Cu.SystemID=@SystemID))
		AND(Cu.BankID=@BankID)
		IF EXISTS(SELECT top 1 1 FROM TblOtherCreditCardDtl WITH(NOLOCK) where CustomerID=@CustomerID)
		BEGIN
		  SELECT CardType,IssuedBy,convert(date,ISNULL(IssuedDate,''),103) as[IssuedDate],Limit,Overdue,convert(date,ISNULL(ExpiryDate,''),103)  as[ExpiryDate] 
		  FROM TblOtherCreditCardDtl WITH(NOLOCK) WHERE CustomerID=@CustomerID

		END
END
END

GO
