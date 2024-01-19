using AGSPGP;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace CustomerDataUpdate
{
	public class CustomerDataUpdate
	{
		public void Process()
		{
			EmailDataObject emailDataObject = CustomerDataUpdate.LoadEmailObject();
			try
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  customer data update dll started", System.Reflection.MethodBase.GetCurrentMethod().Name);
				DataTable configuration = new ModuleDAL().GetConfiguration();
				if (configuration.Rows.Count > 0)
				{
					foreach (DataRow dataRow in configuration.Rows)
					{
                        //continue;
                        ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  DataTable to Object BInding", System.Reflection.MethodBase.GetCurrentMethod().Name);
						customerDataObject customerDataObject = new customerDataObject();
						customerDataObject.ArchivePath = System.Convert.ToString(dataRow["FilePathArchive"]);
						customerDataObject.Bankid = System.Convert.ToInt32(dataRow["bankid"]);
						customerDataObject.FileHeader = System.Convert.ToString(dataRow["FileHeader"]);
						customerDataObject.FilePath = System.Convert.ToString(dataRow["FilePath"]);
						customerDataObject.InputPath = System.Convert.ToString(dataRow["FilePathInput"]);
						customerDataObject.issuerNr = System.Convert.ToInt32(dataRow["issuerNr"]);
						customerDataObject.KeyPath = System.Convert.ToString(dataRow["keyPath"]);
						customerDataObject.OutPutPath = System.Convert.ToString(dataRow["FilePathOutput"]);
						customerDataObject.Passphrase = System.Convert.ToString(dataRow["Passphrase"]);
						customerDataObject.Password = System.Convert.ToString(dataRow["Password"]);
						customerDataObject.port = System.Convert.ToInt32(dataRow["ServerPort"]);
						customerDataObject.ServerIP = System.Convert.ToString(dataRow["ServerIP"]);
						customerDataObject.UserName = System.Convert.ToString(dataRow["UserName"]);
						customerDataObject.ISPGP = System.Convert.ToBoolean(dataRow["isPGP"]);
						customerDataObject.Trace = System.Convert.ToBoolean(dataRow["Trace"]);
						customerDataObject.DecInputFilePath = System.Convert.ToString(dataRow["InputFilePath_PGP"]);
						customerDataObject.DecOutputFilePath = customerDataObject.FilePath;
						customerDataObject.EncInputFilePath = customerDataObject.FilePath;
						customerDataObject.EncOutputFilePath = System.Convert.ToString(dataRow["InputFilePath_PGP"]);
						customerDataObject.PublicKeyFilePath = System.Convert.ToString(dataRow["PublicKeyFilePath"]);
						customerDataObject.PrivateKeyFilePath = System.Convert.ToString(dataRow["PrivateKeyFilePath"]);
						customerDataObject.Password_PGP = System.Convert.ToString(dataRow["Password_PGP"]);
						customerDataObject.FiledCount = System.Convert.ToInt32(dataRow["FieldCount"]);
						customerDataObject.IsOutPutEncrypted = System.Convert.ToBoolean(dataRow["IsOutPutEncrypted"]);
						customerDataObject.FileExtension = System.Convert.ToString(dataRow["FileExtension"]);
                        customerDataObject.FileProcessorSP = System.Convert.ToString(dataRow["FileProcessorSP"]); ///Added by uddesh on 29-04-2019 ATPCM-656
                        if (CustomerDataUpdate.FileMove(customerDataObject, "", true, customerDataObject.ArchivePath, emailDataObject))
						{
							if (customerDataObject.ISPGP)
							{
								this.DecryptPGPfile(customerDataObject);
							}
							bool flag = RequestProcesser.process(customerDataObject, emailDataObject);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				emailDataObject.EmailMsg = ex.ToString();
				EmailAlert.FunSendMailMessage("", emailDataObject);
			}
		}

		public static bool FileMove(customerDataObject obj, string fileName, bool isdownload, string outputPath, EmailDataObject Eobj)
		{
			bool result;
			if (string.IsNullOrEmpty(obj.KeyPath) && string.IsNullOrEmpty(obj.Passphrase))
			{
				result = SFTPConnection.__SFTPConncetionwithoutKey(obj.ServerIP, obj.UserName, obj.Password, obj.port, outputPath, obj.InputPath, obj.ISPGP ? obj.DecInputFilePath : obj.FilePath, isdownload, fileName, Eobj, obj.FileExtension);
			}
			else
			{
				result = SFTPConnection.__SFTPConncetionwithKey(obj.ServerIP, obj.UserName, obj.Password, obj.port, outputPath, obj.InputPath, obj.ISPGP ? obj.DecInputFilePath : obj.FilePath, isdownload, fileName, obj.KeyPath, obj.Passphrase, Eobj, obj.FileExtension);
			}
			return result;
		}

		public static void Encrypt(customerDataObject model)
		{
			PGPEncryptDecrypt.EncryptFile(model.EncInputFilePath, model.EncOutputFilePath, model.PublicKeyFilePath, true, true);
		}

		public static void Decrypt(customerDataObject model)
		{
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP Decrption Started", System.Reflection.MethodBase.GetCurrentMethod().Name);
			string[] files = System.IO.Directory.GetFiles(model.DecInputFilePath, "*.pgp", System.IO.SearchOption.TopDirectoryOnly);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # : PGP Decrption item:",
					text.ToString(),
					"|PrivateKeyFilePath:",
					model.PrivateKeyFilePath,
					"|Password_PGP:",
					model.Password_PGP,
					"|DecOutputFilePath:",
					model.DecOutputFilePath,
					System.IO.Path.GetFileNameWithoutExtension(text),
					".txt"
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				PGPEncryptDecrypt.Decrypt(text.ToString(), model.PrivateKeyFilePath, model.Password_PGP, model.DecOutputFilePath + System.IO.Path.GetFileNameWithoutExtension(text) + ".txt");
				System.IO.File.Delete(text.ToString());
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP Decrption END", System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
		}

		public void DecryptPGPfile(customerDataObject ObjConfig)
		{
			try
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP DECRYPTION STARTED!", System.Reflection.MethodBase.GetCurrentMethod().Name);
				string[] files = System.IO.Directory.GetFiles(ObjConfig.DecInputFilePath, "*.pgp", System.IO.SearchOption.TopDirectoryOnly);
				if (files != null)
				{
					if (files.Length > 0)
					{
						string[] array = files;
						for (int i = 0; i < array.Length; i++)
						{
							string text = array[i];
							ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP FILE FOUND WITH  NAME |" + text.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
							bool flag = this.PGP_Decrypt(ObjConfig.PrivateKeyFilePath, System.IO.Path.Combine(ObjConfig.DecInputFilePath, text), System.IO.Path.Combine(ObjConfig.DecInputFilePath, text).Split(new char[]
							{
								'.'
							})[0] + ".txt", ObjConfig.PrivateKeyFilePath, ObjConfig.Password_PGP);
							if (flag)
							{
								if (System.IO.File.Exists(System.IO.Path.Combine(ObjConfig.DecInputFilePath, text)))
								{
								}
								System.IO.File.Delete(System.IO.Path.Combine(ObjConfig.DecInputFilePath, text));
							}
						}
					}
					else
					{
						ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :NO PGP FILE FOUND|", System.Reflection.MethodBase.GetCurrentMethod().Name);
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :" + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP DECRYPTION ENDED", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		public bool PGP_Decrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string PGPPassword)
		{
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : PGP CIF file decryption started", System.Reflection.MethodBase.GetCurrentMethod().Name);
			PGPModel pGPModel = new PGPModel();
			pGPModel.DecInputFilePath= fileFrom;
			pGPModel.DecOutputFilePath = fileTo;
			pGPModel.PrivateKeyFilePath =PgpKeyPath;
			pGPModel.Password= PGPPassword;
			bool flag = false;
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileFrom);
			bool result;
			if (!fileInfo.Exists)
			{
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # :Cannot find the file to decrypt PgpPath=",
					PgpKeyPath,
					",InputFile=",
					fileFrom,
					"Outputfile=",
					fileTo
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else if (!System.IO.File.Exists(PgpKeyPath))
			{
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # :Cannot find PGP Key PgpPath=",
					PgpKeyPath,
					",InputFile=",
					fileFrom,
					"Outputfile=",
					fileTo
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else if (System.IO.File.Exists(fileTo))
			{
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # :Cannot decrypt file.  File already exists PgpPath=",
					PgpKeyPath,
					",InputFile=",
					fileFrom,
					"Outputfile=",
					fileTo
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else
			{
				try
				{
					new PGP().Decrypt(pGPModel);
					if (System.IO.File.Exists(fileTo))
					{
						flag = true;
						ModuleDAL.InsertLog(string.Concat(new string[]
						{
							System.DateTime.Now.ToString(),
							">> Message # :File decrypted successfully. PgpPath=",
							PgpKeyPath,
							",InputFile=",
							fileFrom,
							"Outputfile=",
							fileTo
						}), System.Reflection.MethodBase.GetCurrentMethod().Name);
					}
				}
				catch (System.Exception ex)
				{
					flag = false;
					ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :" + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				}
				result = flag;
			}
			return result;
		}

		public bool PGP_Encrypt(string keyName, string fileFrom, string fileTo, string PgpKeyPath, string IssuerNo)
		{
			PGPModel pGPModel = new PGPModel();
			pGPModel.EncInputFilePath=fileFrom;
			pGPModel.EncOutputFilePath=fileTo;
			pGPModel.PublicKeyFilePath=PgpKeyPath;
			bool flag = false;
			System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileFrom);
			bool result;
			if (!fileInfo.Exists)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # PGP_Encrypt failed|Missing file.  Cannot find the file to encrypt", System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else if (!System.IO.File.Exists(PgpKeyPath))
			{
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # Cannot find PGP Key PgpPath=",
					PgpKeyPath,
					",InputFile=",
					fileFrom,
					"Outputfile=",
					fileTo
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else if (System.IO.File.Exists(fileTo))
			{
				ModuleDAL.InsertLog(string.Concat(new string[]
				{
					System.DateTime.Now.ToString(),
					">> Message # Cannot encrypt file.  File already exists PgpPath=",
					PgpKeyPath,
					",InputFile=",
					fileFrom,
					"Outputfile=",
					fileTo
				}), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = flag;
			}
			else
			{
				try
				{
					new PGP().Encrypt(pGPModel);
				}
				catch (System.Exception ex)
				{
					ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # Cannot encrypt file. " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				}
				if (System.IO.File.Exists(fileTo))
				{
					flag = true;
				}
				result = flag;
			}
			return result;
		}

		public static EmailDataObject LoadEmailObject()
		{
			EmailDataObject emailDataObject = new EmailDataObject();
			DataTable emailConfiguration = new ModuleDAL().GetEmailConfiguration();
			if (emailConfiguration.Rows.Count > 0)
			{
				emailDataObject.SMTPCLIENT = System.Convert.ToString(emailConfiguration.Rows[0]["SMTPCLIENT"]);
				emailDataObject.EmailBCC = System.Convert.ToString(emailConfiguration.Rows[0]["EmailBCC"]);
				emailDataObject.EmailFrom = System.Convert.ToString(emailConfiguration.Rows[0]["EmailFrom"]);
				emailDataObject.EmailMsg = System.Convert.ToString(emailConfiguration.Rows[0]["EmailMsg"]);
				emailDataObject.EmailPassWord = System.Convert.ToString(emailConfiguration.Rows[0]["EmailPassWord"]);
				emailDataObject.EmailPort = System.Convert.ToInt32(emailConfiguration.Rows[0]["EmailPort"]);
				emailDataObject.EmailTo = System.Convert.ToString(emailConfiguration.Rows[0]["EmailTo"]);
				emailDataObject.EmailUserName = System.Convert.ToString(emailConfiguration.Rows[0]["EmailUserName"]);
			}
			return emailDataObject;
		}
	}
}
