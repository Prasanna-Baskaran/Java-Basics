using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Net.Sockets;

namespace AGS.RBLCardAutomationISO
{
    class RBLCardGenerationNewISOLogic
    {
        ModuleDAL ObjDAL = new ModuleDAL();
        public bool Process(DataRow dataRow, DataTable dataTable, ConfigDataObject ObjData, DataTable CardConfig)
        {
            bool result;
            try
            {
                string RBLCardGeneration_IPPORT = System.Convert.ToString(ConfigurationManager.AppSettings["RBLCardGeneration_IPPORT"]);
                if (dataTable.Rows.Count > 0)
                {
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "ISO Creation strated ", ObjData.IssuerNo.ToString(), 1);

                    APIMessage ObjAPIRequest = new APIMessage();
                    ObjAPIRequest = SETAPIRequestMsg(dataRow, dataTable, ObjData);

                    // stan removed because or redundancy and response 140
                    // record_id (Code) will be unique Bcause recorde is is identiti 
                    ObjAPIRequest.stan = ObjData.RetryCnt.ToString()+ this.GetStan(ObjAPIRequest.Code); //this.randomno();
                    ObjAPIRequest.IssuerNo = Convert.ToInt32("1" + ObjData.IssuerNo.PadLeft(5, '0'));
                    ObjAPIRequest.ServerIP = RBLCardGeneration_IPPORT.Split(new char[] { '|' })[0];
                    ObjAPIRequest.Port = int.Parse(RBLCardGeneration_IPPORT.Split(new char[] { '|' })[1]);

                    if ((ObjData.ISOCallCounter == 3) && (!ObjData.APIRequestType.Equals("AccountLinkingDelinking")) )
                    {
                        if (string.IsNullOrEmpty(ObjData.ISODataTemp))
                        {
                            ObjData.ISODataTemp = ObjAPIRequest.ISOData;
                        }
                        //else
                        //{
                        //    ObjAPIRequest.stan = this.GetStan(ObjAPIRequest.Code, 1);
                        //}

                        if (ObjAPIRequest.ISOData.Contains("|Retry|"))
                        {
                            // Retry counter 0
                            dataRow["ISOData"] = ObjAPIRequest.ISOData = ObjAPIRequest.ISOData.Replace("|Retry|", "|0|");
                        }
                        else
                        {
                            // Retry counter 1
                            ObjAPIRequest.ISOData = ObjData.ISODataTemp.Replace("|Retry|", "|1|");
                        }

                    }



                    ObjAPIRequest.proessingcode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("ProcessingCode")).FirstOrDefault()["DefaultValue"].ToString();
                    ObjAPIRequest.trace = true;

                    APIResponseObject ObjAPIResponse = new APIResponseObject();
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "ISO Creation started for APIRequestType '" + ObjData.APIRequestType + "'" + " Record ID:" + ObjAPIRequest.Code + " TraceFile: " + Convert.ToString(ObjAPIRequest.TraceFileName), ObjData.IssuerNo.ToString(), 1);

                    if (ObjData.APIRequestType.Equals("generateCard"))
                    {
                        ObjAPIResponse.Status = this.GenerateCardISOBuild(ObjAPIRequest, CardConfig, ObjAPIResponse, ObjData);
                    }
                    else if (ObjData.APIRequestType.Equals("AccountLinkingDelinking"))
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + ObjAPIRequest.Code + " | and ISOCallCounter:" + Convert.ToString(ObjData.ISOCallCounter), ObjData.IssuerNo.ToString(), 1);
                        ObjAPIResponse.Status = this.AccountLinkingISOBuild(ObjAPIRequest, CardConfig, ObjAPIResponse, ObjData);
                    }
                    else if (ObjData.APIRequestType.Equals("CardRenewal"))
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + ObjAPIRequest.Code + " | and ISOCallCounter:" + Convert.ToString(ObjData.ISOCallCounter), ObjData.IssuerNo.ToString(), 1);
                        ObjAPIResponse.Status = this.CardRenewalISOBuild(ObjAPIRequest, CardConfig, ObjAPIResponse, ObjData);
                    }
                    else {

                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + ObjAPIRequest.Code + " | and ISOCallCounter:" + Convert.ToString(ObjData.ISOCallCounter), ObjData.IssuerNo.ToString(), 1);
                        ObjAPIResponse.Status = this.CardUpgradeISOBuild(ObjAPIRequest, CardConfig, ObjAPIResponse, ObjData);

                    }


                    //if (!ObjData.IsNewCardGenerate) ObjAPIResponse.CardNo = ObjAPIRequest.CardNo;

                    if (!ObjData.APIRequestType.Equals("AccountLinkingDelinking"))
                    {
                        if (!string.IsNullOrEmpty(ObjAPIResponse.NewRspCode))
                        {
                            if (ObjAPIResponse.NewRspCode.Equals("42"))
                            {
                                ObjAPIResponse.Status = ObjAPIResponse.NewRspCode;
                            }
                        }

                        DataTable ObjDsOutPut = new DataTable();
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + ObjAPIRequest.Code + " | and ISOCallCounter:" + Convert.ToString(ObjData.ISOCallCounter), ObjData.IssuerNo.ToString(), 1);
                        ObjDsOutPut = new ModuleDAL().ManageSwitchRespStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);

                        if (ObjData.ISOCallCounter == 3)
                        {
                            if (ObjDsOutPut.Rows.Count > 0)
                            {
                                if (ObjDsOutPut.Rows[0]["cStatus"].ToString() == "1")// counter will be set to 0 after 3 try's if cStatus is 1 then retry
                                {
                                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "Record_Id: " + ObjAPIRequest.Code + " | Retry process !! Calling again RBLCardGeneration.Process().", ObjData.IssuerNo.ToString(), 1);
                                    ConfigDataObject ObjDataRetry = new ConfigDataObject();
                                    ObjDataRetry = ObjData;
                                    ObjData.RetryCnt++;
                                    Process(dataRow, dataTable, ObjData, CardConfig);
                                }
                            }

                        }
                    

                    }
                    else
                    {
                        new ModuleDAL().updateAccountLinkingStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                    }
                }
            }
            catch (System.Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.StepStatus = true;
                ObjData.ErrorDesc = "Error While ISO Creation |" + ex.ToString();
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        private string randomno()
        {
            System.Random random = new System.Random();
            return random.Next(0, 1000000).ToString();
        }

        private string GetStan(string code)
        {
            if (!string.IsNullOrEmpty(code) && code.Length>=5)
            {
                code = code.Substring(code.Length - 5);
            }
            else
            {
                code = code.PadLeft(5, '0');
            }
            return code;
        }

        private string GenerateCardISOBuild(APIMessage ObjRequest, DataTable CardConfig, APIResponseObject DataAPIRspObject, ConfigDataObject ObjConfigData)
        {
            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", "GenerateCardISOBuild strated for Record Id: " + Convert.ToString(ObjRequest.Code) + " TraceFile: " + Convert.ToString(ObjRequest.TraceFileName), ObjConfigData.IssuerNo.ToString(), 1);
            string result;
            try
            {
                string[] array = new string[130];
                string[] array2 = new string[130];
                string[] array3 = new string[65];
                string Time = System.DateTime.Now.ToString("HHmmss");
                string Date = System.DateTime.Now.ToString("MMdd");
                string TerminalId = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("TerminalId")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptor = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptor")).FirstOrDefault()["DefaultValue"].ToString();
                string MCC = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MCC")).FirstOrDefault()["DefaultValue"].ToString();
                string POSEntryMode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSEntryMode")).FirstOrDefault()["DefaultValue"].ToString();
                string POSConditionCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSConditionCode")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptorLocation = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptorLocation")).FirstOrDefault()["DefaultValue"].ToString();
                string CurrencyCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CurrencyCode")).FirstOrDefault()["DefaultValue"].ToString();
                string MTI = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MTI")).FirstOrDefault()["DefaultValue"].ToString();
                array[3] = (string.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode);
                array[7] = (string.IsNullOrEmpty(Date + Time) ? null : (Date + Time));
                array[11] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan);
                array[12] = (string.IsNullOrEmpty(Time) ? null : Time);
                array[13] = (string.IsNullOrEmpty(Date) ? null : Date);
                array[18] = (string.IsNullOrEmpty(MCC) ? null : MCC);
                array[22] = (string.IsNullOrEmpty(POSEntryMode) ? null : POSEntryMode);
                array[25] = (string.IsNullOrEmpty(POSConditionCode) ? null : POSConditionCode);
                array[32] = (string.IsNullOrEmpty(ObjConfigData.IssuerNo) ? null : ObjRequest.IssuerNo.ToString());
                array[41] = (string.IsNullOrEmpty(TerminalId) ? null : TerminalId);
                array[42] = (string.IsNullOrEmpty(CardAcceptor) ? null : CardAcceptor);
                array[43] = (string.IsNullOrEmpty(CardAcceptorLocation) ? null : CardAcceptorLocation);
                array[46] = (string.IsNullOrEmpty(ObjRequest.reserved10) ? null : ObjRequest.reserved10);
                array[48] = (string.IsNullOrEmpty(ObjRequest.ISOData) ? null : ObjRequest.ISOData);
                array[49] = (string.IsNullOrEmpty(CurrencyCode) ? null : CurrencyCode);
                array[129] = (string.IsNullOrEmpty(MTI) ? null : MTI);
                ISO8583 iSO = new ISO8583("POSTTERM", ObjRequest.trace, ObjRequest.TraceFileName);
                ObjRequest.RequestMessage = iSO.Build(array, array3, array[129]);
                StringBuilder StrISOLog = new StringBuilder();
                array = iSO.Parse(ObjRequest.RequestMessage, false, ref StrISOLog);
                //array = iSO.Parse(text5, false);
                //byte[] array4 = ISGenerixLib.stringToByteArray(text5);
                //SwitchInterface switchInterface = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
                //byte[] array5 = switchInterface.sendDataToSwitch(array4);

                // sending request over tcp
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Sending request over TCP ", ObjConfigData.IssuerNo.ToString(), 1);

                TCPCommunicator _TCPCommunicator = new TCPCommunicator();
                _TCPCommunicator.SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);
                //SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);

                if (DataAPIRspObject.ResponseMessage == null)
                {
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | No Response From Switch for Card Generation", ObjRequest.IssuerNo.ToString(), 1);
                    result = "140";
                }
                else
                {
                    //string text6 = ISGenerixLib.byteToHex(array5);
                    array2 = iSO.Parse(DataAPIRspObject.ResponseMessage, true, ref StrISOLog);
                    array3 = iSO.getDE127();
                    result = System.Convert.ToString(array2[39]);
                    result = (result.Length == 2) ? "0" + result : result;

                    if (result.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Reponse from switch >> " + (string.IsNullOrEmpty(result) ? "" : result), ObjConfigData.IssuerNo.ToString(), 1);
                        string[] rspArr1 = array2[48].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var iteminner in rspArr1)
                        {
                            string[] rspArr2 = iteminner.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            string key = "";
                            string value = "";
                            try
                            {
                                value = rspArr2[1];
                                if (!string.IsNullOrEmpty((rspArr2[0])))
                                {
                                    key = Convert.ToString(rspArr2[0]).Trim();
                                }
                                if (!string.IsNullOrEmpty((rspArr2[1])))
                                {
                                    value = Convert.ToString(rspArr2[1]).Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.Message.ToString(), "Record Id >> " + Convert.ToString(ObjRequest.Code), ObjConfigData.IssuerNo.ToString(), 0);
                            }

                            PropertyInfo propertyInfo = DataAPIRspObject.GetType().GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (propertyInfo != null)
                                propertyInfo.SetValue(DataAPIRspObject, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, ex.ToString(), " Record_Id: " + Convert.ToString(ObjRequest.Code) + "", ObjConfigData.IssuerNo.ToString(), 0);
                ObjConfigData.StepStatus = true;
                ObjConfigData.ErrorDesc = "Error While ISO Creation for Card Generation |" + ex.ToString();
                result = "140";
            }
            return result;
        }

        private string AccountLinkingISOBuild(APIMessage ObjRequest, DataTable CardConfig, APIResponseObject DataAPIRspObject, ConfigDataObject ObjConfigData)
        {
            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | AccountLinkingISOBuild strated | TraceFile: " + Convert.ToString(ObjRequest.TraceFileName), ObjConfigData.IssuerNo.ToString(), 1);
            string result;
            try
            {
                string[] array = new string[130];
                string[] array2 = new string[130];
                string[] array3 = new string[65];
                string Time = System.DateTime.Now.ToString("HHmmss");
                string Date = System.DateTime.Now.ToString("MMdd");
                string POSEntryMode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSEntryMode")).FirstOrDefault()["DefaultValue"].ToString();
                string POSConditionCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSConditionCode")).FirstOrDefault()["DefaultValue"].ToString();
                string CurrencyCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CurrencyCode")).FirstOrDefault()["DefaultValue"].ToString();
                string Rev = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("Rev")).FirstOrDefault()["DefaultValue"].ToString();
                string MTI = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MTI")).FirstOrDefault()["DefaultValue"].ToString();
                array[2] = (string.IsNullOrEmpty(ObjRequest.CardNo) ? null : ObjRequest.CardNo);
                array[3] = (string.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode);
                array[7] = (string.IsNullOrEmpty(Date + Time) ? null : (Date + Time));
                array[11] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan);
                array[12] = (string.IsNullOrEmpty(Time) ? null : Time);
                array[13] = (string.IsNullOrEmpty(Date) ? null : Date);
                array[22] = (string.IsNullOrEmpty(POSEntryMode) ? null : POSEntryMode);
                array[25] = (string.IsNullOrEmpty(POSConditionCode) ? null : POSConditionCode);
                array[32] = (string.IsNullOrEmpty(ObjConfigData.IssuerNo) ? null : ObjRequest.IssuerNo.ToString());
                array[48] = (string.IsNullOrEmpty(ObjRequest.ISOData) ? null : ObjRequest.ISOData);
                array[49] = (string.IsNullOrEmpty(CurrencyCode) ? null : CurrencyCode);
                array[123] = (string.IsNullOrEmpty(Rev) ? null : Rev);
                array[129] = (string.IsNullOrEmpty(MTI) ? null : MTI);
                ISO8583 iSO = new ISO8583("POSTTERM", ObjRequest.trace, ObjRequest.TraceFileName);
                ObjRequest.RequestMessage = iSO.Build(array, array3, array[129]);
                StringBuilder StrISOLog = new StringBuilder();
                array = iSO.Parse(ObjRequest.RequestMessage, false, ref StrISOLog);
                //byte[] array4 = ISGenerixLib.stringToByteArray(text5);
                //SwitchInterface switchInterface = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
                //byte[] array5 = switchInterface.sendDataToSwitch(array4);

                // sending request over tcp
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Sending request over TCP ", ObjConfigData.IssuerNo.ToString(), 1);
                TCPCommunicator _TCPCommunicator = new TCPCommunicator();
                _TCPCommunicator.SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);
                //SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);
                if (DataAPIRspObject.ResponseMessage == null)
                {
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | No Response From Switch while Account Linking-Delinking", ObjRequest.IssuerNo.ToString(), 1);
                    result = "140";
                }
                else
                {
                    //string text6 = ISGenerixLib.byteToHex(array5);
                    array2 = iSO.Parse(DataAPIRspObject.ResponseMessage, true, ref StrISOLog);
                    array3 = iSO.getDE127();
                    result = System.Convert.ToString(array2[39]);
                    result = (result.Length == 2) ? "0" + result : result;
                }
            }
            catch (System.Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, ex.ToString(), " Record_Id: " + Convert.ToString(ObjRequest.Code), ObjConfigData.IssuerNo.ToString(), 0);
                ObjConfigData.StepStatus = true;
                ObjConfigData.ErrorDesc = "Record ID: " + ObjRequest.Code + " Error While ISO Creation for Account Linking-Delinking |" + ex.ToString();
                result = "140";
            }
            return result;
        }


        private string CardRenewalISOBuild(APIMessage ObjRequest, DataTable CardConfig, APIResponseObject DataAPIRspObject, ConfigDataObject ObjConfigData)
        {
            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", "GenerateCardISOBuild strated for Record Id: " + Convert.ToString(ObjRequest.Code) + " TraceFile: " + Convert.ToString(ObjRequest.TraceFileName), ObjConfigData.IssuerNo.ToString(), 1);
            string result;
            try
            {
                string[] array = new string[130];
                string[] array2 = new string[130];
                string[] array3 = new string[65];
                string Time = System.DateTime.Now.ToString("HHmmss");
                string Date = System.DateTime.Now.ToString("MMdd");
                string TerminalId = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("TerminalId")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptor = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptor")).FirstOrDefault()["DefaultValue"].ToString();
                string MCC = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MCC")).FirstOrDefault()["DefaultValue"].ToString();
                string POSEntryMode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSEntryMode")).FirstOrDefault()["DefaultValue"].ToString();
                string POSConditionCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSConditionCode")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptorLocation = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptorLocation")).FirstOrDefault()["DefaultValue"].ToString();
                string CurrencyCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CurrencyCode")).FirstOrDefault()["DefaultValue"].ToString();
                string MTI = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MTI")).FirstOrDefault()["DefaultValue"].ToString();
                string flag = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("flag")).FirstOrDefault()["DefaultValue"].ToString();
                string RRN = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("RRN")).FirstOrDefault()["DefaultValue"].ToString();
                string Rev = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("Rev")).FirstOrDefault()["DefaultValue"].ToString();


                array[3] = (string.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode);
                array[7] = (string.IsNullOrEmpty(Date + Time) ? null : (Date + Time));
                array[11] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan);
                array[12] = (string.IsNullOrEmpty(Time) ? null : Time);
                array[13] = (string.IsNullOrEmpty(Date) ? null : Date);
                array[18] = (string.IsNullOrEmpty(MCC) ? null : MCC);
                array[22] = (string.IsNullOrEmpty(POSEntryMode) ? null : POSEntryMode);
                array[25] = (string.IsNullOrEmpty(POSConditionCode) ? null : POSConditionCode);
                array[32] = (string.IsNullOrEmpty(ObjConfigData.IssuerNo) ? null : ObjRequest.IssuerNo.ToString());
                array[37] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan) + Time;
                array[41] = (string.IsNullOrEmpty(TerminalId) ? null : TerminalId);               
                array[42] = (string.IsNullOrEmpty(CardAcceptor) ? null : CardAcceptor);
                array[43] = (string.IsNullOrEmpty(CardAcceptorLocation) ? null : CardAcceptorLocation);
                array[46] = (string.IsNullOrEmpty(flag) ? null : flag);
                array[48] = (string.IsNullOrEmpty(ObjRequest.ISOData) ? null : ObjRequest.ISOData);
                array[49] = (string.IsNullOrEmpty(CurrencyCode) ? null : CurrencyCode);
                array[123] = (string.IsNullOrEmpty(Rev) ? null : Rev);
                array[129] = (string.IsNullOrEmpty(MTI) ? null : MTI);
                ISO8583 iSO = new ISO8583("POSTTERM", ObjRequest.trace, ObjRequest.TraceFileName);
                ObjRequest.RequestMessage = iSO.Build(array, array3, array[129]);
                StringBuilder StrISOLog = new StringBuilder();
                array = iSO.Parse(ObjRequest.RequestMessage, false, ref StrISOLog);
                //array = iSO.Parse(text5, false);
                //byte[] array4 = ISGenerixLib.stringToByteArray(text5);
                //SwitchInterface switchInterface = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
                //byte[] array5 = switchInterface.sendDataToSwitch(array4);

                // sending request over tcp
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Sending request over TCP ", ObjConfigData.IssuerNo.ToString(), 1);

                TCPCommunicator _TCPCommunicator = new TCPCommunicator();
                _TCPCommunicator.SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);
                //SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);

                if (DataAPIRspObject.ResponseMessage == null)
                {
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | No Response From Switch for Card Generation", ObjRequest.IssuerNo.ToString(), 1);
                    result = "140";
                }
                else
                {
                    //string text6 = ISGenerixLib.byteToHex(array5);
                    array2 = iSO.Parse(DataAPIRspObject.ResponseMessage, true, ref StrISOLog);
                    array3 = iSO.getDE127();
                    result = System.Convert.ToString(array2[39]);
                    result = (result.Length == 2) ? "0" + result : result;

                    if (result.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Reponse from switch >> " + (string.IsNullOrEmpty(result) ? "" : result), ObjConfigData.IssuerNo.ToString(), 1);
                        string[] rspArr1 = array2[48].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var iteminner in rspArr1)
                        {
                            string[] rspArr2 = iteminner.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            string key = "";
                            string value = "";
                            try
                            {
                                //value = rspArr2[1];
                                if (!string.IsNullOrEmpty((rspArr2[0])))
                                {
                                    key = Convert.ToString(rspArr2[0]).Trim();
                                }
                                if (!string.IsNullOrEmpty((rspArr2[1])))
                                {
                                    value = Convert.ToString(rspArr2[1]).Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.Message.ToString(), "Record Id >> " + Convert.ToString(ObjRequest.Code), ObjConfigData.IssuerNo.ToString(), 0);
                            }

                            PropertyInfo propertyInfo = DataAPIRspObject.GetType().GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (propertyInfo != null)
                                propertyInfo.SetValue(DataAPIRspObject, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, ex.ToString(), " Record_Id: " + Convert.ToString(ObjRequest.Code) + "", ObjConfigData.IssuerNo.ToString(), 0);
                ObjConfigData.StepStatus = true;
                ObjConfigData.ErrorDesc = "Error While ISO Creation for Card Generation |" + ex.ToString();
                result = "140";
            }
            return result;
        }

        private string CardUpgradeISOBuild(APIMessage ObjRequest, DataTable CardConfig, APIResponseObject DataAPIRspObject, ConfigDataObject ObjConfigData)
        {
            new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", "GenerateCardISOBuild strated for Record Id: " + Convert.ToString(ObjRequest.Code) + " TraceFile: " + Convert.ToString(ObjRequest.TraceFileName), ObjConfigData.IssuerNo.ToString(), 1);
            string result;
            try
            {
                string[] array = new string[130];
                string[] array2 = new string[130];
                string[] array3 = new string[65];
                string Time = System.DateTime.Now.ToString("HHmmss");
                string Date = System.DateTime.Now.ToString("MMdd");
                string TerminalId = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("TerminalId")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptor = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptor")).FirstOrDefault()["DefaultValue"].ToString();
                string MCC = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MCC")).FirstOrDefault()["DefaultValue"].ToString();
                string POSEntryMode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSEntryMode")).FirstOrDefault()["DefaultValue"].ToString();
                string POSConditionCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("POSConditionCode")).FirstOrDefault()["DefaultValue"].ToString();
                string CardAcceptorLocation = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CardAcceptorLocation")).FirstOrDefault()["DefaultValue"].ToString();
                string CurrencyCode = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("CurrencyCode")).FirstOrDefault()["DefaultValue"].ToString();
                string MTI = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("MTI")).FirstOrDefault()["DefaultValue"].ToString();
                string flag = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("flag")).FirstOrDefault()["DefaultValue"].ToString();
                string RRN = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("RRN")).FirstOrDefault()["DefaultValue"].ToString();
                string Rev = CardConfig.AsEnumerable().Where(r => r.Field<string>("AppVariable").Equals("Rev")).FirstOrDefault()["DefaultValue"].ToString();


                array[3] = (string.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode);
                array[7] = (string.IsNullOrEmpty(Date + Time) ? null : (Date + Time));
                array[11] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan);
                array[12] = (string.IsNullOrEmpty(Time) ? null : Time);
                array[13] = (string.IsNullOrEmpty(Date) ? null : Date);
                array[18] = (string.IsNullOrEmpty(MCC) ? null : MCC);
                array[22] = (string.IsNullOrEmpty(POSEntryMode) ? null : POSEntryMode);
                array[25] = (string.IsNullOrEmpty(POSConditionCode) ? null : POSConditionCode);
                array[32] = (string.IsNullOrEmpty(ObjConfigData.IssuerNo) ? null : ObjRequest.IssuerNo.ToString());
                array[37] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan) + Time;
                array[41] = (string.IsNullOrEmpty(TerminalId) ? null : TerminalId);
                array[42] = (string.IsNullOrEmpty(CardAcceptor) ? null : CardAcceptor);
                array[43] = (string.IsNullOrEmpty(CardAcceptorLocation) ? null : CardAcceptorLocation);
                array[46] = (string.IsNullOrEmpty(flag) ? null : flag);
                array[48] = (string.IsNullOrEmpty(ObjRequest.ISOData) ? null : ObjRequest.ISOData);
                array[49] = (string.IsNullOrEmpty(CurrencyCode) ? null : CurrencyCode);
                array[123] = (string.IsNullOrEmpty(Rev) ? null : Rev);
                array[129] = (string.IsNullOrEmpty(MTI) ? null : MTI);
                ISO8583 iSO = new ISO8583("POSTTERM", ObjRequest.trace, ObjRequest.TraceFileName);
                ObjRequest.RequestMessage = iSO.Build(array, array3, array[129]);
                StringBuilder StrISOLog = new StringBuilder();
                array = iSO.Parse(ObjRequest.RequestMessage, false, ref StrISOLog);
                //array = iSO.Parse(text5, false);
                //byte[] array4 = ISGenerixLib.stringToByteArray(text5);
                //SwitchInterface switchInterface = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
                //byte[] array5 = switchInterface.sendDataToSwitch(array4);

                // sending request over tcp
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Sending request over TCP ", ObjConfigData.IssuerNo.ToString(), 1);

                TCPCommunicator _TCPCommunicator = new TCPCommunicator();
                _TCPCommunicator.SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);
                //SendRequest(ObjRequest, DataAPIRspObject, ObjConfigData);

                if (DataAPIRspObject.ResponseMessage == null)
                {
                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | No Response From Switch for Card Generation", ObjRequest.IssuerNo.ToString(), 1);
                    result = "140";
                }
                else
                {
                    //string text6 = ISGenerixLib.byteToHex(array5);
                    array2 = iSO.Parse(DataAPIRspObject.ResponseMessage, true, ref StrISOLog);
                    array3 = iSO.getDE127();
                    result = System.Convert.ToString(array2[39]);
                    result = (result.Length == 2) ? "0" + result : result;

                    if (result.Equals("000", StringComparison.OrdinalIgnoreCase))
                    {
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), "", " Record_Id: " + Convert.ToString(ObjRequest.Code) + " | Reponse from switch >> " + (string.IsNullOrEmpty(result) ? "" : result), ObjConfigData.IssuerNo.ToString(), 1);
                        string[] rspArr1 = array2[48].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var iteminner in rspArr1)
                        {
                            string[] rspArr2 = iteminner.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            string key = "";
                            string value = "";
                            try
                            {
                                //value = rspArr2[1];
                                if (!string.IsNullOrEmpty((rspArr2[0])))
                                {
                                    key = Convert.ToString(rspArr2[0]).Trim();
                                }
                                if (!string.IsNullOrEmpty((rspArr2[1])))
                                {
                                    value = Convert.ToString(rspArr2[1]).Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId.ToString(), ex.Message.ToString(), "Record Id >> " + Convert.ToString(ObjRequest.Code), ObjConfigData.IssuerNo.ToString(), 0);
                            }

                            PropertyInfo propertyInfo = DataAPIRspObject.GetType().GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (propertyInfo != null)
                                propertyInfo.SetValue(DataAPIRspObject, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfigData.FileID, ObjConfigData.ProcessId, ex.ToString(), " Record_Id: " + Convert.ToString(ObjRequest.Code) + "", ObjConfigData.IssuerNo.ToString(), 0);
                ObjConfigData.StepStatus = true;
                ObjConfigData.ErrorDesc = "Error While ISO Creation for Card Generation |" + ex.ToString();
                result = "140";
            }
            return result;
        }




        internal APIMessage SETAPIRequestMsg(DataRow dtRow, DataTable ObjDTOutPut, ConfigDataObject ObjData)
        {
            APIMessage ObjAPImsg = new APIMessage();
            try
            {
                ObjAPImsg = CardAutomation.BindDatatableToClass<APIMessage>(dtRow, ObjDTOutPut);
            }
            catch (Exception ex)
            {
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return ObjAPImsg;
            }

            return ObjAPImsg;
        }


        //private void SendRequest(APIMessage ObjAPIRequest, APIResponseObject DataAPIRspObject, ConfigDataObject ObjData)
        //{
        //    TcpClient tclSocket = new TcpClient();
        //    NetworkStream ns = null;
        //    Byte[] byResponse = null;
        //    String sResponse = "";
        //    //String sUniqueID = "";
        //    //String sResponseUniqueiID = "";
        //    bool isEmpty = false;
        //    DateTime emptyTime = DateTime.Now;

        //    // Read Server IP & Port
        //    String sServerIP = ObjAPIRequest.ServerIP;
        //    int iPort = Convert.ToInt32(ObjAPIRequest.Port); // Convert.ToInt16(ConfigurationSettings.AppSettings["RBLCardGeneration_IPPORT"].ToString());

        //    try
        //    {
        //        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "TCP Connection started for Record Id: " + Convert.ToString(ObjAPIRequest.Code), ObjData.IssuerNo.ToString(), 1);
        //        int Wait = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RBLCardGeneration_Wait"].ToString());
        //        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "Wait for :" + Wait.ToString() + " Sec...", ObjData.IssuerNo.ToString(), 1);
        //        Byte[] byRequest = ISGenerixLib.stringToByteArray(ObjAPIRequest.RequestMessage);
        //        // Connect to server
        //        tclSocket.Connect(sServerIP, iPort);
        //        ns = tclSocket.GetStream();

        //        // Send Data to Server
        //        ns.Write(byRequest, 0, byRequest.Length);

        //        // Read response from Server
        //        while (tclSocket.Connected)
        //        {
        //            if (ns.DataAvailable)
        //            {
        //                byResponse = new Byte[tclSocket.Available];
        //                ns.Read(byResponse, 0, byResponse.Length);
        //                sResponse = ISGenerixLib.byteToHex(byResponse);
        //                DataAPIRspObject.ResponseMessage = (string.IsNullOrEmpty(sResponse) ? null : sResponse);
        //                tclSocket.Close();
        //                //new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "Reponse from switch >> " + ((byResponse==null) ? "" : byResponse.ToString()), ObjData.IssuerNo.ToString(), 1);
        //            }
        //            else
        //            {
        //                if (isEmpty)
        //                {
        //                    if ((DateTime.Now - emptyTime).Seconds >= Wait)
        //                    {
        //                        tclSocket.Close();
        //                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "Disconnecting due to timeout for Record Id: " + Convert.ToString(ObjAPIRequest.Code), ObjData.IssuerNo.ToString(), 1);
        //                    }
        //                }
        //                else
        //                {
        //                    isEmpty = true;
        //                    emptyTime = DateTime.Now;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(DateTime.Now + " : Exception: " + e.ToString());
        //        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "Exception: " + e.ToString(), "Record Id: " + Convert.ToString(ObjAPIRequest.Code), ObjData.IssuerNo.ToString(), 0);
        //        if (tclSocket != null)
        //            if (tclSocket.Connected)
        //                tclSocket.Close();
        //        ObjData.StepStatus = true;
        //        ObjData.ErrorDesc = "ISO Call failed";
        //        DataAPIRspObject.ResponseMessage = null;
        //        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "TCP connection closed for Record Id: " + Convert.ToString(ObjAPIRequest.Code), ObjData.IssuerNo.ToString(), 0);
        //    }

        //}


    }
}
