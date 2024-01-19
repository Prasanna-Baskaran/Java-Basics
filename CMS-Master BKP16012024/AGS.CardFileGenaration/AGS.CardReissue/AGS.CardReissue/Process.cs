/************************************************************************
Object Name: Reissue for Banks
Purpose:Reissue Card file Generation 
Change History
-------------------------------------------------------------------------
Date            Changed By          Reason
-------------------------------------------------------------------------
26-Sept-2017    Diksha Walunj       Newly Developed
01-Oct-2017     Prerna              ATPCM-123: Fees modification (for Reffernce)
*************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Renci.SshNet;
using System.IO.Compression;
using AGS.SqlClient;

namespace AGS.CardReissue
{
    public class CardReissue
    {

        public string ErrorLogFilePath { get; set; }
        public CardReissue()
        {
            ErrorLogFilePath = string.Empty;
        }
        public void Process(int IssurNo)
        {
            try
            {
                FunInsertTextLog("Reissue process started******************", IssurNo);
                ClsPathsBO objPath = new ClsPathsBO();

                objPath = FunGetPaths(IssurNo);
                if (!string.IsNullOrEmpty(objPath.ID))
                {
                    //check file not  exists at source path and input path
                    if ((new DirectoryInfo(objPath.CardAutoSourcePath).GetFiles("*.txt")
                        .Where(file => file.Name.EndsWith(".txt") && file.Name.Contains(objPath.ReissueFileName))
                        .Select(file => file.Name).ToList().Count() == 0)
                        &&
                        (new DirectoryInfo(objPath.BAT_SourceFilePath).GetFiles("*.txt")
                        .Where(file => file.Name.EndsWith(".txt") && file.Name.Contains(objPath.ReissueFileName))
                        .Select(file => file.Name).ToList().Count() == 0)
                        )
                    {
                        //Generate CardReissue File
                        DownloadReissueFile(Convert.ToInt16(objPath.IssuerNo), objPath.ZipCardFilesPath, objPath.BAT_SourceFilePath, objPath.CardAutoFailedPath, objPath.IsSaveError);

                    }
                    else
                    {
                        FunInsertTextLog("Reissue file is found, no need to download from db, AutoSourcePath path:" + objPath.CardAutoSourcePath + " BAT_SourceFilePath path:" + objPath.BAT_SourceFilePath, IssurNo);
                    }

                }
                else
                {
                    FunInsertTextLog("Reissue is not configured", IssurNo);
                }
                FunInsertTextLog("Reissue process End***********************", IssurNo);

            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("CardReissue|Process", ex.Message, "", IssurNo.ToString(), "");
                FunInsertTextLog("CardReissue|Process" + ex.ToString(), IssurNo);

            }

        }

        /// <summary>
        /// Get all configuration paths for file processing
        /// </summary>
        /// <param name="IssuerNo">Bank IssuerNo</param>
        /// <returns>ClsPathsBO</returns>
        public ClsPathsBO FunGetPaths(int IssuerNo)
        {
            ClsPathsBO objPath = new ClsPathsBO();

            DataTable ObjDTOutPut = new DataTable();

            try
            {
                SqlConnection ObjConn = null;
                DataTable ObjDtOutPut = new DataTable();

                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_CA_GetPaths_For_CardGen", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, IssuerNo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {

                        List<ClsPathsBO> objList = new List<ClsPathsBO>();
                        // objList = ConvertToList<ClsPathsBO>(ObjDTOutPut);

                        objPath = BindDatatableToClass<ClsPathsBO>(ObjDTOutPut);
                    }

                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
            }
            catch (Exception ex)
            {
                objPath.ID = string.Empty;
                FunInsertIntoErrorLog("FunGetPaths", ex.Message, "IssuerNo=" + IssuerNo, IssuerNo.ToString(), string.Empty);
                FunInsertTextLog("FunGetPaths|Para, error:" + ex.ToString(), IssuerNo);



            }
            return objPath;
        }

        /// <summary>
        /// Download CardReissue File
        /// </summary>
        /// <param name="IssuerNo"></param>
        /// <param name="CardFilesInputPath"></param>
        /// <param name="CardFilesSourcePath"></param>
        /// <param name="CardFileFailedPath"></param>
        /// <param name="IsSaveError"></param>
        /// <returns></returns>
        internal string[] DownloadReissueFile(int IssuerNo, string CardFilesInputPath, string CardFilesSourcePath, string CardFileFailedPath, string IsSaveError)
        {
            FunInsertTextLog("Download Reissue File started", IssuerNo);
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            //bool blnfileexists = false;
            try
            {
                ObjConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_CACardReissueProcess", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Redownload", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, string.Empty);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }
                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    //if (IsSaveError == "1")
                    //{
                    //    FunInsertIntoErrorLog("DownloadFiles", null, "countCardFilesSourcePath=" + CardFilesSourcePath + ",CardFileFailedPath=" + ",filename=" + ObjDsOutPut.Tables[0].Rows[0]["FileNames"] + ",Table count=" + ObjDsOutPut.Tables.Count, IssuerNo.ToString(), string.Empty);
                    //}
                    //var BankList = ((from r in ObjDsOutPut.Tables[1].AsEnumerable()
                    //                 select r["Bank"].ToString()).Distinct().ToList());

                    string[] filenames = Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["FileNames"]).Split(',');
                    string[] sCompressedFile = new string[filenames.Count()];
                    if (filenames.Length + 1 == ObjDsOutPut.Tables.Count)
                    {

                        List<string> physicalfiles = new List<string>();
                        string filename;
                        string BankName = string.Empty;
                        ObjDsOutPut.Tables.RemoveAt(0);
                        String sCreationSuffix = DateTime.Now.ToString("ddMMyyhhmmss").Replace(" ", "_") + "_" + rnd.Next();
                        int i = 0;

                        foreach (DataTable dtOutput in ObjDsOutPut.Tables)
                        {
                            if (dtOutput.Rows.Count > 0)
                            {
                                filename = filenames[i].Trim();
                                if (!string.IsNullOrEmpty(filename))
                                {
                                    if (dtOutput.Rows.Count > 0)
                                    {

                                        try
                                        {
                                            string strLocFile = CreateDownloadCSVFile(dtOutput.ToSelectedTableColumns("Result"), getCurrentDirectory(CardFilesInputPath,IssuerNo), false, "", "", "",IssuerNo);
                                            System.IO.File.Move(getCurrentDirectory(CardFilesInputPath,IssuerNo) + "\\" + strLocFile, CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt");
                                            physicalfiles.Add(CardFilesSourcePath + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                            //blnfileexists = true;
                                            sCompressedFile[i] = CardFilesSourcePath + filename + "_" + sCreationSuffix + ".txt";
                                            i++;
                                            FunInsertTextLog("Download from db and File moved, path:"+ CardFilesSourcePath + "\\" + filename + "_" + sCreationSuffix + ".txt", IssuerNo);

                                        }
                                        catch (Exception ex)
                                        {
                                            if (IsSaveError == "1")
                                            {
                                                FunInsertIntoErrorLog("DownloadReissueFile|Create CardFiles", ex.Message, "CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                                            }
                                            FunInsertTextLog("DownloadReissueFile | Create CardFiles | Para:CardFilesSourcePath = " + CardFilesSourcePath + ",Error:" + ex.ToString(), IssuerNo);
                                        }
                                    }
                                }

                            }

                        }
                        return sCompressedFile.Where(c => c != null).ToArray();

                    }
                    else
                    {
                        return sCompressedFile;
                    }
                }
                else
                {
                    FunInsertTextLog("Download Reissue failed", IssuerNo);
                    return null;
                }

            }
            catch (Exception ex)
            {
                ObjConn.Close();
                FunInsertIntoErrorLog("DownloadReissueFile", ex.Message, "IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath, IssuerNo.ToString(), string.Empty);
                FunInsertTextLog("DownloadReissueFile|Para:IssuerNo =" + IssuerNo.ToString() + ",CardFilesInputPath =" + CardFilesInputPath + ", CardFilesSourcePath=" + CardFilesSourcePath + ",Error:" + ex.ToString(), IssuerNo);
                return null;
            }
        }
        //****************** Datatable functions ********************
        public DataTable GetTextFileIntoDataTable(string FilePath, string TableName, string delimiter, string IssuerNo)
        {
            //The DataTable to Return
            DataTable result = new DataTable();
            try
            {

                string AllData = string.Empty;
                //Open the file in a stream reader.
                using (StreamReader s = new StreamReader(FilePath))
                {

                    //Add specific column to datatable
                    DataColumn workCol = result.Columns.Add("CIF_ID", typeof(String));
                    workCol.AllowDBNull = false;
                    workCol.Unique = false;
                    result.Columns.Add("CustomerName", typeof(String));
                    result.Columns.Add("NameOnCard", typeof(String));
                    result.Columns.Add("Bin_Prefix", typeof(String));
                    result.Columns.Add("AccountNo", typeof(String));
                    result.Columns.Add("AccountOpeningDate", typeof(String));
                    result.Columns.Add("CIF_Creation_Date", typeof(String));
                    result.Columns.Add("Address1", typeof(String));
                    result.Columns.Add("Address2", typeof(String));
                    result.Columns.Add("Address3", typeof(String));
                    result.Columns.Add("City", typeof(String));
                    result.Columns.Add("State", typeof(String));
                    result.Columns.Add("PinCode", typeof(String));
                    result.Columns.Add("Country", typeof(String));
                    result.Columns.Add("Mothers_Name", typeof(String));
                    result.Columns.Add("DOB", typeof(String));
                    result.Columns.Add("CountryCode", typeof(String));
                    result.Columns.Add("STDCode", typeof(String));
                    result.Columns.Add("MobileNo", typeof(String));
                    result.Columns.Add("EmailID", typeof(String));
                    result.Columns.Add("SCHEME_Code", typeof(String));
                    result.Columns.Add("BRANCH_Code", typeof(String));
                    result.Columns.Add("Entered_Date", typeof(String));
                    result.Columns.Add("Verified_Date", typeof(String));
                    result.Columns.Add("PAN_No", typeof(String));
                    result.Columns.Add("Mode_Of_Operation", typeof(String));
                    result.Columns.Add("Fourth_Line_Embossing", typeof(String));
                    result.Columns.Add("Aadhar_No", typeof(String));
                    result.Columns.Add("Issue_DebitCard", typeof(String));
                    result.Columns.Add("Pin_Mailer", typeof(String));
                    result.Columns.Add("AccountType", typeof(String));
                    result.Columns.Add("SystemID", typeof(String));
                    //result.Columns.Add("Reason", typeof(String));
                    //result.Columns.Add("Gender", typeof(String));
                    //result.Columns.Add("Nationality", typeof(String));
                    //result.Columns.Add("StatementDelivery", typeof(String));
                    //result.Columns.Add("Email_For_Statement", typeof(String));
                    //result.Columns.Add("ProductType", typeof(String));

                    //Read the rest of the data in the file.        
                    AllData = s.ReadToEnd();
                    s.Close();
                    s.Dispose();

                }
                //Split off each row at the Carriage Return/Line Feed
                //Default line ending in most windows exports.  
                //You may have to edit this to match your particular file.
                //This will work for Excel, Access, etc. default exports.
                string[] rows = AllData.Split("\r\n".ToCharArray());


                //Now add each row to the DataTable        
                foreach (string r in rows)
                {
                    if (!string.IsNullOrEmpty(r))
                    {
                        //Split the row at the delimiter.
                        string[] items = r.Split(delimiter.ToCharArray());

                        //Add the item
                        result.Rows.Add(items);
                    }
                }
            }
            catch (Exception ex)
            {
                FunInsertIntoErrorLog("GetTextFileIntoDataTable", ex.Message, "CIF_File=" + FilePath, IssuerNo, string.Empty);
                FunInsertTextLog("GetTextFileIntoDataTable|Para : CIF_File = " + FilePath + ",Error:" + ex.ToString(), Convert.ToInt32(IssuerNo));
                result = new DataTable();
            }
            //Return the imported data.        
            return result;
        }


        public string CreateDownloadCSVFile(DataTable Data, string FilePath, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter, int IssuerNo)
        {
            string sFileName = "";

            try
            {
                sFileName = System.IO.Path.GetRandomFileName();

                using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(FilePath + "/" + sFileName))
                {
                    // Data.Columns.Remove("");
                    swObj.Write(ConvertDataTableToCSV(Data, WithHeader, Delimeter, CustomHeader, CustomFooter, IssuerNo));

                    swObj.Close();
                    swObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                sFileName = "";
                FunInsertTextLog("CreateDownloadCSVFile|Para:FilePath =" + FilePath + "/" + sFileName+ ",Error:" + ex.ToString(), IssuerNo);
            }
            finally
            {
            }

            return sFileName;
        }

        public string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter, int IssuerNo)
        {
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
            catch (Exception ex)
            {
                sbOutput = new StringBuilder();
                FunInsertTextLog("ConvertDataTableToCSV" + ",Error:" + ex.ToString(), IssuerNo);

            }

            return sbOutput.ToString();
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            try
            {
                var columnNames = dt.Columns.Cast<DataColumn>()
            .Select(c => c.ColumnName)
            .ToList();
                var properties = typeof(T).GetProperties();
                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();
                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name))
                        {
                            System.Reflection.PropertyInfo pI = objT.GetType().GetProperty(pro.Name);

                            Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                            object safeValue = row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], t);
                            pro.SetValue(objT, safeValue, null);
                        }
                    }
                    return objT;
                }).ToList();
            }
            catch (Exception ex)
            {
                //ErrorLogger.DBLog(ex, "DBHelper");
                throw ex;
            }
        }
        /// <summary>
        /// Bind Datatable to Class properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T BindDatatableToClass<T>(DataTable dt)
        {

            DataRow dr = dt.Rows[0];

            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object
            var ob = Activator.CreateInstance<T>();

            // Get all fields
            var fields = typeof(T).GetFields();
            foreach (var fieldInfo in fields)
            {
                if (columns.Contains(fieldInfo.Name))
                {
                    // Fill the data into the field
                    fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                }
            }

            // Get all properties
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (columns.Contains(propertyInfo.Name))
                {
                    // Fill the data into the property
                    System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                    Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                    object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                    propertyInfo.SetValue(ob, safeValue, null);

                    // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                }
            }

            return ob;
        }
        //**************** Save Error Logs ************
        public void FunInsertIntoErrorLog(string procedureName, string errorDesc, string parameterList, string IssuerNo, string BatchNo)
        {
            SqlConnection ObjConn = null;
            try
            {
                ObjConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CARDFILEGENERATECONSTR"].ConnectionString);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Sp_CAInsertErrorLog", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@procedureName", SqlDbType.VarChar, 200, ParameterDirection.Input, procedureName);
                    sspObj.AddParameterWithValue("@errorDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, errorDesc);
                    if (!string.IsNullOrEmpty(parameterList))
                        sspObj.AddParameterWithValue("@parameterList", SqlDbType.VarChar, 0, ParameterDirection.Input, parameterList);
                    if (!string.IsNullOrEmpty(IssuerNo))
                        sspObj.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    if (!string.IsNullOrEmpty(BatchNo))
                        sspObj.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, BatchNo);
                    sspObj.ExecuteNonQuery();
                    sspObj.Dispose();
                }
            }
            catch (Exception ex)
            {
                FunInsertTextLog("Insert db log error" + ex.ToString(),Convert.ToInt32(IssuerNo));
            }
        }
        public void FunInsertTextLog(string Message, int issuerNo = 0)
        {
            string LogPath = "";
            try
            {
                LogPath = System.Configuration.ConfigurationManager.AppSettings["DebugLogPath"].ToString();
                if (!string.IsNullOrEmpty(LogPath))
                {
                    string filename = issuerNo.ToString() + "ReissueDebug_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                    string filepath = LogPath + filename;
                    if (File.Exists(filepath))
                    {
                        using (StreamWriter writer = new StreamWriter(filepath, true))
                        {
                            writer.WriteLine(DateTime.Now + ":" + issuerNo.ToString() + ": " + Message);
                            writer.Close();
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = File.CreateText(filepath))
                        {
                            writer.WriteLine(DateTime.Now + ":" + issuerNo.ToString() + ": " + Message);
                            writer.Close();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                FunInsertIntoErrorLog("FunInsertIntoLogFile", Ex.Message, "LogPath=" + LogPath, "", string.Empty);
            }
        }
        //----------------------------------------------------------
        public string FunGetNewFileName(string Input)
        {
            string strNewName = string.Empty;
            strNewName = Input.Split(new string[] { ".txt" }, StringSplitOptions.RemoveEmptyEntries)[0] + "_D_" + DateTime.Now.ToString("ddMMyyhhmmss") + ".txt";
            return strNewName;
        }
        //-----------------------------------------------------------
        private string getCurrentDirectory(string ZipFilePath, int IssurNo)
        {
            string Path = ZipFilePath;

            if (!(System.IO.Directory.Exists(Path)))
            {
                try
                {
                    Directory.CreateDirectory(Path);
                }
                catch (Exception ex)
                {
                    FunInsertTextLog("getCurrentDirectory|Para:ZipFilePath =" + ZipFilePath + ",Error:" + ex.ToString(), IssurNo);
                }
            }


            return Path;
        }


        //*********** Classes ************
        public class ClsReturnStatusBO
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public string OutPutCode { get; set; }
            public int OutPutID { get; set; }
        }

        public class ClsPathsBO
        {
            public string ID { get; set; }
            public string BankName { get; set; }
            public string AGS_SFTPServer { get; set; }
            public string AGS_SFTPPath { get; set; }
            public string AGS_SFTP_User { get; set; }
            public string AGS_SFTP_Pwd { get; set; }
            public string AGS_SFTP_Port { get; set; }
            public string SFTP_CIF_Source_Path { get; set; }
            public string CardCIF_Input_Path { get; set; }
            public string CardCIF_Backup { get; set; }
            public string ZipCardFilesPath { get; set; }
            public string CardAutoSourcePath { get; set; }
            public string CardAutoOutputPath_SFTP { get; set; }
            public string PRE_InputPath { get; set; }
            public string PRE_ProcessPath { get; set; }
            public string PRE_OutputPath { get; set; }
            public string PRE_BackUp_Path { get; set; }
            public string B_SFTPServer { get; set; }
            public string B_SFTPPath { get; set; }
            public string B_SFTP_User { get; set; }
            public string B_SFTP_Pwd { get; set; }
            public string B_SFTP_Port { get; set; }
            public string B_PRE_DestinationPath_SFTP { get; set; }
            public string Zip_Exe_Path { get; set; }
            public string SFTP_CIF_BackUp_Path { get; set; }
            public string CardAutoBackUpPath { get; set; }
            public string CardAutoFailedPath { get; set; }
            public string IsSaveError { get; set; }
            public string IssuerNo { get; set; }
            public string ErrorLogPath { get; set; }
            public string FailedCIFPath { get; set; }
            public string B_SFTP_FailedCIFPath { get; set; }
            public int ProcessID { get; set; }
            public string FileName { get; set; }
            public string BAT_SourceFilePath { get; set; }
            public string BAT_SourceFilePath_BK { get; set; }
            public string SFTP_BAT_SourceFilePath { get; set; }
            public string SFTP_BAT_SourceFilePath_BK { get; set; }
            public string SFTP_OutputFile_BK_Path { get; set; }
            public string ReissueFileName { get; set; }
        }

    }
    public static class ExtentionMethods
    {
        public static DataTable ToSelectedTableColumns(this DataTable dtTable, string commaSepColumns)
        {
            String[] selectedColumns = new string[commaSepColumns.Split(',').Count()]; // = new  new[30] //{ "Column1", "Column2" };
            int i = 0;
            foreach (string columns in commaSepColumns.Split(','))
            {
                selectedColumns[i] = columns;
                i++;
            }
            return new DataView(dtTable).ToTable(false, selectedColumns);
        }
    }
}
