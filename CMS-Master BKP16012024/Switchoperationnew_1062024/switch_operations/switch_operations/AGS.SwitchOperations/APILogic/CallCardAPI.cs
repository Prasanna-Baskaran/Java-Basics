//using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
//using ReflectionIT.Common.Data.Configuration;
using System.Net.Security;
using System.Reflection;
using AGS.SqlClient;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AGS.SwitchOperations
{
    class CallCardAPI
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();

        public void Process(ConfigDataObject ObjDataConfig, APIMessage ObjAPImsg, APIResponseObject ObjAPIResponseObject)
        {
            try
            {
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId, "", "**************************Card API Service Started*********************", ObjDataConfig.IssuerNo.ToString(), 1);
                _ClsCommonBAL.FunInsertPortalISOLog(ObjDataConfig.APIRequestType, "", "", "", "CallCardAPIService Process START", "", ObjDataConfig.LoginId);

                DataTable DtRequest = ModuleDAL.GetCardAPIRequest(ObjDataConfig.APIRequestType, ObjDataConfig);

                JObject ObjJSON = new JObject();
                if (DtRequest.Rows.Count > 0)
                {
                    foreach (DataRow item in DtRequest.Rows)
                    {
                        object currentobj = ObjAPImsg.GetType().GetProperty(Convert.ToString(item["ParamsName"]), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(ObjAPImsg);
                        ObjJSON.Add(Convert.ToString(item["ParamsName"]), Convert.ToString(currentobj));
                    }
                }

                APIRequestDataObject ObjDataAPIreq = new APIRequestDataObject();
                ObjDataAPIreq.SourceID = Convert.ToString(ObjDataConfig.SourceID);
                ObjDataAPIreq.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ObjDataAPIreq.RequestID = DateTime.Now.ToString("MMdd") + DateTime.Now.ToString("HHmmss");

                CheckSessionID(ObjDataAPIreq, ObjDataConfig);
                if (string.IsNullOrEmpty(ObjDataAPIreq.SessionId))
                {
                    ObjDataAPIreq.TranType = "GetSession";
                    APICall(ObjDataAPIreq, "", ObjAPIResponseObject, ObjDataConfig.CardAPIURL, ObjDataConfig);
                }

                string msg = Newtonsoft.Json.JsonConvert.SerializeObject(ObjJSON);
                _ClsCommonBAL.FunInsertPortalISOLog(ObjDataConfig.APIRequestType, msg, "", "", "CallCardAPIService Process msg", msg, ObjDataConfig.LoginId);

                ObjDataAPIreq.Msg = AESEncryptionAndDecryption.Encrypt(msg, ObjDataAPIreq.SessionId);
                ObjDataAPIreq.TranType = ObjDataConfig.APIRequestType;

                _ClsCommonBAL.FunInsertPortalISOLog(ObjDataConfig.APIRequestType, "", "", "", "APICall START", "", ObjDataConfig.LoginId);
                APICall(ObjDataAPIreq, ObjDataAPIreq.RequestID, ObjAPIResponseObject, ObjDataConfig.CardAPIURL, ObjDataConfig);


            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjDataConfig.APIRequestType, "", "", "", "CallCardAPI Process() >> Error", ex.ToString(), ObjDataConfig.LoginId);
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId.ToString(), ex.ToString(), "", ObjDataConfig.IssuerNo.ToString(), 0);

            }

            //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId, "", "**************************Card API Service ENDED**************************", ObjDataConfig.IssuerNo.ToString(), 1);


        }
        public void InitiateSSLTrust(ConfigDataObject ObjData)
        {

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //Change SSL checks so that all checks pass

                ServicePointManager.ServerCertificateValidationCallback =

                   new RemoteCertificateValidationCallback(

                        delegate

                        { return true; }
                    );

            }

            catch (Exception ex)
            {
                //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                //ActivityLog.InsertSyncActivity(ex);
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.APIRequestType, "", "", "", "CallCardAPI InitiateSSLTrust()", ex.ToString(), ObjData.LoginId);

            }

        }
        public void APICall(APIRequestDataObject ObjAPIRequestDataObject, string RequestId, APIResponseObject ObjAPIResponseObject, string CardAPIURL, ConfigDataObject ObjConfigData)
        {
            try
            {
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************Card API Call Started**************************", ObjConfigData.IssuerNo.ToString(), 1);
                APIResponseDataObject DataAPIRspObject = new APIResponseDataObject();
                //if (ObjAPIRequestDataObject.TranType == "GetAccountList")
                //{
                //    ObjAPIResponseObject.Status = "000";
                //    DataAPIRspObject.Msg = "{'Description':'{'CustomerDetailsWithLimits': [{'AccountNo': '1234567891234,10,4,2019-04-23 18:11:24.08,9004578909872@99675284115,10,7,2019-04-23 18:11:50.727,9004578909872@30089708872,10,1,null,9004578909872@97681114431,10,5,2019-04-23 18:13:00.78,9004578909872@9996855555555,20,2,2019-04-23 18:16:24.77,9004578909872@996752841412345899,20,6,2019-04-23 18:16:47.783,9004578909872@7755888888888,20,8,2019-04-23 18:19:28.577,9004578909872@95753852145,20,3,2019-04-23 18:20:03.377,9004578909872@'}]}'}";

                //    getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg.Replace("'", "\""));
                //    if (ObjConfigData.IsCardDetailsSearch)
                //    {
                //        SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                //        dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                //        ObjAPIResponseObject.Date_issued = dynamicObject.CustomerDetailsWithLimits[0].Date_issued;
                //        ObjAPIResponseObject.Date_activated = dynamicObject.CustomerDetailsWithLimits[0].Date_activated;
                //        ObjAPIResponseObject.CardStatus = dynamicObject.CustomerDetailsWithLimits[0].CardStatus;
                //        ObjAPIResponseObject.CardNo = dynamicObject.CustomerDetailsWithLimits[0].CardNo;

                //        ObjAPIResponseObject.ExpiryDate = dynamicObject.CustomerDetailsWithLimits[0].ExpiryDate;
                //        ObjAPIResponseObject.HoldRspCode = dynamicObject.CustomerDetailsWithLimits[0].HoldRspCode;
                //        ObjAPIResponseObject.MobileNo = dynamicObject.CustomerDetailsWithLimits[0].MobileNo;
                //        ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;

                //        ObjAPIResponseObject.CustomerName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                //        ObjAPIResponseObject.Address1 = dynamicObject.CustomerDetailsWithLimits[0].Address1;
                //        ObjAPIResponseObject.Address2 = dynamicObject.CustomerDetailsWithLimits[0].Address2;

                //        ObjAPIResponseObject.DateOfBirth = dynamicObject.CustomerDetailsWithLimits[0].DateOfBirth;
                //        ObjAPIResponseObject.EmailID = dynamicObject.CustomerDetailsWithLimits[0].EmailID;

                //        ObjAPIResponseObject.POSLimitCount = dynamicObject.CustomerDetailsWithLimits[0].POSLimitCount;
                //        ObjAPIResponseObject.POSLimit = dynamicObject.CustomerDetailsWithLimits[0].POSLimit;
                //        ObjAPIResponseObject.PTPOSLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPOSLimit;

                //        ObjAPIResponseObject.ATMLimitCount = dynamicObject.CustomerDetailsWithLimits[0].ATMLimitCount;
                //        ObjAPIResponseObject.ATMLimit = dynamicObject.CustomerDetailsWithLimits[0].ATMLimit;
                //        ObjAPIResponseObject.PTATMLimit = dynamicObject.CustomerDetailsWithLimits[0].PTATMLimit;

                //        ObjAPIResponseObject.PaymentsCount = dynamicObject.CustomerDetailsWithLimits[0].PaymentsCount;
                //        ObjAPIResponseObject.PaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PaymentsLimit;
                //        ObjAPIResponseObject.PTPaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPaymentsLimit;

                //        ObjAPIResponseObject.EComLimit = dynamicObject.CustomerDetailsWithLimits[0].EComLimit;
                //        ObjAPIResponseObject.PTEComLimit = dynamicObject.CustomerDetailsWithLimits[0].PTEComLimit;

                //        ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;
                //        ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                //        ObjAPIResponseObject.PinTryCount = dynamicObject.CustomerDetailsWithLimits[0].PinTryCount;
                //        _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                //    }
                //    if (ObjConfigData.IsAccountDetailsSearch)
                //    {
                //        SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                //        dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                //        ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                //    }
                //}
                //else if (ObjAPIRequestDataObject.TranType == "CustomerDataUpdateInsta")
                //{
                //    ObjAPIResponseObject.Status = "000";
                //    DataAPIRspObject.Msg = "Customer Data Updated Successfully";
                //}
                //else if (ObjAPIRequestDataObject.TranType == "CardDetails") //SDB NIC based ACC DETAILS
                //{
                //    DataAPIRspObject.Msg = "[{'CFPRNM':'Staff Security Savings Account','MEMOBAL':'15435.08','DMDOPN':'2012-06-11','CURBAL':'15935.08','DMACCT':'993454','DMBRCH':'56','DMTYPE':'113'},{'CFPRNM':'Staff Savings Account','MEMOBAL':'107911.65','DMDOPN':'2012-06-14','CURBAL':'108661.65','DMACCT':'996775','DMBRCH':'56','DMTYPE':'118'}]";
                //}
                //return;

                InitiateSSLTrust(ObjConfigData);
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(CardAPIURL);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:5470//agscardoperation.api");
                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";
                string postjsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ObjAPIRequestDataObject);

                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API Request:" + postjsonString, "", ObjConfigData.LoginId);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "Card API CONNECTION STARTED", "", ObjConfigData.LoginId);

                    streamWriter.Write(postjsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responsedata = streamReader.ReadToEnd();
                    DataAPIRspObject = JsonConvert.DeserializeObject<APIResponseDataObject>(responsedata);
                }
                if (ObjAPIRequestDataObject.TranType.Equals("GetSession", StringComparison.OrdinalIgnoreCase))
                {
                    ObjAPIRequestDataObject.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjAPIRequestDataObject.SourceID.Substring(0, 12) + ObjAPIRequestDataObject.RequestID.Substring(0, 4));
                    getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(ObjAPIRequestDataObject.Msg);
                    SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                    ObjAPIRequestDataObject.SessionId = Session.SessionId;
                }
                else
                {
                    DataAPIRspObject.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjAPIRequestDataObject.SessionId);
                    ObjAPIResponseObject.Status = DataAPIRspObject.Status;
                    //ObjAPIResponseObject.StatusDesc = JsonConvert.SerializeObject(new { Description = DataAPIRspObject.Msg });
                    //ObjAPIResponseObject.StatusDesc = JsonConvert.DeserializeObject<ObjAPIResponseObject.StatusDesc>(DataAPIRspObject.Msg);
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "Card API Reponsecode:" + ObjAPIResponseObject.Status.ToString(), "Card API ReponseDesc: " + Convert.ToString(DataAPIRspObject.Msg), ObjConfigData.LoginId);

                    try
                    {
                        if (ObjAPIResponseObject.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg);
                            if (ObjConfigData.IsCardDetailsSearch)
                            {
                                string cardstatus = string.Empty;

                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                ObjAPIResponseObject.Date_issued = dynamicObject.CustomerDetailsWithLimits[0].Date_issued;
                                ObjAPIResponseObject.Date_activated = dynamicObject.CustomerDetailsWithLimits[0].Date_activated;
                                ObjAPIResponseObject.CardStatus = dynamicObject.CustomerDetailsWithLimits[0].CardStatus;

                                ObjAPIResponseObject.CardNo = dynamicObject.CustomerDetailsWithLimits[0].CardNo;

                                ObjAPIResponseObject.ExpiryDate = dynamicObject.CustomerDetailsWithLimits[0].ExpiryDate;
                                ObjAPIResponseObject.HoldRspCode = dynamicObject.CustomerDetailsWithLimits[0].HoldRspCode;
                                ObjAPIResponseObject.MobileNo = dynamicObject.CustomerDetailsWithLimits[0].MobileNo;
                                ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;

                                ObjAPIResponseObject.CustomerName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                                ObjAPIResponseObject.Address1 = dynamicObject.CustomerDetailsWithLimits[0].Address1;
                                ObjAPIResponseObject.Address2 = dynamicObject.CustomerDetailsWithLimits[0].Address2;

                                ObjAPIResponseObject.DateOfBirth = dynamicObject.CustomerDetailsWithLimits[0].DateOfBirth;
                                ObjAPIResponseObject.EmailID = dynamicObject.CustomerDetailsWithLimits[0].EmailID;

                                ObjAPIResponseObject.POSLimitCount = dynamicObject.CustomerDetailsWithLimits[0].POSLimitCount;
                                ObjAPIResponseObject.POSLimit = dynamicObject.CustomerDetailsWithLimits[0].POSLimit;
                                ObjAPIResponseObject.PTPOSLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPOSLimit;

                                ObjAPIResponseObject.ATMLimitCount = dynamicObject.CustomerDetailsWithLimits[0].ATMLimitCount;
                                ObjAPIResponseObject.ATMLimit = dynamicObject.CustomerDetailsWithLimits[0].ATMLimit;
                                ObjAPIResponseObject.PTATMLimit = dynamicObject.CustomerDetailsWithLimits[0].PTATMLimit;

                                ObjAPIResponseObject.PaymentsCount = dynamicObject.CustomerDetailsWithLimits[0].PaymentsCount;
                                ObjAPIResponseObject.PaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PaymentsLimit;
                                ObjAPIResponseObject.PTPaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPaymentsLimit;

                                ObjAPIResponseObject.EComLimit = dynamicObject.CustomerDetailsWithLimits[0].EComLimit;
                                ObjAPIResponseObject.PTEComLimit = dynamicObject.CustomerDetailsWithLimits[0].PTEComLimit;

                                ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;
                                ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                                ObjAPIResponseObject.AC1 = dynamicObject.CustomerDetailsWithLimits[0].AC1;
                                ObjAPIResponseObject.AC2 = dynamicObject.CustomerDetailsWithLimits[0].AC2;
                                ObjAPIResponseObject.AC3 = dynamicObject.CustomerDetailsWithLimits[0].AC3;
                                ObjAPIResponseObject.AccountNumber4 = dynamicObject.CustomerDetailsWithLimits[0].AC4;
                                ObjAPIResponseObject.PinTryCount = dynamicObject.CustomerDetailsWithLimits[0].PinTryCount;
                                ObjAPIResponseObject.NameOnCard = dynamicObject.CustomerDetailsWithLimits[0].NameOnCard;

                                ObjAPIResponseObject.MotherMaidenName = dynamicObject.CustomerDetailsWithLimits[0].MotherMaidenName;
                                ObjAPIResponseObject.NicNr = dynamicObject.CustomerDetailsWithLimits[0].NicNr;
                                ObjAPIResponseObject.PGKNo = dynamicObject.CustomerDetailsWithLimits[0].pg_id;

                                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                            }
                            if (ObjConfigData.IsAccountDetailsSearch)
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                //ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                                ObjAPIResponseObject.AccountNumber1 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber1;

                            }
                            //24/11/202 $@chin
                            if (ObjConfigData.IsCustomerDetailsSearch)
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                ObjAPIResponseObject.CustomerName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                                ObjAPIResponseObject.FirstName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                                ObjAPIResponseObject.LastName = dynamicObject.CustomerDetailsWithLimits[0].LastName;
                                ObjAPIResponseObject.FullName = dynamicObject.CustomerDetailsWithLimits[0].FullName;
                                ObjAPIResponseObject.MobileNo = dynamicObject.CustomerDetailsWithLimits[0].MobileNo;
                                ObjAPIResponseObject.EmailID = dynamicObject.CustomerDetailsWithLimits[0].EmailID;
                                ObjAPIResponseObject.NICNumber = dynamicObject.CustomerDetailsWithLimits[0].NICNumber;
                                ObjAPIResponseObject.MotherName = dynamicObject.CustomerDetailsWithLimits[0].MotherName;
                                ObjAPIResponseObject.Address1 = dynamicObject.CustomerDetailsWithLimits[0].Address1;
                                ObjAPIResponseObject.Address2 = dynamicObject.CustomerDetailsWithLimits[0].Address2;
                                ObjAPIResponseObject.Address3 = dynamicObject.CustomerDetailsWithLimits[0].Address3;
                                ObjAPIResponseObject.Address4 = dynamicObject.CustomerDetailsWithLimits[0].Address4;
                                ObjAPIResponseObject.Address5 = dynamicObject.CustomerDetailsWithLimits[0].Address5;
                                ObjAPIResponseObject.DateOfBirth = dynamicObject.CustomerDetailsWithLimits[0].DateOfBirth;


                            }
                            else
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                ObjAPIResponseObject.AccountNumber1 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber1;
                                ObjAPIResponseObject.AccountNumber2 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber2;
                                ObjAPIResponseObject.AccountNumber3 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber3;
                                ObjAPIResponseObject.AccountNumber4 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber4;
                                ObjAPIResponseObject.AccountNumber5 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber5;
                                ObjAPIResponseObject.AccountNumber6 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber6;
                                ObjAPIResponseObject.AccountNumber7 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber7;
                                ObjAPIResponseObject.AccountNumber8 = dynamicObject.CustomerDetailsWithLimits[0].AccountNumber8;
                            }
                        }
                        else
                        {
                            var JMsg = JObject.Parse(DataAPIRspObject.Msg);
                            ObjAPIResponseObject.StatusDesc = Convert.ToString(JMsg["Description"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                        //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.ToString(), "", ObjConfigData.IssuerNo.ToString(), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                ObjAPIResponseObject.Status = "140";
            }
            _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "API Call ENDED", "", ObjConfigData.LoginId);

            //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************API Call ENDED**************************", ObjConfigData.IssuerNo.ToString(), 1);
        }

        public void APICall2(APIRequestDataObject ObjAPIRequestDataObject, string RequestId, APIResponseObject ObjAPIResponseObject, string CardAPIURL, ConfigDataObject ObjConfigData)
        {
            try
            {
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************Card API Call Started**************************", ObjConfigData.IssuerNo.ToString(), 1);
                APIResponseDataObject DataAPIRspObject = new APIResponseDataObject();
                if (ObjAPIRequestDataObject.TranType == "GetAccountList" || ObjAPIRequestDataObject.TranType == "GetCardLimitDetails")
                {
                    ObjAPIResponseObject.Status = "000";
                    if (ObjAPIRequestDataObject.TranType == "GetAccountList")
                    {
                        ////DataAPIRspObject.Msg = "{'Description':'{'CustomerDetailsWithLimits': [{'AccountNo': '1234567891234,10,4,2019-04-23 18:11:24.08,9004578909872@99675284115,10,7,2019-04-23 18:11:50.727,9004578909872@30089708872,10,1,null,9004578909872@97681114431,10,5,2019-04-23 18:13:00.78,9004578909872@9996855555555,20,2,2019-04-23 18:16:24.77,9004578909872@996752841412345899,20,6,2019-04-23 18:16:47.783,9004578909872@7755888888888,20,8,2019-04-23 18:19:28.577,9004578909872@95753852145,20,3,2019-04-23 18:20:03.377,9004578909872@'}]}'}";

                        /// DataAPIRspObject.Msg = "{'CustomerDetailsWithLimits': [{'AccountNo': '1234567891234,10,4,2019-04-23 18:11:24.08,9004578909872@99675284115,10,7,2019-04-23 18:11:50.727,9004578909872@30089708872,10,1,null,9004578909872@97681114431,10,5,2019-04-23 18:13:00.78,9004578909872@9996855555555,20,2,2019-04-23 18:16:24.77,9004578909872@996752841412345899,20,6,2019-04-23 18:16:47.783,9004578909872@7755888888888,20,8,2019-04-23 18:19:28.577,9004578909872@95753852145,20,3,2019-04-23 18:20:03.377,9004578909872@'}]}";


                        if (ObjConfigData.IsCardDetailsSearch)
                        {
                            ObjAPIResponseObject.Date_issued = "2020-12-17 21:55:17.0";
                            ObjAPIResponseObject.Date_activated = "1900-01-01 00:00:00.0";
                            ObjAPIResponseObject.CardStatus = "0";
                            ObjAPIResponseObject.CardNo = "6087660000000137";

                            ObjAPIResponseObject.ExpiryDate = "2512";
                            ObjAPIResponseObject.HoldRspCode = "";
                            ObjAPIResponseObject.MobileNo = "0000000000";
                            ObjAPIResponseObject.CifId = "CIFSDF18";

                            ObjAPIResponseObject.CustomerName = "INSTA189";
                            ObjAPIResponseObject.Address1 = "Dummy ADD1";
                            ObjAPIResponseObject.Address2 = "Dummy ADD2";

                            ObjAPIResponseObject.DateOfBirth = "19000101";
                            ObjAPIResponseObject.EmailID = "Dummy@1234.com";

                            ObjAPIResponseObject.POSLimitCount = 0;
                            ObjAPIResponseObject.POSLimit = 0;
                            ObjAPIResponseObject.PTPOSLimit = 0;

                            ObjAPIResponseObject.ATMLimitCount = 999;
                            ObjAPIResponseObject.ATMLimit = 20000000;
                            ObjAPIResponseObject.PTATMLimit = 20000000;

                            ObjAPIResponseObject.PaymentsCount = 0;
                            ObjAPIResponseObject.PaymentsLimit = 0;
                            ObjAPIResponseObject.PTPaymentsLimit = 0;

                            ObjAPIResponseObject.EComLimit = 0;
                            ObjAPIResponseObject.PTEComLimit = 0;

                            ObjAPIResponseObject.CifId = "CIFSDF18";
                            ObjAPIResponseObject.AccountNo = "34434443333";
                            ObjAPIResponseObject.PinTryCount = "0";
                            _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                        }
                        if (ObjConfigData.IsAccountDetailsSearch)
                        {

                            ObjAPIResponseObject.AccountNo = "1234567891234,10,4,2019-04-23 18:11:24.08,9004578909872@99675284115,10,7,2019-04-23 18:11:50.727,9004578909872@30089708872,10,1,null,9004578909872@97681114431,10,5,2019-04-23 18:13:00.78,9004578909872@9996855555555,20,2,2019-04-23 18:16:24.77,9004578909872@996752841412345899,20,6,2019-04-23 18:16:47.783,9004578909872@7755888888888,20,8,2019-04-23 18:19:28.577,9004578909872@95753852145,20,3,2019-04-23 18:20:03.377,9004578909872@";
                        }
                    }
                    else if (ObjAPIRequestDataObject.TranType == "GetCardLimitDetails")
                    {
                        if (ObjConfigData.IsCardDetailsSearch)
                        {
                            ObjAPIResponseObject.Date_issued = "2020-12-17 21:55:17.0";
                            ObjAPIResponseObject.Date_activated = "1900-01-01 00:00:00.0";
                            ObjAPIResponseObject.CardStatus = "0";
                            ObjAPIResponseObject.CardNo = "6087660000000137";

                            ObjAPIResponseObject.ExpiryDate = "2512";
                            ObjAPIResponseObject.HoldRspCode = "";
                            ObjAPIResponseObject.MobileNo = "0000000000";
                            ObjAPIResponseObject.CifId = "CIFSDF18";

                            ObjAPIResponseObject.CustomerName = "INSTA189";
                            ObjAPIResponseObject.Address1 = "Dummy ADD1";
                            ObjAPIResponseObject.Address2 = "Dummy ADD2";

                            ObjAPIResponseObject.DateOfBirth = "19000101";
                            ObjAPIResponseObject.EmailID = "Dummy@1234.com";

                            ObjAPIResponseObject.POSLimitCount = 0;
                            ObjAPIResponseObject.POSLimit = 0;
                            ObjAPIResponseObject.PTPOSLimit = 0;

                            ObjAPIResponseObject.ATMLimitCount = 999;
                            ObjAPIResponseObject.ATMLimit = 20000000;
                            ObjAPIResponseObject.PTATMLimit = 20000000;

                            ObjAPIResponseObject.PaymentsCount = 0;
                            ObjAPIResponseObject.PaymentsLimit = 0;
                            ObjAPIResponseObject.PTPaymentsLimit = 0;

                            ObjAPIResponseObject.EComLimit = 0;
                            ObjAPIResponseObject.PTEComLimit = 0;

                            ObjAPIResponseObject.CifId = "CIFSDF18";
                            ObjAPIResponseObject.AccountNo = "34434443333";
                            ObjAPIResponseObject.PinTryCount = "0";
                            _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                        }
                        if (ObjConfigData.IsAccountDetailsSearch)
                        {

                            ObjAPIResponseObject.AccountNo = "34434443333";
                        }
                        //DataAPIRspObject.Msg = "{'Description':'{\r\n  \'CustomerDetailsWithLimits\': [\r\n    {\r\n      \'AccountNo\': \'34434443333\',\r\n      \'CardNo\': \'6087660000000137\',\r\n      \'ExpiryDate\': \'2512\',\r\n      \'HoldRspCode\': \'  \',\r\n      \'MobileNo\': \'0000000000\',\r\n      \'CardStatus\': \'0\',\r\n      \'Date_issued\': \'2020-12-17 21:55:17.0\',\r\n      \'Date_activated\': \'1900-01-01 00:00:00.0\',\r\n      \'CifId\': \'CIFSDF18\',\r\n      \'CustomerName\': \'  INSTA189  \',\r\n      \'DateOfBirth\': \'19000101\',\r\n      \'EmailID\': \'Dummy@1234.com\',\r\n      \'POSLimitCount\': 0.0,\r\n      \'POSLimit\': 0.0,\r\n      \'PTPOSLimit\': 0.0,\r\n      \'ATMLimitCount\': 999.0,\r\n      \'ATMLimit\': 20000000.0,\r\n      \'PTATMLimit\': 20000000.0,\r\n      \'PaymentsCount\': 0.0,\r\n      \'PaymentsLimit\': 0.0,\r\n      \'PTPaymentsLimit\': 0.0,\r\n      \'EComLimit\': 0.0,\r\n      \'PTEComLimit\': 0.0,\r\n      \'Address1\': \'Dummy ADD1\',\r\n      \'Address2\': \'Dummy ADD2\'\r\n    }\r\n  ]\r\n}'}";
                    }
                    //getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg);
                    //getSessionRsp SessionRsp1 = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg.Replace("'", "\""));
                    //if (ObjConfigData.IsCardDetailsSearch)
                    //{
                    //    SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                    //    dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                    //    ObjAPIResponseObject.Date_issued = dynamicObject.CustomerDetailsWithLimits[0].Date_issued;
                    //    ObjAPIResponseObject.Date_activated = dynamicObject.CustomerDetailsWithLimits[0].Date_activated;
                    //    ObjAPIResponseObject.CardStatus = dynamicObject.CustomerDetailsWithLimits[0].CardStatus;
                    //    ObjAPIResponseObject.CardNo = dynamicObject.CustomerDetailsWithLimits[0].CardNo;

                    //    ObjAPIResponseObject.ExpiryDate = dynamicObject.CustomerDetailsWithLimits[0].ExpiryDate;
                    //    ObjAPIResponseObject.HoldRspCode = dynamicObject.CustomerDetailsWithLimits[0].HoldRspCode;
                    //    ObjAPIResponseObject.MobileNo = dynamicObject.CustomerDetailsWithLimits[0].MobileNo;
                    //    ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;

                    //    ObjAPIResponseObject.CustomerName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                    //    ObjAPIResponseObject.Address1 = dynamicObject.CustomerDetailsWithLimits[0].Address1;
                    //    ObjAPIResponseObject.Address2 = dynamicObject.CustomerDetailsWithLimits[0].Address2;

                    //    ObjAPIResponseObject.DateOfBirth = dynamicObject.CustomerDetailsWithLimits[0].DateOfBirth;
                    //    ObjAPIResponseObject.EmailID = dynamicObject.CustomerDetailsWithLimits[0].EmailID;

                    //    ObjAPIResponseObject.POSLimitCount = dynamicObject.CustomerDetailsWithLimits[0].POSLimitCount;
                    //    ObjAPIResponseObject.POSLimit = dynamicObject.CustomerDetailsWithLimits[0].POSLimit;
                    //    ObjAPIResponseObject.PTPOSLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPOSLimit;

                    //    ObjAPIResponseObject.ATMLimitCount = dynamicObject.CustomerDetailsWithLimits[0].ATMLimitCount;
                    //    ObjAPIResponseObject.ATMLimit = dynamicObject.CustomerDetailsWithLimits[0].ATMLimit;
                    //    ObjAPIResponseObject.PTATMLimit = dynamicObject.CustomerDetailsWithLimits[0].PTATMLimit;

                    //    ObjAPIResponseObject.PaymentsCount = dynamicObject.CustomerDetailsWithLimits[0].PaymentsCount;
                    //    ObjAPIResponseObject.PaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PaymentsLimit;
                    //    ObjAPIResponseObject.PTPaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPaymentsLimit;

                    //    ObjAPIResponseObject.EComLimit = dynamicObject.CustomerDetailsWithLimits[0].EComLimit;
                    //    ObjAPIResponseObject.PTEComLimit = dynamicObject.CustomerDetailsWithLimits[0].PTEComLimit;

                    //    ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;
                    //    ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                    //    ObjAPIResponseObject.PinTryCount = dynamicObject.CustomerDetailsWithLimits[0].PinTryCount;
                    //    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                    //}
                    //if (ObjConfigData.IsAccountDetailsSearch)
                    //{
                    //    SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                    //    dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                    //    ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                    //}
                }
                else if (ObjAPIRequestDataObject.TranType == "CustomerDataUpdateInsta")
                {
                    ObjAPIResponseObject.Status = "000";
                    DataAPIRspObject.Msg = "Customer Data Updated Successfully";
                }
                else if (ObjAPIRequestDataObject.TranType == "CardDetails") //SDB NIC based ACC DETAILS
                {
                    DataAPIRspObject.Msg = "[{'CFPRNM':'Staff Security Savings Account','MEMOBAL':'15435.08','DMDOPN':'2012-06-11','CURBAL':'15935.08','DMACCT':'993454','DMBRCH':'56','DMTYPE':'113'},{'CFPRNM':'Staff Savings Account','MEMOBAL':'107911.65','DMDOPN':'2012-06-14','CURBAL':'108661.65','DMACCT':'996775','DMBRCH':'56','DMTYPE':'118'}]";
                }
                else if (ObjAPIRequestDataObject.TranType == "CardStatusChange") //SDB NIC based ACC DETAILS
                {
                    ObjAPIResponseObject.Status = "000";
                    DataAPIRspObject.Msg = "Card Status Change Successfully";
                }
                else
                {
                    ObjAPIResponseObject.Status = "000";
                    DataAPIRspObject.Msg = "Account Linking Successfully";
                }



                return;

                InitiateSSLTrust(ObjConfigData);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(CardAPIURL);
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5470//agscardoperation.api");
                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";
                string postjsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ObjAPIRequestDataObject);

                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API Request:" + postjsonString, "", ObjConfigData.LoginId);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "Card API CONNECTION STARTED", "", ObjConfigData.LoginId);

                    streamWriter.Write(postjsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responsedata = streamReader.ReadToEnd();
                    DataAPIRspObject = JsonConvert.DeserializeObject<APIResponseDataObject>(responsedata);
                }
                if (ObjAPIRequestDataObject.TranType.Equals("GetSession", StringComparison.OrdinalIgnoreCase))
                {
                    ObjAPIRequestDataObject.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjAPIRequestDataObject.SourceID.Substring(0, 12) + ObjAPIRequestDataObject.RequestID.Substring(0, 4));
                    getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(ObjAPIRequestDataObject.Msg);
                    SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                    ObjAPIRequestDataObject.SessionId = Session.SessionId;
                }
                else
                {
                    DataAPIRspObject.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjAPIRequestDataObject.SessionId);
                    ObjAPIResponseObject.Status = DataAPIRspObject.Status;
                    //ObjAPIResponseObject.StatusDesc = JsonConvert.SerializeObject(new { Description = DataAPIRspObject.Msg });
                    //ObjAPIResponseObject.StatusDesc = JsonConvert.DeserializeObject<ObjAPIResponseObject.StatusDesc>(DataAPIRspObject.Msg);
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "Card API Reponsecode:" + ObjAPIResponseObject.Status.ToString(), "Card API ReponseDesc: " + Convert.ToString(DataAPIRspObject.Msg), ObjConfigData.LoginId);

                    try
                    {
                        if (ObjAPIResponseObject.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg);
                            if (ObjConfigData.IsCardDetailsSearch)
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                ObjAPIResponseObject.Date_issued = dynamicObject.CustomerDetailsWithLimits[0].Date_issued;
                                ObjAPIResponseObject.Date_activated = dynamicObject.CustomerDetailsWithLimits[0].Date_activated;
                                ObjAPIResponseObject.CardStatus = dynamicObject.CustomerDetailsWithLimits[0].CardStatus;
                                ObjAPIResponseObject.CardNo = dynamicObject.CustomerDetailsWithLimits[0].CardNo;

                                ObjAPIResponseObject.ExpiryDate = dynamicObject.CustomerDetailsWithLimits[0].ExpiryDate;
                                ObjAPIResponseObject.HoldRspCode = dynamicObject.CustomerDetailsWithLimits[0].HoldRspCode;
                                ObjAPIResponseObject.MobileNo = dynamicObject.CustomerDetailsWithLimits[0].MobileNo;
                                ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;

                                ObjAPIResponseObject.CustomerName = dynamicObject.CustomerDetailsWithLimits[0].CustomerName;
                                ObjAPIResponseObject.Address1 = dynamicObject.CustomerDetailsWithLimits[0].Address1;
                                ObjAPIResponseObject.Address2 = dynamicObject.CustomerDetailsWithLimits[0].Address2;

                                ObjAPIResponseObject.DateOfBirth = dynamicObject.CustomerDetailsWithLimits[0].DateOfBirth;
                                ObjAPIResponseObject.EmailID = dynamicObject.CustomerDetailsWithLimits[0].EmailID;

                                ObjAPIResponseObject.POSLimitCount = dynamicObject.CustomerDetailsWithLimits[0].POSLimitCount;
                                ObjAPIResponseObject.POSLimit = dynamicObject.CustomerDetailsWithLimits[0].POSLimit;
                                ObjAPIResponseObject.PTPOSLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPOSLimit;

                                ObjAPIResponseObject.ATMLimitCount = dynamicObject.CustomerDetailsWithLimits[0].ATMLimitCount;
                                ObjAPIResponseObject.ATMLimit = dynamicObject.CustomerDetailsWithLimits[0].ATMLimit;
                                ObjAPIResponseObject.PTATMLimit = dynamicObject.CustomerDetailsWithLimits[0].PTATMLimit;

                                ObjAPIResponseObject.PaymentsCount = dynamicObject.CustomerDetailsWithLimits[0].PaymentsCount;
                                ObjAPIResponseObject.PaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PaymentsLimit;
                                ObjAPIResponseObject.PTPaymentsLimit = dynamicObject.CustomerDetailsWithLimits[0].PTPaymentsLimit;

                                ObjAPIResponseObject.EComLimit = dynamicObject.CustomerDetailsWithLimits[0].EComLimit;
                                ObjAPIResponseObject.PTEComLimit = dynamicObject.CustomerDetailsWithLimits[0].PTEComLimit;

                                ObjAPIResponseObject.CifId = dynamicObject.CustomerDetailsWithLimits[0].CifId;
                                ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                                ObjAPIResponseObject.PinTryCount = dynamicObject.CustomerDetailsWithLimits[0].PinTryCount;
                                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                            }
                            if (ObjConfigData.IsAccountDetailsSearch)
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);

                                ObjAPIResponseObject.AccountNo = dynamicObject.CustomerDetailsWithLimits[0].AccountNo;
                            }
                        }
                        else
                        {
                            var JMsg = JObject.Parse(DataAPIRspObject.Msg);
                            ObjAPIResponseObject.StatusDesc = Convert.ToString(JMsg["Description"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                        //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.ToString(), "", ObjConfigData.IssuerNo.ToString(), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                ObjAPIResponseObject.Status = "140";
            }
            _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.APIRequestType, "", "", "", "API Call ENDED", "", ObjConfigData.LoginId);

            //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************API Call ENDED**************************", ObjConfigData.IssuerNo.ToString(), 1);
        }

        public void CheckSessionID(APIRequestDataObject ObjDataAPIReq, ConfigDataObject ObjData)
        {
            DataTable dtReturn;
            SqlConnection oConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref oConn, 2);

                //oConn = new SqlConnection((("Data Source=13.71.120.14;Initial Catalog=Card_API;User ID=uagsrep;Password=ags@1234")));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_GetSessionIdBySourceId", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@SourceId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjDataAPIReq.SourceID);
                    sspObj.AddParameterWithValue("@IsCreate", SqlDbType.Int, 0, ParameterDirection.Input, 0);
                    dtReturn = sspObj.ExecuteDataTable();
                    if (dtReturn.Rows.Count > 0)
                    {
                        ObjDataAPIReq.SessionId = Convert.ToString(dtReturn.Rows[0]["SessionId"]);
                    }

                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.APIRequestType, "", "", "", "CallCardAPI CheckSessionID >> Error", ex.ToString(), ObjData.LoginId);
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }
        }
    }
}
