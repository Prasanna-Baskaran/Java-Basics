using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsCardFeeConfigurationBO
    {
        #region Class for Properties required for Card fee Configuration
        public int IssuerNo { get; set; }
        public int SequenceNo { get; set; }
        public string BankStatus { get; set; }
        public string SFTPIncomingFilePath { get; set; }
        public string SFTPOutputFilePath { get; set; }
        public string SFTPRejectedFilePath { get; set; }
        public string SFTPUserName { get; set; }
        public string SFTPPassword { get; set; }
        public string SFTPServerIP { get; set; }
        public int SFTPServerPort { get; set; }

        //
        public string DateCriteria { get; set; }
        public string FileNameFormat { get; set; }
        public int SequenceForCatagory { get; set; }
        public string Status { get; set; }
        public string FileCategory { get; set; }
#endregion
    }
}
