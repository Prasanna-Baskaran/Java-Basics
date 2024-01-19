﻿using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AGS.RBLCardAutomationISO
{
    class GenerateCardAPIRequest
    {
        ModuleDAL ObjDAL = new ModuleDAL();
        internal void CallCardAPIService(DataRow dtrecord, DataTable DataTable, ConfigDataObject ObjData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************CallCardAPIService START**************************", ObjData.IssuerNo.ToString(), 1);
                APIMessage ObjAPIRequest = new APIMessage();
                APIResponseObject ObjAPIResponse = new APIResponseObject();
                ObjAPIRequest = SETAPIRequestMsg(dtrecord, DataTable, ObjData);
                /*RBL autojob three times ISO call for 108 switch response ATPBF-1034 Start*/
                if (ObjData.SwitchRespID != 0)
                {
                    ObjAPIRequest.Retry = "1";
                }
                /*RBL autojob three times ISO call for 108 switch response ATPBF-1034  END*/
                string Request = "ObjAPIRequest.Code:" + ObjAPIRequest.Code + "|" + "ObjAPIRequest.CifId:" + ObjAPIRequest.CifId + "|";
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************" + Request + "**************************", ObjData.IssuerNo.ToString(), 1);
                ObjAPIRequest.IssuerNo = Convert.ToInt32("1" + ObjData.IssuerNo.PadLeft(5, '0'));
                new CallCardAPI().Process(ObjData, ObjAPIRequest, ObjAPIResponse);
                if (!ObjData.IsNewCardGenerate) ObjAPIResponse.CardNo = ObjAPIRequest.CardNo;
                if (!ObjData.APIRequestType.Equals("AccountLinkingDelinking"))
                {
                    if (!string.IsNullOrEmpty(ObjAPIResponse.NewRspCode))
                    {
                        if (ObjAPIResponse.NewRspCode.Equals("42"))
                        {
                            ObjAPIResponse.Status = ObjAPIResponse.NewRspCode;
                        }
                    }

                    /*Logic change for deadlock*/
                    /*START*/
                    /*RBL autojob three times ISO call for 108 switch response ATPBF-1034  START*/
                    if (ObjData.ISOCallCounter == 0)///|| ObjAPIResponse.Status!="108")
                    {
                        ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " ObjData.ISOCallCounter==0", ObjData.IssuerNo.ToString(), 1);
                        ObjDAL.INSERTSwitchRespStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                    }
                    else
                    {
                        DataTable ObjDsOutPut = new DataTable();
                        ObjDsOutPut = ObjDAL.ManageSwitchRespStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                        if (ObjDsOutPut.Rows.Count > 0)
                        {
                            if (ObjDsOutPut.Rows[0]["cStatus"].ToString() == "1")
                            {
                                ObjData.SwitchRespID = Convert.ToInt32(ObjDsOutPut.Rows[0]["SwitchRespID"]);
                                CallCardAPIService(dtrecord, DataTable, ObjData);
                            }

                        }

                    }
                    /*RBL autojob three times ISO call for 108 switch response ATPBF-1034  END*/
                    /*END*/
                    //ObjDAL.UpdateRecordISOStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                }
                else
                {
                    ObjDAL.updateAccountLinkingStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                }




            }
            catch (Exception ex)
            {
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                ObjData.StepStatus = true;
                ObjData.ErrorDesc = "Error While ISO Processing |" + ex.ToString();
            }
            ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************CallCardAPIService END**************************", ObjData.IssuerNo.ToString(), 1);

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



    }
}