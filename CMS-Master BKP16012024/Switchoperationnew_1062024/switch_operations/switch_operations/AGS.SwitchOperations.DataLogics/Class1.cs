using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AGS.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AGS.Configuration;

namespace AGS.SwitchOperations.DataLogics
{
    public class Class1
    {
        public static DataTable FunGetCommonDataTable(Int32 IntPriContextKey, string StrPriPara)
        {
            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                string[] StrPriArrParam = { };
                if (StrPriPara != "")
                {
                    StrPriPara = StrPriPara + ",";
                    StrPriArrParam = StrPriPara.Split(',');
                }


                FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SpCommonGetDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPriContextKey", SqlDbType.VarChar, 0, ParameterDirection.Input, IntPriContextKey);
                    if (StrPriPara != "")
                    {
                        if (StrPriArrParam.Count() > 0)
                        {
                            for (Int16 IntPriParaCnt = 0; IntPriParaCnt < StrPriArrParam.Count(); IntPriParaCnt++)
                            {
                                if ((StrPriArrParam[IntPriParaCnt] != "") && (StrPriArrParam[IntPriParaCnt] != "''"))
                                {
                                    ObjCmd.AddParameterWithValue(("@StrPriPara" + (IntPriParaCnt + 1)), SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriArrParam[IntPriParaCnt].Trim());
                                }
                            }
                        }
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                }
            }

            catch (Exception ObjExc)
            {
                throw ObjExc;
            }

            return ObjDTOutPut;
        }


        //To be removed
        internal static void FunGetConnection(ref SqlConnection Connection, int Source)
        {
            switch (Source)
            {
                case 1:
                    Connection = ConfigManager.GetRBSQLDBOLAPConnection;
                    break;
            }
        }
    }

}
