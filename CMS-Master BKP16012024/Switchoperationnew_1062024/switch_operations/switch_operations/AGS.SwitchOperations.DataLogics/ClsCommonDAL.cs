using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AGS.Configuration;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SqlClient;
using System.Text;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsCommonDAL
    {
        public  bool FunLog(string StrPriTransactionType, string StrPriRequestData, string StrPriOutPutData)
        {
            Boolean bReturn = true;
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrPriTransactionType", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriTransactionType);
                    sspObj.AddParameterWithValue("@StrPriRequestData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriRequestData);
                    sspObj.AddParameterWithValue("@StrPriOutPutData", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriOutPutData);

                    sspObj.ExecuteNonQuery();

                    sspObj.Dispose();
                    FuncloseConnection(ref ObjConn);

                }
            }
            catch (Exception xObj)
            {
                bReturn = false;
            }

            return bReturn;
        }

        public  string createTable_delete(DataTable objDTOutPut)
        {
            throw new NotImplementedException();
        }

        public  ClsReturnStatusBO FunLoginValidate(ClsLoginBO ObjPriLoginBO)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 0;
            ObjReturnStatus.Description = "Successful";

            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                
                String StrUsrSessionKey = Create16DigitString();

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SpValidateUserLogin", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@StrUserName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPriLoginBO.UserName.Trim());
                    ObjCmd.AddParameterWithValue("@StrUserPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjPriLoginBO.Password.Trim());
                    ObjCmd.AddParameterWithValue("@StrSessionKey", SqlDbType.VarChar, 0, ParameterDirection.Input, StrUsrSessionKey.Trim());

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    FuncloseConnection(ref ObjConn);
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                        if (ObjDTOutPut.Rows[0]["Code"].ToString() =="0")
                        {
                            ObjPriLoginBO.UserID = ObjDTOutPut.Rows[0]["UserID"].ToString();
                            ObjPriLoginBO.FirstName = ObjDTOutPut.Rows[0]["FirstName"].ToString();
                            ObjPriLoginBO.Lastname = ObjDTOutPut.Rows[0]["LastName"].ToString();
                            ObjPriLoginBO.UserRoleID = ObjDTOutPut.Rows[0]["UserRoleID"].ToString();
                            ObjPriLoginBO.MobileNo = ObjDTOutPut.Rows[0]["MobileNo"].ToString();
                            ObjPriLoginBO.EmailId = ObjDTOutPut.Rows[0]["EmailId"].ToString();
                        }                     
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "User Not Register !";
                    }
                }
            }

            catch (Exception ObjExc)
            {

                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = ObjExc.Message;
            }

            return ObjReturnStatus;
        }

        public static void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_InsertErrorLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    sspObj.AddParameterWithValue("@parameterList", SqlDbType.VarChar, 0, ParameterDirection.Input, parameterList);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                    FuncloseConnection(ref ObjConn);
                }
            }
            catch (Exception xObj)
            {

            }
        }

        public  void FunInsertIntoISOLog(string StrFunName, string StrPriParam, string StrISOString,string StrOutput)
        {
            SqlConnection ObjConn = null;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SPSetISOLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@StrFunName", SqlDbType.VarChar, 200, ParameterDirection.Input, StrFunName);
                    sspObj.AddParameterWithValue("@StrParam", SqlDbType.VarChar, 0, ParameterDirection.Input, StrPriParam);
                    sspObj.AddParameterWithValue("@StrISO", SqlDbType.VarChar, 0, ParameterDirection.Input, StrISOString);
                    sspObj.AddParameterWithValue("@StrOutput", SqlDbType.VarChar, 0, ParameterDirection.Input, StrOutput);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                    FuncloseConnection(ref ObjConn);
                }
            }
            catch (Exception xObj)
            {

            }
        }
        
        public  DataTable FunGetCommonDataTable(Int32 IntPriContextKey, string StrPriPara)
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
                    FuncloseConnection(ref ObjConn);
                }
            }

            catch (Exception ObjExc)
            {
                throw ObjExc;
            }

            return ObjDTOutPut; 
        }

        private  string Create16DigitString()
        {
            var builder = new StringBuilder();
            try
            {
                Random RNG = new Random();
                while (builder.Length < 16)
                {
                    builder.Append(RNG.Next(10).ToString());
                }
            }
            catch(Exception Ex)
            {
                builder.Append("");
            }
            return builder.ToString();
        }

        public  static void FunGetConnection(ref SqlConnection Connection, int Source)
        {
            switch (Source)
            {
                case 1:
                    Connection = ConfigManager.GetRBSQLDBOLAPConnection;
                    break;               
            }
        }

        public static void FuncloseConnection(ref SqlConnection Connection)
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Dispose();
            }
            Connection = null;
        }
        
        public  string FunGetParameter()
        {
            string StrConfigDtl = string.Empty;
            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {

                FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SpGetParam", ObjConn, CommandType.StoredProcedure))
                {
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut != null)
                    {
                        if (ObjDTOutPut.Rows.Count > 0)
                        {
                            StrConfigDtl = ObjDTOutPut.Rows[0][0].ToString() + "|" + ObjDTOutPut.Rows[0][1].ToString();
                        }
                    }
                    ObjCmd.Dispose();
                    FuncloseConnection(ref ObjConn);
                }
            }

            catch (Exception ObjExc)
            {
                throw ObjExc;
            }

            return StrConfigDtl;
        }
        public  string createTable_edit(DataTable dtResult)
        {
            StringBuilder strHTML = new StringBuilder();
            string strTRStart = string.Empty;
            string strTREnd = "</tr>";
            string strTDStart = "<td>";

            string strTDStartSuccess = "<td><span class='label label-success'>";
            string strTDStartPending = "<td><span class='label label-warning'>";
            string strTDStartCancel = "<td><span class='label label-danger'>";

            string strTDEndSuccess = "</span></td>";
            string strTDEnd = "</td>";

            try
            {
                //CREATE TABLE HEADER FROM DATATABLE
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        //ADD TABLE HEADERS
                        string[] columnNames = dtResult.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                        strHTML.Append("<thead class='tHead_style' >" + "<tr>");
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            strHTML.Append("<th class='text_middle'>" + columnNames[i].Trim() + "</th>");
                        }
                        strHTML.Append("<th class='text_middle'>" + "Edit" + "</th>");

                        strHTML.Append("</tr>" + "</thead>" + "<tbody>");

                        //ADD TABLE DATA 
                        foreach (DataRow currentRow in dtResult.Rows)
                        {
                            if (dtResult.Rows.IndexOf(currentRow) % 2 == 0)
                            {
                                strTRStart = "<tr class='odd gradeX text_middle'>";
                            }
                            else
                            {
                                strTRStart = "<tr class='even gradeC text_middle'>";
                            }

                            strHTML.Append(strTRStart);
                            for (int loopVar = 0; loopVar < columnNames.Length; loopVar++)
                            {
                                strHTML.Append(strTDStart + currentRow[columnNames[loopVar]].ToString().Trim() + strTDEnd);
                            }
                            //Start Prerna Patil btn btn-link  class='edit_btn'
                            strHTML.Append(strTDStart + "<input type=Button value='Edit' id='EditBtn' Text='Edit' class='btn btn-link' onClick='funedit($(this))'  >" + strTDEnd);

                            strHTML.Append(strTREnd);
                        }
                        strHTML.Append("</tbody>");
                    }
                }
                if (strHTML.ToString() == "")
                    return "No Results Found !";
                else
                    return strHTML.ToString();
            }
            catch (Exception Ex)
            {
                // Generix.insertIntoErrorLog("CS, frmMerchantTranHistroy.aspx, createTable()", Ex.Message, "");
                return "";
            }
        }

        //Encrypt Pin
        public  string FunEncryptPin(string CardNo, string Pin)
        {
            CardNo = CardNo.Trim();
            string StrResult = string.Empty;
            string StrCardNo = string.Empty;
            string StrPinCnt = string.Empty;
            string strOutput = string.Empty;
            try
            {
                //string StrZPK = "35D24122ACEF17DE20CDCC61442BC525";
                StrCardNo = CardNo.Substring(0, CardNo.Length - 1); //remove last digit
                StrCardNo = StrCardNo.Substring(StrCardNo.Length - 12, 12);  //get last 12 digits


                string strInputC1 = (Convert.ToString(Pin.Count()).PadLeft(2, '0') + Pin).PadRight(16, 'F'); //combination pin count +pin

                string strInputC2 = StrCardNo.PadLeft(16, '0');  //Card No  12 Digits

                long strInput = Convert.ToInt64(strInputC1, 16) ^ Convert.ToInt64(strInputC2, 16);  //XOR Operation

                //strOutput = TripleDes.Encrypt(strInput.ToString(), true);    //Final o/p
            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCommonDAL, FunEncryptPin()", Ex.Message, "CardNo= " + CardNo + " ,Pin= " + Pin);
            }
            return strOutput;
        }
        public static DataTable FunGetEmailConfig(string issuer, string processing_code)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetEmailConfig", ObjConn, CommandType.StoredProcedure))
                {
                    if (!string.IsNullOrEmpty(issuer))
                    {
                        ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, issuer);
                    }
                    if (!string.IsNullOrEmpty(processing_code))
                    {
                        ObjCmd.AddParameterWithValue("@ProcessCode", SqlDbType.VarChar, 0, ParameterDirection.Input, processing_code);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception e) { }
            return ObjDTOutPut;
        }

    }
}
