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
    public class ClsBankConfigurationDAL
    {
        

        public string FunConfigureBank(ClsBankConfigureBO ObjBankConfig)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigure",ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@BankName ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankName);
                    ObjCmd.AddParameterWithValue("@BankCode ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.IssuerNo);
                    ObjCmd.AddParameterWithValue("@SystemID ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SystemID);
                    ObjCmd.AddParameterWithValue("@SourceNodes ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SourceNodes);
                    ObjCmd.AddParameterWithValue("@SinkNodes ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SinkNodes);
                    ObjCmd.AddParameterWithValue("@UserPrefix ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.UserPrefix);
                    ObjCmd.AddParameterWithValue("@CustIdentity  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CustIdentity);
                    ObjCmd.AddParameterWithValue("@CustomerIDLen  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CustomerIDLen);

                    //tblcardautomation param
                    ObjCmd.AddParameterWithValue("@BankFolder  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankFolder);
                    ObjCmd.AddParameterWithValue("@SwitchInstitutionID  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SwitchInstitutionID);
                    //ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.Bank);
                    ObjCmd.AddParameterWithValue("@WinSCP_User", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_User);
                    ObjCmd.AddParameterWithValue("@WinSCP_PWD  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_PWD);

                    //newly added params for tblcardautomation and tblcardautofilepath
                    ObjCmd.AddParameterWithValue("@WinSCP_IP  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_IP);
                    ObjCmd.AddParameterWithValue("@WinSCP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_Port);
                    ObjCmd.AddParameterWithValue("@PGP_KeyName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.PGP_KeyName);
                    ObjCmd.AddParameterWithValue("@PGP_PWD", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.PGP_PWD);
                    ObjCmd.AddParameterWithValue("@AGS_PGP_KeyName  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_PGP_KeyName);
                    ObjCmd.AddParameterWithValue("@AGS_PGP_PWD", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_PGP_PWD);
                    ObjCmd.AddParameterWithValue("@RCVREmailID  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.RCVREmailID);
                    //new params
                    ObjCmd.AddParameterWithValue("@AGS_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTPServer);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_User);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_Port);

                    ObjCmd.AddParameterWithValue("@B_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTPServer);
                    ObjCmd.AddParameterWithValue("@B_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_User);
                    ObjCmd.AddParameterWithValue("@B_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@B_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_Port);

                    ObjCmd.AddParameterWithValue("@C_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTPServer);
                    ObjCmd.AddParameterWithValue("@C_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_User);
                    ObjCmd.AddParameterWithValue("@C_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@C_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_Port);



                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunConfigureBank()", Ex.Message, "");

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
        //to delete bank
        public string FunDeleteBank(ClsBankConfigureBO ObjBankConfig)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeleteBank", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@Bankname ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankName);
                    ObjCmd.AddParameterWithValue("@Issuerno ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.IssuerNo);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunDeleteBank()", Ex.Message, "");

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


        //END SHEETAL

        //12-12-17
        // function to check bank exist or not

        public string FunIsBankExist(ClsBankConfigureBO ObjBankConfig)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckIsBankExist", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardPrefix ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CardPrefix);
                    //ObjCmd.AddParameterWithValue("@CardProgram ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CardProgram);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsBankExist()", Ex.Message, "");

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

        //12-12-17
        // function to check bank exist or not

        public string FunIsBankFolderExist(ClsBankConfigureBO ObjBankConfig)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckBankFolderExist", ObjConn, CommandType.StoredProcedure))
                {

                    
                    

                  ObjCmd.AddParameterWithValue("@BankFolder ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankFolder);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsBankExist()", Ex.Message, "");

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
        //to update bank details and validate folder
        public string FunUpdateConfiguredBank(ClsBankConfigureBO ObjBankConfig)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_UpdateConfiguredBank", ObjConn, CommandType.StoredProcedure))
                {




                    //new param require to update
                    ObjCmd.AddParameterWithValue("@BankName ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankName);
                    ObjCmd.AddParameterWithValue("@Issuerno ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.IssuerNo);
                    ObjCmd.AddParameterWithValue("@SystemID ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SystemID);
                    ObjCmd.AddParameterWithValue("@SourceNodes ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SourceNodes);
                    ObjCmd.AddParameterWithValue("@SinkNodes ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SinkNodes);
                    ObjCmd.AddParameterWithValue("@UserPrefix ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.UserPrefix);
                    ObjCmd.AddParameterWithValue("@CustIdentity  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CustIdentity);
                    ObjCmd.AddParameterWithValue("@CustomerIDLen  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.CustomerIDLen);

                    //tblcardautomation param
                    ObjCmd.AddParameterWithValue("@BankFolder ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.BankFolder);
                    ObjCmd.AddParameterWithValue("@SwitchInstitutionID  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.SwitchInstitutionID);
                    //ObjCmd.AddParameterWithValue("@Bank", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.Bank);
                    ObjCmd.AddParameterWithValue("@WinSCP_User", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_User);
                    ObjCmd.AddParameterWithValue("@WinSCP_PWD  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_PWD);

                    //newly added params for tblcardautomation and tblcardautofilepath
                    ObjCmd.AddParameterWithValue("@WinSCP_IP  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_IP);
                    ObjCmd.AddParameterWithValue("@WinSCP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.WinSCP_Port);
                    ObjCmd.AddParameterWithValue("@PGP_KeyName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.PGP_KeyName);
                    ObjCmd.AddParameterWithValue("@PGP_PWD", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.PGP_PWD);
                    ObjCmd.AddParameterWithValue("@AGS_PGP_KeyName  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_PGP_KeyName);
                    ObjCmd.AddParameterWithValue("@AGS_PGP_PWD", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_PGP_PWD);
                    ObjCmd.AddParameterWithValue("@RCVREmailID  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.RCVREmailID);
                    //new params
                    ObjCmd.AddParameterWithValue("@AGS_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTPServer);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_User);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@AGS_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.AGS_SFTP_Port);

                    ObjCmd.AddParameterWithValue("@B_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTPServer);
                    ObjCmd.AddParameterWithValue("@B_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_User);
                    ObjCmd.AddParameterWithValue("@B_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@B_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.B_SFTP_Port);

                    ObjCmd.AddParameterWithValue("@C_SFTPServer", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTPServer);
                    ObjCmd.AddParameterWithValue("@C_SFTP_User  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_User);
                    ObjCmd.AddParameterWithValue("@C_SFTP_Pwd  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_Pwd);
                    ObjCmd.AddParameterWithValue("@C_SFTP_Port  ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankConfig.C_SFTP_Port);
                    //ObjCmd.AddParameterWithValue("@ReturnStatus  ", SqlDbType.Int, 0, ParameterDirection.Input, ObjBankConfig.ReturnStatus);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsBankExist()", Ex.Message, "");

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
        //18-12-17
        //check whether module for thant bank and bank exist or not 
        public string FunIsModuleAndBankExist(BankModuleDataBO ObjBankModule)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckModuleAndBank_Exist", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankModule.IssuerNum);
                    //ObjCmd.AddParameterWithValue("@Modulename ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBankModule.ModuleName);
                    

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsModuleAndBankExist()", Ex.Message, "");

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
        //18-12-17
        //check whether bank,cardprogram and token exist or not in tblprestandard table

        public string FunIsBank_Cardprogram_TokenExist(ClsBankPREStandardBO PREStand)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckBank_Cardprogram_TokenExist", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardProgram ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.CardProgram);
                    //ObjCmd.AddParameterWithValue("@Token ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.Token);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsBank_Cardprogram_TokenExist()", Ex.Message, "");

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

        //check whether bank,cardprogram and cardprefix exist or not in tblbin table

        public string FunIsBankBinExist(ClsBankConfigureBO ObjBank)
        {
            //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            string ObjReturnStatus = string.Empty;
            SqlConnection ObjConn = null;
            DataTable ObjDTOutPut = new DataTable();
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                //using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CardAutomationBankConfigurationNew", ObjConn, CommandType.StoredProcedure))
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_Check_Bank_CardPrefix_CardprogrmExist", ObjConn, CommandType.StoredProcedure))
                {

                    //tblbank params
                    ObjCmd.AddParameterWithValue("@IssuerNo ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardProgram ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardProgram);
                    //ObjCmd.AddParameterWithValue("@CardPrefix ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardPrefix);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }

            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunIsBankBinExist()", Ex.Message, "");

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



        //function to get BankModuledata
        //public DataTable FunSearchBankModuleData(string ModuleName, string IssuerNo)
        public DataTable FunSearchBankModuleData(BankModuleDataBO Obj)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetModuleDataForbank", ObjConn, CommandType.StoredProcedure))
                {

                    //ObjCmd.AddParameterWithValue("@ModuleName", SqlDbType.VarChar, 0, ParameterDirection.Input,ModuleName);
                    //ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, IssuerNo);
                    //ObjCmd.AddParameterWithValue("@ModuleName", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.ModuleName);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Obj.IssuerNum);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunSearchBankModuleData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //function to put BankModuleData
        public string FunPutBankModuleData(BankModuleDataBO Obj)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_InsertModuleDataForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@ModuleName", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.ModuleName);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, Obj.IssuerNum);
                    ObjCmd.AddParameterWithValue("@Frequency ", SqlDbType.Int, 0, ParameterDirection.Input, Obj.Frequency);
                    ObjCmd.AddParameterWithValue("@FrequencyUnit ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.FrequencyUnit);
                    //ObjCmd.AddParameterWithValue("@EnableState ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.EnableState);
                    ObjCmd.AddParameterWithValue("@EnableState ", SqlDbType.Int, 0, ParameterDirection.Input, Obj.EnableState);
                    ObjCmd.AddParameterWithValue("@Status ", SqlDbType.Int, 0, ParameterDirection.Input, Obj.Status);
                    ObjCmd.AddParameterWithValue("@DllPath ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.DllPath);
                    ObjCmd.AddParameterWithValue("@ClassName ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.ClassName);
                    //ObjCmd.AddParameterWithValue("@CreatedBy ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.CreatedBy);
                    //ObjCmd.AddParameterWithValue("@ModifiedBy ", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.ModifiedBy);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunPutBankModuleData()", Ex.Message, "");

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

        //function to get Accounttype whether saving or current
        public DataTable FunGetAccountType()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetAccountType", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetAccountType()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        //function to get getMagstrip for bank
        //4-1-17
        public DataTable FunGetIsMagstrip()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetddlMagstrip", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetIsMagstrip()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //function to get Cardtype for bank
        //12-12-17
        public DataTable FunGetCardType()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCardType", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetCardType()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //12-12-17

        //function to get bankdata

        public DataTable FunGetBankData(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetBankConfigureData", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjBank.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.CardPrefix);
                    //ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.CardProgram);
                    


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetBankData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        //function to get bank PRE standard data
        public DataTable FunGetPREStandardData(ClsBankPREStandardBO PREStand)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetPREStandardDataForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, PREStand.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.CardProgram);
                    //ObjCmd.AddParameterWithValue("@Token", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.Token);
                    
                    

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetPREStandardData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        //function to get bank bin data
        public DataTable FunGetBankBinData(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetBINConfigureData", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input,ObjBank.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardProgram);
                    //ObjCmd.AddParameterWithValue("@CardPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardPrefix);



                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetBankBinData()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        //function to put PREStandard data for bank

        //public string FunPutPREStandardDataForBank(ClsBankPREStandardBO PREStand,string Strtype)
        //{
        //    string _dtReturn = string.Empty;
        //    DataTable ObjDTOutPut = new DataTable();
        //    SqlConnection ObjConn = null;
        //    try
        //    {
        //        ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
        //        using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_PutPREStandardDataForBank", ObjConn, CommandType.StoredProcedure))
        //        {
        //            ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.IssuerNo );
        //            ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.CardProgram);
        //            ObjCmd.AddParameterWithValue("@Token ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.Token);
        //            ObjCmd.AddParameterWithValue("@OutputPosition ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.OutputPosition);
        //            ObjCmd.AddParameterWithValue("@Padding ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.Padding);
        //            ObjCmd.AddParameterWithValue("@FixLength ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.FixLength);
        //            ObjCmd.AddParameterWithValue("@StartPos ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.StartPos);
        //            ObjCmd.AddParameterWithValue("@EndPos ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.EndPos);
        //            ObjCmd.AddParameterWithValue("@Direction ", SqlDbType.VarChar, 0, ParameterDirection.Input,PREStand.Direction);
        //            ObjCmd.AddParameterWithValue("@Mode ", SqlDbType.VarChar, 0, ParameterDirection.Input,Strtype);

        //            ObjDTOutPut = ObjCmd.ExecuteDataTable();
        //            ObjCmd.Dispose();
        //            ObjConn.Close();
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunPutPREStandardDataForBank()", Ex.Message, "");

        //        ObjDTOutPut = new DataTable();
        //    }
        //    finally
        //    {
        //        if (ObjConn.State == ConnectionState.Open)
        //            ObjConn.Close();
        //    }
        //    if (ObjDTOutPut.Rows.Count > 0)
        //    {
        //        _dtReturn = Convert.ToString(ObjDTOutPut.Rows[0]["StatusMessage"]);
        //    }
        //    return _dtReturn;
        //}

        public string FunPutPREStandardDataForBank(ClsBankPREStandardBO PREStand)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_PutPREStandardDataForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.CardProgram);
                    ObjCmd.AddParameterWithValue("@Token ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.Token);
                    ObjCmd.AddParameterWithValue("@OutputPosition ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.OutputPosition);
                    ObjCmd.AddParameterWithValue("@Padding ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.Padding);
                    ObjCmd.AddParameterWithValue("@FixLength ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.FixLength);
                    ObjCmd.AddParameterWithValue("@StartPos ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.StartPos);
                    ObjCmd.AddParameterWithValue("@EndPos ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.EndPos);
                    ObjCmd.AddParameterWithValue("@Direction ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.Direction);
                    //ObjCmd.AddParameterWithValue("@Mode ", SqlDbType.VarChar, 0, ParameterDirection.Input, Strtype);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunPutPREStandardDataForBank()", Ex.Message, "");

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
        //to delete PRErecord
        public string FunDeletePRERecordForBank(ClsBankPREStandardBO PREStand)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeletePRERecordForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.CardProgram);
                    ObjCmd.AddParameterWithValue("@Token ", SqlDbType.VarChar, 0, ParameterDirection.Input, PREStand.Token);
                    //ObjCmd.AddParameterWithValue("@Mode ", SqlDbType.VarChar, 0, ParameterDirection.Input, Strtype);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunDeletePRERecordForBank()", Ex.Message, "");

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

        //to delete Modulerecord
        public string FunDeleteModuleRecordForBank(BankModuleDataBO Obj)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeleteModuleRecordForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, Obj.IssuerNum);
                    ObjCmd.AddParameterWithValue("@ModuleName", SqlDbType.VarChar, 0, ParameterDirection.Input,Obj.ModuleName);
                    
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunDeleteModuleRecordForBank()", Ex.Message, "");

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

        //to delete Bin record
        public string FunDeleteBINRecordForBank(ClsBankConfigureBO ObjBank)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_DeleteBINRecordForBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardProgram);
                    ObjCmd.AddParameterWithValue("@CardPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardPrefix);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunDeleteBINRecordForBank()", Ex.Message, "");

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

        public string FunPutBinDataForBank(ClsBankConfigureBO ObjBank)
        {
            string _dtReturn = string.Empty;
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_PutBinDetailsOfBank", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.IssuerNo);
                    ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardProgram);
                    ObjCmd.AddParameterWithValue("@CardPrefix ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardPrefix);
                    ObjCmd.AddParameterWithValue("@InstitutionID ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.SwitchInstitutionID);
                    ObjCmd.AddParameterWithValue("@AccountType ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.AccountType);
                    ObjCmd.AddParameterWithValue("@CardType ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.CardType);
                    ObjCmd.AddParameterWithValue("@BinDesc ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.BinDesc);
                    ObjCmd.AddParameterWithValue("@Currency ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.Currency);
                    ObjCmd.AddParameterWithValue("@Switch_CardType ", SqlDbType.VarChar, 0, ParameterDirection.Input,ObjBank.SwitchCardType);
                    ObjCmd.AddParameterWithValue("@IsMagstrip ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.IsMagstrip);
                    ObjCmd.AddParameterWithValue("@PREFormat ", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.PREFormat);
                    //ObjCmd.AddParameterWithValue("@Mode ", SqlDbType.VarChar, 0, ParameterDirection.Input, Strtype);

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunPutBinDataForBank()", Ex.Message, "");

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

        //function to get system id from dropdown for bank
        public DataTable FunGetSystemId()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetSystemId", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetSystemId()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //function to get dropdown for frequency
        public DataTable FunGetFrequency()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetFrequencyForBankModule", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetFrequency()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }


        //function to get dropdown for frequencyunit
        public DataTable FunGetFrequencyUnit()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetFrequencyUnitForBankModule", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetFrequencyUnit()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        //function to get dropdown for Enable State
        public DataTable FunGetEnableState()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetEnablStateForBankModule", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetEnableState()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //13-12-17
        //function to get status for bank module
        public DataTable FunGetStatus()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetStatus", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetStatus()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }


        //function to get bankfolder

        public DataTable FunGetBankFolder(ClsBankConfigureBO ObjBank)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetBankFolderName", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, ObjBank.IssuerNo);
                    //ObjCmd.AddParameterWithValue("@CardPrefix", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.CardPrefix);
                    //ObjCmd.AddParameterWithValue("@CardProgram", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjBank.CardProgram);
                    


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ObjExc)
            {
                ObjDTOutPut = new DataTable();
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetBankFolder()", ObjExc.Message, "");
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }
        //get dropdown for Pre file element padding
        public DataTable FunGetPREPadding()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetpaddingForPRE", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetPREPadding()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }

        //get dropdown for predirection
        public DataTable FunGetPREDirection()
        {

            DataTable ObjDTOutPut = new DataTable();

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetDirectionForPRE", ObjConn, CommandType.StoredProcedure))
                {


                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }

            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsBankConfigurationDAL, FunGetPREDirection()", Ex.Message, "");
                ObjDTOutPut = new DataTable();
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
