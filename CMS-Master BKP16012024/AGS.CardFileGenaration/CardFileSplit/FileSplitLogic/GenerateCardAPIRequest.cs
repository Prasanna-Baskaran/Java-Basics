using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileSplit
{
    class GenerateCardAPIRequest
    {
        ModuleDAL ObjDAL = new ModuleDAL();
        internal void CallCardAPIService(DataRow dtrecord, DataTable DataTable, ConfigDataObject ObjData)
        {
            try
            {
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************CallCardAPIService START**************************", ObjData.IssuerNo.ToString(), 1);
                APIMessage ObjAPIRequest = new APIMessage();
                APIResponseObject ObjAPIResponse = new APIResponseObject();
                ObjAPIRequest = SETAPIRequestMsg(dtrecord, DataTable, ObjData);
                string Request = "ObjAPIRequest.Code:" + ObjAPIRequest.Code + "|" + "ObjAPIRequest.CifId:" + ObjAPIRequest.CifId + "|";
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", "**************************"+Request+"**************************", ObjData.IssuerNo.ToString(), 1);
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
                    ObjDAL.INSERTSwitchRespStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                    /*END*/
                    //ObjDAL.UpdateRecordISOStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                }
                else
                {
                    ObjDAL.updateAccountLinkingStatus(ObjAPIResponse, ObjData, ObjAPIRequest.Code);
                }
                
                
                
                
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
               return ObjAPImsg;
            }
            
            return ObjAPImsg;
        }
  

        
    }
}
