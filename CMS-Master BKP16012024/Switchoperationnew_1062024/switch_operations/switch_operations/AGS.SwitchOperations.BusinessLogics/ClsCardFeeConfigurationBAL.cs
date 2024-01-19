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
    public class ClsCardFeeConfigurationBAL
    {
        /// <summary>
        /// get dropdown of issuerno
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetIssuerNo()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardFeeConfigurationDAL().FunGetIssuerNo();
            return ObjDTOutPut;
        }
        /// <summary>
        /// validate bank is exist or not for card fee
        /// </summary>
        /// <param name="ObjCrdFeeCnfg"></param>
        /// <returns></returns>
        public string FunIsBankExistIncardFeeMaster(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsCardFeeConfigurationDAL().FunIsBankExistIncardFeeMaster(ObjCrdFeeCnfg);
            return ObjReturnStatus;
        }
        /// <summary>
        /// get issuer data from cardfee 
        /// </summary>
        /// <param name="ObjCrdFeeCnfg"></param>
        /// <returns></returns>
        public DataTable FunGetIssuerDataForcardFee(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardFeeConfigurationDAL().FunGetIssuerDataForcardFee(ObjCrdFeeCnfg);
            return ObjDTOutPut;
        }
        /// <summary>
        /// add and edit bank data for card fee in CardFeeProcessConfiguration ,CardFeeInitiatorConfiguration table
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>

        public string FunAddEditCardFeeMasterData(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsCardFeeConfigurationDAL().FunAddEditCardFeeMasterData(ObjCrdFeeCnfg);
            }
            return ObjReturnStatus;
        }
        /// <summary>
        /// get drop down of filecategory
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetFileCategory()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsCardFeeConfigurationDAL().FunGetFileCategory();
            return ObjDTOutPut;
        }
        /// <summary>
        /// delete bank from cardfee table
        /// </summary>
        /// <param name="ObjCIFUpdt"></param>
        /// <returns></returns>
        public string FunDeleteBankForCardFee(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsCardFeeConfigurationDAL().FunDeleteBankForCardFee(ObjCrdFeeCnfg);
            return ObjReturnStatus;
        }
    }
}
