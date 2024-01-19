using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    class EmailAlert
    {
        #region This Method is used Send the email alert.Create by Gufran Khan.
        public static void FunSendMailMessage(string fileName,EmailDataObject obj)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(obj.SMTPCLIENT);
                mailMessage.From = new MailAddress(obj.EmailFrom);
                smtpClient.Port = Convert.ToInt32(obj.EmailPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = false;
                smtpClient.Credentials = new NetworkCredential(obj.EmailUserName,obj.EmailPassWord);
                //mailMessage.Bcc.Add(ConfigurationSettings.AppSettings["BCC"]);
                //mailMessage.To.Add(to);                
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "CardREPIN ";
                string SendTo = "";
                string BCC = "";
                

                    SendTo = obj.EmailTo;
                    BCC = obj.EmailBCC;
                    mailMessage.Body = "Dear Team, <br/> "+ obj.EmailMsg +" <br/> " + fileName + ".<br/><br/><br/><br/>Regards,<br/>AGS Team";

                   
                
                string[] ArrToID = SendTo.Split(',');
                foreach (string StrLocMultiToID in ArrToID)
                {
                    if (StrLocMultiToID != null || StrLocMultiToID != "")
                    {
                        mailMessage.To.Add(new MailAddress(StrLocMultiToID));
                    }
                }
                string[] ArrBCCId = BCC.Split(',');
                foreach (string StrLocMultiArrBCCId in ArrBCCId)
                {
                    if (StrLocMultiArrBCCId != null || StrLocMultiArrBCCId != "")
                    {
                        mailMessage.Bcc.Add(new MailAddress(StrLocMultiArrBCCId));
                    }
                }
                //mailMessage.Body = body;
                mailMessage.Priority = MailPriority.High;
                smtpClient.Send(mailMessage);
                Logger.WriteLog(DateTime.Now.ToString() + "Alert Mail Send", "MailLog_");
                smtpClient.Dispose();
            }
            catch (SmtpException ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);

            }

        }
        #endregion
    }
}
