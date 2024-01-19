using System;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace CustomerDataUpdate
{
	internal class GenerateISO
	{
		public bool Process(customerDataObject obj, EmailDataObject Eobj)
		{
			bool result;
			try
			{
				DataTable dataTable = new ModuleDAL().getcustomerDataRecord(obj.Bankid);
				string text = System.Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_IPPORT"]);
				if (dataTable.Rows.Count > 0)
				{
					ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  Message # : ISO Creation started", System.Reflection.MethodBase.GetCurrentMethod().Name);
					foreach (DataRow dataRow in dataTable.Rows)
					{
						CustomerdataRequestObject customerdataRequestObject = new CustomerdataRequestObject();
						customerdataRequestObject.stan = this.randomno();
						customerdataRequestObject.ServerIP = text.Split(new char[]
						{
							'|'
						})[0];
						customerdataRequestObject.Port = int.Parse(text.Split(new char[]
						{
							'|'
						})[1]);
						customerdataRequestObject.CODE = System.Convert.ToString(dataRow["code"]);
						customerdataRequestObject.IssuerNo = System.Convert.ToString(dataRow["IssuerNo"]);
						customerdataRequestObject.CIF = System.Convert.ToString(dataRow["CIF"]);
						customerdataRequestObject.CIFDATA = System.Convert.ToString(dataRow["CIFDATA"]);
						customerdataRequestObject.proessingcode = "910000";
						customerdataRequestObject.trace = obj.Trace;
						string text2 = this.ISOBuild(customerdataRequestObject, Eobj);
						if (!string.IsNullOrEmpty(text2))
						{
							bool flag = new ModuleDAL().UpdatecustomerDataStatus(customerdataRequestObject.CODE, customerdataRequestObject.CIF, text2, obj.Bankid);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				Eobj.EmailMsg = ex.ToString();
				Eobj.EmailMsg = ex.ToString();
				EmailAlert.FunSendMailMessage("", Eobj);
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		private string randomno()
		{
			System.Random random = new System.Random();
			return random.Next(0, 1000000).ToString();
		}

		private string ISOBuild(CustomerdataRequestObject ObjRequest, EmailDataObject Eobj)
		{
			string result;
			try
			{
				string[] array = new string[130];
				string[] array2 = new string[130];
				string[] array3 = new string[65];
				string text = System.DateTime.Now.ToString("HHmmss");
				string text2 = System.DateTime.Now.ToString("MMdd");
				string text3 = System.Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_TID"]);
				string text4 = System.Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_CardAcceptorID"]);
				array[3] = (string.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode);
				array[7] = (string.IsNullOrEmpty(text2 + text) ? null : (text2 + text));
				array[11] = (string.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan);
				array[12] = (string.IsNullOrEmpty(text) ? null : text);
				array[13] = (string.IsNullOrEmpty(text2) ? null : text2);
				array[18] = "6012";
				array[22] = "000";
				array[25] = "00";
				array[32] = ObjRequest.IssuerNo;
				array[41] = (string.IsNullOrEmpty(text3) ? null : text3);
				array[42] = (string.IsNullOrEmpty(text4) ? null : text4);
				array[43] = ConfigurationManager.AppSettings["HotListCardReissue_DE43"].ToString();
				array[46] = ObjRequest.CIFDATA;
				array[48] = "3300";
				array[49] = "356";
				array[129] = "0320";
				ISO8583 iSO = new ISO8583("POSTTERM", ObjRequest.trace);
				string text5 = iSO.Build(array, array3, array[129]);
				array = iSO.Parse(text5, false);
				byte[] array4 = ISGenerixLib.stringToByteArray(text5);
				SwitchInterface switchInterface = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
				byte[] array5 = switchInterface.sendDataToSwitch(array4);
				if (array5 == null)
				{
					Eobj.EmailMsg = "No Response From  switch for CIF ID";
					result = "01";
				}
				else
				{
					string text6 = ISGenerixLib.byteToHex(array5);
					array2 = iSO.Parse(text6, true);
					array3 = iSO.getDE127();
					result = System.Convert.ToString(array2[39]);
				}
			}
			catch (System.Exception ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
				Eobj.EmailMsg = ex.ToString();
				Eobj.EmailMsg = ex.ToString();
				EmailAlert.FunSendMailMessage(ObjRequest.CIF, Eobj);
				result = "01";
			}
			return result;
		}
	}
}
