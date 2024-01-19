using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace CustomerDataUpdate
{
	public static class RequestProcesser
	{
		public static bool process(customerDataObject obj, EmailDataObject Eobj)
		{
			bool result;
			try
			{
				string text = string.Empty;
				string empty = string.Empty;
				string[] files = System.IO.Directory.GetFiles(obj.FilePath, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
				if (files != null)
				{
					if (files.Length > 0)
					{
						string[] array = files;
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array[i];
							string text3 = "";
							text = System.IO.Path.GetFileName(text2);
							obj.Filename = text;
							string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(text);
							ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : File Process Stared for |" + fileNameWithoutExtension, System.Reflection.MethodBase.GetCurrentMethod().Name);
							System.IO.FileInfo f = new System.IO.FileInfo(text2);
							if (!f.IsLocked())
							{


                                ///Added by uddesh on 29-04-2019 ATPCM-656 start
                                 ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + text + " Check ForDuplicate File start", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                string msgcheck = "";
                               DataTable dtcheck =new ModuleDAL().CheckForDuplicateFile(text,obj.Bankid);

                                if (dtcheck.Rows.Count > 0)
                                {
                                    msgcheck = dtcheck.Rows[0][0].ToString().ToUpper();
                                    if (msgcheck == "DUPLICATE FILE")
                                    {
                                        System.IO.File.Delete(obj.FilePath + "\\" + text);
                                        RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref msgcheck);
                                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  DUPLICATE FILE", "files loop");
                                        return false;
                                    }
                                }
                                else
                                {
                                    msgcheck = "No data while checking duplicate file";
                                    System.IO.File.Delete(obj.FilePath + "\\" + text);
                                    RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref msgcheck);
                                    ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  No data while checking duplicate file", "files loop");
                                    return false;

                                }
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + text + " Check ForDuplicate File END", System.Reflection.MethodBase.GetCurrentMethod().Name);

                                ///Added by uddesh on 29-04-2019 ATPCM-656 end
                                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + text + " File is being read", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                DataTable dataTable = RequestProcesser.ReadFile(text2, obj.FileHeader, Eobj, obj.FiledCount, ref text3);
								if (dataTable.Rows.Count > 0)
								{
									bool flag = new ModuleDAL().InsertCustomerData(dataTable, text, obj.Bankid,obj.FileProcessorSP, ref text3); ///Added by uddesh on 29-04-2019 ATPCM-656
									System.IO.File.Delete(obj.FilePath + "\\" + text);
									if (flag)
									{
										if (!new GenerateISO().Process(obj, Eobj))
										{
											RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref text3);
										}
										GenerateReports.Report(obj, Eobj);
									}
									else
									{
										RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref text3);
									}
								}
								else
								{
									System.IO.File.Delete(obj.FilePath + "\\" + text);
									RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref text3);
									ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  No content in file", "files loop");
								}




							}
							else
							{
								RequestProcesser.SetRejected(fileNameWithoutExtension, empty, obj, Eobj, ref text3);
								ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + text + " File Format error", System.Reflection.MethodBase.GetCurrentMethod().Name);
								if (CustomerDataUpdate.FileMove(obj, text, false, obj.OutPutPath, Eobj))
								{
									System.IO.File.Delete(obj.FilePath + "\\" + text);
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				Eobj.EmailMsg = ex.ToString();
				EmailAlert.FunSendMailMessage("", Eobj);
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		private static void MoveFile(customerDataObject obj, EmailDataObject Eobj, string _fileName)
		{
			try
			{
				if (CustomerDataUpdate.FileMove(obj, _fileName, false, obj.OutPutPath, Eobj))
				{
					System.IO.File.Delete(obj.FilePath + "\\" + _fileName);
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # Move file error:  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
		}

		public static void SetRejected(string _fileName, string _RejectFileName, customerDataObject obj, EmailDataObject Eobj, ref string StrErrorMessages)
		{
			try
			{
				string text = obj.FilePath + "\\" + obj.Filename + "_Rejected.txt";
				System.IO.File.AppendAllText(text, "File processing error: " + System.Environment.NewLine + StrErrorMessages);
				SFTPConnection.UploadErrorFile(obj.ServerIP, obj.UserName, obj.Password, obj.port, text, obj.OutPutPath, _fileName, obj.KeyPath, obj.Passphrase);
				System.IO.File.Delete(text);
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Error # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
		}

		public static bool IsLocked(this System.IO.FileInfo f)
		{
			bool result;
			try
			{
				string fullName = f.FullName;
				System.IO.FileStream fileStream = System.IO.File.OpenWrite(fullName);
				fileStream.Close();
				result = false;
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = true;
			}
			return result;
		}

		public static System.Collections.Generic.IEnumerable<string> ReadAsLines(string filename)
		{
			using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filename))
			{
				while (!streamReader.EndOfStream)
				{
					yield return streamReader.ReadLine();
				}
			}
			yield break;
		}

		public static DataTable ReadFile(string file, string FileHeader, EmailDataObject Eobj, int filedCount, ref string StrErrorMessages)
		{
			string text = string.Empty;
			DataTable dataTable = new DataTable();
			string[] array = FileHeader.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string columnName = array2[i];
				dataTable.Columns.Add(columnName);
			}
			DataTable result;
			try
			{
				System.Collections.Generic.IEnumerable<string> enumerable = RequestProcesser.ReadAsLines(file);
				System.Collections.Generic.IEnumerable<string> enumerable2 = enumerable;
				foreach (string current in enumerable2)
				{
					if (current != "\t" && current != "")
					{
						if (current.Split(new char[]
						{
							'|'
						}).Length != dataTable.Columns.Count)
						{
							text = current;
							for (int j = 0; j < dataTable.Columns.Count - current.Split(new char[]
							{
								'|'
							}).Length; j++)
							{
								text += "|Error In Record";
							}
							dataTable.Rows.Add(text.Split(new char[]
							{
								'|'
							}));
						}
						else
						{
							dataTable.Rows.Add(current.Split(new char[]
							{
								'|'
							}));
						}
					}
				}
				int count = dataTable.Columns.Count;
				if (count < filedCount)
				{
					int num = filedCount - count;
					for (int j = 0; j < num; j++)
					{
						dataTable.Columns.Add("Default_" + j.ToString());
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				Eobj.EmailMsg = ex.ToString();
				StrErrorMessages = "File format is not proper in all records" + System.Environment.NewLine;
				EmailAlert.FunSendMailMessage("", Eobj);
				result = dataTable;
				return result;
			}
			result = dataTable;
			return result;
		}
	}
}
