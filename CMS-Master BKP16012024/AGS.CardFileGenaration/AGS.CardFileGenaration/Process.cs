using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Configuration;
using System.IO;
using System.Net;

namespace AGS.InstaCardsFileGenaration
{
    public class Process
    {
        public void GenerateFile()
        {
            try
            {
                string[] strRarFileName = DownloadFiles();

                //if (strRarFileName != null)
                //{
                //    var sourcePath = string.Empty;
                   
                //    //Here it will accept SFTP path and transfered file to that path
                //    foreach (string StrfileName in strRarFileName)
                //    {
                //        if (!string.IsNullOrEmpty(StrfileName))
                //        {

                //            string[] ResultArr = StrfileName.Split('_');

                //            if (ResultArr.Contains("PrabhuBank"))
                //            {
                //                sourcePath = ConfigurationManager.AppSettings["SFTPSourceIP"];
                //            }

                //            using (var sftp = new SftpClient(sourcePath, 220, ConfigurationManager.AppSettings["SFTPUser"], ConfigurationManager.AppSettings["SFTPPassword"]))   //".\\dipak.gole","Di$@12345"
                //            {
                //                sftp.Connect();
                //                string str = sftp.WorkingDirectory;
                //                sftp.Get(ConfigurationManager.AppSettings["zipSFTPAGSCardPath"]);
                //                //Stream fileStream = File.OpenWrite(getCurrentDirectory() + StrfileName);
                //                var fileStream = new FileStream(getCurrentDirectory() + StrfileName, FileMode.Open);
                //                sftp.UploadFile(fileStream, ConfigurationManager.AppSettings["zipSFTPAGSCardPath"]+"//"+StrfileName, true);
                //                sftp.Disconnect();
                //            }
                //        }
                //    }
            
                    
                //}
                //else
                //{
                //    //display message
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void Upload(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://10.167.2.85/switchteam/Dipak/AGS_REPORTS/" + fileInf.Name;
            FtpWebRequest reqFTP;
            // Create FtpWebRequest object from the Uri provided
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://10.167.23.8/switchteam/Dipak/AGS_REPORTS/" + fileInf.Name));
            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SFTPUser"], ConfigurationManager.AppSettings["SFTPPassword"]);
            // By default KeepAlive is true, where the control connection is not closed
            // after a command is executed.
            reqFTP.KeepAlive = false;
            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Specify the data transfer type.
            reqFTP.UseBinary = true;
            // Notify the server about the size of the uploaded file
            reqFTP.ContentLength = fileInf.Length;
            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            FileStream fs = fileInf.OpenRead();
            try
            {
                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();
                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);
                // Until Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Upload Error");
            }
        }

        internal string[] DownloadFiles()
        {
            DataSet ObjDsOutPut = new DataSet();
            SqlConnection ObjConn = null;
            Random rnd = new Random();
            bool blnfileexists = false;
            try
            {
                FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.SP_getCardProdAccountsDetails", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Redownload", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                    ObjCmd.AddParameterWithValue("@BatchNo", SqlDbType.VarChar, 0, ParameterDirection.Input, string.Empty);
                    ObjDsOutPut = ObjCmd.ExecuteDataSet();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

               

                if (Convert.ToInt32(ObjDsOutPut.Tables[0].Rows[0]["Code"]) == 0)
                {
                    var BankList = ((from r in ObjDsOutPut.Tables[1].AsEnumerable()
                                     select r["Bank"].ToString()).Distinct().ToList());

                    string[] sCompressedFile = new string[BankList.Count()];


                    string[] filenames = Convert.ToString(ObjDsOutPut.Tables[0].Rows[0]["FileNames"]).Split(',');
                    if (filenames.Length + 1 == ObjDsOutPut.Tables.Count)
                    {
                        int CompressFilecount = 0;
                        List<string> physicalfiles = new List<string>();
                        string filename;
                        string BankName = string.Empty;
                        ObjDsOutPut.Tables.RemoveAt(0);
                        foreach (string bank in BankList.ToList())
                        {
                            String sCreationSuffix = DateTime.Now.ToString("ddMMyy hhmmss").Replace(" ", "_") + "_" + rnd.Next();
                            int i = 0;
                            foreach (DataTable dtOutputAll in ObjDsOutPut.Tables)
                            {
                                if (dtOutputAll.Rows.Count > 0)
                                {
                                    //filter data bank wise
                                    DataTable dtOutput = dtOutputAll.AsEnumerable()
                                                        .Where(row => row.Field<string>("Bank") == bank).CopyToDataTable();
                                    //foreach (DataTable dtOutput in dtOutputAll.Select("Bank=" + bank).CopyToDataTable().Rows)
                                    //foreach (DataTable dtOutput in DtFilterd)
                                    //{
                                    BankName = dtOutput.Rows[0]["BankName"].ToString();
                                    filename = filenames[i].Trim(); i++;
                                    if (!string.IsNullOrEmpty(filename))
                                    {
                                        if (dtOutput.Rows.Count > 0)
                                        {
                                            string strLocFile = CreateDownloadCSVFile(dtOutput.ToSelectedTableColumns("Result"), getCurrentDirectory(), false, "", "", "");
                                            System.IO.File.Move(getCurrentDirectory() + "\\" + strLocFile, getCurrentDirectory() + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                            physicalfiles.Add(getCurrentDirectory() + "\\" + filename + "_" + sCreationSuffix + ".txt");
                                            blnfileexists = true;

                                        }
                                        else
                                        {
                                            //ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, downloadfiles()", filename + " table have no records", JsonConvert.SerializeObject(objCardProduction));
                                        }
                                    }
                                    else
                                    {
                                        //ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, downloadfiles()", filename + " found empty", JsonConvert.SerializeObject(objCardProduction));
                                    }
                                    //}
                                }

                            }

                            /// using  WinRAR 
                            //sCompressedFile[CompressFilecount] = BankName + "_cardProd_" + sCreationSuffix + ".rar";


                            //System.Diagnostics.ProcessStartInfo zipProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                            //zipProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            //zipProcessStartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["rarWorkingDirectory"]; //"C:\\Program Files\\Winrar\\";
                            //zipProcessStartInfo.FileName = "rar.exe";
                           
                            //zipProcessStartInfo.Arguments = "a -df -ep -m5 -prbl_" + DateTime.Now.ToString("ddMMyy") + " " + Chr(34) + getCurrentDirectory() + sCompressedFile[CompressFilecount] + Chr(34) + " " + Chr(34) + getCurrentDirectory() + "*_" + sCreationSuffix + ".txt" + Chr(34);

                            //using (System.Diagnostics.Process zipProcess = System.Diagnostics.Process.Start(zipProcessStartInfo))
                            //{
                            //    zipProcess.WaitForExit();
                            //}

                            //zipProcessStartInfo = null;

                            //// start  Using & 7z.exe

                            sCompressedFile[CompressFilecount] = BankName + "_cardProd_" + sCreationSuffix + ".7z";


                            System.Diagnostics.ProcessStartInfo zipProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                            zipProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            zipProcessStartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["7ZipWorkingDirectory"]; //"C:\\Program Files\\Winrar\\";
                                                                                                                                                 //zipProcessStartInfo.FileName = "rar.exe";
                            zipProcessStartInfo.FileName = "7z.exe";
                            zipProcessStartInfo.Arguments = "a -t7z " + Chr(34) + getCurrentDirectory() + sCompressedFile[CompressFilecount] + Chr(34) + " " + Chr(34) + getCurrentDirectory() + "*_" + sCreationSuffix + ".txt" + Chr(34)+ " -prbl_" + DateTime.Now.ToString("ddMMyy");

                            using (System.Diagnostics.Process zipProcess = System.Diagnostics.Process.Start(zipProcessStartInfo))
                            {
                                zipProcess.WaitForExit();
                            }

                            zipProcessStartInfo = null;
                            
                            //Delete archiving files after successful zip
                            if(File.Exists(getCurrentDirectory() + sCompressedFile[CompressFilecount]))
                            {
                                string[] files = System.IO.Directory.GetFiles(getCurrentDirectory(), @"*_" + sCreationSuffix + ".txt");

                                foreach(string fileName in files)
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                            //// end  Using & 7z.exe

                            CompressFilecount++;


                        }

                        return sCompressedFile;

                    }
                    else
                    {
                        //ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, downloadfiles()", "Count of file names written in stored procedures and count of table returns does not match(excluding 1st status table)", JsonConvert.SerializeObject(objCardProduction));
                        return sCompressedFile;
                    }
                }
                else
                {
                    return null;
                }

                //if (!blnfileexists)
                //    return null;

                //string sCompressedFile = "cardProd_" + sCreationSuffix + ".rar";

                //System.Diagnostics.ProcessStartInfo zipProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                //zipProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //zipProcessStartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["rarWorkingDirectory"]; //"C:\\Program Files\\Winrar\\";
                //zipProcessStartInfo.FileName = "rar.exe";
                //zipProcessStartInfo.Arguments = "a -df -ep -m5 -prbl_" + DateTime.Now.ToString("ddMMyy") + " " + Chr(34) + getCurrentDirectory() + sCompressedFile + Chr(34) + " " + Chr(34) + getCurrentDirectory() + "*_" + sCreationSuffix + ".txt" + Chr(34);

                //using (System.Diagnostics.Process zipProcess = System.Diagnostics.Process.Start(zipProcessStartInfo))
                //{
                //    zipProcess.WaitForExit();
                //}

                //zipProcessStartInfo = null;
                //return string.Empty;
            }
            catch (Exception ex)
            {
                //ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardProductionDAL, downloadfiles()", ex.Message, JsonConvert.SerializeObject(objCardProduction));
                return null;
            }
        }

        internal static void FunGetConnection(ref SqlConnection Connection, int Source)
        {
            switch (Source)
            {
                case 1:
                    Connection = ConfigManager.GetRBSQLDBOLAPConnection;
                    break;
            }
        }

        public static string CreateDownloadCSVFile(DataTable Data, string FilePath, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
        {
            string sFileName = "";

            try
            {
                sFileName = System.IO.Path.GetRandomFileName();

                using (System.IO.StreamWriter swObj = new System.IO.StreamWriter(FilePath + "/" + sFileName))
                {
                    // Data.Columns.Remove("");
                    swObj.Write(ConvertDataTableToCSV(Data, WithHeader, Delimeter, CustomHeader, CustomFooter));

                    swObj.Close();
                }
            }
            catch (Exception ex)
            {
                sFileName = "";
            }
            finally
            {
            }

            return sFileName;
        }

        public static string ConvertDataTableToCSV(DataTable dtSource, Boolean WithHeader, String Delimeter, String CustomHeader, String CustomFooter)
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
            catch
            {
                sbOutput = new StringBuilder();
            }

            return sbOutput.ToString();
        }

        public static string Chr(Int32 CharCode)
        {
            string strChar = "";

            try
            {
                strChar = Convert.ToString((char)CharCode);
            }
            catch
            {
                strChar = "";
            }
            finally
            {

            }

            return strChar;
        }

        private static string getCurrentDirectory()
        {
            string Path = string.Empty;
            Path = ConfigurationManager.AppSettings["CARDGENPATH"];

            if (!(System.IO.Directory.Exists(Path)))
            {
                try
                {
                    Directory.CreateDirectory(Path);
                }
                catch (Exception ex)
                { }
            }


            return Path;
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
