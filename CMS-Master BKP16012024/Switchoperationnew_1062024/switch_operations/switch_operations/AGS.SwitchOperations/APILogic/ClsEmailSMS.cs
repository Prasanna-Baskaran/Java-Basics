using AGS.SwitchOperations.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGS.SwitchOperations
{
	public class ClsEmailSMS
	{
		public string SendEmail(DataRow Dr)
		{
			string result = "";
			int flag = 0;
			try
			{
				//BAL.DebugLog("Email send started:ID:" , Dr["ID"].ToString());
				MailMessage oMessage = new MailMessage();
				oMessage.From = new MailAddress(Convert.ToString(Dr["sender"]));
				string[] emails = Convert.ToString(Dr["Email"]).Split(',');
				foreach (string id in emails)
                {
					if(!string.IsNullOrWhiteSpace(id))
						oMessage.To.Add(new MailAddress(id));
				}
				oMessage.IsBodyHtml = true;
				oMessage.Body = Dr["body"].ToString();
				oMessage.IsBodyHtml = true;
				if (string.IsNullOrEmpty(oMessage.Body))
				{
					//BAL.DebugLog("Email body is blank:ID:" , Dr["ID"].ToString());
					result = "Message Body Not found";
				}
				else
				{
					oMessage.Subject = Dr["subject"].ToString();
                    string PDFPath = string.Concat(new string[]
                    {
                        Dr["attachmentpath"].ToString(),
                        Dr["FileName"].ToString().Trim()
                    });
                    try
                    {
						if(Path.GetFileName(PDFPath)!="")
							oMessage.Attachments.Add(new Attachment(PDFPath));
						//BAL.DebugLog("PDF attached.path:", PDFPath + ",ID:" + Dr["ID"].ToString());
                    }
                    catch (Exception exx)
                    {
                        result = "Attachment failed";
                        //BAL.ErrorLog(exx, "Attachment failed");
                        return result;
                    }
					new SmtpClient(Dr["smtpaddress"].ToString(), Convert.ToInt32(Dr["smtpport"].ToString()))
					{
						Credentials = new NetworkCredential(Convert.ToString(Dr["smtpusername"]), Convert.ToString(Dr["smtppassword"])),
						EnableSsl = Convert.ToBoolean(Dr["smtpsslenabled"])
					}.Send(oMessage);
					try
					{
						if (oMessage != null)
						{
							oMessage.Dispose();
							flag = 1;
						}
						//BAL.DebugLog("Send message disposed success", "");
					}
					catch (Exception ex)
					{
						//BAL.ErrorLog(ex,"Send message disposed");
						flag = 1;
					}
					try
					{
						//BAL.DebugLog("Error: PDF deleted started:ID:" , Dr["ID"].ToString());
						if (File.Exists(PDFPath))
						{
							//File.Delete(PDFPath);
							//BAL.DebugLog("Error: PDF deleted successfully:ID:" , Dr["ID"].ToString());
						}
						else
						{
							//BAL.DebugLog("Error: PDF file not exists:ID:" , Dr["ID"].ToString());
						}
						flag = 1;
					}
					catch (Exception ef)
					{
						//BAL.ErrorLog(ef,"Error: PDF delete error:ID:" + Dr["ID"].ToString());
						flag = 1;
					}
					Thread.Sleep(100);
					//BAL.DebugLog("Email sent succesfully:ID:", string.Concat(new object[]
					//{
					//	Dr["ID"],
					//	", Email",
					//	Dr["Email"]
					//}));
					result = "success";
					flag = 2;
				}
			}
			catch (Exception ex2)
			{
				//BAL.ErrorLog(ex2,"Error in Email sent:ID:" + Dr["ID"].ToString());
				result = "failed";
				flag = 1;
			}
			finally
			{
				//Dictionary<string, string> Params = Dr.Table.Columns
				//								  .Cast<DataColumn>()
				//								  .ToDictionary(c => "p_"+c.ColumnName.ToLower(), c => Convert.ToString(Dr[c]));
				//Params.Add("p_flag",flag.ToString());
				//PLSQL.ExecutePlsqlDataTable(Params,"fun_Emaillog");
				//BAL.DebugLog("Email Logged", Dr["ID"].ToString());
			}
			return result;
		}

		private string createEmailBody(string message, string TempleatePath)
		{
			string body = string.Empty;
			try
			{
				using (StreamReader reader = new StreamReader(TempleatePath))
				{
					body = reader.ReadToEnd();
				}
				body = body.Replace("{EmailBody}", message);
			}
			catch (Exception ex)
			{
				//BAL.ErrorLog(ex,"Error in Email body creation");
			}
			return body;
		}

		public string sendSMS(DataRow Dr)
		{
			string sResponse = "";
			try
			{
				//BAL.DebugLog("SMS send started, ID:" , Dr["ID"].ToString());
				string postData = Dr["subject"].ToString();
				string URL = Dr["smtpAddress"].ToString() + Dr["body"].ToString();
				sResponse = new WebClient
				{
					Headers =
					{
						{
							"Content-Type",
							"application/x-www-form-urlencoded"
						}
					}
				}.UploadString(URL, postData);
				if (sResponse.IndexOf("ERROR") != -1)
				{
					sResponse = "Url Not Connected";
				}
				else
				{
					//BAL.DebugLog("SMS sent successfully: ID:" , Dr["ID"].ToString());
					sResponse = "success";
				}
			}
			catch (Exception ex)
			{
				sResponse = "Url Not Connected";
				//BAL.ErrorLog(ex,"Error in SMS sent: ID" + Dr["ID"].ToString());
			}
			return sResponse;
		}
	}
}
