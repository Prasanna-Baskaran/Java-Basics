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

namespace AccountLinking
{
    class GenerateCardAPIRequest
    {
        ModuleDAL ObjDAL = new ModuleDAL();
        internal void CallCardAPIService(DataTable dtrecord)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "**************************CallCardAPIService START**************************", 0);
                APIMessage ObjAPIRequest = new APIMessage();
                DataTable DtConfig = ObjDAL.GETAPIURLANDSOurecID();
                string CardAPIURL=Convert.ToString(DtConfig.Rows[0]["URL"]);
                string SourceID=Convert.ToString(DtConfig.Rows[0]["SourceID"]);
                APIResponseObject ObjAPIResponse = new APIResponseObject();
                foreach (DataRow item in dtrecord.Rows)
                {

                    ObjAPIRequest = SETAPIRequestMsg(item, dtrecord);
                    ObjAPIRequest.IssuerNo = Convert.ToInt32("1"+ Convert.ToString(item["IssuerNo"]).PadLeft(5,'0'));                    

                    new CallCardAPI().Process(ObjAPIRequest,ObjAPIResponse,CardAPIURL,SourceID);
                    ObjDAL.UpdateRecordISOStatus(ObjAPIResponse, ObjAPIRequest.Code);                   
                    
                }
                
            }
            catch(Exception ex)
            {
                ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
                
            }
            ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "**************************CallCardAPIService ENDED**************************", 0);
                      
        }
        internal APIMessage SETAPIRequestMsg(DataRow dtRow, DataTable ObjDTOutPut)
        {
            APIMessage ObjAPImsg = new APIMessage();
            try
            {
                
                ObjAPImsg = AccountLinking.BindDatatableToClass<APIMessage>(dtRow, ObjDTOutPut);
            }
            catch(Exception ex)
            {
               ObjDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", 0);
               return ObjAPImsg;
            }
            
            return ObjAPImsg;
        }
  

        
    }
}
