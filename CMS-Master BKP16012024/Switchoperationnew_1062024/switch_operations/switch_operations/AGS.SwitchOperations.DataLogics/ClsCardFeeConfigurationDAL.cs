using AGS.SqlClient;
using AGS.SwitchOperations.BusinessObjects;
using AGS.SwitchOperations.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsCardFeeConfigurationDAL
    {

        //to dropdown of bank to get issuerno
        public DataTable FunGetIssuerNo()
        {
            #region 
            
            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetIssuerNoForCardFee", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunGetIssuerNo()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
            #endregion
        }

        //check issuer exist or not in Card fee Mastertable
        public string FunIsBankExistIncardFeeMaster(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {

            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckIsIssuerExistForCardFee", ObjConn, CommandType.StoredProcedure))
                {

                    
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileCategory ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.FileCategory);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunIsBankExistIncardFeeMaster()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                ObjReturnStatus = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return ObjReturnStatus;
            

        }
//get issuer data from cardfee master
        public DataTable FunGetIssuerDataForcardFee(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetIssuerDataForcardFee", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileCategory", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.FileCategory);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunGetIssuerDataForcardFee()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //add edit card fee data
        public string FunAddEditCardFeeMasterData(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_AddEditCardFeeMasterData", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.IssuerNo);
                    ObjCmd.AddParameterWithValue("@Sequence", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SequenceNo);
                    ObjCmd.AddParameterWithValue("@IsActive ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.BankStatus);
                    ObjCmd.AddParameterWithValue("@SFTPIncomingFilePath ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPIncomingFilePath);
                    //ObjCmd.AddParameterWithValue("@EnableState ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.EnableState);
                    ObjCmd.AddParameterWithValue("@SFTPOutputFilePath ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPOutputFilePath);
                    ObjCmd.AddParameterWithValue("@SFTPRejectedFilePath ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPRejectedFilePath);
                    ObjCmd.AddParameterWithValue("@Username ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPUserName);
                    ObjCmd.AddParameterWithValue("@password ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPPassword);
                    ObjCmd.AddParameterWithValue("@ServerPort ", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPServerPort);
                    ObjCmd.AddParameterWithValue("@serverIP ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SFTPServerIP);
                    //
                    ObjCmd.AddParameterWithValue("@DateCriteria ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.DateCriteria);
                    ObjCmd.AddParameterWithValue("@FileNameFormat ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.FileNameFormat);
                    ObjCmd.AddParameterWithValue("@SequenceByCategory ", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.SequenceForCatagory);
                    ObjCmd.AddParameterWithValue("@Status ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.Status);
                    ObjCmd.AddParameterWithValue("@FileCategory ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.FileCategory);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunAddEditCardFeeMasterData()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return _dtReturn;
        }
        //get filecategory
        public DataTable FunGetFileCategory()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_FileCategory", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunGetFileCategory()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        public string FunDeleteBankForCardFee(ClsCardFeeConfigurationBO ObjCrdFeeCnfg)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeleterecordFromCardFee", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCrdFeeCnfg.IssuerNo);
                    ObjCmd.AddParameterWithValue("@FileCategory ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCrdFeeCnfg.FileCategory);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCardFeeConfigurationDAL, FunDeleteBankForCardFee()", Ex.Message, "");

                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            if (ObjDTOutPut.Rows.Count > 0)
            {
                _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
            }
            return _dtReturn;
        }

    }
}
