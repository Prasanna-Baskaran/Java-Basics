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
    class GenerateReports
    {
        public static void GenerateReport(CardRePINDataObject obj,EmailDataObject Eobj)
        {
            try
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : Report geneartion Started", System.Reflection.MethodBase.GetCurrentMethod().Name);               
                
                DataSet _Report = new ModuleDAL().getReport(obj.FileName);
                foreach ( DataTable Table in _Report.Tables)
                {
                    if (Table.Rows.Count > 0)
                    {
                        DataTable __ReportdataTable = Table.Copy();
                        string _ProcessedFileName = Path.GetFileNameWithoutExtension(obj.FileName) + __ReportdataTable.Rows[0]["ReportType"] + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                        __ReportdataTable.Columns.Remove("ReportType");
                        CreateExcelFile.CreateExcelDocument(__ReportdataTable, obj.FilePath + _ProcessedFileName);
                        if (obj.ISPGP)
                        {
                            obj.EncInputFilePath = obj.EncInputFilePath + _ProcessedFileName;
                            obj.EncOutputFilePath = obj.EncOutputFilePath + Path.GetFileNameWithoutExtension(_ProcessedFileName);

                            CardRePIN.Encrypt(obj);
                        }

                        if (CardRePIN.FileMove(obj, (obj.ISPGP ? Path.GetFileNameWithoutExtension(_ProcessedFileName) : _ProcessedFileName), false, obj.OutPutPath, Eobj))
                        {
                            File.Delete(obj.FilePath + "\\" + _ProcessedFileName);
                            if (obj.ISPGP)
                                File.Delete(obj.EncOutputFilePath);
                        }
                    }
                    
                }                 
                
            }
            catch(Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :  "+ ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);               
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage(obj.FileName, Eobj);
            }

        }
    }
    
}
