using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AGS.Configuration;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SqlClient;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsMasterDAL
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


            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_SaveMasterDtl", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();

                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    ObjCmd.AddParameterWithValue("@Code", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.InstitutionID);
                    ObjCmd.AddParameterWithValue("@Desc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.INSTDesc);
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        if (ObjFilter.IntPara == 0)
                        {
                            ObjReturnStatus.Description = "Institution information is not saved";
                        }
                        else if (ObjFilter.IntPara == 1)
                        {
                            ObjReturnStatus.Description = "Card type is not saved";
                        }

                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunSaveMasterDetails()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                if (ObjFilter.IntPara == 0)
                {
                    ObjReturnStatus.Description = "Institution information is not saved";
                }
                else if (ObjFilter.IntPara == 1)
                {
                    ObjReturnStatus.Description = "Card type is not saved";
                }

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        //Save Bin Master

        public  ClsReturnStatusBO FunSaveBinDetails(ClsBINDtl ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;

            ObjReturnStatus.Description = "Bin information is not saved";


            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_InsertUpdateBin", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    ObjCmd.AddParameterWithValue("@BIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BIN);
                    ObjCmd.AddParameterWithValue("@INSTID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.INSTID);
                    ObjCmd.AddParameterWithValue("@BINDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BinDesc);
                    ObjCmd.AddParameterWithValue("@MakerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.MakerID);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Bin information is not saved";
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunSaveBinDetails()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Bin information is not saved";

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunAccept_RejectBin(ClsBINDtl Obj)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_ApproveRejectBIN", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@ID", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.ID);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.CheckerID);
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, Obj.FormStatusID);

                    if (!string.IsNullOrEmpty(Obj.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.Remark);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        if (Obj.FormStatusID == 1)
                        {
                            ObjReturnStatus.Description = "Bin is not approved";
                        }
                        else
                        {
                            ObjReturnStatus.Description = "Bin is not rejected";
                        }
                    }
                }
            }
            catch (Exception ObjExc)
            {
                ObjReturnStatus.Code = 1;

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunAccept_RejectBin()", ObjExc.Message, "");
                if (Obj.FormStatusID == 1)
                {
                    ObjReturnStatus.Description = "Bin is not approved";
                }
                else
                {
                    ObjReturnStatus.Description = "Bin is not rejected";
                }
                //ObjReturnStatus.Description = ObjExc.Message;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        //Save ProductType Master

        public  ClsReturnStatusBO FunSaveProductType(ClsMasterBO ObjFilter)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;

            ObjReturnStatus.Description = "Product type is not saved";


            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_InsertUpdateProductType", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    ObjCmd.AddParameterWithValue("@BIN_ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.BinID);
                    ObjCmd.AddParameterWithValue("@INST_ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.INSTID);
                    ObjCmd.AddParameterWithValue("@CardType_ID", SqlDbType.Int, 0, ParameterDirection.Input, ObjFilter.CardTypeID);
                    ObjCmd.AddParameterWithValue("@ProductType", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ProductType);
                    ObjCmd.AddParameterWithValue("@ProductTypeDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ProductTypeDesc);
                    ObjCmd.AddParameterWithValue("@MakerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.MakerID);
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Product type is not saved";
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunSaveProductType()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Product type is not saved";

            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunAccept_RejectProductType(ClsMasterBO Obj)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_ApproveRejectProductType", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@ID", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.ID);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.CheckerID);
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, Obj.FormStatusID);

                    if (!string.IsNullOrEmpty(Obj.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.Remark);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        if (Obj.FormStatusID == 1)
                        {
                            ObjReturnStatus.Description = "Product type is not approved";
                        }
                        else
                        {
                            ObjReturnStatus.Description = "Product type is not rejected";
                        }
                    }
                }
            }
            catch (Exception ObjExc)
            {
                ObjReturnStatus.Code = 1;

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsMasterDAL, FunAccept_RejectProductType()", ObjExc.Message, "");
                if (Obj.FormStatusID == 1)
                {
                    ObjReturnStatus.Description = "Product type is not approved";
                }
                else
                {
                    ObjReturnStatus.Description = "Product type is not rejected";
                }
                //ObjReturnStatus.Description = ObjExc.Message;
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        // get bulk upload file status
        public DataTable FunGetFileUploadDetails(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_GetBulkUploadFileStatus", ObjConn, CommandType.StoredProcedure))
                {
                    
                    if (ObjFilter.CreatedDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@UploadDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.CreatedDate);
                    }                  
                  
                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, Convert.ToInt16(ObjFilter.SystemID));
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    }
                   
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardMasterDAL, FunGetCardRequestByID()", Ex.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
    }
}