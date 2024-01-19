using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.Common
{
    public class SFTPDetails
    {

        public string Bank_SFTPServer { get; set; }
        public int Bank_SFTPPort { get; set; }
        public string Bank_SFTPUser { get; set; }
        public string Bank_SFTPPassword { get; set; }
        public string Bank_Input { get; set; }
         
        public string DestServerSSH_Private_key_file_Path { get; set; }
        public string DestServerpassphrase { get; set; }

        public string SwitchPortal_Local_Input { get; set; }
        public string SwitchPortal_Local_Archive { get; set; }

        public string ErrorlogPath { get; set; } //USED FOR DELETE THE FILE FROM SFTP
        public string FileName { get; set; }

    }
}
