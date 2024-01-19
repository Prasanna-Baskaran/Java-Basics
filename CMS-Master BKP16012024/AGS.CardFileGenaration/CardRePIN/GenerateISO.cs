using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    class GenerateISO
    {
        public void Process(CardRePINDataObject obj,EmailDataObject Eobj)
        {
            try
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # : ISO generation started", System.Reflection.MethodBase.GetCurrentMethod().Name);               
                DataTable Dt = new ModuleDAL().getCardRePINData();
                string config_data = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_IPPORT"]);
                if (Dt.Rows.Count > 0)
                {

                   
                    foreach (DataRow item in Dt.Rows)
                    {
                        RePINISOObject ObjRequest = new RePINISOObject();
                        ObjRequest.stan = randomno();
                        ObjRequest.ServerIP = config_data.Split('|')[0];
                        ObjRequest.Port = int.Parse(config_data.Split('|')[1]);
                        ObjRequest.CODE = Convert.ToString(item["code"]);
                        ObjRequest.IssuerNo = Convert.ToString(item["IssuerNo"]);
                        ObjRequest.cardNo= Convert.ToString(item["CardNo"]);
                        ObjRequest.CIFID = Convert.ToString(item["CIFID"]);
                        ObjRequest.AccountNo = Convert.ToString(item["AccountNo"]);
                        ObjRequest.proessingcode = "910000";
                        ObjRequest.trace = obj.Trace;
                        string responseCode= ISOBuild(ObjRequest,Eobj);
                        if (!string.IsNullOrEmpty(responseCode))
                        {
                            bool IsStatusUpdated = new ModuleDAL().UpdatecardRePINStatus(ObjRequest.CODE, ObjRequest.cardNo,responseCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :" +ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);               
                Eobj.EmailMsg = ex.ToString();
                EmailAlert.FunSendMailMessage(obj.FileName, Eobj);
            }



        }
        private string randomno()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString();
            return r;
        }
        private String ISOBuild(RePINISOObject ObjRequest, EmailDataObject Eobj)
        {


            try
            {
                
                string[] rqDE = new string[130], rspDE = new string[130], reDE127 = new string[65];
                string rsp = "";
                string time = DateTime.Now.ToString("HHmmss");
                string date = DateTime.Now.ToString("MMdd");
                string _StrTerminalId = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_TID"]);
                string cardAcceptorIdCode = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_CardAcceptorID"]);
                rqDE[2] = String.IsNullOrEmpty(ObjRequest.cardNo) ? null : ObjRequest.cardNo;
                rqDE[3] = String.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode;
                rqDE[7] = String.IsNullOrEmpty(date + time) ? null : date + time;
                rqDE[11] = String.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan;
                rqDE[12] = String.IsNullOrEmpty(time) ? null : time;
                rqDE[13] = String.IsNullOrEmpty(date) ? null : date;
                rqDE[18] = "6012";
                rqDE[22] = "000";
                rqDE[25] = "00";
                rqDE[32] = ObjRequest.IssuerNo;
                rqDE[41] = String.IsNullOrEmpty(_StrTerminalId) ? null : _StrTerminalId;
                rqDE[42] = String.IsNullOrEmpty(cardAcceptorIdCode) ? null : cardAcceptorIdCode;
                rqDE[43] = ConfigurationManager.AppSettings["HotListCardReissue_DE43"].ToString();
                rqDE[46] = "3302|02";
                rqDE[48] =Convert.ToString(ObjRequest.CIFID);
                rqDE[102] = Convert.ToString(ObjRequest.AccountNo);
                rqDE[129] = "0600";
                ISO8583 isoObj = new ISO8583("POSTTERM", ObjRequest.trace);
                string req = isoObj.Build(rqDE, reDE127, rqDE[129]);
                rqDE = isoObj.Parse(req, false);
                byte[] rqBytes = ISGenerixLib.stringToByteArray(req);
                SwitchInterface post_termObj = new SwitchInterface(ObjRequest.ServerIP, ObjRequest.Port);
                byte[] rspBytes = post_termObj.sendDataToSwitch(rqBytes);
                if (rspBytes==null)
                {
                    Eobj.EmailMsg = "No Response From  switch for Card No";
                    if (ObjRequest.cardNo.ToString()!="")
                    { EmailAlert.FunSendMailMessage(ObjRequest.cardNo.Substring(0, 6) + "******" + ObjRequest.cardNo.Substring(ObjRequest.cardNo.Length-4, 4), Eobj); }
                    else
                    {
                        EmailAlert.FunSendMailMessage(ObjRequest.CIFID, Eobj); 
                    }
                    
                    return "01";
                }

                rsp = ISGenerixLib.byteToHex(rspBytes);
                rspDE = isoObj.Parse(rsp, true);
                reDE127 = isoObj.getDE127();
                return Convert.ToString(rspDE[39]);
                
            }
            catch (Exception ex)
            {
                ModuleDAL.InsertLog(DateTime.Now.ToString() + ">> Message # :" + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);               
                Eobj.EmailMsg = ex.ToString();
                if (ObjRequest.cardNo.ToString() != "")
                { EmailAlert.FunSendMailMessage(ObjRequest.cardNo.Substring(0, 6) + "******" + ObjRequest.cardNo.Substring(ObjRequest.cardNo.Length-4, 4), Eobj); }
                else
                {
                    EmailAlert.FunSendMailMessage(ObjRequest.CIFID, Eobj);
                }
                return "01";
            }
        }
    }
}
