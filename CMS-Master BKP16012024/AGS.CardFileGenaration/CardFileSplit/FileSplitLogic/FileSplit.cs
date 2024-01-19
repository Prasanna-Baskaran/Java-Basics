using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileSplit
{
    class FileSplit
    {
     
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
                new ModuleDAL().FunInsertIntoErrorLog("CreateDownloadCSVFileWithFileName", "", "", xObj.ToString(), "**************************FILE SPLIT**************************", " ", 0);
                //  sFileName = "";
               return null;
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
                new ModuleDAL().FunInsertIntoErrorLog("convertDataTable2CSV", "", "", xObj.ToString(), "**************************FILE SPLIT**************************", " ", 0);
                throw xObj;
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
                new ModuleDAL().FunInsertIntoErrorLog("Chr", "","",xObj.ToString(),"**************************FILE SPLIT**************************"," ",0);
                
                strChar = "";
            }
            finally
            {

            }

            return strChar;
        }
    }
}
