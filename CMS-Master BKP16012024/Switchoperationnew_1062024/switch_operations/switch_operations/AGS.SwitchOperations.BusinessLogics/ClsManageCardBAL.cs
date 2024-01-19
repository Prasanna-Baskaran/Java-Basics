using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.DataLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsManageCardBAL
    {
        public DataTable FunGetNewBINPrefix(string BankID)
        {

            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsManageCardDAL().FunGetNewBINPrefix(BankID);
            return ObjDTOutPut;
        }

        public string FunOldCardRPANIDExist(ClsCardCheckerDetailsBO ObjCardcheck, string StrType)
        {
            string ObjReturnStatus = string.Empty;
            // ObjReturnStatus.Code = 1;
            //ObjReturnStatus.Description = "Failed";
            if (StrType == "CardReissue")
            {
                ObjReturnStatus = new ClsManageCardDAL().FunOldCardRPANIDExist(ObjCardcheck);
            }
            return ObjReturnStatus;
        }

        public bool FunSaveCard(ClsCardCheckerDetailsBO ObjCardcheck, string StrType)
        {
            bool ObjReturnStatus = false;
            // ObjReturnStatus.Code = 1;
            //ObjReturnStatus.Description = "Failed";
            if (StrType == "CardReissue")
            {
                ObjReturnStatus = new ClsManageCardDAL().FunSaveCard(ObjCardcheck);
            }
            return ObjReturnStatus;
        }
        //updation for account link
        public string FunUpdateCarddetails(ClsAccountLinkingBO ObjAcclink)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsManageCardDAL().FunUpdateCarddetails(ObjAcclink);
            }
            return ObjReturnStatus;
        }

        public string FunValidateReissueCardRequest(ClsCardCheckerDetailsBO ObjCardcheck)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsManageCardDAL().FunValidateReissueCardRequest(ObjCardcheck);
            return ObjReturnStatus;
        }

    }
}
