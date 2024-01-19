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
   public class ClsLimitSetAdvisoryDAL
    {
        public ClsReturnStatusBO FunSaveLimitSetAdvisoryDetails(ClsLimitSetAdvisoryBO ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;        


            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_SaveLimitSetAdvisoryDetails", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@BIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BIN);
                    ObjCmd.AddParameterWithValue("@ThresholdLimit", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ThresholdLimit);
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    ObjCmd.AddParameterWithValue("@EmailID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.EmailId);
                    ObjCmd.AddParameterWithValue("@EmailTamplate", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.EmailTamplate);
                    ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.MobileNo);
                    ObjCmd.AddParameterWithValue("@SMSTamplate", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SMSTamplate);
                    ObjCmd.AddParameterWithValue("@USERID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.USERID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (Convert.ToString(ObjDTOutPut.Rows[0][0]).Equals("09",StringComparison.OrdinalIgnoreCase))
                    {
                        ObjReturnStatus.Description = "Invalid BIN Enter !";
                        ObjReturnStatus.Code = 1;
                    }
                    else
                    {
                        ObjReturnStatus.Description = "THRESHOLD LIMIT OF " + ObjFilter.ThresholdLimit.ToString() + " IS " + (Convert.ToString(ObjDTOutPut.Rows[0][0]).Equals("01", StringComparison.OrdinalIgnoreCase) ? "Save" : "Updated") + " FOR " + (ObjFilter.BIN.ToString().Equals("RBL",StringComparison.OrdinalIgnoreCase)?"Bank" : "BIN") +" : " + ObjFilter.BIN.ToString();
                        ObjReturnStatus.Code = 0;
                    }
                    
                        

                   
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunSaveLimitSetAdvisoryDetails()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "FAILED TO SAVE THE LIMIT";
                

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }
      
    }
}
