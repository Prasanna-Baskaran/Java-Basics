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
    public class ClsCIFUpdationBAL
    {/// <summary>
     /// check bank with CIF format is exist or not in tblmasconfiguration table
     /// </summary>
     /// <param name="ObjCIFUpdt"></param>
     /// <returns></returns>
        public string FunIsBankExistWithCIFFormat(ClsCIFUpdationBO ObjCIFUpdt)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsCIFUpdationDAL().FunIsBankExistWithCIFFormat(ObjCIFUpdt);
            return ObjReturnStatus;
        }
        /// <summary>
        /// get CIF format of each bank from tblmasconfiguration table
        /// </summary>
        /// <param name="ObjCIFUpdt"></param>
        /// <returns></returns>
        public DataTable FunGetBankCIFFormat(ClsCIFUpdationBO ObjCIFUpdt)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCIFUpdationDAL().FunGetBankCIFFormat(ObjCIFUpdt);
            return ObjDTOutPut;
        }
        /// <summary>
        /// Add and edit bank with CIF format in tblmasconfiguration table
        /// </summary>
        /// <param name="ObjCIFUpdt"></param>
        /// <returns></returns>
        public string FunAddEditBankCIFData(ClsCIFUpdationBO ObjCIFUpdt)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsCIFUpdationDAL().FunAddEditBankCIFData(ObjCIFUpdt);
            }
            return ObjReturnStatus;
        }
        /// <summary>
        /// Delete bank from tblmasconfiguration table
        /// </summary>
        /// <param name="ObjCIFUpdt"></param>
        /// <returns></returns>
        public string FunDeleteBankForCIF(ClsCIFUpdationBO ObjCIFUpdt)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsCIFUpdationDAL().FunDeleteBankForCIF(ObjCIFUpdt);
            return ObjReturnStatus;
        }

    }
}
