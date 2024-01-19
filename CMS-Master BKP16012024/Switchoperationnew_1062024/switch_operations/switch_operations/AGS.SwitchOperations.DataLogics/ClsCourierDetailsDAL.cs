﻿using System;
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
    public class ClsCourierDetailsDAL
    {

        public  ClsReturnStatusBO FunInsertIntoCourierDetails(string couriername, string officename,  string mobileno, int status, int createdby,string SystemID,string BankID)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpSetCourierDetails", ObjConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@CourierName", SqlDbType.VarChar, 0, ParameterDirection.Input, couriername);
                    sspObj.AddParameterWithValue("@OfficeName", SqlDbType.VarChar, 0, ParameterDirection.Input, officename);
                    sspObj.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, mobileno);
                    sspObj.AddParameterWithValue("@Status", SqlDbType.Int, 0, ParameterDirection.Input, status);
                    sspObj.AddParameterWithValue("@CreatedBy", SqlDbType.Int, 0, ParameterDirection.Input, createdby);
                    if (!string.IsNullOrEmpty(SystemID))
                    {
                        sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID.Trim());
                    }
                    if (!string.IsNullOrEmpty(BankID))
                    {
                        sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID.Trim());
                    }
                    DtinsertInstallOutput = sspObj.ExecuteDataTable();
                    sspObj.Dispose();

                    if (DtinsertInstallOutput.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(DtinsertInstallOutput.Rows[0]["Code"]);
                        ObjReturnStatus.Description = DtinsertInstallOutput.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Addition failed !";
                    }
                }
            }
            catch (Exception xObj)
            {
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = xObj.Message;
            }
            finally
            {
                ObjConn.Close();
            }
            return ObjReturnStatus;
        }


        public  ClsReturnStatusBO FunUpdateIntoCourierDetails(string couriername, string officename, string mobileno, int status, int modifiedby,int courierid,string SystemID,string BankID)
        {
            SqlConnection ObjConn = null;
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            try
            {
                DataTable DtinsertInstallOutput = new DataTable();
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.SpUpdateCourierDetails", ObjConn, CommandType.StoredProcedure))
                {

                    sspObj.AddParameterWithValue("@CourierName", SqlDbType.VarChar, 0, ParameterDirection.Input, couriername);
                    sspObj.AddParameterWithValue("@OfficeName", SqlDbType.VarChar, 0, ParameterDirection.Input, officename);
                    sspObj.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, mobileno);
                    sspObj.AddParameterWithValue("@Status", SqlDbType.Int, 0, ParameterDirection.Input, status);
                    sspObj.AddParameterWithValue("@ModifiedBy", SqlDbType.Int, 0, ParameterDirection.Input, modifiedby);
                    sspObj.AddParameterWithValue("@CourierId", SqlDbType.Int, 0, ParameterDirection.Input, courierid);
                    if (!string.IsNullOrEmpty(SystemID))
                    {
                        sspObj.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, SystemID.Trim());
                    }
                    if (!string.IsNullOrEmpty(BankID))
                    {
                        sspObj.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, BankID.Trim());
                    }
                    DtinsertInstallOutput = sspObj.ExecuteDataTable();
                    sspObj.Dispose();

                    if (DtinsertInstallOutput.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(DtinsertInstallOutput.Rows[0]["Code"]);
                        ObjReturnStatus.Description = DtinsertInstallOutput.Rows[0]["OutputDescription"].ToString();
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Updation failed !";
                    }
                }
            }
            catch (Exception xObj)
            {
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = xObj.Message;
            }
            finally
            {
                ObjConn.Close();
            }
            return ObjReturnStatus;
        }
    
    }
}