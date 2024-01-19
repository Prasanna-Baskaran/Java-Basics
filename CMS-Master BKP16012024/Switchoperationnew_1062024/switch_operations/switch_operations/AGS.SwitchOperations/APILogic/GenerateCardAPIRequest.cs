using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.BusinessLogics;
using System.Net;

namespace AGS.SwitchOperations
{
    class GenerateCardAPIRequest
    {
        ModuleDAL ObjDAL = new ModuleDAL();
        ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();

        internal void CallCardAPIService(DataRow dtRow, DataTable DataTable, ConfigDataObject ObjData, APIResponseObject ObjAPIResponse)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.APIRequestType, "", "", "", "Card API request builder START", "", ObjData.LoginId);
                APIMessage ObjAPIRequest = new APIMessage();
                ObjAPIRequest = SETAPIRequestMsg(dtRow, DataTable, ObjData);
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.APIRequestType, "", "", "", "SETAPIRequestMsg Called.", "", ObjData.LoginId);

                ObjAPIRequest.IssuerNo = Convert.ToInt32("1" + ObjData.IssuerNo.PadLeft(5, '0'));

                new CallCardAPI().Process(ObjData, ObjAPIRequest, ObjAPIResponse);
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjData.APIRequestType, dtRow[0].ToString(), "", "", "CallCardAPIService >> Error", ex.ToString(), ObjData.LoginId);

                ObjData.ErrorDesc = "Error While ISO Processing |" + ex.ToString();
            }
        }

        internal APIMessage SETAPIRequestMsg(DataRow dtRow, DataTable ObjDTOutPut, ConfigDataObject ObjData)
        {
            APIMessage ObjAPImsg = new APIMessage();
            ProcessCardRequest _ProcessCardRequest = new ProcessCardRequest();
            try
            {
                ObjAPImsg = ModuleDAL.BindDatatableToClass<APIMessage>(dtRow, ObjDTOutPut);
            }
            catch (Exception ex)
            {
                //ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return ObjAPImsg;
            }

            return ObjAPImsg;
        }



    }
}
