using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AGS.SwitchOperations.DataLogics;
using AGS.SwitchOperations.BusinessObjects;


namespace AGS.SwitchOperations.BusinessLogics
{
    public class ClsMasterBAL:IDisposable
    {

        public  ClsReturnStatusBO FunSaveMasterDetails(ClsMasterBO ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            if (ObjFilter.IntPara == 0)
            {
                ObjReturnStatus.Description = "Institution information is not saved";
            }
            else if (ObjFilter.IntPara == 1)
            {
                ObjReturnStatus.Description = "Card type is not saved";
            }

            ObjReturnStatus =(new ClsMasterDAL()).FunSaveMasterDetails(ObjFilter);
            
            return ObjReturnStatus;
        }

        //Save Bin Master

        public  ClsReturnStatusBO FunSaveBinDetails(ClsBINDtl ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;

            ObjReturnStatus.Description = "Bin information is not saved";
            ObjReturnStatus = (new ClsMasterDAL()).FunSaveBinDetails(ObjFilter);        

            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunAccept_RejectBin(ClsBINDtl Obj)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            ObjReturnStatus = (new ClsMasterDAL()).FunAccept_RejectBin(Obj);

            return ObjReturnStatus;
        }

        //Save ProductType Master

        public  ClsReturnStatusBO FunSaveProductType(ClsMasterBO ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;

            ObjReturnStatus.Description = "Product type is not saved";
            ObjReturnStatus = (new ClsMasterDAL()).FunSaveProductType(ObjFilter);
            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunAccept_RejectProductType(ClsMasterBO Obj)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            ObjReturnStatus = (new ClsMasterDAL()).FunAccept_RejectProductType(Obj);
            return ObjReturnStatus;
        }

        public DataTable FunGetFileUploadDetails(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            ObjDTOutPut = new ClsMasterDAL().FunGetFileUploadDetails(ObjFilter);
            return ObjDTOutPut;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}