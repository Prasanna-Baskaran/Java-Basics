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
    public class ClsCIFUpdationDAL
    {
        public string FunIsBankExistWithCIFFormat(ClsCIFUpdationBO ObjCIFUpdt)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckIsBankExistWithCIFFormat", ObjConn, CommandType.StoredProcedure))
                {

                    
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.Int, 0, ParameterDirection.Input,ObjCIFUpdt.IssuerNo);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCIFUpdationDAL, FunIsBankExistWithCIFFormat()", Ex.Message, "");

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

        public DataTable FunGetBankCIFFormat(ClsCIFUpdationBO ObjCIFUpdt)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetBankCIFFormat", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCIFUpdt.IssuerNo);
                    


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCIFUpdationDAL, FunGetBankCIFFormat()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public string FunAddEditBankCIFData(ClsCIFUpdationBO ObjCIFUpdt)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_AddEditBankCIFData", ObjConn, CommandType.StoredProcedure))
                {
                    
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjCIFUpdt.IssuerNo);
                    ObjCmd.AddParameterWithValue("@ServerIp", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.ServerIp);
                    ObjCmd.AddParameterWithValue("@ServerPort", SqlDbType.Int, 0, ParameterDirection.Input, ObjCIFUpdt.ServerPort);
                    ObjCmd.AddParameterWithValue("@FilePathInput", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathInput);
                    ObjCmd.AddParameterWithValue("@FilePathOutPut", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathOutPut);
                    ObjCmd.AddParameterWithValue("@FilePathArchive", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathArchive);
                    ObjCmd.AddParameterWithValue("@Username", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.Username);
                    ObjCmd.AddParameterWithValue("@password", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.password);
                    ObjCmd.AddParameterWithValue("@Enable", SqlDbType.Bit, 0, ParameterDirection.Input, ObjCIFUpdt.EnableState);
                    ObjCmd.AddParameterWithValue("@filepath", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.filepath);
                    ObjCmd.AddParameterWithValue("@FileHeader", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FileHeader);
                    ObjCmd.AddParameterWithValue("@FilePathInput_RePIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathInput_RePIN);
                    ObjCmd.AddParameterWithValue("@FilePathOutPut_RePIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathOutPut_RePIN);
                    ObjCmd.AddParameterWithValue("@FilePathArchive_RePIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePathArchive_RePIN);
                    ObjCmd.AddParameterWithValue("@FilePath_RePIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.FilePath_RePIN);
                    ObjCmd.AddParameterWithValue("@fileHeader_RePIN", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.fileHeader_RePIN);
                    ObjCmd.AddParameterWithValue("@IsPGP", SqlDbType.Bit, 0, ParameterDirection.Input, ObjCIFUpdt.IsPGP);
                    ObjCmd.AddParameterWithValue("@Trace", SqlDbType.Bit, 0, ParameterDirection.Input, ObjCIFUpdt.Trace);
                    ObjCmd.AddParameterWithValue("@PGPPublicKeyFilePath", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.PublicKeyFilePath);
                    ObjCmd.AddParameterWithValue("@PGPPrivateKeyFilePath", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.PrivateKeyFilePath);
                    ObjCmd.AddParameterWithValue("@PGPPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.Password_PGP);
                    ObjCmd.AddParameterWithValue("@PGPInputFilePath", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCIFUpdt.InputFilePath_PGP);
                    //ObjCmd.AddParameterWithValue("@Trace ", SqlDbType.Bit, 0, ParameterDirection.Input, Obj.EnableState);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCIFUpdationDAL, FunAddEditBankCIFData()", Ex.Message, "");

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


        public string FunDeleteBankForCIF(ClsCIFUpdationBO ObjCIFUpdt)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeleteBankForCIF", ObjConn, CommandType.StoredProcedure))
                {

                   
                   
                    ObjCmd.AddParameterWithValue("@Issuerno", SqlDbType.Int, 0, ParameterDirection.Input, ObjCIFUpdt.IssuerNo);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCIFUpdationDAL, FunDeleteBankForCIF()", Ex.Message, "");

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

    }
}
