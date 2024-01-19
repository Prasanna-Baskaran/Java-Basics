using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.IO;

namespace VDCToPDCSTD4
{
    public class RunSTD4
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public void Process(int IssuerNo)
        {
            ConfigDataObject ObjConfig = new ConfigDataObject();
            DataTable dtBankConfig = ModuleDAL.getBankConfig(IssuerNo);
            foreach (DataRow item in dtBankConfig.Rows)
            {

                /*CONVERT THE DATATABLE TO CLASS OBJECT*/
                /*STEP 1*/
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "Conversion From DataTable To Class Objected Started", IssuerNo.ToString(), 1);
                ObjConfig = BindDatatableToClass<ConfigDataObject>(item, dtBankConfig);
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ObjConfig.ProcessId.ToString(), "", "Conversion From DataTable To Class Objected ENDED", IssuerNo.ToString(), 1);
            }
            string Processid = ConfigurationManager.AppSettings["ProcessID"];
            DataTable dt = ModuleDAL.checkedSwitchProcessRecord(IssuerNo, Processid);
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    ObjConfig.FileID = dt.Rows[i]["ID"].ToString();
                    ModuleDAL.RunSTD4(ObjConfig);
                    ModuleDAL.UpdateFileStatus(ObjConfig, 8);
                }
                /*RUN PRE  DLL*/
                new RunSTD4().FunInsertTextLog("PRE Processing Started FOR !" + ObjConfig.FileID, Convert.ToInt32(ObjConfig.IssuerNo), ObjConfig.ErrorLogPath);
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfig.FileID, ObjConfig.ProcessId.ToString(), "", "PRE PROCESSING STARTED FOR:" + ObjConfig.FileName, ObjConfig.IssuerNo.ToString(), 1);
                AGS.PREFileGeneration.PREFile ObjPRE = new AGS.PREFileGeneration.PREFile();
                ObjPRE.Process(Convert.ToInt32(ObjConfig.IssuerNo), ObjConfig.FileID, ObjConfig.FileName, ObjConfig.ProcessId);
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjConfig.FileID, ObjConfig.ProcessId.ToString(), "", "PRE PROCESSING ENDED FOR:" + ObjConfig.FileName, ObjConfig.IssuerNo.ToString(), 1);
                /*PRE END*/

            }

        }
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
                    if (columns.Where(r => r.Equals(Name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                    {
                        // Fill the data into the property
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                        propertyInfo.SetValue(ob, safeValue, null);

                        // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return ob;
        }

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
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", Ex.ToString(), "", "", 0);
            }
        }
    }
}
