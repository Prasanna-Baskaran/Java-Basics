using System;
using System.Configuration;
using System.IO;

namespace CustomerDataUpdate
{
	internal class Logger
	{
		public static void WriteLog(string lines, string LogFileName)
		{
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(string.Concat(new string[]
			{
				ConfigurationManager.AppSettings["LogPath"],
				"\\",
				LogFileName,
				System.DateTime.Now.ToString("yyyyMMddHH"),
				".log"
			}), true);
			streamWriter.WriteLine(lines);
			streamWriter.Close();
		}
	}
}
