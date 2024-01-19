using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRePIN
{
    class Logger
    {
        #region This Method is used to generate the log file.Create by Gufran Khan
        public static void WriteLog(String lines, string LogFileName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogPath"] + LogFileName + DateTime.Now.ToString("yyyyMMddHH") + ".log", true);
            file.WriteLine(lines);
            file.Close();
        }
        #endregion
    }
}
