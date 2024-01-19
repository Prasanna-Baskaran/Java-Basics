using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.PREFileGenerationRBLWithOutFileID
{
    class CreateExcelFile
    {
        public string CreateExcel(DataTable Data, string FilePath, string fileName)
        {
            string sFileName = "";
            Int32 iRow = 1;
            Int32 iCol = 1;
            string sPrefix = "";

            try
            {
                //sFileName = System.IO.Path.GetRandomFileName();
                sFileName = fileName;

                using (ExcelPackage objExcelPackage = new ExcelPackage())
                {
                    ExcelWorksheet objExcelWorksheet = objExcelPackage.Workbook.Worksheets.Add("Data");

                    objExcelWorksheet.Cells["A1"].LoadFromDataTable(Data, true);

                    //objExcelWorksheet.Cells[objExcelWorksheet.Dimension.Address].AutoFitColumns();
                    //objExcelWorksheet.Cells[objExcelWorksheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    //objExcelWorksheet.Cells[objExcelWorksheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //objExcelWorksheet.Cells[objExcelWorksheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //objExcelWorksheet.Cells[objExcelWorksheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //objExcelWorksheet.Protection.SetPassword("ags@1234");

                    using (ExcelRange objExcelRange = objExcelWorksheet.Cells["1:1"])
                    {
                        objExcelRange.Style.Font.Bold = true;
                    }

                    foreach (DataColumn dataColumn in Data.Columns)
                    {
                        iCol = dataColumn.Ordinal + 65;
                        if (iCol > 90)
                        {
                            sPrefix = "A";
                            iCol = iCol - dataColumn.Ordinal;
                        }

                        if (dataColumn.DataType == System.Type.GetType("System.Decimal"))
                        {
                            using (ExcelRange objExcelRange = objExcelWorksheet.Cells[sPrefix + Chr(iCol) + ":" + sPrefix + Chr(iCol)])
                            {
                                objExcelRange.Style.Numberformat.Format = "###0";
                            }
                        }

                        if (dataColumn.DataType == System.Type.GetType("System.DateTime"))
                        {
                            using (ExcelRange objExcelRange = objExcelWorksheet.Cells[sPrefix + Chr(iCol) + ":" + sPrefix + Chr(iCol)])
                            {
                                objExcelRange.Style.Numberformat.Format = "dd-mmm-yyyy";
                            }
                        }
                    }

                    //FileInfo objFile = new FileInfo(FilePath + "\\" + sFileName);

                    objExcelPackage.SaveAs(new FileInfo(FilePath + "\\" + sFileName), "");
                }
            }
            catch (Exception ex)
            {

               
                sFileName = "";
            }

            return sFileName;
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
                
                strChar = "";
            }
            finally
            {

            }

            return strChar;
        }
    }
}
