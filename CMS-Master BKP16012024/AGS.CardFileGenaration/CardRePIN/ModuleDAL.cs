using AGS.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    class ModuleDAL
    {
        public DataTable getCardRePINData()
        {
            DataTable __CardRePINData = new DataTable();
            InsertLog(DateTime.Now.ToString() + ">> Message # :SQL conncetion started for the Fatching records ", System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                ObjDic.Add("@Mode", "select");
                ObjDic.Add("@Code", "");
                ObjDic.Add("@CardNO", "");
                ObjDic.Add("@RespCode","");
                __CardRePINData = SQLDAL.ExecuteSQLDataTable(ObjDic, "USP_MakeCardRePINAData", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return __CardRePINData;
            }


            return __CardRePINData;
        }
        public DataTable GetConfiguration()
        {
            DataTable __Configuration = new DataTable();
            InsertLog(DateTime.Now.ToString() + ">> Message # : Message # :SQL conncetion started for the Fatching Mass Config", System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                __Configuration = SQLDAL.ExecuteSQLDataTable(ObjDic, "Usp_getMasConfiguration", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return __Configuration;
            }

            return __Configuration;
        }
        public DataTable InsertRePINData(DataTable CardREPIN, string Filename, int BANK)
        {
            DataTable _dtRejectedRecords = new DataTable();
            InsertLog(DateTime.Now.ToString() + ">> Message # :SQL conncetion started for the Inserting the Records", System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                ObjDic.Add("@DataTable", CardREPIN);
                ObjDic.Add("@Filename", Filename);
                ObjDic.Add("@BANK", BANK);
                _dtRejectedRecords = SQLDAL.ExecuteSQLDataTable(ObjDic, "usp_InsertCardRePINRequest", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return _dtRejectedRecords;
            }

            return _dtRejectedRecords;
        }
        public bool UpdatecardRePINStatus(string code, string cardNo,string respCode)
        {
            InsertLog(DateTime.Now.ToString() + ">> Message # :SQL conncetion started for  the Updating the records ", System.Reflection.MethodBase.GetCurrentMethod().Name);
           
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                ObjDic.Add("@Code", code);
                ObjDic.Add("@cardNO", cardNo);
                ObjDic.Add("@RespCode", respCode);
                ObjDic.Add("@Mode", "Update");
                DataTable dt = AGS.DAL.SQLDAL.ExecuteSQLDataTable(ObjDic, "USP_MakeCardRePINAData", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;

            }
            return false;
        }
        public DataSet getReport(string fileName)
        {
            DataSet __CardRePINData = new DataSet();
            InsertLog(DateTime.Now.ToString() + ">> Message # :SQL conncetion started for  the Fatching records for records", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                ObjDic.Add("@FileName", fileName);
                ObjDic.Add("@DateTime", DateTime.Now.ToString("yyyyMMdd"));
                __CardRePINData = SQLDAL.ExecuteSQLDataSet(ObjDic, "USP_GetProcessedCardRePINAData", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return __CardRePINData;
            }


            return __CardRePINData;
        }
        public DataTable GetEmailConfiguration()
        {
            DataTable __Configuration = new DataTable();
            InsertLog(DateTime.Now.ToString() + ">> Message # :SQL conncetion started for the Fatching Mass Config for Email", System.Reflection.MethodBase.GetCurrentMethod().Name);
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                __Configuration = SQLDAL.ExecuteSQLDataTable(ObjDic, "Usp_getEmailConfiguration", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                InsertLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return __Configuration;
            }

            return __Configuration;
        }
        public static void InsertLog(string exception, string method)
        {
            try
            {
                Dictionary<string, object> ObjDic = new Dictionary<string, object>();
                ObjDic.Add("@Exception", exception);
                ObjDic.Add("@MethodName", method);
                SQLDAL.ExecuteSQLNonQuery(ObjDic, "Usp_InsertLog", Convert.ToString(ConfigurationManager.ConnectionStrings["CardRePIN"]));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(DateTime.Now.ToString() + ">> Message # : " + ex.ToString(), "Log_");
            }
        }
    }
}
