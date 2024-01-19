using ExportToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
  public static  class RequestProcesser
    {
        public static bool process(CardRePINDataObject obj,EmailDataObject Eobj)
        {

            try
            {
                string _fileName = string.Empty;
                string _RejectFileName = string.Empty;
                String[] _files = Directory.GetFiles(obj.FilePath, "*.txt", SearchOption.TopDirectoryOnly);
                if (_files != null)
                {
                    if (_files.Length > 0)
                    {
                        foreach (var files in _files)
                        {
                            string StrErrorMessages = "";
                            _fileName = Path.GetFileName(files);
                            obj.FileName = _fileName;
                            
                            FileInfo fi = new FileInfo(files);
                            if (!IsLocked(fi))
                            {
                                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :"+ _fileName+ "File is being read" , System.Reflection.MethodBase.GetCurrentMethod().Name);
                                DataTable _dataTable = ReadFile(files, obj.FileHeader, Eobj,ref StrErrorMessages);
                                if (_dataTable.Rows.Count > 0)
                                {
                                    DataTable RejectcedRecords = new ModuleDAL().InsertRePINData(_dataTable, _fileName, obj.Bankid);
                                    File.Delete(obj.FilePath + "\\" + _fileName);
                                    if (RejectcedRecords.Rows.Count > 0)
                                    {
                                        _RejectFileName = Path.GetFileNameWithoutExtension(_fileName) + "_Rejected"+DateTime.Now.ToString("yyyyMMdd") + ".xls";
                                        CreateExcelFile.CreateExcelDocument(RejectcedRecords, obj.FilePath + _RejectFileName);
                                        if (CardRePIN.FileMove(obj, _RejectFileName, false,obj.OutPutPath,Eobj))
                                        {
                                            File.Delete(obj.FilePath + "\\" + _RejectFileName);
                                        }
                                        
                                    }
                                    new GenerateISO().Process(obj,Eobj);
                                    GenerateReports.GenerateReport(obj,Eobj);
                                }
                            }
                            else
                            {
                                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :" + _fileName + "File Format error", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                
                                if (CardRePIN.FileMove(obj, _fileName, false,obj.OutPutPath,Eobj))
                                {
                                    File.Delete(obj.FilePath + "\\" + _fileName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :" + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage(obj.FileName,Eobj);
                return false;
            }
            return true;
        }
        #region This Method is used to check if the file is process by other function before the file reading start .Create by Gufran Khan.
        public static bool IsLocked(this FileInfo f)
        {
            try
            {
                string fpath = f.FullName;
                FileStream fs = File.OpenWrite(fpath);
                fs.Close();
                return false;
            }

            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :" + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return true;
            }
        }
        #endregion
        #region This Method is used to read the file .Create by Gufran Khan.
        public static IEnumerable<string> ReadAsLines(string filename)
        {

            using (var reader = new StreamReader(filename))

                while (!reader.EndOfStream)
                    yield return reader.ReadLine();


        }
        #endregion
        public static DataTable ReadFile(string file, string FileHeader, EmailDataObject Eobj, ref string StrErrorMessages)
        {
            int count = 0;
            string ErrorRecords = string.Empty;
            var _dataTable = new DataTable();
            var headers = FileHeader.Split('|');
            foreach (var header in headers)
                _dataTable.Columns.Add(header);
            try
            {
                var reader = ReadAsLines(file);
                //this assume the first record is filled with the column names   
                var records = reader;
                foreach (var record in records)
                {
                    if (record != "\t" && record != "")
                        if (record.Split('|').Length != _dataTable.Columns.Count)
                        {
                            ErrorRecords = record;
                            for (int i = 0; i < (_dataTable.Columns.Count) - (record.Split('|').Length); i++)
                            {
                                ErrorRecords = ErrorRecords + "|Error In Record";
                            }
                            _dataTable.Rows.Add(ErrorRecords.Split('|'));
                        }
                        else
                        {
                            _dataTable.Rows.Add(record.Split('|'));
                        }


                    



                }
                count = _dataTable.Columns.Count;
                //if (count < filedCount)
                //{
                //    int loopcount = filedCount - count;
                //    for (int i = 0; i < loopcount; i++)
                //    {
                //        _dataTable.Columns.Add("Default_" + i.ToString());
                //    }

                //}

            }
            catch (Exception ex)
            {

                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Eobj.EmailMsg = ex.ToString();
                StrErrorMessages = "File format is not proper in all records" + Environment.NewLine;
                EmailAlert.FunSendMailMessage("", Eobj);
                return _dataTable;
            }
            return _dataTable;
        }
     
    }
}
