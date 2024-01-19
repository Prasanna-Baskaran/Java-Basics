using AGS.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace CustomerDataUpdate
{
	internal class ModuleDAL
	{
		public DataTable GetCustomerData()
		{
			System.Collections.Generic.Dictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>();
			return SQLDAL.ExecuteSQLDataTable(dictionary, "", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
		}

		public DataTable GetConfiguration()
		{
			DataTable dataTable = new DataTable();
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :SQL conncetion started for the Fatching Mass Config ", System.Reflection.MethodBase.GetCurrentMethod().Name);
			DataTable result;
			try
			{
				System.Collections.Generic.Dictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>();
				dataTable = SQLDAL.ExecuteSQLDataTable(dictionary, "Usp_getMasConfiguration", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = dataTable;
				return result;
			}
			result = dataTable;
			return result;
		}

		public bool InsertCustomerData(DataTable customerData, string Filename, int BANK,String FileProcessorSP, ref string StrErrorMessages)
		{
			DataTable dataTable = new DataTable();
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # SQL conncetion started for Inserting the Records ", System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool result;
			try
			{
				SQLDAL.ExecuteSQLNonQuery(new System.Collections.Generic.Dictionary<string, object>
				{
					{
						"@DataTable",
						customerData
					},
					{
						"@Filename",
						Filename
					},
					{
						"@BANK",
						BANK
					}
				}, FileProcessorSP, System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"])); ///Added by uddesh on 29-04-2019 ATPCM-656

                //"usp_InsertCustomerDataModificationRequest"

            }
            catch (System.Exception ex)
			{
				StrErrorMessages = "Error while inserting records in database" + System.Environment.NewLine;
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		public DataTable getcustomerDataRecord(int bank)
		{
			DataTable dataTable = new DataTable();
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # SQL conncetion started for for the Fatching records ", System.Reflection.MethodBase.GetCurrentMethod().Name);
			DataTable result;
			try
			{
				dataTable = SQLDAL.ExecuteSQLDataTable(new System.Collections.Generic.Dictionary<string, object>
				{
					{
						"@Mode",
						"select"
					},
					{
						"@Code",
						""
					},
					{
						"@CIFId",
						""
					},
					{
						"@respCode",
						""
					},
					{
						"@bankID",
						bank.ToString()
					}
				}, "USP_MakeCustomerData", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = dataTable;
				return result;
			}
			result = dataTable;
			return result;
		}

		public bool UpdatecustomerDataStatus(string code, string cifid, string respCode, int bank)
		{
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # SQL conncetion started for the Updating the records ", System.Reflection.MethodBase.GetCurrentMethod().Name);
			bool result;
			try
			{
				DataTable dataTable = SQLDAL.ExecuteSQLDataTable(new System.Collections.Generic.Dictionary<string, object>
				{
					{
						"@Code",
						code
					},
					{
						"@CIFId",
						cifid
					},
					{
						"@respCode",
						respCode
					},
					{
						"@Mode",
						"Update"
					},
					{
						"@bankID",
						bank.ToString()
					}
				}, "USP_MakeCustomerData", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
				if (dataTable.Rows.Count > 0)
				{
					result = true;
					return result;
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = false;
				return result;
			}
			result = false;
			return result;
		}

		public DataSet getReport(string fileName, int bankId)
		{
			DataSet dataSet = new DataSet();
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :conncetion started for the Fatching records", System.Reflection.MethodBase.GetCurrentMethod().Name);
			DataSet result;
			try
			{
				dataSet = SQLDAL.ExecuteSQLDataSet(new System.Collections.Generic.Dictionary<string, object>
				{
					{
						"@FileName",
						fileName
					},
					{
						"@DateTime",
						System.DateTime.Now.ToString("yyyyMMdd")
					},
					{
						"@bankID",
						bankId.ToString()
					}
				}, "USP_GetProcessedCustomerData", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = dataSet;
				return result;
			}
			result = dataSet;
			return result;
		}

		public DataTable GetEmailConfiguration()
		{
			DataTable dataTable = new DataTable();
			ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :conncetion started for the Fatching Mass Config", System.Reflection.MethodBase.GetCurrentMethod().Name);
			DataTable result;
			try
			{
				System.Collections.Generic.Dictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>();
				dataTable = SQLDAL.ExecuteSQLDataTable(dictionary, "Usp_getEmailConfiguration", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				result = dataTable;
				return result;
			}
			result = dataTable;
			return result;
		}

		public static void InsertLog(string exception, string method)
		{
			try
			{
				SQLDAL.ExecuteSQLNonQuery(new System.Collections.Generic.Dictionary<string, object>
				{
					{
						"@Exception",
						exception
					},
					{
						"@MethodName",
						method
					}
				}, "Usp_InsertLog", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
			}
			catch (System.Exception ex)
			{
				Logger.WriteLog(System.DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), "Log_");
			}
		}

        ///Added by uddesh on 29-04-2019 ATPCM-656 Start
        public DataTable CheckForDuplicateFile(String FileName,Int32 Bankid)
        {
            DataTable dataTable = new DataTable();
            ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :SQL conncetion started for CheckForDuplicateFile ", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DataTable result;
            try
            {
                System.Collections.Generic.Dictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>();
                dictionary.Add("@FileName", FileName);
                dictionary.Add("@bankid", Bankid);

                dataTable = SQLDAL.ExecuteSQLDataTable(dictionary, "usp_CheckForDuplicateFile", System.Convert.ToString(ConfigurationManager.ConnectionStrings["CustomerDataUpdate"]));
            }
            catch (System.Exception ex)
            {
                ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                result = dataTable;
                return result;
            }
            result = dataTable;
            return result;
        }
        ///Added by uddesh on 29-04-2019 ATPCM-656 end

    }
}
