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
    public class ClsCardCheckerDetailsBAL
    {
        //START SHEETAL
        public DataTable FunGetProcessType()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardCheckerDetailsDAL().FunGetProcessType();
            return ObjDTOutPut;
        }

        public DataTable FunGetChecker(ClsCardCheckerDetailsBO ObjChecker, string StrType)
        {
            DataTable ObjDTOutPut = null;
            if (StrType == "CardReissue")
            {
                ObjDTOutPut = new ClsCardCheckerDetailsDAL().FunGetChecker(ObjChecker);
            }
            else
                if (StrType == "Renew")
                {
                    ObjDTOutPut = new ClsCardCheckerDetailsDAL().FunGetChecker(ObjChecker);
                }
            return ObjDTOutPut;
        }

        public bool FunUpdateChecker(ClsCardCheckerDetailsBO ObjChecker, string StrType)
        {
            bool IsSuccess = false;
            if (StrType == "CardReissue")
            {
                IsSuccess = new ClsCardCheckerDetailsDAL().FunUpdateChecker(ObjChecker);
            }
            return IsSuccess;
        }


        public DataTable ValidateReissueCardRequest(ProcessCardReissueRequest ObjChecker)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardCheckerDetailsDAL().ValidateReissueCardRequest(ObjChecker);
            return ObjDTOutPut;
        }
    }
}
