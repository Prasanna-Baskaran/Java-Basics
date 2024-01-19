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
    public class ClsBankConfigurationBAL
    {
        //start sheetal
        // public ClsReturnStatusBO FunConfigureBank(ClsBankConfigureBO Obj)
        //{
        //     //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
        //     return (new ClsBankConfigurationDAL()).FunConfigureBank(Obj);
        //     //return ObjReturnStatus;
        // }
        /// <summary>
        /// To configure now bank
        /// </summary>
        /// <param name="ObjBankConfig"></param>
        /// <returns></returns>
        public string FunConfigureBank(ClsBankConfigureBO ObjBankConfig)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunConfigureBank(ObjBankConfig);
            return ObjReturnStatus;
        }
        /// <summary>
        /// To delete configured bank
        /// </summary>
        /// <param name="ObjBankConfig"></param>
        /// <returns></returns>
        //
        public string FunDeleteBank(ClsBankConfigureBO ObjBankConfig)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunDeleteBank(ObjBankConfig);
            return ObjReturnStatus;
        }
        /// <summary>
        ///  To check bank  exist or not in tblbanks table
        /// </summary>
        /// <param name="ObjBankConfig"></param>
        /// <returns></returns>

        public string FunIsBankExist(ClsBankConfigureBO ObjBankConfig)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunIsBankExist(ObjBankConfig);
            return ObjReturnStatus;
        }

        /// <summary>
        /// To check bankFolder  exist or not for other banks
        /// </summary>
        /// <param name="ObjBankConfig"></param>
        /// <returns></returns>

        public string FunIsBankFolderExist(ClsBankConfigureBO ObjBankConfig)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunIsBankFolderExist(ObjBankConfig);
            return ObjReturnStatus;
        }
        /// <summary>
        /// To update bank data
        /// </summary>
        /// <param name="ObjBankConfig"></param>
        /// <returns></returns>
        public string FunUpdateConfiguredBank(ClsBankConfigureBO ObjBankConfig)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunUpdateConfiguredBank(ObjBankConfig);
            return ObjReturnStatus;
        }

        /// <summary>
        /// Check whether modules for thant bank exist or not in tblmassmodule table
        /// </summary>
        /// <param name="ObjBankModule"></param>
        /// <returns></returns>
        public string FunIsModuleAndBankExist(BankModuleDataBO ObjBankModule)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunIsModuleAndBankExist(ObjBankModule);
            return ObjReturnStatus;
        }
        /// <summary>
        /// Check PREstandard data exist or not for respective bank and cardprogram in tblprestandard table
        /// </summary>
        /// <param name="PREStand"></param>
        /// <returns></returns>
        public string FunIsBank_Cardprogram_TokenExist(ClsBankPREStandardBO PREStand)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunIsBank_Cardprogram_TokenExist(PREStand);
            return ObjReturnStatus;
        }
        /// <summary>
        /// Check whether (bins)cardprogram  exist or not for respective  bank in tblbin table
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public string FunIsBankBinExist(ClsBankConfigureBO ObjBank)
        {
            string ObjReturnStatus = string.Empty;
            ObjReturnStatus = new ClsBankConfigurationDAL().FunIsBankBinExist(ObjBank);
            return ObjReturnStatus;
        }


        /// <summary>
        /// To get Bank module data from tblmassmodule table
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        
        public DataTable FunSearchBankModuleData(BankModuleDataBO Obj)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunSearchBankModuleData(Obj);
            return ObjDTOutPut;
        }

        /// <summary>
        /// To put BanK ModuleData in tblmassmodule table
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>

        public string FunPutBankModuleData(BankModuleDataBO Obj)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunPutBankModuleData(Obj);
            }
            return ObjReturnStatus;
        }

        /// <summary>
        /// To give Accounttype whether saving or current
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetAccountType()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetAccountType();
            return ObjDTOutPut;
        }



        /// <summary>
        /// To get Cardtype is it Debit,Remittance,Credit
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetCardType()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetCardType();
            return ObjDTOutPut;
        }

        /// <summary>
        /// to get card type whether IsMagstrip or EMV
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetIsMagstrip()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetIsMagstrip();
            return ObjDTOutPut;
        }

        /// <summary>
        /// To get bank data from tblbanks table
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public DataTable FunGetBankData(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetBankData(ObjBank);
            return ObjDTOutPut;
        }
        /// <summary>
        /// To get Bank PREStandard Data from tblprestandard table
        /// </summary>
        /// <param name="PREStand"></param>
        /// <returns></returns>
        public DataTable FunGetPREStandardData(ClsBankPREStandardBO PREStand)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetPREStandardData(PREStand);
            return ObjDTOutPut;
        }
        /// <summary>
        /// To get Bank BIN Data from tblbin table
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public DataTable FunGetBankBinData(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetBankBinData(ObjBank);
            return ObjDTOutPut;
        }

        /// <summary>
        /// To put PREStandardData in tblprestandard table for bank and respective cardprogram
        /// </summary>
        /// <param name="PREStand"></param>
        /// <returns></returns>
        public string FunPutPREStandardDataForBank(ClsBankPREStandardBO PREStand)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunPutPREStandardDataForBank(PREStand);
            }
            return ObjReturnStatus;
        }
        /// <summary>
        /// To delete PRERecords
        /// </summary>
        /// <param name="PREStand"></param>
        /// <returns></returns>
        public string FunDeletePRERecordForBank(ClsBankPREStandardBO PREStand)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunDeletePRERecordForBank(PREStand);
            }
            return ObjReturnStatus;
        }

        /// <summary>
        /// To delete ModuleRecords
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string FunDeleteModuleRecordForBank(BankModuleDataBO Obj)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunDeleteModuleRecordForBank(Obj);
            }
            return ObjReturnStatus;
        }

        /// <summary>
        /// To delete BIN Records
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public string FunDeleteBINRecordForBank(ClsBankConfigureBO ObjBank)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunDeleteBINRecordForBank(ObjBank);
            }
            return ObjReturnStatus;
        }

        /// <summary>
        /// To put bin details for bank
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public string FunPutBinDataForBank(ClsBankConfigureBO ObjBank)
        {
            string ObjReturnStatus = string.Empty;
            {
                ObjReturnStatus = new ClsBankConfigurationDAL().FunPutBinDataForBank(ObjBank);
            }
            return ObjReturnStatus;
        }
        /// <summary>
        /// To get systemid for bank whether system is for remittance,credit,debit
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetSystemId()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetSystemId();
            return ObjDTOutPut;
        }
        /// <summary>
        /// To get dropdown for frequency 
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetFrequency()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetFrequency();
            return ObjDTOutPut;
        }
        /// <summary>
        ///  To get dropdown for frequencyunit(Time period to execute the service or load the dll)
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetFrequencyUnit()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetFrequencyUnit();
            return ObjDTOutPut;
        }
        /// <summary>
        /// To get dropdown for enable state(Should be true to execute the service which is running)
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetEnableState()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetEnableState();
            return ObjDTOutPut;
        }

        /// <summary>
        /// to get status for bankmodule(Status=1start service,2=runnig,3=error in dll load)
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetStatus()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetStatus();
            return ObjDTOutPut;
        }

        /// <summary>
        /// To get Bankfolder
        /// </summary>
        /// <param name="ObjBank"></param>
        /// <returns></returns>
        public DataTable FunGetBankFolder(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetBankFolder(ObjBank);
            return ObjDTOutPut;
        }

        /// <summary>
        /// To get dropdown for prefile element Padding
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetPREPadding()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetPREPadding();
            return ObjDTOutPut;
        }
        /// <summary>
        /// To get dropdown for PreDirection
        /// </summary>
        /// <returns></returns>
        public DataTable FunGetPREDirection()
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsBankConfigurationDAL().FunGetPREDirection();
            return ObjDTOutPut;
        }
    }
}
