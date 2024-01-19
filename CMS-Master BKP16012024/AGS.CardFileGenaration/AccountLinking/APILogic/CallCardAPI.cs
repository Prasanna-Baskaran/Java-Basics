
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

namespace AccountLinking
{
    class CallCardAPI
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public void Process(APIMessage ObjAPImsg, APIResponseObject ObjData, string CardAPIURL, string sourceId)
        {
            try
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "PROCESS METHOD CALLED", 1);
                DataTable DtRequest = ModuleDAL.GetCardAPIRequest("AccountLinkingDelinking");
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
                ObjDataAPIreq.SourceID = sourceId;
                ObjDataAPIreq.TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ObjDataAPIreq.RequestID = DateTime.Now.ToString("MMdd") + DateTime.Now.ToString("HHmmss");

                CheckSessionID(ObjDataAPIreq);
                if (string.IsNullOrEmpty(ObjDataAPIreq.SessionId))
                {
                    ObjDataAPIreq.TranType = "GetSession";
                    APICall(ObjDataAPIreq, "", ObjData, CardAPIURL);
                }

                string msg = Newtonsoft.Json.JsonConvert.SerializeObject(ObjJSON);
                ObjDataAPIreq.Msg = AESEncryptionAndDecryption.Encrypt(msg, ObjDataAPIreq.SessionId);
                ObjDataAPIreq.TranType = "AccountLinkingDelinking";
                APICall(ObjDataAPIreq, ObjDataAPIreq.RequestID, ObjData, CardAPIURL);
            }
            catch (Exception ex)
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);

            }
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "****************************************************CARD API CALL ENDED**************************", 1);



        }
        public static void InitiateSSLTrust()
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
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);


                //ActivityLog.InsertSyncActivity(ex);



            }

        }
        public void APICall(APIRequestDataObject ObjDataAPIReq, string RequestId, APIResponseObject Objdata, string CardAPIURL)
        {
            try
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "APICall METHOD CALLED", 1);
                APIResponseDataObject DataAPIRspObject = new APIResponseDataObject();
                InitiateSSLTrust();
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(CardAPIURL);
                 var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:5470//agscardoperation.api");
                httpWebRequest.ContentType = "application/json";

                httpWebRequest.Method = "POST";
                string postjsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ObjDataAPIReq);
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "CARD API POST METHOD CALLED", 1);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
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
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "CARD API RSP:" + Objdata.Status, 1);

                }
            }
            catch (Exception ex)
            {

                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
                Objdata.Status = "140";
            }
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "**************************API Call ENDED**************************", 1);

        }

        public void CheckSessionID(APIRequestDataObject ObjDataAPIReq)
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
                    if (dtReturn.Rows.Count > 0)
                    {
                        ObjDataAPIReq.SessionId = Convert.ToString(dtReturn.Rows[0]["SessionId"]);
                    }

                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
            }
        }



    }
}
