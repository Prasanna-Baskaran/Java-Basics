using AGS.SwitchOperations.DataLogics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SwitchOperations.BusinessObjects;
namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsGeneratePrepaidCardsBAL
    {
        
        /// <summary>
        /// to get card program of bank
        /// </summary>
        /// <param name="BankName"></param>
        /// <returns></returns>
        public DataSet GeneratePrepaidCard_Operations(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().GeneratePrepaidCard_Operations(ObjPrepaid);
            return ObjDTOutPut;
        }

        
        public DataSet PinReissuanceRequest(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().PinReissuanceRequest(ObjPrepaid);
            return ObjDTOutPut;
        }
        public DataSet AcceptRejectInstaCards(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().AcceptRejectInstaCards(ObjPrepaid);
            return ObjDTOutPut;
        }
        public DataSet GetSetAccountLinkrequest(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().GetSetAccountLinkrequest(ObjPrepaid);
            return ObjDTOutPut;
        }

        public DataSet GetSetAccountLinkrequestOperations(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().GetSetAccountLinkrequestOperations(ObjPrepaid);
            return ObjDTOutPut;
        }
        public DataSet AcceptRejectAccountlink(ClsGeneratePrepaidCardBO ObjPrepaid)
        {
            DataSet ObjDTOutPut = new DataSet();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().AcceptRejectAccountlink(ObjPrepaid);
            return ObjDTOutPut;
        }

        /// <summary>
        /// get status of file
        /// </summary>
        /// <param name="BankName"></param>
        /// <param name="Mode"></param>
        /// <param name="LastCIF_ID"></param>
        /// <param name="CardProgram"></param>
        /// <param name="LastAccountNo"></param>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public DataTable FunGetPrepaidCIFFileDetails(int IssuerNo, string Filename,string Mode)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsGeneratePrepaidCardsDAL().FunGetPrepaidCIFFileDetails(IssuerNo, Filename, Mode);
            return ObjDTOutPut;
        }
    }
}
