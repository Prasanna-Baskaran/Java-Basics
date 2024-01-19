using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.CardAutomation
{
    class GenerateCardAPIRequest
    {
        public void process(AGS.CardAutomation.CardAutomation.ClsPathsBO Obj,Int32 IssuerNo)
        {
            try
            {
                DataTable DtRecordForProcessing = new DataTable();
                switch (Obj.ProcessID)
                {
                    case 1:
                        DtRecordForProcessing = getCIFRecordForISOProcessing(IssuerNo);
                        break;
                    case 2:
                        break;
                    case 3:
                        DtRecordForProcessing = getCIFRecordForISOProcessing(IssuerNo);
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        DtRecordForProcessing = getCIFRecordForISOProcessing(IssuerNo);
                        break;
                }

                if (DtRecordForProcessing.Rows.Count>0)
                {
                    CallCardAPIService(DtRecordForProcessing);
                }
                    
                


            }
            catch(Exception ex)
            {
                CardAutomation.FunInsertIntoErrorLog("GenerateCardAPIRequest" + "|" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", IssuerNo.ToString(), "");
            }
           
        }
        /// <summary>
        /// Download CIF RECORD 
        /// </summary>
        /// <param name="IssuerNo"></param>
        /// <param name="CardFilesInputPath"></param>
        /// <param name="CardFilesSourcePath"></param>
        /// <param name="CardFileFailedPath"></param>
        /// <param name="IsSaveError"></param>
        /// <returns></returns>
      
        internal APIMessage SETAPIRequestMsg(DataRow dtRow,DataTable ObjDTOutPut)
        {
            APIMessage ObjAPImsg = new APIMessage();
            ObjAPImsg = CardAutomation.BindDatatableToClass<APIMessage>(dtRow, ObjDTOutPut);
            return ObjAPImsg;
        }
        internal void CallCardAPIService( DataTable dtrecord)
        {
            APIMessage ObjAPIRequest = new APIMessage();
                string RequestType = string.Empty;
                RequestType = "GenerateCard";
                foreach (DataRow item in dtrecord.Rows)
                {

                    ObjAPIRequest = SETAPIRequestMsg(item, dtrecord);
                    if (ObjAPIRequest.LinkingFlag.Equals("true",StringComparison.OrdinalIgnoreCase))
                    {
                        RequestType = "AccountLinking";
                    }
                    string SwitchRsp = new CallCardAPI().Process(RequestType, ObjAPIRequest);
                    UpdateCIFRecordStatus(ObjAPIRequest.CIFDBID, SwitchRsp,ObjAPIRequest.IssuerNo);
  
                }   
        }
        internal DataTable getCIFRecordForISOProcessing(int IssuerNo)
        {
            DataTable ObjDsOutPut = new DataTable();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                new CardAutomation().FunInsertTextLog("Record Fetch: Start", IssuerNo);
                ObjConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CAGETCIFRECORDFORISOPROCESSING", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, string.Empty);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                CardAutomation.FunInsertIntoErrorLog("GenerateCardAPIRequest" + "|" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", IssuerNo.ToString(), "");
                return ObjDsOutPut;
            }
            return ObjDsOutPut;
        }
        internal void UpdateCIFRecordStatus(string id, string switchrsp, int IssuerNo)
        {
            SqlConnection ObjConn = null;
            try
            {
                new CardAutomation().FunInsertTextLog("Updating the Switch status in DB", IssuerNo);
                ObjConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_UpdateSwitchRsp", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@id", SqlDbType.VarChar, 0, ParameterDirection.Input, id);
                    ObjCmd.AddParameterWithValue("@SwitchRsp", SqlDbType.VarChar, 0, ParameterDirection.Input,switchrsp);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjCmd.ExecuteNonQuery();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }


            }
            catch (Exception ex)
            {
                CardAutomation.FunInsertIntoErrorLog("UpdateCIFRecordStatus" + "|" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString(), "", IssuerNo.ToString(), "");
                
            }
            
        }
    }
}
