using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AGS.Configuration;
using AGS.SqlClient;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using Newtonsoft.Json;
using System.Text;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsCardProductionDAL
    {
        public DataTable GetPendingCardDetails()
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetCustomerDetailsForCard", ObjConn, CommandType.StoredProcedure))
                {
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                //ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { control = Controls.Checkbox, hideColumnName = true, index=0 } });
            }
            catch(Exception ex) {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, getPendingCardDetails()", ex.Message, "");
                //return ""; 
            }
            return ObjDTOutPut;
        }

        public  string GetUnauthorisedCardDetails()
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetUnauthorisedDetailsForCard", ObjConn, CommandType.StoredProcedure))
                {
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                return ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { control = Controls.Checkbox, hideColumnName = true, index = 0, attributes = new AGS.Utilities.Attribute[] { new AGS.Utilities.Attribute() { AttributeName = "cdcode", BindTableColumnValueWithAttribute = "#Code" } } } });
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, GetUnauthorisedCardDetails()", ex.Message, "");
                return "";
            }
        }

        public  string GetBatchHistory(ClsCardProductionBO objCardProduction)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetBatchHistory", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@BatchDate", SqlDbType.DateTime, 0, ParameterDirection.Input, objCardProduction.ProcessDate);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.BatchNo);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

                return ObjDTOutPut.ToHtmlTableString(); //"", new AddedTableData[] { new AddedTableData() { control = Controls.Checkbox, hideColumnName = true, index = 0 } });
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, GetBatchHistory()", ex.Message, "");
                return "";
            }
        }
            
    
        public  string ProcessPendingCardDetails( ClsCardProductionBO objCardProduction )
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_processCustomerDetailsForCard", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CardNos", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.CardNo);
                    ObjCmd.AddParameterWithValue("@UserId", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.UserId);
                    if (!string.IsNullOrEmpty(objCardProduction.SystemID))
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.SystemID);
                    if (!string.IsNullOrEmpty(objCardProduction.BankID))
                        ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.BankID);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    return ObjDsOutPut.Tables[1].ToHtmlTableString();
                }
                else
                {
                    return Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["Description"]);
                }
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, ProcessPendingCardDetails()", ex.Message, JsonConvert.SerializeObject(objCardProduction));
                return "An error occured..." + ex.Message;
            }
        }

        public  string AuthoriseeCardDetails(ClsCardProductionBO objCardProduction)
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_AuthoriseCustomerDetailsForCard", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CardNos", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.CardNo);
                    ObjCmd.AddParameterWithValue("@UserId", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.UserId);
                    if (!string.IsNullOrEmpty(objCardProduction.SystemID))
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.SystemID);
                    if (!string.IsNullOrEmpty(objCardProduction.BankID))
                        ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.BankID);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    return ObjDsOutPut.Tables[1].ToHtmlTableString();
                }
                else
                {
                    return Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["Description"]);
                }
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, AuthoriseeCardDetails()", ex.Message, JsonConvert.SerializeObject(objCardProduction));
                return "An error occured..." + ex.Message;
            }
        }

        public  string DownloadFiles(ClsCardProductionBO objCardProduction)
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            bool blnfileexists = false;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_getCardProdAccountsDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Redownload", SqlDbType.Bit, 0, ParameterDirection.Input, objCardProduction.ReDownload);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.BatchNo);
                    if (!string.IsNullOrEmpty(objCardProduction.SystemID))
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.SystemID);
                    if (!string.IsNullOrEmpty(objCardProduction.BankID))
                        ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, objCardProduction.BankID);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                String sCreationSuffix = DateTime.Now.ToString("ddMMMyyyyHHmmss") + "_" + HttpContext.Current.Session.SessionID.ToString();
                
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    string[] filenames = Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["FileNames"]).Split(',');
                    if (filenames.Length + 1 == ObjDsOutPut.Tables.Count)
                    {
                        int i = 0;
                        List<string> physicalfiles = new List<string>();
                        string filename; 
                        ObjDsOutPut.Tables.RemoveAt(0);
                        foreach (DataTable dtOutput in ObjDsOutPut.Tables)
                        {
                            filename = filenames[i].Trim(); i++;
                            if (!string.IsNullOrEmpty(filename))
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    string strLocFile = CreateDownloadCSVFile(dtOutput, HttpContext.Current.Server.MapPath("tempOutputs"), false, "", "", "");
                                    System.IO.File.Move(HttpContext.Current.Server.MapPath("tempOutputs") + "\\" + strLocFile, HttpContext.Current.Server.MapPath("tempOutputs") + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                    physicalfiles.Add(HttpContext.Current.Server.MapPath("tempOutputs") + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                    blnfileexists = true;
                                }
                                else
                                {
                                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, DownloadFiles()", filename + " table have no records", JsonConvert.SerializeObject(objCardProduction));
                                }
                            }
                            else
                            {
                                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, DownloadFiles()", filename + " found empty", JsonConvert.SerializeObject(objCardProduction));
                            }
                        }
                    }
                    else
                    {
                        ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, DownloadFiles()", "Count of file names written in stored procedures and count of table returns does not match(excluding 1st status table)", JsonConvert.SerializeObject(objCardProduction));
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }

                if (!blnfileexists)
                    return string.Empty;

                string sCompressedFile = "cardProd_" + sCreationSuffix + ".rar";

                System.Diagnostics.ProcessStartInfo zipProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                zipProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                zipProcessStartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["rarWorkingDirectory"]; //"C:\\Program Files\\Winrar\\";
                zipProcessStartInfo.FileName = "rar.exe";
                zipProcessStartInfo.Arguments = "a -df -ep -m5 -prbl_" + DateTime.Now.ToString("ddMMyy") + " " + Chr(34) + HttpContext.Current.Server.MapPath("tempOutputs") + "\\" + sCompressedFile + Chr(34) + " " + Chr(34) + HttpContext.Current.Server.MapPath("tempOutputs") + "\\*_" + sCreationSuffix + ".txt" + Chr(34);

                using (System.Diagnostics.Process zipProcess = System.Diagnostics.Process.Start(zipProcessStartInfo))
                {
                    zipProcess.WaitForExit();
                }

                zipProcessStartInfo = null;
                return sCompressedFile;
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, DownloadFiles()", ex.Message, JsonConvert.SerializeObject(objCardProduction));
                return string.Empty;
            }
        }

        public  string CreateDownloadCSVFile(DataTable Data, string FilePath, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            string sFileName = "";

            try
            {
                sFileName = System.IO.Path.GetRandomFileName();

                using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(FilePath + "/" + sFileName))
                {
                    swObj.Write(ConvertDataTableToCSV(Data, WithHeader, Delimeter, CustomHeader, CustomFooter));

                    swObj.Close();
                }
            }
            catch (Exception xObj)
            {
                sFileName = "";
            }
            finally
            {
            }

            return sFileName;
        }

        public  string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            string sOutput = "";
            string sRow = "";
            StringBuilder sbOutput = new StringBuilder();

            try
            {
                foreach (DataColumn dcTable in dtSource.Columns)
                {
                    if (dcTable.ColumnName.IndexOf('~') > 0)
                    {
                        dtSource.Columns.Remove(dcTable);
                    }
                }

                if (WithHeader)
                {
                    var columnNames = dtSource.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                    sbOutput.AppendLine(string.Join(Delimeter, columnNames));
                }
                else
                {
                    if (!String.IsNullOrEmpty(CustomHeader)) sbOutput.AppendLine(CustomHeader);
                }

                foreach (DataRow row in dtSource.Rows)
                {
                    var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    sbOutput.AppendLine(string.Join(Delimeter, fields));
                }

                if (!String.IsNullOrEmpty(CustomFooter)) sbOutput.AppendLine(CustomFooter);
            }
            catch (Exception xObj)
            {
                sbOutput = new StringBuilder();
            }

            return sbOutput.ToString();
        }

        public  string Chr(Int32 CharCode)
        {
            string strChar = "";

            try
            {
                strChar = Convert.ToString((char)CharCode);
            }
            catch (Exception xObj)
            {
                strChar = "";
            }
            finally
            {

            }

            return strChar;
        }

        public  List<ClsCardProductionBO> GetBatchNos(ClsCardProductionBO objCardProduction)
        {
            DataSet ObjDsOutPut = new DataSet();
            List<ClsCardProductionBO> lstCrdProductionBO = new List<ClsCardProductionBO>();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetProcessDate", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ProcessDate", SqlDbType.DateTime, 0, ParameterDirection.Input, objCardProduction.ProcessDate);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    foreach (DataRow dr in ObjDsOutPut.Tables[1].Rows)
                    {
                        lstCrdProductionBO.Add(new ClsCardProductionBO() { BatchNo = Convert.ToString(dr["BatchNo"]) });
                    }
                }
            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, GetBatchNos()", ex.Message, JsonConvert.SerializeObject(objCardProduction));
            }
            return lstCrdProductionBO;
        }


    }
}