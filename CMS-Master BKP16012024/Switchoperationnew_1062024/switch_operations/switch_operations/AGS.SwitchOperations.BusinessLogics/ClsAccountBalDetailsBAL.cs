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
    public class ClsAccountBalDetailsBAL
    {
        public DataTable FunSearchAccountBalanceDetails(ClsAccountBalanceBO ObjAccBal)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsAccountBalDetailsDAL().FunSearchAccountBalanceDetails(ObjAccBal);
            return ObjDTOutPut;
        }
    }
}
