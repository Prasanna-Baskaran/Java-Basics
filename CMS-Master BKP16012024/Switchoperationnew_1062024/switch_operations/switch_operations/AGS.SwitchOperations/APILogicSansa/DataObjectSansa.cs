using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace AGS.SwitchOperations
{
    public class DataObjectSansa
    {
        /// <summary>
        /// To stroe the Card No
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// To Stroe the SFTP Server IP
        /// </summary>
        public string SFTP_ServerIP { get; set; }
        /// <summary>
        /// To stroe the SFTP Server Port
        /// </summary>
        public Int32 SFTP_Port { get; set; }
        /// <summary>
        /// To stroe the SFTP UserName
        /// </summary>
        public string SFTP_Username { get; set; }
        /// <summary>
        /// To stroe the SFTP password
        /// </summary>
        public string SFTP_Password { get; set; }
        /// <summary>
        /// To stroe the path for the SFTP SSH key
        /// </summary>
        public string SSH_Private_key_file_Path { get; set; }
        /// <summary>
        /// To stroe the SFTP passphrase
        /// </summary>
        public string passphrase { get; set; }
        /// <summary>
        /// To stroe the SFTP Path for the Input file 
        /// </summary>
        public string InputFilePath { get; set; }
        /// <summary>
        /// To stroe the SFTP Path for the output file 
        /// </summary>
        public string OutputFilePath { get; set; }
        /// <summary>
        /// To stroe the SFTP Path for the Archive file 
        /// </summary>
        public string ArchiveFilePath { get; set; }
        /// <summary>
        /// To stroe the server  Path for the file 
        /// </summary>
        public string ServerFilePath { get; set; }
        /// <summary>
        ///Tos stroe  the input file header
        /// </summary>
        public string InputFileHeader { get; set; }
        /// <summary>
        /// To stroe the Public key file path for the PGP
        /// </summary>
        public string PGPPublicKeyPath { get; set; }
        /// <summary>
        /// To stroe the Secret key file path for the PGP
        /// </summary>
        public string PGPSecretKeyPath { get; set; }
        /// <summary>
        /// tTo stroe the  PGP password
        /// </summary>
        public string PGPPassword { get; set; }
        /// <summary>
        /// To stroe the flag for the PGP
        /// </summary>
        public bool IsPGP { get; set; }
        /// <summary>
        /// To stroe the File name of the input file
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// To stroe the flag for any error in application
        /// </summary>
        public bool isError { get; set; }
        /// <summary>
        /// flag to check if file is to be downloaded form SFTP.
        /// </summary>
        public bool isdownload { get; set; }
        public string[] Files { get; set; }
        public string RequestID { get; set; }

    }
    public class APIResponseDataObjectSansa
    {
        public string Status { get; set; }
        public string Msg { get; set; }

    }
    public class getSessionRspSansa
    {
        public string Description { get; set; }
    }
    public class SessionIDRspSansa
    {

        public string SessionId { get; set; }

    }
    public class APIRequestDataObjectSansa
    {
        public string SessionId { get; set; }
        public string TranType { get; set; }
        public string SourceID { get; set; }
        public string RequestID { get; set; }
        public string TxnDateTime { get; set; }
        public string Msg { get; set; }
        public string APIURL { get; set; }

    }
    public class APIMessageSansa
    {
        /*CIF DATA for New Card generation*/
        public string CifId { get; set; }
        public string cifdata { get; set; }
        public string CustomerName { get; set; }
        public string NameOnCard { get; set; }
        public Int64 BinPrefix { get; set; }

        public string AccountNo { get; set; }
        public string Status { get; set; }

        public string BlockType { get; set; }
        public string InternationalCard { get; set; }

        public string AccountOpeningDate { get; set; }
        public string CifCreationDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string MothersMaidenName { get; set; }
        public string DateOfBirth { get; set; }
        public string CountryCode { get; set; }
        public string StdCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string SchemeCode { get; set; }
        public string BranchCode { get; set; }
        public string EnteredDate { get; set; }
        public string VerifiedDate { get; set; }
        public string PanNumber { get; set; }
        public string ModeOfOperation { get; set; }
        public string FourthLineEmbossing { get; set; }
        public string AadharNo { get; set; }
        public string IssueDebitCard { get; set; }
        public string PinMailer { get; set; }
        public string PhysicalPin { get; set; }
        public string Retry { get; set; }
        public string AccountType { get; set; }
        public string SuccessMessage { get; set; }
        public string FailedMessage { get; set; }
         
        public string VaidateBINPrefix { get; set; }
        public string AccountData { get; set; }
        public string AccountQualifier { get; set; }
        public string LinkingFlag { get; set; }
        public int IssuerNo { get; set; }
        public string PGKNo { get; set; }
        public string Code { get; set; }
        public string reserved10 { get; set; } //used for cardgeneration DE46
        /*CardReissue*/
        public string CardNo { get; set; }

        /*CardLimit*/
        public int nr_of_purchase { get; set; }
        public int purchase_daily_limit { get; set; }
        public int purchase_per_txn_limit { get; set; }

        public int nr_of_withdrawal { get; set; }
        public int withdrwal_daily_limit { get; set; }
        public int withdrwal_per_txn_limit { get; set; }

        public int nr_of_payment { get; set; }
        public int payment_daily_limit { get; set; }
        public int payment_per_txn_limit { get; set; }

        public int CNP_daily_limit { get; set; }
        public int CNP_per_txn_limit { get; set; }

        public int POSLimit { get; set; }  //Daily Transaction Limit for POS.
        public int ATMLimit { get; set; }  //Daily Transaction Limit for ATM.
        public int EComLimit { get; set; } //Daily Transaction Limit for CNP.

        public int POSLimitCount { get; set; }  //NO of POS Transaction allow.
        public int ATMLimitCount { get; set; }  //NO of ATM Transaction allow.
        public int PaymentsCount { get; set; } //NO of ECOM Transaction allow.

        public Double PTPOSLimit { get; set; }     //Per Transaction Limit for POS.
        public Double PTATMLimit { get; set; }     //Per Transaction Limit for ATM.
        public Double PTPaymentsLimit { get; set; }     //Per Transaction Limit for ATM.
        public Double PaymentsLimit { get; set; }  //Daily Transaction Limit for POS.
        public Double PTEComLimit { get; set; }    //Per Transaction Limit for CNP.


        public string Currency { get; set; }

        public string reserve1 { get; set; }
        public string reserve2 { get; set; }
        public string reserve3 { get; set; }
        public string reserve4 { get; set; }
        public string reserve5 { get; set; }
        public string SegmentCode { get; set; }

    }
    public class APIResponseObjectSansa
    {
        public string AccountNo { get; set; }
        public string EncAccountNo { get; set; }
        public string EncCardNo { get; set; }
        public string Status { get; set; }
        public string NewRspCode { get; set; }
        public string StatusDesc { get; set; }

        public string CifId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string EmailID { get; set; }
        public string CardNo { get; set; }
        public string Expiry { get; set; }
        public string Date_issued { get; set; }
        public string Date_activated { get; set; }
        public string HoldRspCode { get; set; }
        public string CardStatus { get; set; }

        public string ExpiryDate { get; set; }

        public int nr_of_purchase { get; set; }
        public int purchase_daily_limit { get; set; }
        public int purchase_per_txn_limit { get; set; }

        public int nr_of_withdrawal { get; set; }
        public int withdrwal_daily_limit { get; set; }
        public int withdrwal_per_txn_limit { get; set; }

        public int nr_of_payment { get; set; }
        public int payment_daily_limit { get; set; }
        public int payment_per_txn_limit { get; set; }

        public int CNP_daily_limit { get; set; }
        public int CNP_per_txn_limit { get; set; }


        public int POSLimit { get; set; }  //Daily Transaction Limit for POS.
        public int ATMLimit { get; set; }  //Daily Transaction Limit for ATM.
        public int EComLimit { get; set; } //Daily Transaction Limit for CNP.

        public int POSLimitCount { get; set; }  //NO of POS Transaction allow.
        public int ATMLimitCount { get; set; }  //NO of ATM Transaction allow.
        public int PaymentsCount { get; set; } //NO of ECOM Transaction allow.

        public Double PTPOSLimit { get; set; }     //Per Transaction Limit for POS.
        public Double PTATMLimit { get; set; }     //Per Transaction Limit for ATM.
        public Double PTPaymentsLimit { get; set; }     //Per Transaction Limit for ATM.
        public Double PaymentsLimit { get; set; }  //Daily Transaction Limit for POS.
        public Double PTEComLimit { get; set; }    //Per Transaction Limit for CNP.

        public string PinTryCount { get; set; }

        public string NameOnCard { get; set; }
        public Int64 BinPrefix { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string MothersMaidenName { get; set; }
        public string CountryCode { get; set; }
        public string SchemeCode { get; set; }
        public string BranchCode { get; set; }
        public string PanNumber { get; set; }
        public string FourthLineEmbossing { get; set; }
        public string AadharNo { get; set; }
        public string IssueDebitCard { get; set; }
        public string AccountType { get; set; }
        public string SuccessMessage { get; set; }
        public string FailedMessage { get; set; }
        public string VaidateBINPrefix { get; set; }
        public string AccountData { get; set; }
        public string AccountQualifier { get; set; }
        public string LinkingFlag { get; set; }
        public string Retry { get; set; }
        public string PGKNo { get; set; }
        public bool Trace { get; set; }

        public String DrivingLicExpDT { get; set; }
        public String NewNICNo { get; set; }
        public String CustomerOpeningDate { get; set; }
        public String CustomerType { get; set; }
        public String Gender { get; set; }

        public String OldNICNo { get; set; }
        public String PassportExpDt { get; set; }
        public String CustomerClassificatn { get; set; }
        public String MarketSeqment { get; set; }
        public String PassportNo { get; set; }

        public String BusinessRegNo { get; set; }
        public String FullOrDispName { get; set; }
        public String Age { get; set; }
        public String DrivingLicNo { get; set; }


        public String CFPRNM { get; set; }
        public String MEMOBAL{ get; set;}
        public String DMDOPN { get; set; }
        public String CURBAL { get; set; }
        public String DMACCT { get; set; }
        public String DMBRCH { get; set; }
        public String DMTYPE { get; set; }

        public DataTable dtdetails { get; set; }




    }
}
