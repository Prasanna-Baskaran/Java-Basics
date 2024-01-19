using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCommonProp
    {

    }

    public class ClsReturnStatusBO
    {

        public int Code { get; set; }
        public string Description { get; set; }
        public string OutPutCode { get; set; }
        public int OutPutID { get; set; }
        public long CardREqID { get; set; }
        public string BankLogoPath { get; set; }
        
    }
    public class ClsLoginBO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string UserRoleID { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string SystemID { get; set; }
        public string SystemName { get; set; }
        public string BankID { get; set; }
        public string BankCode { get; set; }
        public Dictionary<string, string> UserRightsList { get; set; }
        public string AuthKey { get; set; }
        public string UserPrefix { get; set; }
        //Source id=for card  api call  to link delink account by sourceid of bank table 
        public string SourceId { get; set; }
        //sheetal change for bankname logo
        public string BankName { get; set; }

        public string IssuerNo { get; set; }
        public int IsEPS { get; set; }
        public Boolean IsAdmin { get; set; }
        
        public int iInstaEdit { get; set; } //added for ATPCM-759

        public int ParticipantId { get; set; } // added for NTPCM-132 denomination count update dfcc
        
        public string LoginPagePath { get; set; } // added to store Login path
        public string BankLogoPath { get; set; } // added to store bank logo path

        public int EnableOtp { get; set; }

    }

    public class ClsEMIBreakUpBO
    {
        public string RRN { get; set; }
        public int NumberOfEMI { get; set; }
        public Decimal TxnAmount { get; set; }
        public Decimal PerEMIAmount { get; set; }
        public DateTime EMIStartDate { get; set; }
        public string IntrestRate { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public Decimal TxnAmtWithIntrest { get; set; }

        public string CustomerID { get; set; }
        public DateTime DOB { get; set; }
        public string StrCardNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Flag { get; set; }
    }

    public class ClsEMIPaymentBO
    {

    }

    public class ClsAccountBalanceBO
    {
        public string CardNo { get; set; }
        public string BankId { get; set; }
    }

    public class ClsCahngePass
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string BankId { get; set; }
        public string Role { get; set; }
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }
        public string ConfirmNewPass { get; set; }
        public int ChangedBy { get; set; }
        public int Result { get; set; }
        public string Description { get; set; }
        public int mode { get; set; }
    }

    public class ClsResetPassword
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BankId { get; set; }
        public string Role { get; set; }
        public string CurrentPass { get; set; }
        public string NewPass { get; set; }
        public string ConfirmNewPass { get; set; }
        public int ChangedBy { get; set; }
        public int Result { get; set; }
        public string Description { get; set; }
        public string MobileNo { get; set; }
        public string UniqueId { get; set; }

    }
}