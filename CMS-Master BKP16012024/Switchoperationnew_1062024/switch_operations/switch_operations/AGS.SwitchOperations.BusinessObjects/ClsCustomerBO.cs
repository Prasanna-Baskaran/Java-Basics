using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCustomerBO
    {

    }

    public class ClsCustomer
    {
        public Int64 CustomerID { get; set; }
        public Int64 UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DOB_BS { get; set; }
        public DateTime? DOB_AD { get; set; }
        public string Nationality { get; set; }
        public string GenderID { get; set; }
        public string MaritalStatusID { get; set; }
        public string PassportNo_CitizenShipNo { get; set; }
        public string IssueDate_District { get; set; }
        public string ResidenceTypeID { get; set; }
        public string ResidenceDesc { get; set; }
        public string VehicleTypeID { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string CardTypeID { get; set; }
        public string SpouseName { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string GrandFatherName { get; set; }
        public int FormStatusID { get; set; }
        public string Maker_Date_NE { get; set; }
        public string Maker_Date_IND { get; set; }
        public Int64 MakerID { get; set; }
        public Int64 ModifiedByID { get; set; }
        public string ModifiedDate_NE { get; set; }
        public string ModifiedDate_IND { get; set; }
        public string Checker_Date_NE { get; set; }
        public string Checker_Date_IND { get; set; }
        public Int64 CheckerID { get; set; }
        public string ApplicationNo { get; set; }
        public string CustomerName { get; set; }
        public int IntPara { get; set; }
        public string Remark { get; set; }
        public Int32 ProductTypeID { get; set; }
        public Int32 INSTID { get; set; }

        //Kyc document
        public HttpPostedFile Signature { get; set; }
        public HttpPostedFile Photo { get; set; }
        public HttpPostedFile IDProof { get; set; }

        public string SignatureFileName { get; set; }
        public string PhotoFileName { get; set; }
        public string IDProofFileName { get; set; }
        public string NameOnCard { get; set; }
        public string ReqCustomerId { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
    }

    public class ClsCustOccupationDtl
    {
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public string ProfessionTypeID { get; set; }
        public string ProfessionType { get; set; }
        public string OrganizationTypeID { get; set; }
        public string OrganizationTypeDesc { get; set; }
        public string PreviousEmployment { get; set; }
        public string Designation { get; set; }
        public string CompanyName { get; set; }
        public string FatherName { get; set; }
        public string BusinessType { get; set; }
        public string WorkSince { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string AnnualSalary { get; set; }
        public string AnnualIncentive { get; set; }
        public string AnnualBuisnessIncome { get; set; }
        public string RentalIncome { get; set; }
        public string Agriculture { get; set; }
        public string Income { get; set; }
        public string TotalAnnualIncome { get; set; }
        public byte IsOtherCreditCard { get; set; }
        public string PrincipalBankName { get; set; }
        public string AccountTypeID { get; set; }
        public string AccountTypeDesc { get; set; }
        public byte IsPrabhuBankAcnt { get; set; }
        public string PrabhuBankAccountNo { get; set; }
        public string PrabhuBankBranch { get; set; }
        public byte IsCollectStatement { get; set; }
        public byte IsEmailStatemnt { get; set; }
        public string EmailForStatement { get; set; }
        public string ReffName1 { get; set; }
        public string ReffDesignation1 { get; set; }
        public string ReffPhoneNo1 { get; set; }
        public string ReffName2 { get; set; }
        public string ReffDesignation2 { get; set; }
        public string ReffPhoneNo2 { get; set; }
        public string Documentation { get; set; }

        public string ISORSPCODE { get; set; }
        public string ISORSPDESC { get; set; }

    }

    public class ClsCustomerAddress
    {
        public Int64 CustAddressID { get; set; }
        public Int64 CustomerID { get; set; }
        public string PO_Box_P { get; set; }
        public string HouseNo_P { get; set; }
        public string StreetName_P { get; set; }
        public string Tole_P { get; set; }
        public string WardNo_P { get; set; }
        public string City_P { get; set; }
        public string District_P { get; set; }
        public string Phone1_P { get; set; }
        public string Phone2_P { get; set; }
        public string FAX_P { get; set; }
        public string Mobile_P { get; set; }
        public string Email_P { get; set; }
        public byte IsSameAsPermAddr { get; set; }
        public string PO_Box_C { get; set; }
        public string HouseNo_C { get; set; }
        public string StreetName_C { get; set; }
        public string Tole_C { get; set; }
        public string WardNo_C { get; set; }
        public string City_C { get; set; }
        public string District_C { get; set; }
        public string Phone1_C { get; set; }
        public string Phone2_C { get; set; }
        public string FAX_C { get; set; }
        public string Mobile_C { get; set; }
        public string Email_C { get; set; }
        public string PO_Box_O { get; set; }
        public string HouseNo_O { get; set; }
        public string StreetName_O { get; set; }
        public string Tole_O { get; set; }
        public string WardNo_O { get; set; }
        public string City_O { get; set; }
        public string District_O { get; set; }
        public string Phone1_O { get; set; }
        public string Phone2_O { get; set; }
        public string FAX_O { get; set; }
        public string Mobile_O { get; set; }
        public string Email_O { get; set; }
    }

    public class ClsOtherCreditCardDtl
    {
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public string CardType { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDate { get; set; }
        public string Limit { get; set; }
        public string Overdue { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class CustSearchFilter
    {
        public int ActionType { get; set; }
        public string IssuerNo { get; set; }
        public Int64 ID { get; set; }
        public string CustomerName { get; set; }
        public string NameOnCard { get; set; }
        public string MobileNo { get; set; }
        public string ApplicationNo { get; set; }
        public string IdentificationNo { get; set; }
        public DateTime? DOB_AD { get; set; }
        public DateTime? DOB_BS { get; set; }
        public int IntPara { get; set; }
        public string CustomerID { get; set; }
        public string CardNo { get; set; }
        public decimal? Limit { get; set; }
        public Int64? MakerID { get; set; }
        public Int64? CheckerId { get; set; }
        public int FormStatusID { get; set; }
        public string Remark { get; set; }
        //Card Limit
        public Int32 PurNOTran { get; set; }
        public decimal PurDailyLimit { get; set; }
        public decimal PurPTLimit { get; set; }

        public Int32 WithDrawNOTran { get; set; }
        public decimal WithDrawDailyLimit { get; set; }
        public decimal WithDrawPTLimit { get; set; }

        public Int32 PaymentNOTran { get; set; }
        public decimal PaymentDailyLimit { get; set; }
        public decimal PaymentPTLimit { get; set; }

        public decimal CNPDailyLimit { get; set; }
        public decimal CNPPTLimit { get; set; }
        public string UpdateRemark { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestIDs { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string FromDateTranRpt { get; set; }
        public string ToDateTranRpt { get; set; }
        public string ProductType { get; set; }
        public string Status { get; set; }
        public string BankCustID { get; set; }
        public string RPAN_ID { get; set; }
        public string AccountNo { get; set; }
        public string OldCardNo { get; set; }

        public int PINResetFlag { get; set; }

        public string ISORSPCode { get; set; }
        public string ISORSPDesc { get; set; }
        public string APIRequest { get; set; }

        public int IsEPS { get; set; }

        public string RRN { get; set; }

        public string tranType { get; set; }
        public Int32 LoginId { get; set; }

        public string SDBAPIURL { get; set; }
        public string CIF { get; set; }
        public string PARA { get; set; }
        public string USER { get; set; }
        public string TOKEN { get; set; }
        // added by nitin on 01/11/2019
        public string NIC { get; set; }
        public string UserBranchCode { get; set; }
        public Boolean IsAdmin { get; set; }
        public string UserID { get; set; }
        public string MotherName { get; set; }
    }


    public class CustomerRegistrationDetails
    {
        public string CardNo { get; set; }//added for ATPCM-759
        public string CIFID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPreferredName { get; set; }
        public string CardPrefix { get; set; }
        public string ACID { get; set; }
        public string ACOpenDate { get; set; }
        public string CIFCreationDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public string Country { get; set; }
        public string MothersMaidenName { get; set; }
        public string DOB { get; set; }
        public string CountryDialcode { get; set; }
        public string STDCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string AccountType { get; set; }
        public string BranchCode { get; set; }
        public string EnteredDate { get; set; }
        public string VerifiedDate { get; set; }
        public string PANNumber { get; set; }
        public string ModeOfOperation { get; set; }
        public string FourthLineEmbossing { get; set; }
        public string Aadhaar { get; set; }
        public string Pin_Mailer { get; set; }
        public int SystemID { get; set; }
        public string BankID { get; set; }
        public int IssuerNo { get; set; }

        public string RequestedBy { get; set; }
        public string ProcessedBy { get; set; }
        public string RequestType { get; set; }
        public string iInstaEdit { get; set; }//added for ATPCM-759
        public Int64 AuthId { get; set; }//added for ATPCM-759

        public String OldCardNo { get; set; } //added for ATPBF-1355

        //Added by nitin on 08/1/2019 start
        public String Gender { get; set; }
        public String MarketSeqment { get; set; }
        public String OldNICNo { get; set; }
        public String DrivingLicNo { get; set; }
        public String DrivingLicExpDT { get; set; }
        public String PassportNo { get; set; }
        public String PassportExpDt { get; set; }
        public String CustomerClassification { get; set; }
        public String CustomerType { get; set; }
        public String BusinessRegNo { get; set; }
        public String Age { get; set; }
        public string CustLanguage { get; set; }
        public string UserBranchCode { get; set; }
        public string AllSelectedValues { get; set; }
        public string AccCategory { get; set; }

        //Added by nitin on 08/1/2019 end
    }

    ///*Added for international usage RBL-ATPCM-862* START/
    public class ClsIntNaCutDetails
    {
        public string CardNo { get; set; }
        public string Remark { get; set; }
        public string IntNaUsage { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
        public string UserID { get; set; }
        public string[] RequestIDs { get; set; }

        public string ISORSPCode { get; set; }
        public string ISORSPDesc { get; set; }
        public Int32 id { get; set; }

        public DataTable DtBulkData { get; set; }

        public string IssuerNo { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    ///*Added for international usage RBL-ATPCM-862* END/
}