using AGS.Configuration;
using AGS.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.Common;

namespace AGS.SwitchOperations
{
    class ModuleDAL
    {
        ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();

        public DataTable GetCardAPIRequest(String tranType, ConfigDataObject ObjData)
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
                _ClsCommonBAL.FunInsertPortalISOLog(tranType, "", "", "", "CardAPIService GetCardAPIRequest>> Error", ex.ToString(), ObjData.LoginId);
                //FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return dtReturn;
            }
            return dtReturn;
        }

        public static T BindDatatableToClass<T>(DataRow dtRow, DataTable dtTable)
        {
            var ob = Activator.CreateInstance<T>();
            try
            {
                DataRow dr = dtRow;

                // Get all columns' name
                List<string> columns = new List<string>();
                foreach (DataColumn dc in dtTable.Columns)
                {
                    columns.Add(dc.ColumnName);
                }

                // Create object


                // Get all fields
                var fields = typeof(T).GetFields();
                foreach (var fieldInfo in fields)
                {
                    if (columns.Contains(fieldInfo.Name))
                    {
                        // Fill the data into the field
                        fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                    }
                }

                // Get all properties
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    string Name = propertyInfo.Name;
                    if (columns.Where(r => r.Equals(Name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                    {
                        // Fill the data into the property
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                        propertyInfo.SetValue(ob, safeValue, null);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return ob;
        }


    }
}
