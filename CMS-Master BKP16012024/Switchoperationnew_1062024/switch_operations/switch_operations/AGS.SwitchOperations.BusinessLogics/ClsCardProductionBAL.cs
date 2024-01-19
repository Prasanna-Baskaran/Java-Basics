using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.DataLogics;
using AGS.SwitchOperations.BusinessObjects;
using System.Data.SqlClient;
using AGS.Utilities;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsCardProductionBAL:IDisposable
    {
        public string GetPendingCardDetails()
        {
            using (DataTable dt = (new ClsCardProductionDAL()).GetPendingCardDetails()) {
                return dt.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { control = Controls.Checkbox, hideColumnName = true, index = 0 } });
            }
        }

        public string GetUnauthorisedCardDetails()
        {
            string StrOutput = string.Empty;
            StrOutput = (new ClsCardProductionDAL()).GetUnauthorisedCardDetails();
            return StrOutput;
        }

        public string GetBatchHistory(ClsCardProductionBO objCardProduction)
        {
            string StrOutput = string.Empty;
            StrOutput = (new ClsCardProductionDAL()).GetBatchHistory(objCardProduction);
            return StrOutput;
        }

        public string ProcessPendingCardDetails( ClsCardProductionBO objCardProduction )
        {
            string StrOutput = string.Empty;
            StrOutput = (new ClsCardProductionDAL()).ProcessPendingCardDetails(objCardProduction);
            return StrOutput;
        }

        public string AuthoriseeCardDetails(ClsCardProductionBO objCardProduction)
        {
            string StrOutput = string.Empty;
            StrOutput = (new ClsCardProductionDAL()).AuthoriseeCardDetails(objCardProduction);
            return StrOutput;
        }

        public string DownloadFiles(ClsCardProductionBO objCardProduction)
        {
            string StrOutput = string.Empty;
            StrOutput = (new ClsCardProductionDAL()).DownloadFiles(objCardProduction);
            return StrOutput;
        }
     
        public List<ClsCardProductionBO> GetBatchNos(ClsCardProductionBO objCardProduction)
        {           
            List<ClsCardProductionBO> lstCrdProductionBO = new List<ClsCardProductionBO>();
            lstCrdProductionBO = (new ClsCardProductionDAL()).GetBatchNos(objCardProduction);
            return lstCrdProductionBO;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}