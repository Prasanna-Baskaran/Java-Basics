using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotListCardReissue
{
    public class ClsHotListCardReissue
    {
        public void Process()
        {
            try
            {
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # : Application started ", System.Reflection.MethodBase.GetCurrentMethod().Name);
                DataTable Dt = new CardReissueDAL().FunGetCardReissueData();
                foreach (DataRow item in Dt.Rows)
                {
                    HostModelRequest ObjRequest = new HostModelRequest();
                    ObjRequest.stan = randomno();
                    ObjRequest.cardno = Convert.ToString(item["cardno"]);
                    ObjRequest.CardReissueId = Convert.ToString(item["Id"]);
                    ObjRequest.proessingcode = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_processingcode"]);
                    ObjRequest.IssuerNo = Convert.ToString(item["IssuerNo"]);
                    ObjRequest.HoldRespCode = Convert.ToString(item["HoldRespCode"]);
                    bool IsResult = ISOBuild(ObjRequest);
                    if (IsResult)
                    {
                        bool IsStatusUpdated = new CardReissueDAL().FunUpdateReissueStatus(ObjRequest.CardReissueId, Convert.ToString(item["OldCardRPANID"]));
                    }
                }
            }
            catch(Exception ex)
            {
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            
        }
        private string randomno()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString();
            return r;
        }

        private bool ISOBuild(HostModelRequest ObjRequest)
        {
            
            HostModel rspObj = new HostModel();
            try
            {
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :ISO Request started ", System.Reflection.MethodBase.GetCurrentMethod().Name);
                ISO8583 isoObj = new ISO8583("POSTTERM", Boolean.Parse(ConfigurationManager.AppSettings["HotListCardReissue_trace_write"]));
                string[] rqDE = new string[130], rspDE = new string[130], reDE127 = new string[65];
                string rsp = "", config_data = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_IPPORT"]);
                string time = DateTime.Now.ToString("HHmmss");
                string date = DateTime.Now.ToString("MMdd");
                string _StrTerminalId = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_TID"]);
                string cardAcceptorIdCode = Convert.ToString(ConfigurationManager.AppSettings["HotListCardReissue_CardAcceptorID"]);
                rqDE[2] = String.IsNullOrEmpty(ObjRequest.cardno) ? null : ObjRequest.cardno;
                rqDE[3] = String.IsNullOrEmpty(ObjRequest.proessingcode) ? null : ObjRequest.proessingcode;
                rqDE[7] = String.IsNullOrEmpty(date + time) ? null : date + time;
                rqDE[11] = String.IsNullOrEmpty(ObjRequest.stan) ? null : ObjRequest.stan;
                rqDE[12] = String.IsNullOrEmpty(time) ? null : time;
                rqDE[13] = String.IsNullOrEmpty(date) ? null : date;
                rqDE[22] = "000";
                rqDE[25] = "00";
                rqDE[32] = ObjRequest.IssuerNo;//issuer no 
                rqDE[41] = String.IsNullOrEmpty(_StrTerminalId) ? null : _StrTerminalId;
                rqDE[42] = String.IsNullOrEmpty(cardAcceptorIdCode) ? null : cardAcceptorIdCode;
                rqDE[43] = "Mumbai                   THANE      MHIN";
                rqDE[46] = "3302|"+ObjRequest.HoldRespCode;
                rqDE[129] = "0600";
                string req = isoObj.Build(rqDE, reDE127, rqDE[129]);
                rqDE = isoObj.Parse(req, false);
                byte[] rqBytes = ISGenerixLib.stringToByteArray(req);
                SwitchInterface post_termObj = new SwitchInterface(config_data.Split('|')[0], int.Parse(config_data.Split('|')[1]));
                byte[] rspBytes = post_termObj.sendDataToSwitch(rqBytes);
                if (rspBytes != null)
                {
                    rsp = ISGenerixLib.byteToHex(rspBytes);
                    rspDE = isoObj.Parse(rsp, true);
                    reDE127 = isoObj.getDE127();
                    new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :switch Resp" + rspDE[39].ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (rspDE[39].ToString().Equals("00"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                
                
                
            }
            catch (Exception ex)
            {
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                string strResponse = JsonConvert.SerializeObject(rspObj);
                return false;

            }
        }

    }
}
