using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Generix
/// </summary>
public class CardReissueDAL
{
    public string FunGetResponseDescription(string strResponseCode)
    {
        string strResponse = string.Empty;
        SqlConnection conn = null;
        try
        {
            using (conn = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["conStrHotListCardReissue"])))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.usp_GetResponseDescription", conn);
                cmd.Parameters.Add(new SqlParameter("@ResponseCode", String.IsNullOrEmpty(strResponseCode) ? (object)DBNull.Value : strResponseCode));
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        strResponse = Convert.ToString(rdr["Description"]);
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            strResponse = string.Empty;
            
        }
        return strResponse;
    }
    public DataTable FunGetCardReissueData()
    {
        DataTable dt = new DataTable();
        string strResponse = string.Empty;
        try
        {
            using (SqlConnection conn = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["conStrHotListCardReissue"])))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.USP_MakeCardHotList", conn);
                cmd.Parameters.Add(new SqlParameter("@Mode", "select"));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            strResponse = string.Empty;
            return new DataTable();
            
        }
        return dt;
    }
    public bool FunUpdateReissueStatus(string Id, string OldCardPanID)
    {
        DataTable dt = new DataTable();
        string strResponse = string.Empty;
        SqlConnection conn = null;
        try
        {
            using (conn = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["conStrHotListCardReissue"])))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.USP_MakeCardHotList", conn);
                cmd.Parameters.Add(new SqlParameter("@Mode", "update"));
                cmd.Parameters.Add(new SqlParameter("@Id", Id));
                cmd.Parameters.Add(new SqlParameter("@OldCardRPANID", OldCardPanID));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            strResponse = string.Empty;
            //FunLog("Error in FunGetResponseDescription >>>>> declined response : message:" + ex.Message + "| StackTrace:" + ex.StackTrace + "|Source:" + ex.Source);
        }
        return false;
    }

    public bool FunAddModifyCustomerData(string CustomerId, string CardNo, string Remarks, string ModifyData, string UserId)
    {
        DataTable dt = new DataTable();
        string strResponse = string.Empty;
        SqlConnection conn = null;
        try
        {
            using (conn = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["conStrHotListCardReissue"])))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.USP_MakeCardHotList", conn);
                cmd.Parameters.Add(new SqlParameter("@Mode", "Reissue"));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));
                cmd.Parameters.Add(new SqlParameter("@ModifyData", ModifyData));
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.Parameters.Add(new SqlParameter("@CardNo", CardNo));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);
                conn.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            InsertLog(DateTime.Now.ToString() + ">> Message # :  " + ex.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            strResponse = string.Empty;
            //FunLog("Error in FunGetResponseDescription >>>>> declined response : message:" + ex.Message + "| StackTrace:" + ex.StackTrace + "|Source:" + ex.Source);
        }
        return false;
    }
    public  void InsertLog(string exception, string method)
    {
        
            DataTable dt = new DataTable();
            string strResponse = string.Empty;
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["conStrHotListCardReissue"])))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.Usp_InsertLog", conn);
                    cmd.Parameters.Add(new SqlParameter("@Exception", exception));
                    cmd.Parameters.Add(new SqlParameter("@MethodName", method));

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
                    conn.Close();
                }

            }
            catch (Exception ex)
            {

             
            }
        
    }
}