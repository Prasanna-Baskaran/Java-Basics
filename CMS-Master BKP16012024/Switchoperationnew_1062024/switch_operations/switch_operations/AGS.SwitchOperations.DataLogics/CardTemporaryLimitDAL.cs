using AGS.SqlClient;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.DataLogics
{
    public class CardTemporaryLimitDAL
    {
        
        public string FunLogTemporarylimitRequest(TemporaryLimitRequestMSGParam ObjReqmsg, ClsAPIRequestBO APIRequestParam, string Response, string ResponseCode, string AccountLogId)
        {
            
            string ReturnLogId;
            SqlConnection ObjConn = null;
            try
            {
                //ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_LogTemporaryLimitRequestResponse", ObjConn, CommandType.StoredProcedure))
                {
                   
                    //ObjCmd.AddParameterWithValue("@Bankid", SqlDbType.Int, 0, ParameterDirection.Input, ObjAcclink.BankID);

                    //AccountLogId is blank it means this request
                    if (string.IsNullOrEmpty(AccountLogId))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjReqmsg.CardNo);
                        ObjCmd.AddParameterWithValue("@PerTxnLimit", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjReqmsg.reserved1);
                        ObjCmd.AddParameterWithValue("@PerTxnCount", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjReqmsg.reserved2);
                        ObjCmd.AddParameterWithValue("@OverallLimit", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjReqmsg.reserved3);
                        ObjCmd.AddParameterWithValue("@OverallCount", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjReqmsg.reserved4);
                        ObjCmd.AddParameterWithValue("@FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjReqmsg.reserved6);
                        ObjCmd.AddParameterWithValue("@ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjReqmsg.reserved5);
                        
                    }
                    else//Response
                    {
                        ObjCmd.AddParameterWithValue("@Response", SqlDbType.VarChar, 0, ParameterDirection.Input, Response);
                        ObjCmd.AddParameterWithValue("@ResponseDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponseCode);
                        ObjCmd.AddParameterWithValue("@LogId", SqlDbType.VarChar, 0, ParameterDirection.Input, AccountLogId);
                    }

                    ReturnLogId = Convert.ToString(ObjCmd.ExecuteScalar());
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ReturnLogId = "";
                ClsCommonDAL.FunInsertIntoErrorLog("CS, CardTemporaryLimitDAL, FunLogTemporarylimitRequest()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ReturnLogId;
        }

    }
}
