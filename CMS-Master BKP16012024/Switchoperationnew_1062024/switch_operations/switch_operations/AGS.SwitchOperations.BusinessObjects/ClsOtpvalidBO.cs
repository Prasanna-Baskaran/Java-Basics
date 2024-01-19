using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsOtpvalidBO
    {
        public ClsOtpvalidBO()
        {
            Createdon = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(0, 10, 0);
            Validtill = Createdon + timeSpan;
        }
        public int Otp { get; set; }
        public string Username { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime Validtill { get; set; }
        public string request { get; set; }
        public int Limits { get; set; }
    }
}
