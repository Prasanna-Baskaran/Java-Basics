//using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Reflection;
//using ReflectionIT.Common.Data.Configuration;
using System.Net.Security;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AGS.SqlClient;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.Common;


namespace AGS.SwitchOperations
{
    class CallCardAPISansa
    {
        ModuleDALSansa ModuleDAL = new ModuleDALSansa();
        ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();

        public void Process(CustSearchFilter ObjFilter, APIResponseObjectSansa ObjAPIResponseObject)
        {
            try
            {
                ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();
                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallSansaAPIService Process START", "", ObjFilter.LoginId);
                DataTable ObjDTOutPut = new DataTable();
                DataTable DtRequest = ModuleDAL.GetCardAPIRequest(ObjFilter.tranType, ObjFilter);
                JObject ObjJSON = new JObject();
                if (DtRequest.Rows.Count > 0)
                {
                    foreach (DataRow item in DtRequest.Rows)
                    {
                        object currentobj = ObjFilter.GetType().GetProperty(Convert.ToString(item["ParamsName"]), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(ObjFilter);
                        ObjJSON.Add(Convert.ToString(item["ParamsName"]), Convert.ToString(currentobj));
                    }
                }

                string msg = Newtonsoft.Json.JsonConvert.SerializeObject(ObjJSON);


                APIRequestDataObjectSansa ObjDataAPIreq = new APIRequestDataObjectSansa();
                //ObjDataAPIreq.SourceID = Convert.ToString(ObjDataConfig.SourceID);
                ObjDataAPIreq.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ObjDataAPIreq.RequestID = DateTime.Now.ToString("MMdd") + DateTime.Now.ToString("HHmmss");
                ObjDataAPIreq.Msg = msg;
              


                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "APICall START", "", ObjFilter.LoginId);
                APICall(ObjDataAPIreq, ObjDataAPIreq.RequestID, ObjAPIResponseObject, ObjFilter);


            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallCardAPI Process() >> Error", ex.ToString(), ObjFilter.LoginId);
                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId.ToString(), ex.ToString(), "", ObjDataConfig.IssuerNo.ToString(), 0);

            }

            //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId, "", "**************************Card API Service ENDED**************************", ObjDataConfig.IssuerNo.ToString(), 1);


        }
        public void InitiateSSLTrust(CustSearchFilter ObjData)
        {

            try
            {
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
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.tranType, "", "", "", "CallCardAPI InitiateSSLTrust()", ex.ToString(), ObjData.LoginId);

            }

        }
        public void APICall(APIRequestDataObjectSansa ObjAPIRequestDataObject, string RequestId, APIResponseObjectSansa ObjAPIResponseObject, CustSearchFilter ObjConfigData)
        {
            try
            {
                APIResponseDataObjectSansa DataAPIRspObject = new APIResponseDataObjectSansa();
                ////DataAPIRspObject.Msg ="{"CIFDETAILS":[{"CIFDETAILS":"100","DETAILS":[{"CIF":"0000809161","DrivingLicExpDT":"","BusinessName":"Mr.P.A.C.Kumara","NewNICNo":"853132322V","CustomerOpeningDate":"110612","ShortName":"KUMARA PAC","City":"Kamburupitiya03","Gender":"M","CustomerType":"P","OldNICNo":"","Nationality":"S","PassportExpDt":"","CustomerClassification":"K","MarketSeqment":"EOP","DOB":"1985312","PassportNo":"","BusinessRegNo":"853132322V","FullOrDispName":"Mr.P.A.C.Kumara","BranchCode":"56","Age":"","DrivingLicNo":""}]}]}";
                ////DataAPIRspObject.Msg ="'{'COMPLETEACCOUNTLIST':[{'COMPLETEACCOUNTLIST':'100','SAVING':[{'CFPRNM':'Staff Security Savings Account','MEMOBAL':'15435.08','DMDOPN':'2012 - 06 - 11','CURBAL':'15935.08','DMACCT':'993454','DMBRCH':'56','DMTYPE':'113'},{'CFPRNM':'Staff Savings Account','MEMOBAL':'107911.65','DMDOPN':'2012 - 06 - 14','CURBAL':'108661.65','DMACCT':'996775','DMBRCH':'56','DMTYPE':'118'}]}]}'";
                //if (ObjConfigData.tranType == "SDBNICDETAILS" || ObjConfigData.tranType == "SDBACCDETAILS") //SDB NIC based ACC DETAILS
                //{
                //    DataAPIRspObject.Msg = "[{'CFPRNM':'Staff Security Savings Account','MEMOBAL':'15435.08','DMDOPN':'2012-06-11','CURBAL':'15935.08','DMACCT':'993454','DMBRCH':'56','DMTYPE':'113'},{'CFPRNM':'Staff Savings Account','MEMOBAL':'107911.65','DMDOPN':'2012-06-14','CURBAL':'108661.65','DMACCT':'0002380796','DMBRCH':'56','DMTYPE':'118'}]";
                //}
                //else if (ObjConfigData.tranType == "SDBCUSTDETAILS") // SDB CUST based DETAILS 
                //{
                //    DataAPIRspObject.Msg = "[{'CIF':'0000809161','DrivingLicExpDT':'','BusinessName':'Mr P A C Kumara','NewNICNo':'853132322V','CustomerOpeningDate':'31122018','ShortName':'KUMARA PAC','City':'Kamburupitiya','Gender':'F','CustomerType':'P','OldNICNo':'','Nationality':'S','PassportExpDt':'','CustomerClassification':'K','MarketSeqment':'EOP','DOB':'1985312','PassportNo':'','BusinessRegNo':'853132322V','FullOrDispName':'Mr.P.A.C.Kumara','BranchCode':'56','Age':'','DrivingLicNo':''}]";
                //}
                //else if (ObjConfigData.tranType == "SDBNICUSTCDETAILS") // SDB NIC based DETAILS
                //{
                //    DataAPIRspObject.Msg = "[{'CIF':'0000809161','DrivingLicExpDT':'','BusinessName':'Mr P A C Kumara','NewNICNo':'853132322V','CustomerOpeningDate':'31122018','ShortName':'KUMARA PAC','City':'Kamburupitiya','Gender':'M','CustomerType':'P','OldNICNo':'','Nationality':'S','PassportExpDt':'','CustomerClassification':'K','MarketSeqment':'EOP','DOB':'1985312','PassportNo':'','BusinessRegNo':'853132322V','FullOrDispName':'Mr.P.A.C.Kumara','BranchCode':'56','Age':'','DrivingLicNo':''}]";
                //}
                //else if (ObjConfigData.tranType == "SDBCUSTDETAILS") // SDB Card based DETAILS 
                //{
                //    DataAPIRspObject.Msg = "[{'CIF':'0000809161','DrivingLicExpDT':'','BusinessName':'Mr P A C Kumara','NewNICNo':'853132322V','CustomerOpeningDate':'31122018','ShortName':'KUMARA PAC','City':'Kamburupitiya','Gender':'F','CustomerType':'P','OldNICNo':'','Nationality':'S','PassportExpDt':'','CustomerClassification':'K','MarketSeqment':'EOP','DOB':'1985312','PassportNo':'','BusinessRegNo':'853132322V','FullOrDispName':'Mr.P.A.C.Kumara','BranchCode':'56','Age':'','DrivingLicNo':''}]";
                //}
                //var json = DataAPIRspObject.Msg;
                //ObjAPIResponseObject.dtdetails = JsonConvert.DeserializeObject<DataTable>(json);
                //ObjAPIResponseObject.Status = "000";
                //return;

                //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************Card API Call Started**************************", ObjConfigData.IssuerNo.ToString(), 1);
                //APIResponseDataObjectSansa DataAPIRspObject = new APIResponseDataObjectSansa();
                InitiateSSLTrust(ObjConfigData);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ObjConfigData.SDBAPIURL);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

                httpWebRequest.Method = "POST";
                string postjsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ObjAPIRequestDataObject);

                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "Sansa API Request:" + postjsonString, "", ObjConfigData.LoginId);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "Sansa API CONNECTION STARTED", "", ObjConfigData.LoginId);

                    streamWriter.Write(postjsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responsedata = streamReader.ReadToEnd();
                    DataAPIRspObject = JsonConvert.DeserializeObject<APIResponseDataObjectSansa>(responsedata);

                }

                //DataAPIRspObject.Msg = "";// "{"CIFDETAILS":[{"CIFDETAILS":"100","DETAILS":[{"CIF":"0000809161","DrivingLicExpDT":"","BusinessName":"Mr.P.A.C.Kumara","NewNICNo":"853132322V","CustomerOpeningDate":"110612","ShortName":"KUMARA PAC","City":"Kamburupitiya03","Gender":"M","CustomerType":"P","OldNICNo":"","Nationality":"S","PassportExpDt":"","CustomerClassification":"K","MarketSeqment":"EOP","DOB":"1985312","PassportNo":"","BusinessRegNo":"853132322V","FullOrDispName":"Mr.P.A.C.Kumara","BranchCode":"56","Age":"","DrivingLicNo":""}]}]}";
                                                //DataAPIRspObject.Msg ="{"CIFDETAILS":[{"CIFDETAILS":"100","DETAILS":[{"CIF":"0000809161","DrivingLicExpDT":"","BusinessName":"Mr.P.A.C.Kumara","NewNICNo":"853132322V","CustomerOpeningDate":"110612","ShortName":"KUMARA PAC","City":"Kamburupitiya03","Gender":"M","CustomerType":"P","OldNICNo":"","Nationality":"S","PassportExpDt":"","CustomerClassification":"K","MarketSeqment":"EOP","DOB":"1985312","PassportNo":"","BusinessRegNo":"853132322V","FullOrDispName":"Mr.P.A.C.Kumara","BranchCode":"56","Age":"","DrivingLicNo":""}]}]}";
                                                //DataAPIRspObject.Msg ="{"COMPLETEACCOUNTLIST":[{"COMPLETEACCOUNTLIST":"100","SAVING":[{"CFPRNM":"Staff Security Savings Account","MEMOBAL":"15435.08","DMDOPN":"2012-06-11","CURBAL":"15935.08","DMACCT":"993454","DMBRCH":"56","DMTYPE":"113"},{"CFPRNM":"Staff Savings Account","MEMOBAL":"107911.65","DMDOPN":"2012-06-14","CURBAL":"108661.65","DMACCT":"996775","DMBRCH":"56","DMTYPE":"118"}]}]}";

                ObjAPIResponseObject.Status = DataAPIRspObject.Status;

                //ObjAPIResponseObject.StatusDesc = JsonConvert.SerializeObject(new { Description = DataAPIRspObject.Msg });
                //ObjAPIResponseObject.StatusDesc = JsonConvert.DeserializeObject<ObjAPIResponseObject.StatusDesc>(DataAPIRspObject.Msg);
                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "Card API Reponsecode:" + ObjAPIResponseObject.Status.ToString(), "Card API ReponseDesc: " + Convert.ToString(DataAPIRspObject.Msg), ObjConfigData.LoginId);

                    try
                    {
                    getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg);
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
                                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "CArd API rsp dob:" + ObjAPIResponseObject.DateOfBirth, "", ObjConfigData.LoginId);
                       
                    
                    }
                    catch (Exception ex)
                    {
                        _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                        //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.ToString(), "", ObjConfigData.IssuerNo.ToString(), 0);
                    }
                
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "CallCardAPI APICall", ex.ToString(), ObjConfigData.LoginId);
                ObjAPIResponseObject.Status = "140";
            }
            _ClsCommonBAL.FunInsertPortalISOLog(ObjConfigData.tranType, "", "", "", "API Call ENDED", "", ObjConfigData.LoginId);

            //ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************API Call ENDED**************************", ObjConfigData.IssuerNo.ToString(), 1);
        }


        public void CheckSessionID(APIRequestDataObject ObjDataAPIReq, ConfigDataObject ObjData)
        {
            DataTable dtReturn;
            SqlConnection oConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref oConn, 2);

                //oConn = new SqlConnection((("Data Source=10.10.0.54;Initial Catalog=Card_API;User ID=uagsrep;Password=ags@1234")));
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

    public class RecordDataTableConverter : Newtonsoft.Json.Converters.DataTableConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.Load(reader);
                token = new JArray(token.SelectTokens("COMPLETEACCOUNTLIST[*].SAVING"));
                using (var subReader = token.CreateReader())
                {
                    while (subReader.TokenType == JsonToken.None)
                        subReader.Read();
                    return base.ReadJson(subReader, objectType, existingValue, serializer); // Use base class to convert
                }
            }
            else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }
    }

}
