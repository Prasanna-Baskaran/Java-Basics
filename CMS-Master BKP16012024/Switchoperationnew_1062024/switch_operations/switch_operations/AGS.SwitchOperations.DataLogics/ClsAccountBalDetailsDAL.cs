using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.BusinessObjects;
using System.Data.SqlClient;
using AGS.SqlClient;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsAccountBalDetailsDAL
    {
        public DataTable FunSearchAccountBalanceDetails(ClsAccountBalanceBO ObjAccBalDetails)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                //USP_GetCarddetails
                //Sp_GetSwitchAccountDetails
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Usp_GetAccountBalanceDetailsFromSwitch", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@BankId", SqlDbType.Int, 0, ParameterDirection.Input, ObjAccBalDetails.BankId);
                    ObjCmd.AddParameterWithValue("@CardNO", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjAccBalDetails.CardNo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsAccountLinkingDelinkingDAL, FunSearchCardDetails()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
    }
}
