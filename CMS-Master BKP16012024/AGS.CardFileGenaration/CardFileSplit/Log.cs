using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
namespace CardFileSplit
{
    public class Log
    {
        public String FileNameformat = ConfigurationManager.AppSettings["LogShirpurCardNewFile"];
        public String FolderPath = ConfigurationManager.AppSettings["LogShirpurCardNewPath"];

        public Log(String IssuerNo)
        {
            FolderPath = FolderPath + IssuerNo+"\\";
            FileNameformat = FileNameformat + IssuerNo+"_";
        }
        public void WriteLog(string Message)
        {
            Message = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:fff") + "::" + Message;
            try
            {
                if (!string.IsNullOrEmpty(FolderPath))
                {
                    if (!Directory.Exists(FolderPath))
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    if (!File.Exists(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt"))
                    {

                        FileStream fs = new FileStream(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt", FileMode.OpenOrCreate);
                        fs.Close();
                    }
                    StreamWriter sr = new StreamWriter(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt", true);
                    sr.WriteLine(Message);
                    sr.Flush();
                    sr.Close();
 

                }

            }
            catch (Exception Ex)
            {
                WriteLogException(Ex.Message.ToString());            
            }





        }


        public void WriteLogException(string Message)
        {
            Message = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:fff") + "::" + Message;
             FileNameformat = FileNameformat + "Exception_";
             FolderPath = FolderPath + "Exception\\";
            try
            {
                if (!string.IsNullOrEmpty(FolderPath))
                {
                    if (!Directory.Exists(FolderPath))
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    if (!File.Exists(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt"))
                    {
                        FileStream fs = new FileStream(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt", FileMode.OpenOrCreate);
                        fs.Close();
                    }
                    StreamWriter sr = new StreamWriter(FolderPath + FileNameformat + DateTime.Now.ToString("dd-MMM-yyyy_HH") + ".txt", true);
                    sr.WriteLine(Message);
                    sr.Flush();
                    sr.Close();

                }

            }
            catch (Exception Ex)
            {
              
            }

        }
    }
}
