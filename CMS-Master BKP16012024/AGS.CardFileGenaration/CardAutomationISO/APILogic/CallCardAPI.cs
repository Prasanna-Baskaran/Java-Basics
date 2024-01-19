
using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Reflection;
using ReflectionIT.Common.Data.Configuration;
using System.Net.Security;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AGS.CardAutomationISO
{
    class CallCardAPI
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public void Process(ConfigDataObject ObjDataConfig, APIMessage ObjAPImsg, APIResponseObject ObjData)
        {
            try
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId, "", "**************************Card API Service Started*********************", ObjDataConfig.IssuerNo.ToString(), 1);
                DataTable DtRequest = ModuleDAL.GetCardAPIRequest(ObjDataConfig.APIRequestType,ObjDataConfig);
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

                CheckSessionID(ObjDataAPIreq,ObjDataConfig);
                if (string.IsNullOrEmpty(ObjDataAPIreq.SessionId))
                {
                    ObjDataAPIreq.TranType = "GetSession";
                    APICall(ObjDataAPIreq, "", ObjData, ObjDataConfig.CardAPIURL, ObjDataConfig);
                }

                string msg = Newtonsoft.Json.JsonConvert.SerializeObject(ObjJSON);
                ObjDataAPIreq.Msg = AESEncryptionAndDecryption.Encrypt(msg, ObjDataAPIreq.SessionId);
                ObjDataAPIreq.TranType = ObjDataConfig.APIRequestType;

                //Added by gufraan of blank card no in card repin start
                if (ObjDataAPIreq.TranType.Equals("CardRePIN", StringComparison.OrdinalIgnoreCase))
                {
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId.ToString(), "", "Checking if card no is present or not", ObjDataConfig.IssuerNo.ToString(), 1);
                    if (string.IsNullOrEmpty(ObjAPImsg.CardNo))
                    {
                        ObjDataAPIreq.TranType = "CardRePINWithOutCardNo";
                        ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId.ToString(), "", "Change the requet type to CardRePIN to CardRePINWithOutCardNo", ObjDataConfig.IssuerNo.ToString(), 1);
                    }

                }
                //Added by gufraan of blank card no in card repin End

                APICall(ObjDataAPIreq, ObjDataAPIreq.RequestID, ObjData, ObjDataConfig.CardAPIURL, ObjDataConfig);
            }
            catch (Exception ex)
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId.ToString(), ex.ToString(), "", ObjDataConfig.IssuerNo.ToString(), 0);
                
            }

            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjDataConfig.FileID, ObjDataConfig.ProcessId, "", "**************************Card API Service ENDED**************************", ObjDataConfig.IssuerNo.ToString(), 1);


        }
        public static void InitiateSSLTrust(ConfigDataObject ObjData)
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
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                

                //ActivityLog.InsertSyncActivity(ex);



            }

        }
        public void APICall(APIRequestDataObject ObjDataAPIReq, string RequestId, APIResponseObject Objdata, string CardAPIURL, ConfigDataObject ObjConfigData)
        {
            try
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************Card API Call Started**************************", ObjConfigData.IssuerNo.ToString(), 1);
                APIResponseDataObject DataAPIRspObject = new APIResponseDataObject();
                InitiateSSLTrust(ObjConfigData);
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(CardAPIURL);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:5470//agscardoperation.api");
                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";

                #region ATPBF -1039 start
                httpWebRequest.KeepAlive = false;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
                #endregion

                string postjsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ObjDataAPIReq);
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "CArd API Request:" + postjsonString, ObjConfigData.IssuerNo.ToString(), 1);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************Card API CONNECTION STARTED**************************", ObjConfigData.IssuerNo.ToString(), 1);
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


                if (ObjDataAPIReq.TranType.Equals("GetSession", StringComparison.OrdinalIgnoreCase))
                {
                    ObjDataAPIReq.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjDataAPIReq.SourceID.Substring(0, 12) + ObjDataAPIReq.RequestID.Substring(0, 4));
                    getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(ObjDataAPIReq.Msg);
                    SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                    ObjDataAPIReq.SessionId = Session.SessionId;
                }
                else
                {
                    DataAPIRspObject.Msg = AESEncryptionAndDecryption.Decrypt(DataAPIRspObject.Msg, ObjDataAPIReq.SessionId);
                    Objdata.Status = DataAPIRspObject.Status;
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "Card API Reponsecode:" + Objdata.Status.ToString(), ObjConfigData.IssuerNo.ToString(), 1);
                    try
                    {
                        if (Objdata.Status.Equals("000", StringComparison.OrdinalIgnoreCase))
                        {
                            getSessionRsp SessionRsp = JsonConvert.DeserializeObject<getSessionRsp>(DataAPIRspObject.Msg);
                            if (ObjConfigData.IsNewCardGenerate)
                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);
                                Objdata.CardNo = dynamicObject.CustomerData[0].CardNo;
                                Objdata.EncAccountNo = dynamicObject.CustomerData[0].EncAccountNo;
                                Objdata.EncCardNo = dynamicObject.CustomerData[0].EncCardNo;
                                Objdata.NewRspCode = dynamicObject.CustomerData[0].NewRspCode;
                            }
                         else if(ObjDataAPIReq.TranType.Equals("CardRePIN", StringComparison.OrdinalIgnoreCase)|| ObjDataAPIReq.TranType.Equals("CardRePINWithOutCardNo", StringComparison.OrdinalIgnoreCase))
                                
                           {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);
                                Objdata.MobileNo = dynamicObject.CustomerData[0].MobileNo;
                                Objdata.CardProgram = dynamicObject.CustomerData[0].CardProgram;

                            }
                            else if (ObjConfigData.isFileBasedAccLink)

                            {
                                SessionIDRsp Session = JsonConvert.DeserializeObject<SessionIDRsp>(SessionRsp.Description);
                                dynamic dynamicObject = JsonConvert.DeserializeObject(SessionRsp.Description);
                                Objdata.EncAccountNo = dynamicObject.CustomerData[0].AccountNo;

                            }

                        }
                       
                        else
                        {
                            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "Status Code <> 000 Fetching Description", ObjConfigData.IssuerNo.ToString(), 1);
                            var JMsg = JObject.Parse(DataAPIRspObject.Msg);
                            Objdata.StatusDescription = Convert.ToString(JMsg["Description"]);

                            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "DescriptionMsg:" + DataAPIRspObject.Msg + " &Description:" + Objdata.StatusDescription, ObjConfigData.IssuerNo.ToString(), 1);

                        }


                    }
                    catch (Exception ex)
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.ToString(), "", ObjConfigData.IssuerNo.ToString(), 0);
                    }


                }



            }
            catch (Exception ex)
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.ToString(), "", ObjConfigData.IssuerNo.ToString(), 0);
                Objdata.Status = "140";
            }
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, "", "**************************API Call ENDED**************************", ObjConfigData.IssuerNo.ToString(), 1);
        }
       

        public void CheckSessionID(APIRequestDataObject ObjDataAPIReq, ConfigDataObject ObjData)
        {
            DataTable dtReturn;
            SqlConnection oConn = null;
            try
            {
                oConn = new SqlConnection(ConfigManager.parse((System.Configuration.ConfigurationManager.ConnectionStrings["CARD_API"].ConnectionString)));
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_GetSessionIdBySourceId", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@SourceId", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjDataAPIReq.SourceID);
                    sspObj.AddParameterWithValue("@IsCreate", SqlDbType.Int, 0, ParameterDirection.Input, 0);
                    dtReturn = sspObj.ExecuteDataTable();
                    if(dtReturn.Rows.Count>0)
                    {
                        ObjDataAPIReq.SessionId = Convert.ToString(dtReturn.Rows[0]["SessionId"]);
                    }
                    
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
            }
        }
        


    }
}
