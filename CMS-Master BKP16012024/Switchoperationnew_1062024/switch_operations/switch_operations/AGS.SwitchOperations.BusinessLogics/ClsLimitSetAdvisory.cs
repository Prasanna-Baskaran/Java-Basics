using AGS.SwitchOperations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SwitchOperations.DataLogics;

namespace AGS.SwitchOperations.BusinessLogics
{
  public  class ClsLimitSetAdvisory
    {
        public ClsReturnStatusBO FunSaveLimitSetAdvisoryDetails(ClsLimitSetAdvisoryBO ObjFilter)
        {

            
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus = (new ClsLimitSetAdvisoryDAL()).FunSaveLimitSetAdvisoryDetails(ObjFilter);
            return ObjReturnStatus;
        }
    
    }
}
