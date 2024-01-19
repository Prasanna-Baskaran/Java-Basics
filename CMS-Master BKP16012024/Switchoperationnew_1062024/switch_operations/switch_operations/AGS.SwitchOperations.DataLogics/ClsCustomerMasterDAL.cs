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
using AGS.Utilities;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations.DataLogics
{
    public class ClsCustomerMasterDAL
    {
        public ClsReturnStatusBO FunSaveCustomer(ClsCustomer CustomerDtl, ClsCustOccupationDtl OccupationDtl, ClsCustomerAddress CustAddress, DataTable OtherCreditCardDtl)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);
                string SpName = string.Empty;
                //Update Customer
                if (CustomerDtl.IntPara == 1)
                {
                    SpName = "dbo.Sp_UpdateCustomerDetails";
                }
                //Insert Customer
                else
                {
                    SpName = "dbo.Sp_SaveCustomerDetails";
                }

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SpName, ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    //For Update
                    if (CustomerDtl.IntPara == 1 && CustomerDtl.CustomerID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, CustomerDtl.CustomerID);
                    }

                    if (!string.IsNullOrEmpty(CustomerDtl.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt16(CustomerDtl.SystemID));
                    }

                    if (!string.IsNullOrEmpty(CustomerDtl.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt16(CustomerDtl.BankID));
                    }

                    ObjCmd.AddParameterWithValue("@FirstName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.FirstName);
                    if (!string.IsNullOrEmpty(CustomerDtl.MiddleName))
                        ObjCmd.AddParameterWithValue("@MiddleName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.MiddleName);
                    ObjCmd.AddParameterWithValue("@LastName ", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.LastName);
                    ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.MobileNo);
                    if (CustomerDtl.DOB_BS != null)
                    {
                        ObjCmd.AddParameterWithValue("@DOB_BS", SqlDbType.DateTime, 0, ParameterDirection.Input, CustomerDtl.DOB_BS);
                    }
                    if (CustomerDtl.DOB_AD != null)
                    {
                        ObjCmd.AddParameterWithValue("@DOB_AD", SqlDbType.DateTime, 0, ParameterDirection.Input, CustomerDtl.DOB_AD);
                    }
                    ObjCmd.AddParameterWithValue("@Nationality", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.Nationality);
                    ObjCmd.AddParameterWithValue("@GenderID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.GenderID);
                    ObjCmd.AddParameterWithValue("@MaritalStatusID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.MaritalStatusID);
                    ObjCmd.AddParameterWithValue("@PassportNo_CitizenShipNo", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.PassportNo_CitizenShipNo);
                    ObjCmd.AddParameterWithValue("@IssueDate_District", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.IssueDate_District);
                    ObjCmd.AddParameterWithValue("@ResidenceTypeID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.ResidenceTypeID);
                    ObjCmd.AddParameterWithValue("@ResidenceDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.ResidenceDesc);
                    ObjCmd.AddParameterWithValue("@VehicleTypeID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.VehicleTypeID);
                    ObjCmd.AddParameterWithValue("@VehicleType", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.VehicleType);
                    ObjCmd.AddParameterWithValue("@VehicleNo", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.VehicleNo);
                    ObjCmd.AddParameterWithValue("@MakerID", SqlDbType.BigInt, 0, ParameterDirection.Input, CustomerDtl.MakerID);
                    ObjCmd.AddParameterWithValue("@CardTypeID", SqlDbType.Int, 0, ParameterDirection.Input, CustomerDtl.CardTypeID);
                    ObjCmd.AddParameterWithValue("@SpouseName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.SpouseName);
                    if (!string.IsNullOrEmpty(CustomerDtl.MotherName))
                        ObjCmd.AddParameterWithValue("@MotherName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.MotherName);
                    ObjCmd.AddParameterWithValue("@FatherName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.FatherName);
                    ObjCmd.AddParameterWithValue("@GrandFatherName", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.GrandFatherName);
                    ObjCmd.AddParameterWithValue("@PO_Box_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.PO_Box_P);
                    ObjCmd.AddParameterWithValue("@HouseNo_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.HouseNo_P);
                    ObjCmd.AddParameterWithValue("@StreetName_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.StreetName_P);
                    ObjCmd.AddParameterWithValue("@Tole_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Tole_P);
                    ObjCmd.AddParameterWithValue("@WardNo_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.WardNo_P);
                    ObjCmd.AddParameterWithValue("@City_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.City_P);
                    ObjCmd.AddParameterWithValue("@District_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.District_P);
                    ObjCmd.AddParameterWithValue("@Phone1_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone1_P);
                    ObjCmd.AddParameterWithValue("@Phone2_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone2_P);
                    ObjCmd.AddParameterWithValue("@FAX_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.FAX_P);
                    ObjCmd.AddParameterWithValue("@Mobile_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Mobile_P);
                    ObjCmd.AddParameterWithValue("@Email_P", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Email_P);
                    ObjCmd.AddParameterWithValue("@IsSameAsPermAddr", SqlDbType.Bit, 0, ParameterDirection.Input, CustAddress.IsSameAsPermAddr);
                    ObjCmd.AddParameterWithValue("@PO_Box_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.PO_Box_C);
                    ObjCmd.AddParameterWithValue("@HouseNo_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.HouseNo_C);
                    ObjCmd.AddParameterWithValue("@StreetName_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.StreetName_C);
                    ObjCmd.AddParameterWithValue("@Tole_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Tole_C);
                    ObjCmd.AddParameterWithValue("@WardNo_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.WardNo_C);
                    ObjCmd.AddParameterWithValue("@City_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.City_C);
                    ObjCmd.AddParameterWithValue("@District_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.District_C);
                    ObjCmd.AddParameterWithValue("@Phone1_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone1_C);
                    ObjCmd.AddParameterWithValue("@Phone2_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone2_C);
                    ObjCmd.AddParameterWithValue("@FAX_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.FAX_C);
                    ObjCmd.AddParameterWithValue("@Mobile_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Mobile_C);
                    ObjCmd.AddParameterWithValue("@Email_C", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Email_C);
                    ObjCmd.AddParameterWithValue("@PO_Box_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.PO_Box_O);
                    ObjCmd.AddParameterWithValue("@StreetName_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.StreetName_O);
                    ObjCmd.AddParameterWithValue("@City_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.StreetName_O);
                    ObjCmd.AddParameterWithValue("@District_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.District_O);
                    ObjCmd.AddParameterWithValue("@Phone1_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone1_O);
                    ObjCmd.AddParameterWithValue("@Phone2_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Phone2_O);
                    ObjCmd.AddParameterWithValue("@FAX_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.FAX_O);
                    ObjCmd.AddParameterWithValue("@Mobile_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Mobile_O);
                    ObjCmd.AddParameterWithValue("@Email_O", SqlDbType.VarChar, 0, ParameterDirection.Input, CustAddress.Email_O);
                    ObjCmd.AddParameterWithValue("@ProfessionTypeID", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ProfessionTypeID);
                    ObjCmd.AddParameterWithValue("@OrganizationTypeID", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.OrganizationTypeID);
                    ObjCmd.AddParameterWithValue("@OrganizationTypeDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.OrganizationTypeDesc);
                    ObjCmd.AddParameterWithValue("@ProfessionType", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ProfessionType);
                    ObjCmd.AddParameterWithValue("@PreviousEmployment", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.PreviousEmployment);
                    ObjCmd.AddParameterWithValue("@Designation", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.Designation);
                    ObjCmd.AddParameterWithValue("@BusinessType", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.BusinessType);
                    ObjCmd.AddParameterWithValue("@WorkSince", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.WorkSince);
                    ObjCmd.AddParameterWithValue("@AnnualSalary", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.AnnualSalary);
                    ObjCmd.AddParameterWithValue("@AnnualIncentive", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.AnnualIncentive);
                    ObjCmd.AddParameterWithValue("@AnnualBuisnessIncome", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.AnnualBuisnessIncome);
                    ObjCmd.AddParameterWithValue("@RentalIncome", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.RentalIncome);
                    ObjCmd.AddParameterWithValue("@Agriculture", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.Agriculture);
                    ObjCmd.AddParameterWithValue("@Income", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.Income);
                    ObjCmd.AddParameterWithValue("@TotalAnnualIncome", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.TotalAnnualIncome);
                    ObjCmd.AddParameterWithValue("@IsOtherCreditCard", SqlDbType.Bit, 0, ParameterDirection.Input, OccupationDtl.IsOtherCreditCard);
                    ObjCmd.AddParameterWithValue("@PrincipalBankName", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.PrincipalBankName);
                    ObjCmd.AddParameterWithValue("@AccountTypeID", SqlDbType.Int, 0, ParameterDirection.Input, OccupationDtl.AccountTypeID);
                    ObjCmd.AddParameterWithValue("@AccountTypeDesc", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.AccountTypeDesc);
                    ObjCmd.AddParameterWithValue("@IsPrabhuBankAcnt", SqlDbType.Bit, 0, ParameterDirection.Input, OccupationDtl.IsPrabhuBankAcnt);
                    ObjCmd.AddParameterWithValue("@PrabhuBankAccountNo", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.PrabhuBankAccountNo);
                    ObjCmd.AddParameterWithValue("@PrabhuBankBranch", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.PrabhuBankBranch);
                    if (OccupationDtl.IsCollectStatement == 1)
                    {
                        ObjCmd.AddParameterWithValue("@IsCollectStatement", SqlDbType.Bit, 0, ParameterDirection.Input, OccupationDtl.IsCollectStatement);
                    }
                    if (OccupationDtl.IsEmailStatemnt == 1)
                    {
                        ObjCmd.AddParameterWithValue("@IsEmailStatemnt", SqlDbType.Bit, 0, ParameterDirection.Input, OccupationDtl.IsEmailStatemnt);
                    }

                    ObjCmd.AddParameterWithValue("@EmailForStatement", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.EmailForStatement);
                    ObjCmd.AddParameterWithValue("@ReffName1", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffName1);
                    ObjCmd.AddParameterWithValue("@ReffDesignation1", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffDesignation1);
                    ObjCmd.AddParameterWithValue("@ReffPhoneNo1", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffPhoneNo1);
                    ObjCmd.AddParameterWithValue("@ReffName2", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffName2);
                    ObjCmd.AddParameterWithValue("@ReffDesignation2", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffDesignation2);
                    ObjCmd.AddParameterWithValue("@ReffPhoneNo2", SqlDbType.VarChar, 0, ParameterDirection.Input, OccupationDtl.ReffPhoneNo2);
                    ObjCmd.AddParameterWithValue("@ProductType_ID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.ProductTypeID);
                    ObjCmd.AddParameterWithValue("@INST_ID", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.INSTID);
                    ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, CustomerDtl.NameOnCard);

                    //Other Credit Card
                    if (OtherCreditCardDtl.Rows.Count > 0)
                    {
                        ObjCmd.AddParameterWithValue("@OtherCreditCardDtl", SqlDbType.Structured, 0, ParameterDirection.Input, OtherCreditCardDtl);
                    }

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        ObjReturnStatus.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        ObjReturnStatus.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                        ObjReturnStatus.OutPutCode = ObjDTOutPut.Rows[0]["OutPutCode"].ToString();
                        //Save/Update KYC
                        if ((ObjReturnStatus.Code == 0) && (!string.IsNullOrEmpty(ObjReturnStatus.OutPutCode)))
                        {
                            if (Convert.ToInt64(ObjReturnStatus.OutPutCode) > 0)
                            {
                                CustomerDtl.CustomerID = Convert.ToInt64(ObjReturnStatus.OutPutCode);
                                //if (((CustomerDtl.Signature!=null )&& !string.IsNullOrEmpty(CustomerDtl.Signature.FileName)))
                                if ((CustomerDtl.Photo != null) || (CustomerDtl.IDProof != null) || (CustomerDtl.Signature != null))
                                {
                                    if ((CustomerDtl.Photo.ContentLength > 0) || (CustomerDtl.IDProof.ContentLength > 0) || (CustomerDtl.Signature.ContentLength > 0))
                                    {
                                        int KYCResult = 0;
                                        KYCResult = FunSaveKYCDocuments(CustomerDtl);
                                        //if (KYCResult == 1)
                                        //{                                        
                                        //    ObjReturnStatus.Description = ObjReturnStatus.Description + " KYC documents are saved ";
                                        //}
                                        //else
                                        if (KYCResult != 1)
                                        {
                                            ObjReturnStatus.Description = ObjReturnStatus.Description + " KYC documents are not saved ";
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        ObjReturnStatus.Code = 1;
                        ObjReturnStatus.Description = "Customer details are not saved";

                    }
                }
            }
            catch (Exception ObjExc)
            {

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunSaveCustomer()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "Customer details are not saved";
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;
        }

        //Search Customer
        public DataTable FunSearchCustomer(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_SearchCustomer", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IntPara", SqlDbType.TinyInt, 0, ParameterDirection.Input, ObjFilter.IntPara);
                    if (!string.IsNullOrEmpty(ObjFilter.MobileNo))
                    {
                        ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.MobileNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.ApplicationNo))
                    {
                        ObjCmd.AddParameterWithValue("@ApplicationNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.ApplicationNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.IdentificationNo))
                    {
                        ObjCmd.AddParameterWithValue("@IdentificationNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IdentificationNo);
                    }
                    if (ObjFilter.CreatedDate != null)
                    {
                        ObjCmd.AddParameterWithValue("@CreatedDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.CreatedDate);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerName", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CustomerName);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerID))
                    {
                        ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.CustomerID);
                    }
                    if (ObjFilter.DOB_AD != null)
                    {
                        ObjCmd.AddParameterWithValue("@DOB_AD", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.DOB_AD);
                    }
                    if (ObjFilter.DOB_BS != null)
                    {
                        ObjCmd.AddParameterWithValue("@DOB_BS", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjFilter.DOB_BS);
                    }

                    if (!string.IsNullOrEmpty(ObjFilter.SystemID))
                    {
                        ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID.Trim());
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankID))
                    {
                        ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID.Trim());
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo.Trim());
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@BankCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }

                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ex)
            {
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }




        //GetCustomer Details
        public DataSet FunGetCustomerDetails(CustSearchFilter ObjFilter)
        {
            DataSet ObjDTOutPut = new DataSet();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                SqlCommand sqlComm = new SqlCommand("Sp_SearchCustomer", ObjConn);
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.Add("@CustomerID", SqlDbType.BigInt);
                sqlComm.Parameters["@CustomerID"].Value = ObjFilter.CustomerID;
                sqlComm.Parameters.Add("@IntPara", SqlDbType.Int);
                sqlComm.Parameters["@IntPara"].Value = ObjFilter.IntPara;
                sqlComm.Parameters.Add("@SystemID", SqlDbType.VarChar);
                sqlComm.Parameters["@SystemID"].Value = ObjFilter.SystemID;
                sqlComm.Parameters.Add("@BankID", SqlDbType.VarChar);
                sqlComm.Parameters["@BankID"].Value = ObjFilter.BankID;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ObjDTOutPut);
                ObjConn.Close();
                sqlComm.Dispose();

            }
            catch
            {
                ObjDTOutPut = new DataSet();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable FunGetLanguage(int BankID)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.usp_GetLanguage", ObjConn, CommandType.StoredProcedure))
                {
                  
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.Int, 0, ParameterDirection.Input, BankID);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ex)
            {
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        

        public ClsReturnStatusBO FunAccept_RejectCustomer(ClsCustomer ObjCustomer)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_ApproveRejectCustomer", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CustomerID);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CheckerID);
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.FormStatusID);
                    if (ObjCustomer.Checker_Date_NE != null)
                    {
                        ObjCmd.AddParameterWithValue("@Checker_Date_NE", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjCustomer.Checker_Date_NE);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.Remark);
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
                        if (ObjCustomer.FormStatusID == 1)
                        {
                            ObjReturnStatus.Description = "Customer application form is not accepted";
                        }
                        else
                        {
                            ObjReturnStatus.Description = "Customer application form is not rejected";
                        }
                    }
                }
            }
            catch (Exception ObjExc)
            {

                ObjReturnStatus.Code = 1;

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunAccept_RejectCustomer()", ObjExc.Message, "");
                if (ObjCustomer.FormStatusID == 1)
                {
                    ObjReturnStatus.Description = "Customer application form is not accepted";
                }
                else
                {
                    ObjReturnStatus.Description = "Customer application form is not rejected";
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

        public DataTable FunAccept_RejectCustomerForm(ClsCustomer ObjCustomer)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_AcceptRejectCustomer", ObjConn, CommandType.StoredProcedure))
                {

                    ObjCmd.AddParameterWithValue("@ReqCustID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.ReqCustomerId);
                    ObjCmd.AddParameterWithValue("@CheckerID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjCustomer.CheckerID);
                    ObjCmd.AddParameterWithValue("@FormStatusID", SqlDbType.Int, 0, ParameterDirection.Input, ObjCustomer.FormStatusID);
                    if (ObjCustomer.Checker_Date_NE != null)
                    {
                        ObjCmd.AddParameterWithValue("@Checker_Date_NE", SqlDbType.DateTime, 0, ParameterDirection.Input, ObjCustomer.Checker_Date_NE);
                    }
                    if (!string.IsNullOrEmpty(ObjCustomer.Remark))
                    {
                        ObjCmd.AddParameterWithValue("@Remark", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjCustomer.Remark);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();

                }
            }
            catch (Exception ObjExc)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunAccept_RejectCustomerForm()", ObjExc.Message, "");
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjDTOutPut;
        }
        //KYC documents Saving
        public int FunSaveKYCDocuments(ClsCustomer ObjCustomer)
        {
            int Result = 0;
            int SignatureResult = 0;
            string SignatureFileName = string.Empty;
            int PhotoResult = 0;
            string PhotoFileName = string.Empty;
            int IDProofResult = 0;
            string IDProofFileName = string.Empty;
            try
            {
                //for Signature file
                if (ObjCustomer.Signature != null)
                {
                    string fileExt =
                       (System.IO.Path.GetExtension(ObjCustomer.Signature.FileName)).ToLower();

                    if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".png")
                    {
                        // HttpPostedFile SignatureFile = (HttpPostedFile)imgSignatureUpload.PostedFile;
                        SignatureFileName = "KYC_S" + fileExt;
                        SignatureResult = SaveKYCFile(ObjCustomer.CustomerID.ToString(), ObjCustomer.Signature, SignatureFileName);
                    }
                }

                //For Photo File                
                if (ObjCustomer.Photo != null)
                {
                    string fileExt =
                      (System.IO.Path.GetExtension(ObjCustomer.Photo.FileName)).ToLower();

                    if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".png")
                    {
                        // HttpPostedFile SignatureFile = (HttpPostedFile)imgSignatureUpload.PostedFile;
                        PhotoFileName = "KYC_P" + fileExt;
                        PhotoResult = SaveKYCFile(ObjCustomer.CustomerID.ToString(), ObjCustomer.Photo, PhotoFileName);
                    }
                }

                //For IDProof File                
                if (ObjCustomer.IDProof != null)
                {
                    string fileExt =
                       (System.IO.Path.GetExtension(ObjCustomer.IDProof.FileName)).ToLower();

                    if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".png")
                    {
                        // HttpPostedFile SignatureFile = (HttpPostedFile)imgSignatureUpload.PostedFile;
                        IDProofFileName = "KYC_ID" + fileExt;
                        IDProofResult = SaveKYCFile(ObjCustomer.CustomerID.ToString(), ObjCustomer.IDProof, IDProofFileName);
                    }
                }
                if (SignatureResult == 1 && PhotoResult == 1 && IDProofResult == 1)
                {
                    FunSaveKYCDocumentLog(ObjCustomer.CustomerID, SignatureFileName, PhotoFileName, IDProofFileName);
                    Result = 1;
                }
                //Edit KYC  OR for no file to  upload
                if ((ObjCustomer.IntPara == 1) || ((ObjCustomer.Signature != null) && (ObjCustomer.Signature != null) && ((ObjCustomer.Signature != null))))
                {
                    Result = 1;
                }
            }
            catch (Exception ObjExc)
            {
                Result = 0;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunSaveKYCDocument()", ObjExc.Message, "");
            }

            return Result;
        }

        public ClsReturnStatusBO FunSaveKYCDocumentLog(Int64 CustomerID, string SignatureFileName, string PhotofileName, string IDProofFileName)
        {
            ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
            ObjReturnStatus.Code = 1;
            ObjReturnStatus.Description = "Failed";

            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.Sp_SaveKYCDocuments", ObjConn, CommandType.StoredProcedure))
                {
                    DataTable ObjDTOutPut = new DataTable();
                    ObjCmd.AddParameterWithValue("@CustomerID", SqlDbType.BigInt, 0, ParameterDirection.Input, CustomerID);
                    ObjCmd.AddParameterWithValue("@Signature", SqlDbType.VarChar, 0, ParameterDirection.Input, SignatureFileName);
                    ObjCmd.AddParameterWithValue("@Photo", SqlDbType.VarChar, 0, ParameterDirection.Input, PhotofileName);
                    ObjCmd.AddParameterWithValue("@IdProof", SqlDbType.VarChar, 0, ParameterDirection.Input, IDProofFileName);
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
                        ObjReturnStatus.Description = "KYC documents are not saved";

                    }
                }
            }
            catch (Exception ObjExc)
            {

                ObjReturnStatus.Code = 1;

                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunSaveKYCDocumentLog()", ObjExc.Message, "");
                ObjReturnStatus.Code = 1;
                ObjReturnStatus.Description = "KYC documents are not saved";
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }

            return ObjReturnStatus;

        }

        //Save KYC
        public int SaveKYCFile(string CustomerID, HttpPostedFile file, string renamefile = "")
        {
            string path = "";
            int iresult = 0;
            var fileName = Path.GetFileName(file.FileName);
            if (!String.IsNullOrEmpty(renamefile))
            {
                fileName = renamefile;
            }
            try
            {
                //Save files to disk
                path = CreateDocFilePath(CustomerID, fileName);
                file.SaveAs(path);
                iresult = 1;
            }
            catch (Exception ObjExc)
            {
                iresult = 0;
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, SaveKYCFile()", ObjExc.Message, "");
            }

            return iresult;
        }

        //Create Path on server
        static string CreateDocFilePath(string CustomerID, string filename)
        {
            string KYCFilePath = ConfigurationManager.AppSettings["KYCFilePath"].ToString();

            DirectoryInfo rootDi = new DirectoryInfo(KYCFilePath);
            string subDi = Path.Combine(KYCFilePath, CustomerID);
            if (!rootDi.Exists)
            {
                try
                {
                    rootDi.Create();
                }
                catch (Exception ObjExc)
                {
                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, CreateDocFilePath()", ObjExc.Message, "CustomerID =" + CustomerID);
                }

                try
                {
                    Directory.CreateDirectory(subDi);
                }
                catch (Exception ObjExc)
                {
                    ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, CreateDocFilePath()", ObjExc.Message, "CustomerID =" + CustomerID);
                }

            }

            if (!Directory.Exists(subDi))
                Directory.CreateDirectory(subDi);

            string file = string.Empty;


            var path = Path.Combine(subDi, filename);

            return path;
        }

        public ClsReturnStatusBO FunSaveCardRequest(DataTable _Dt, CustomerRegistrationDetails _CustomerRegistrationDetails)
        {
            ClsReturnStatusBO _ClsReturnStatusBO = new ClsReturnStatusBO();
            try
            {
                SqlConnection ObjConn = null;
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                String SP;
                if (_CustomerRegistrationDetails.iInstaEdit == "3")
                {
                    SP = "dbo.USP_CardRequestInsta";
                }
                else {

                    SP = "dbo.USP_CardRequest";

                }

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure(SP, ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@CustBulkData", SqlDbType.Structured, 0, ParameterDirection.Input, _Dt);
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.Int, 0, ParameterDirection.Input, _CustomerRegistrationDetails.IssuerNo);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.Int, 0, ParameterDirection.Input, _CustomerRegistrationDetails.SystemID);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.BankID);
                    ObjCmd.AddParameterWithValue("@RequestedBy", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.RequestedBy);
                    ObjCmd.AddParameterWithValue("@ProcessedBy", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.ProcessedBy);
                    ObjCmd.AddParameterWithValue("@RequestType", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.RequestType);
                    ObjCmd.AddParameterWithValue("@AuthID", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.RequestType);
                    //added for ATPBF-1355 start
                    if (_CustomerRegistrationDetails.iInstaEdit == "3")
                    {
                        ObjCmd.AddParameterWithValue("@OldCardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.OldCardNo);
                    }
                    ObjCmd.AddParameterWithValue("@UserBranchCode", SqlDbType.VarChar, 0, ParameterDirection.Input, _CustomerRegistrationDetails.UserBranchCode);
                    //added for ATPBF-1355 end
                    DataTable ObjDTOutPut = new DataTable();
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    if (ObjDTOutPut.Rows.Count > 0)
                    {
                        _ClsReturnStatusBO.Code = Convert.ToInt16(ObjDTOutPut.Rows[0]["Code"]);
                        _ClsReturnStatusBO.Description = ObjDTOutPut.Rows[0]["OutputDescription"].ToString();
                        _ClsReturnStatusBO.OutPutCode = ObjDTOutPut.Rows[0]["OutPutCode"].ToString();
                        _ClsReturnStatusBO.CardREqID = Convert.ToInt64(ObjDTOutPut.Rows[0]["CardREqID"]);
                        
                    }
                }
            }
            catch (Exception Ex)
            {
                ClsCommonDAL.FunInsertIntoErrorLog("CS, ClsCustomerMasterDAL, FunSaveCustomer()", Ex.Message, "");
                _ClsReturnStatusBO.Code = 1;
                _ClsReturnStatusBO.Description = "Customer details are not saved";
            }
            return _ClsReturnStatusBO;
        }

        //Search Customer
        public DataTable FunViewCardDetails(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_ViewCustomerDetails", ObjConn, CommandType.StoredProcedure))
                 {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.OldCardNo))
                    {
                        ObjCmd.AddParameterWithValue("@OldCardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.OldCardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CustomerName);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.MobileNo))
                    {
                        ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.MobileNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@CIFID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.AccountNo))
                    {
                        ObjCmd.AddParameterWithValue("@AccountId", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.AccountNo);
                    }
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@TblAuthId", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NIC))
                    {
                        ObjCmd.AddParameterWithValue("@NIC", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NIC);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ex)
            {
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        
      public DataTable FunGetCardAPIdata(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_GetCardAPIdata", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@Transtype", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.tranType);
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ex)
            {
                ObjDTOutPut = new DataTable();
            }
            finally
            {
                if (ObjConn.State == ConnectionState.Open)
                    ObjConn.Close();
            }
            return ObjDTOutPut;
        }

        public DataTable GetCardAPIRequest(String tranType, Int32 LoginId)
        {
            DataTable dtReturn = null;
            SqlConnection oConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref oConn, 1);

                //oConn = new SqlConnection("Data Source=10.10.0.54;Initial Catalog=SwitchOperations;User ID=uagsrep;Password=ags@1234");

                using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.Usp_getCardAPIRequest", oConn, CommandType.StoredProcedure))
                {
                    sspObj.AddParameterWithValue("@transtype", SqlDbType.VarChar, 0, ParameterDirection.Input, tranType);
                    dtReturn = sspObj.ExecuteDataTable();
                    sspObj.Dispose();
                }

            }
            catch (Exception ex)
            {
                ClsCommonDAL.FunInsertPortalISOLog(tranType, "", "", "", "CardAPIService GetCardAPIRequest>> Error", ex.ToString(), LoginId);
                //FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return dtReturn;
            }
            return dtReturn;
        }

        public DataTable FunCheckBlockCards(CustSearchFilter ObjFilter)
        {
            DataTable ObjDTOutPut = new DataTable();
            SqlConnection ObjConn = null;
            try
            {
                ClsCommonDAL.FunGetConnection(ref ObjConn, 1);

                using (SqlStoredProcedure ObjCmd = new SqlStoredProcedure("dbo.USP_CheckBlockCards", ObjConn, CommandType.StoredProcedure))
                {
                    ObjCmd.AddParameterWithValue("@IssuerNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.IssuerNo);
                    ObjCmd.AddParameterWithValue("@BankID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankID);
                    ObjCmd.AddParameterWithValue("@SystemID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.SystemID);
                    if (!string.IsNullOrEmpty(ObjFilter.CardNo))
                    {
                        ObjCmd.AddParameterWithValue("@CardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.OldCardNo))
                    {
                        ObjCmd.AddParameterWithValue("@OldCardNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.OldCardNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.CustomerName))
                    {
                        ObjCmd.AddParameterWithValue("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.CustomerName);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.MobileNo))
                    {
                        ObjCmd.AddParameterWithValue("@MobileNo", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.MobileNo);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.BankCustID))
                    {
                        ObjCmd.AddParameterWithValue("@CIFID", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.BankCustID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.AccountNo))
                    {
                        ObjCmd.AddParameterWithValue("@AccountId", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.AccountNo);
                    }
                    if (ObjFilter.ID > 0)
                    {
                        ObjCmd.AddParameterWithValue("@TblAuthId", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjFilter.ID);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NameOnCard))
                    {
                        ObjCmd.AddParameterWithValue("@NameOnCard", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NameOnCard);
                    }
                    if (!string.IsNullOrEmpty(ObjFilter.NIC))
                    {
                        ObjCmd.AddParameterWithValue("@NIC", SqlDbType.VarChar, 0, ParameterDirection.Input, ObjFilter.NIC);
                    }
                    ObjDTOutPut = ObjCmd.ExecuteDataTable();
                    ObjCmd.Dispose();
                    ObjConn.Close();
                }

            }
            catch (Exception ex)
            {
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