using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace CustomerDataUpdate
{
	internal class EmailAlert
	{
		public static void FunSendMailMessage(string fileName, EmailDataObject obj)
		{
			try
			{
				MailMessage mailMessage = new MailMessage();
				SmtpClient smtpClient = new SmtpClient(obj.SMTPCLIENT);
				mailMessage.From = new MailAddress(obj.EmailFrom);
				smtpClient.Port = System.Convert.ToInt32(obj.EmailPort);
				smtpClient.UseDefaultCredentials = false;
				smtpClient.EnableSsl = false;
				smtpClient.Credentials = new NetworkCredential(obj.EmailUserName, obj.EmailPassWord);
				mailMessage.IsBodyHtml = true;
				mailMessage.Subject = "Customer Data Update ";
				string emailTo = obj.EmailTo;
				string emailBCC = obj.EmailBCC;
				mailMessage.Body = string.Concat(new string[]
				{
					"Dear Team, <br/> ",
					obj.EmailMsg,
					" <br/> ",
					fileName,
					".<br/><br/><br/><br/>Regards,<br/>AGS Team"
				});
				string[] array = emailTo.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (text != null || text != "")
					{
						mailMessage.To.Add(new MailAddress(text));
					}
				}
				string[] array3 = emailBCC.Split(new char[]
				{
					','
				});
				array2 = array3;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (text2 != null || text2 != "")
					{
						mailMessage.Bcc.Add(new MailAddress(text2));
					}
				}
				mailMessage.Priority = MailPriority.High;
				smtpClient.Send(mailMessage);
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # : Alert Mail Send ", System.Reflection.MethodBase.GetCurrentMethod().Name);
				smtpClient.Dispose();
			}
			catch (SmtpException ex)
			{
				ModuleDAL.InsertLog(System.DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
			}
		}
	}
}
