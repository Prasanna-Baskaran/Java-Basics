using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLinking
{
    public class AccountLinking
    {
        public void Process()
        {
            
            ModuleDAL ModuleDAL = new ModuleDAL();
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "**************************APPLICATION START**************************", 0);
            try
            {
                
                /*APPLICATION STARTED*/
                /*GETTING THE PROCESS ID CONFIGURATION FOR THE BANK*/
                DataTable DTRecord = ModuleDAL.GetRecordFORISOProcessing();
                if (DTRecord.Rows.Count>0)
                {
                    new GenerateCardAPIRequest().CallCardAPIService(DTRecord);
                }
                else
                {
                    ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "**************************NO Record for AccountLinking Found**************************", 0);
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name,ex.ToString(), "", 0);
            }
            ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name,"", "**************************APPLICATION END**************************", 0);
            
            


        }
        /*Convert DataRow to Obj*/
        public static T BindDatatableToClass<T>(DataRow dtrow, DataTable dtTable)
        {
            var ob = Activator.CreateInstance<T>();
            try
            {
                DataRow dr = dtrow;

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
                    if (columns.Contains(propertyInfo.Name))
                    {
                        // Fill the data into the property
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                        propertyInfo.SetValue(ob, safeValue, null);

                        // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
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
