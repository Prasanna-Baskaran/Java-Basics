using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCardCheckerDetailsBO
    {

        public string IDs { get; set; }
        public string CheckerId { get; set; }

        //inserting values in TblCardGenRequest
        public string CustomerID { get; set; }
        public string CardNo { get; set; }
        public string NewBinPrefix { get; set; }
        public string HoldRSPCode { get; set; }
        public string RSPCode { get; set; }
        public string SwitchResponse { get; set; }
        public string STAN { get; set; }
        public string RRN { get; set; }
        public string AuthID { get; set; }
        public string Remark { get; set; }
        public int FormStatusID { get; set; }
        public int IsRejected { get; set; }
        public string RejectReason { get; set; }
        public string MakerID { get; set; }
        public int IsAuthorized { get; set; }
        public string UploadFileName { get; set; }
        public string BankID { get; set; }
        public string SystemID { get; set; }
        public int ProcessID { get; set; }
        public string schemecode { get; set; }
        public string Account1 { get; set; }
        public string Account2 { get; set; }
        public string Account3 { get; set; }
        public string Account4 { get; set; }
        public string Account5 { get; set; }
        public string Branch_Code { get; set; }
        public string ExpiryDate { get; set; }
        public string New_Card { get; set; }
        public string Customer_Name { get; set; }
        public string New_Card_Activation_Date { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }

        public string IssuerNo { get; set; }

    }

    public class ProcessCardReissueRequest
    {
        public int ActionType { get; set; }
        public string IDs { get; set; }
        public string LoginId { get; set; }
        public string CustomerID { get; set; }
        public string CardNo { get; set; }
        public string NewBinPrefix { get; set; }
        public string HoldRSPCode { get; set; }
        public string RSPCode { get; set; }
        public string SwitchResponse { get; set; }
        public string BankID { get; set; }
        public string SystemID { get; set; }
        public string IssuerNo { get; set; }

    }
}
