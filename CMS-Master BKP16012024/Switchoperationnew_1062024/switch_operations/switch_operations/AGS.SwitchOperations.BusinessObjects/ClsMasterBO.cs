using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGS.SwitchOperations.BusinessObjects
{

    public class ClsFeeMasterBO
    {
        public Int16 IntPriStatus { get; set; }
        public Int64 FeeDtlID { get; set; }
        public string FeeCode { get; set; }
        public string FeeName { get; set; }
        public string PercAmnt { get; set; }
        public string PercAmntDesc { get; set; }
        public Decimal Percentage { get; set; }
        public Decimal Amount { get; set; }
        public Int64 IntPriUser { get; set; }
        public Int64 IsPercAmount { get; set; }
       

       

    }

    public class ClsUserRoleBO
    {
        public string UserRole { get; set; }
    }

    public class ClsUserDetailsBO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Emailid { get; set; }
        public string UserStatus { get; set; }
        public string UserRole { get; set; }
        public string BranchCode { get; set; }
        public string UserPassword { get; set; }
        public string Userid { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
    }

    public class ClsBranchDetailsBO
         {
    public string BranchCode { get; set; }
    public string BranchName { get; set; }
    public string BranchID { get; set; }
    public string UpdatedBy { get; set; }
}
public class ClsCourierDetailsBO
    {
       
        public string CourierName { get; set; }
        public string OfficeName { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
        public string Courierid { get; set; }
    }


   

    public class ClsRewardConfigBO
    {
        public Int32 RewConfigId { get; set; }
        public Int32 CardType { get; set; }
        public int MoneyToPoint { get; set; }
        public Decimal PointToMoney { get; set; }
        public int MinTransferPoints { get; set; }
        public int MaxTransferPoints { get; set; }
        public Int32 UpdateType { get; set; }
        public string LoginId { get; set; }
    }

    

    public class ClsBINDtl
    {
        public int ID { get; set; }
        public string BIN { get; set; }
        public string BinDesc { get; set; }
        public Int64? MakerID { get; set; }
        public Int64? CheckerID { get; set; }
        public string AccountTypeID { get; set; }
        public string CardTypeID { get; set; }
        public string INSTID { get; set; }
        public int FormStatusID { get; set; }
        public string Remark { get; set; }
    }

    public class ClsMasterBO
    {
        public int IntPara { get; set; }
        //Institution master
        public int ID { get; set; }
        public string InstitutionID { get; set; }
        public string INSTDesc { get; set; }

        public int CardTypeID { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeDesc { get; set; }
        public int INSTID { get; set; }
        public int BinID { get;set; }
        public Int64? MakerID { get; set; }
        public Int64? CheckerID { get; set; }
        public int FormStatusID { get; set; }
        public string Remark { get; set; }
        public string SystemID { get; set; }
        public string BankID { get; set; }
    }
}