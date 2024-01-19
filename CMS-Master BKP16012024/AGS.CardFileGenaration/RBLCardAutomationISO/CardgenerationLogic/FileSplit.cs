using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.RBLCardAutomationISO
{
    class FileSplit
    {
        public void Porcess(ConfigDataObject ObjDataConfig)
        {
            ModuleDAL ModuleDAL = new RBLCardAutomationISO.ModuleDAL();
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ObjDataConfig.ProcessId.ToString(), "", "**************************FILE SPLIT STARTED**************************", ObjDataConfig.IssuerNo.ToString(), 1);
            string StrErrorMessages = "";
            String[] _files = Directory.GetFiles(ObjDataConfig.Local_Input, "*" + ObjDataConfig.FileExtension, SearchOption.TopDirectoryOnly);
            if (_files != null)
            {
                if (_files.Length > 0)
                {
                    FileProcessor ObjFileProcessor= new FileProcessor ();
                    foreach (var files in _files)
                    {
                        FileInfo fi = new FileInfo(files);
                        if (!ObjFileProcessor.IsLocked(fi))
                        {
                            DataTable DtRecord = ObjFileProcessor.ReadFile(files, ObjDataConfig.FileHeader, ref StrErrorMessages, ObjDataConfig);
                            foreach (DataRow dataRow in DtRecord.Rows)
                            {

                                dataRow["Pin_Mailer"] = dataRow["Pin_Mailer"].ToString().ToUpper();

                            }
                            List<DataTable> result = DtRecord.AsEnumerable()
                            .GroupBy(row => row.Field<string>("Pin_Mailer"))
                            .Select(g => g.CopyToDataTable())
                            .ToList();
                            if(result.Count>1)
                            {
                                foreach (DataTable Dt in result)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    CreateDownloadCSVFileWithFileName(Dt, ObjDataConfig.Local_Input + Path.GetFileNameWithoutExtension(files) + "_"+ (Convert.ToString(Dt.Rows[0]["Pin_Mailer"]).Equals("Y",StringComparison.OrdinalIgnoreCase) ?"Physical":"Email") + DateTime.Now.ToString("yyyyMMddHHmmss")+ ".txt", false, "|", " ", " ");
                                }

                                File.Move(files, ObjDataConfig.Local_Archive + Path.GetFileNameWithoutExtension(files) + "_Proceesed" + DateTime.Now.ToString("yyyyMMddHHmmss") + ObjDataConfig.FileExtension);
                            }
                            
                            
                        }
                    }
                }
            }

            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", ObjDataConfig.ProcessId.ToString(), "", "**************************FILE SPLIT ENDED**************************", ObjDataConfig.IssuerNo.ToString(), 1);

        }
        public string CreateDownloadCSVFileWithFileName(DataTable Data, string FileCompletePath, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            //string sFileName = "";
            
            try
            {
                //  sFileName = System.IO.Path.GetRandomFileName();

                using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(FileCompletePath))
                {
                    swObj.Write(convertDataTable2CSV(Data, WithHeader, Delimeter, CustomHeader, CustomFooter));


                    swObj.Close();
                }
            }
            catch (Exception xObj)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", xObj.ToString(), "**************************FILE SPLIT**************************", " ", 0);
                //  sFileName = "";
                FileCompletePath = "ERROR";
            }
            finally
            {
            }

            return FileCompletePath;
        }
        public string convertDataTable2CSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            string sOutput = "";
            string sRow = "";

            try
            {
                if (WithHeader)
                {
                    foreach (DataColumn dcTable in dtSource.Columns)
                    {
                        sOutput += ((sOutput == "") ? "" : Delimeter) + Convert.ToString(dcTable.ColumnName);
                    }
                }
                else
                {
                    if (CustomHeader != "")
                    {
                        sOutput += CustomHeader;
                    }
                }

                foreach (DataRow drTable in dtSource.Rows)
                {
                    if (sOutput != "")
                    {
                        sOutput += Chr(13) + Chr(10);
                    }
                    sRow = "";

                    sRow = String.Join(Delimeter, drTable.ItemArray);
                    /*
                    foreach (DataColumn dcTable in dtSource.Columns)
                    {
                        sRow += ((sRow == "") ? "" : Delimeter) + Convert.ToString(drTable[dcTable.ColumnName]);
                    }
                    */
                    sOutput += sRow;
                }

                if (CustomFooter != "")
                {
                    sOutput += Chr(13) + Chr(10) + CustomFooter;
                }
            }
            catch (Exception xObj)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", xObj.ToString(), "**************************FILE SPLIT**************************", " ", 0);
                sOutput = "";
            }

            return sOutput;
        }
        public string Chr(Int32 CharCode)
        {
            string strChar = "";

            try
            {
                strChar = Convert.ToString((char)CharCode);
            }
            catch (Exception xObj)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "","",xObj.ToString(),"**************************FILE SPLIT**************************"," ",0);
                
                strChar = "";
            }
            finally
            {

            }

            return strChar;
        }
    }
}
