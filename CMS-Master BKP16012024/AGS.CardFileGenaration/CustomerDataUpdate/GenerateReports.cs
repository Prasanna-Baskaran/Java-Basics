using ExportToExcel;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace CustomerDataUpdate
{
	internal class GenerateReports
	{
		public static void Report(customerDataObject obj, EmailDataObject Eobj)
		{
			try
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : Report geneartion Started", System.Reflection.MethodBase.GetCurrentMethod().Name);
				DataSet report = new ModuleDAL().getReport(obj.Filename, obj.Bankid);
				foreach (DataTable dataTable in report.Tables)
				{
					if (dataTable.Rows.Count > 0)
					{
						string text = string.Empty;
						DataTable dataTable2 = dataTable.Copy();
						text = string.Concat(new object[]
						{
							System.IO.Path.GetFileNameWithoutExtension(obj.Filename),
							dataTable2.Rows[0]["ReportType"],
							System.DateTime.Now.ToString("yyyyMMdd"),
							".xls"
						});
						dataTable2.Columns.Remove("ReportType");
						CreateExcelFile.CreateExcelDocument(dataTable2, obj.FilePath + text);
						if (obj.ISPGP && obj.IsOutPutEncrypted)
						{
							GenerateReports.UploadPGPReport(obj, text);
							System.IO.File.Delete(obj.FilePath + "\\" + text);
						}
						else
						{
							SFTPConnection.UploadFile(obj.ServerIP, obj.UserName, obj.Password, System.Convert.ToInt32(obj.port), obj.FilePath, obj.OutPutPath, text, obj.KeyPath, obj.Passphrase, ".xls");
							if (CustomerDataUpdate.FileMove(obj, text, false, obj.OutPutPath, Eobj))
							{
								System.IO.File.Delete(obj.FilePath + "\\" + text);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				Eobj.EmailMsg = ex.ToString();
				EmailAlert.FunSendMailMessage(obj.Filename, Eobj);
			}
		}

		public static void UploadPGPReport(customerDataObject obj, string _ProcessedFileName)
		{
			string text = obj.EncInputFilePath.ToString();
			string text2 = obj.EncOutputFilePath.ToString();
			try
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP ENCRYPTION STARTED", System.Reflection.MethodBase.GetCurrentMethod().Name);
				obj.EncInputFilePath = text + _ProcessedFileName;
				obj.EncOutputFilePath = text2 + System.IO.Path.GetFileNameWithoutExtension(_ProcessedFileName) + ".pgp";
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : FILE To BE Encrypted PATH " + obj.EncInputFilePath.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP ENCRYPTION FILE PATH " + obj.EncOutputFilePath.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				CustomerDataUpdate.Encrypt(obj);
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP ENCRYPTION END", System.Reflection.MethodBase.GetCurrentMethod().Name);
				SFTPConnection.UploadFile(obj.ServerIP, obj.UserName, obj.Password, System.Convert.ToInt32(obj.port), text, obj.OutPutPath, obj.ISPGP ? (System.IO.Path.GetFileNameWithoutExtension(_ProcessedFileName) + ".pgp") : _ProcessedFileName, obj.KeyPath, obj.Passphrase, obj.ISPGP ? ".pgp" : ".txt");
				System.IO.File.Delete(obj.EncOutputFilePath);
				obj.EncInputFilePath = " ";
				obj.EncOutputFilePath = " ";
				obj.EncInputFilePath = text;
				obj.EncOutputFilePath = text2;
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
		}
	}
}
