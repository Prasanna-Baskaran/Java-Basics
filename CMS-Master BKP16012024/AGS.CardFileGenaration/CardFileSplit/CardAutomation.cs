/************************************************************************
Object Name: CardAutomation for Banks
Purpose:for slit card file basis on ProcessingCode
Change History
-------------------------------------------------------------------------
Date            Changed By          Reason
-------------------------------------------------------------------------
13-02-2019   Uddesh Hirap       Newly Developed

*************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SqlClient;
using System.Configuration;
using Renci.SshNet;
using System.IO.Compression;
using AGSPGP;


namespace CardFileSplit
{
    public class CardAutomation : IDisposable
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public string ErrorLogFilePath { get; set; }
        public CardAutomation()
        {
            ErrorLogFilePath = string.Empty;
        }
        public void Process(int IssuerNo)
        {

            ConfigDataObject ObjConfig = new ConfigDataObject();

            try
            {
                ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************************APPLICATION STARTED**************************", IssuerNo.ToString(), 1);
                /*APPLICATION STARTED*/   

                /*GETTING THE PROCESS ID CONFIGURATION FOR THE BANK*/
               DataTable dtBankConfig = ModuleDAL.getBankConfig(IssuerNo);
               if (dtBankConfig.Rows.Count < 1)
               {
                   ModuleDAL.FunInsertIntoErrorLog("Process", "", "","Error:NO CONFIGURATION", "**************************NO CONFIGURATION RETRIVE CHECK THE STATUS !**************************", IssuerNo.ToString(), 1);
                   ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************************APPLICATION ENDED!**************************", IssuerNo.ToString(), 1);
                   return;
               }

                foreach (DataRow item in dtBankConfig.Rows)
                {
                    try
                    {

                        /*CONVERT THE DATATABLE TO CLASS OBJECT*/
                        /*STEP 1*/
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "Conversion From DataTable To Class Objected Started", IssuerNo.ToString(), 1);
                        ObjConfig = BindDatatableToClass<ConfigDataObject>(item, dtBankConfig);
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", ObjConfig.ProcessId.ToString(), "", "Conversion From DataTable To Class Objected ENDED", IssuerNo.ToString(), 1);
                        if (!ObjConfig.isSplitBankFile)
                        {
                            return;
                        }
                        /*STEP 2*/
                        /*CONNECT SFTP AND GET FILE*/
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", ObjConfig.ProcessId.ToString(), "", "Starting The SFTP Connection to download the file", IssuerNo.ToString(), 1);
                        new ReadSFTPFile().Process(ObjConfig);
                        if (ObjConfig.StepStatus) { continue; }
                        //if (ObjConfig.StepStatus) { ModuleDAL.usp_MarkBankAsError(ObjConfig); break; }
                        /*STEP 3*/
                        /*DECRYPTED THE PGP FILE  IF PGP FLAG IS TRUE*/
                        if (ObjConfig.IsPGP)
                        {
                            DecryptPGPfile(ObjConfig);
                            if (ObjConfig.StepStatus) { continue; }
                        }

                        /*STEP 4*/
                        /*READ FILE FORM LOCAL SEVER >> BULk upload >> Split file*/
                        ObjConfig.FileExtension = ".txt";

                        if (!new FileProcessor().process(ObjConfig))
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", "", ex.ToString(), "", IssuerNo.ToString(), 0);
                    }
                   }

            }
            catch (Exception ex)
            {
                //ObjConfig.ErrorDesc = ex.ToString();
                ModuleDAL.FunInsertIntoErrorLog("Process", "", "", ex.ToString(), "", IssuerNo.ToString(), 0);
            }
            ModuleDAL.FunInsertIntoErrorLog("Process", ObjConfig.FileID,ObjConfig.ProcessId, "", "**************************APPLICATION END**************************", IssuerNo.ToString(), 1);
      
        }



        public void DecryptPGPfile(ConfigDataObject ObjConfig)
        {
            try
            {
                FunInsertTextLog("PGP DECRYPTION STARTED!", Convert.ToInt32(ObjConfig.IssuerNo), ObjConfig.ErrorLogPath);
                ModuleDAL.FunInsertIntoErrorLog("DecryptPGPfile", "", ObjConfig.ProcessId.ToString(), "", "PGP DECRYPTION STARTED!", ObjConfig.IssuerNo.ToString(), 1);
                String[] _files = Directory.GetFiles(ObjConfig.Local_Input + "\\" + ObjConfig.IssuerNo + "\\" + ObjConfig.ProcessId + "\\", "*" + ObjConfig.FileExtension, SearchOption.TopDirectoryOnly);
                if (_files != null)
                {
                    if (_files.Length > 0)
                    {
                        foreach (var files in _files)
                        {
                            ModuleDAL.FunInsertIntoErrorLog("DecryptPGPfile", "", ObjConfig.ProcessId.ToString(), "", "PGP FILE FOUND WITH  NAME |" + files.ToString(), ObjConfig.IssuerNo.ToString(), 1);
                            bool bResult = PGP_Decrypt(ObjConfig.PGP_KeyName, Path.Combine(ObjConfig.Local_Input + "\\" + ObjConfig.IssuerNo + "\\" + ObjConfig.ProcessId + "\\", files), Path.Combine(ObjConfig.Local_Input + "\\" + ObjConfig.IssuerNo + "\\" + ObjConfig.ProcessId + "\\", files).Split('.')[0] + ".txt", ObjConfig.InputFileSecretKeyPath, Convert.ToString(ObjConfig.IssuerNo), ObjConfig.InputFilePGPPassWord,ObjConfig.ProcessId);
                            if (bResult == true)
                            {
                                if (File.Exists(Path.Combine(ObjConfig.Local_Input + "\\" + ObjConfig.IssuerNo + "\\" + ObjConfig.ProcessId + "\\", files)));
                                {
                                    File.Delete(Path.Combine(ObjConfig.Local_Input + "\\" + ObjConfig.IssuerNo + "\\" + ObjConfig.ProcessId + "\\", files));
                                }
                            }

                        }
                    }
                    else
                    {
                        ModuleDAL.FunInsertIntoErrorLog("DecryptPGPfile", "", ObjConfig.ProcessId.ToString(), "", "NO PGP FILE FOUND ", ObjConfig.IssuerNo.ToString(), 1);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjConfig.StepStatus = true;
                ObjConfig.ErrorDesc = "ERROR IN PGP DECRYPTION FUNCTION |" + ex.ToString();
                ModuleDAL.FunInsertIntoErrorLog("DecryptPGPfile", "", ObjConfig.ProcessId.ToString(), ex.ToString(), "", ObjConfig.IssuerNo.ToString(), 0);
            }
            FunInsertTextLog("PGP DECRYPTION ENDED!", Convert.ToInt32(ObjConfig.IssuerNo), ObjConfig.ErrorLogPath);
            ModuleDAL.FunInsertIntoErrorLog("DecryptPGPfile", "", ObjConfig.ProcessId.ToString(), "", "PGP DECRYPTION ENDED!", ObjConfig.IssuerNo.ToString(), 1);
            ObjConfig.StepStatus =false;
        }
        public bool PGP_Decrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo, string PGPPassword,string ProcessID)
        {
            FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP CIF file decryption started");
            PGPModel objPGP = new PGPModel();
            objPGP.DecInputFilePath = fileFrom;
            objPGP.DecOutputFilePath = fileTo;
            objPGP.PrivateKeyFilePath = PgpKeyPath;
            objPGP.Password = PGPPassword;


            bool processExited = false;
            /// File info
            FileInfo fi = new FileInfo(fileFrom);
            if (!fi.Exists)
            {
                //FunInsertTextLog("File is not exist to decrypt", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "File is not exist to decrypt");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ProcessID, "Cannot find the file to decrypt", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                return processExited;
            }

            if (!File.Exists(PgpKeyPath))
            {
                //FunInsertTextLog("Cannot find PGP Key", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "Cannot find PGP Key");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ProcessID, "Cannot find PGP Key", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                return processExited;
            }

            // Cannot encrypt a file if it already exists
            if (File.Exists(fileTo))
            {

                //FunInsertTextLog("Cannot decrypt file.File already exists", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "Cannot decrypt file.File already exists");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ProcessID, "", "Cannot decrypt file.  File already exists PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                return processExited;

            }
            //encrypt through AGSPGP dll
            try
            {
                new PGP().Decrypt(objPGP);
                //check encrypted file created 
                if (File.Exists(fileTo))
                {
                    processExited = true;
                    //FunInsertTextLog("PGP_Decryption end", Convert.ToInt32(IssuerNo));
                    FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Decryption end");
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ProcessID, "", "File decrypted successfully. PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                    
                }
            }
            catch (Exception ex)
            {
                processExited = false;
                //FunInsertTextLog("PGP_Decrypt Error", Convert.ToInt32(IssuerNo));
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Decrypt Error");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ProcessID, ex.ToString(), "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 0);
                
            }
            return processExited;
        }

        public bool PGP_Encrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo)
        {
            PGPModel objPGP = new PGPModel();
            objPGP.EncInputFilePath = fileFrom;
            objPGP.EncOutputFilePath = fileTo;
            objPGP.PublicKeyFilePath = PgpKeyPath;

            bool processExited = false;
            /// File info
            FileInfo fi = new FileInfo(fileFrom);
            if (!fi.Exists)
            {
                //throw new Exception("Missing file.  Cannot find the file to encrypt.");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Missing file.  Cannot find the file to encrypt");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Cannot find the file to encrypt", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                
                return processExited;
            }

            if (!File.Exists(PgpKeyPath))
            {
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Cannot find PGP Key");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Cannot find PGP Key", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                return processExited;
            }

            /// Cannot encrypt a file if it already exists
            if (File.Exists(fileTo))
            {
                //throw new Exception("Cannot encrypt file.  File already exists");
                FunInsertIntoLogFile(ErrorLogFilePath, null, "PGP_Encrypt failed|Cannot encrypt file.File already exists");
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Cannot encrypt file.  File already exists", "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 1);
                return processExited;

            }
            //encrypt through AGSPGP dll
            try
            {
                new PGP().Encrypt(objPGP);
            }
            catch (Exception Ex)
            {
                //FunInsertTextLog("PGP_Encrypt Error", Convert.ToInt32(IssuerNo));
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", Ex.ToString(), "PgpPath=" + PgpKeyPath + ",InputFile=" + fileFrom + "Outputfile=" + fileTo, IssuerNo.ToString(), 0);
                
            }

            //check encrypted file created 
            if (File.Exists(fileTo))
            {
                processExited = true;
            }
            return processExited;
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

        /*Convert DataRow to Obj*/
        public static T BindDatatableToClass<T>(DataRow dtrow, DataTable dtTable)
        {
            var ob = Activator.CreateInstance<T>();
            try
            {
                DataRow dr = dtrow;

                // Get all columns' name
                List<string> columns = new List<string>();
                foreach (DataColumn dc in dtTable.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                // Create object


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
                    string Name = propertyInfo.Name;
                    if (columns.Select(x=>x.Equals(Name,StringComparison.OrdinalIgnoreCase)).Count()>0)
                    {
                        // Fill the data into the property
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        try
                        {
                            object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                            propertyInfo.SetValue(ob, safeValue, null);
                        }
                        catch (Exception ex)
                        {

                        }
                        
                        

                        // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return ob;
        }

        //**************** Save Error Logs ************




        public void FunInsertTextLog(string Message, int issuerNo, string LogPath)
        {
            
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(issuerNo))) issuerNo = 0;
                if (!string.IsNullOrEmpty(LogPath))
                {
                    string filename = issuerNo.ToString() + "Debug_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                    string filepath = LogPath + filename;
                    if (!Directory.Exists(LogPath))
                    {
                        //Try to create the directory.
                        DirectoryInfo di = Directory.CreateDirectory(LogPath);
                    }
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
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", Ex.ToString(), "", issuerNo.ToString(), 0);
            }
        }
        public void FunInsertIntoLogFile(string LogPath, Exception ex, string functionName)
        {
            try
            {
                //string LogPath = ConfigurationManager.AppSettings["LogPath"].ToString();
                string filename = "Log_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                string filepath = LogPath + filename;
                if (File.Exists(filepath))
                {
                    using (StreamWriter writer = new StreamWriter(filepath, true))
                    {
                        if (ex != null)
                        {
                            writer.WriteLine("——————-START————-" + DateTime.Now);
                            if (!string.IsNullOrEmpty(functionName))
                                writer.WriteLine("FunctionName: " + functionName);

                            writer.WriteLine("Error Message: " + ex.Message);
                            writer.WriteLine("Source: " + ex.Source);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);

                            writer.WriteLine("——————-END————-" + DateTime.Now);
                        }
                        else
                        {
                            writer.WriteLine(functionName + " " + DateTime.Now);
                        }
                        writer.Close();
                    }
                }
                else
                {
                    using (StreamWriter writer = File.CreateText(filepath))
                    {
                        if (ex != null)
                        {
                            writer.WriteLine("——————-START————-" + DateTime.Now);
                            writer.WriteLine("FunctionName : " + functionName);
                            writer.WriteLine("Error Message : " + ex.Message);
                            writer.WriteLine("Source : " + ex.Source);
                            writer.WriteLine("StackTrace : " + ex.StackTrace);
                            writer.WriteLine("——————-END————-" + DateTime.Now);
                        }
                        else
                        {
                            writer.WriteLine(functionName + " " + DateTime.Now);
                        }
                        writer.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", Ex.ToString(), "","", 0);
            }
        }

       

        public void Dispose()
        {
            throw new NotImplementedException();
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
