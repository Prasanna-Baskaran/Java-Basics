using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGS.SqlClient;
using System.Configuration;
using Renci.SshNet;
using System.IO.Compression;
using AGSPGP;

namespace InstaCardsFileGeneration
{
  public  class ClsInstaCardsFileGen : IDisposable
    {

        ModuleDAL ModuleDAL = new ModuleDAL();
        public string ErrorLogFilePath { get; set; }
        public ClsInstaCardsFileGen()
        {
            ErrorLogFilePath = string.Empty;
        }
        public void Process(int IssuerNo)
        {
            ConfigDataObject ObjData = new ConfigDataObject();
            SFTPDataObject ObjSFTP = new SFTPDataObject();

                try
                {
                ObjData.IssuerNo = IssuerNo.ToString();
                ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************************APPLICATION STARTED**************************", IssuerNo.ToString(), 1);
                /*APPLICATION STARTED*/

                /*GETTING Configuration*/
                DataTable DtConfig = ModuleDAL.getConfiguration(IssuerNo);

                ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "total request data for Insta cards: DtConfig.Rows.Count : " + DtConfig.Rows.Count.ToString(), IssuerNo.ToString(), 1);

                if (DtConfig.Rows.Count > 0)
                {
               
                    foreach (DataRow item in DtConfig.Rows)
                   {
                        try
                        {
                            ObjData.ProcessId = item["ProcessId"].ToString();
                            /*CONVERT THE DATATABLE TO CLASS OBJECT*/
                            /*STEP 1*/
                            ModuleDAL.FunInsertIntoErrorLog("Process", "", ObjData.ProcessId, "", "Conversion From DataTable To Class Objected Started", ObjData.IssuerNo, 1);
                            ObjSFTP = BindDatatableToClass<SFTPDataObject>(item, DtConfig);
                            ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, ObjData.ProcessId, "", "Conversion From DataTable To Class Objected ENDED", ObjData.IssuerNo, 1);

                            /*STEP 2*/
                            ObjSFTP.processid = ObjData.ProcessId;
                            ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************************SFTP file download**************************", IssuerNo.ToString(), 1);
                            /*SFTP file download start*/
                            if (SearchFile.DownloadFile(ObjSFTP, IssuerNo.ToString()))
                               {
                                new FileProcessor().process(ObjSFTP, IssuerNo.ToString());
                                }

 

                        }
                        catch (Exception ex)
                        {
                            ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", ex.ToString(), "Exception 1", IssuerNo.ToString(), 0);

                        }

                        //ModuleDAL.UpdateFileUploadStatus(ObjData);
                    }


                }

                else
                {

                    ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "No Configuration data found", "**************************No data found for Insta Cards Generation !**************************", IssuerNo.ToString(), 1);
                    ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************REQEST FILE process ended**************************", IssuerNo.ToString(), 1);


                }


                ObjData = new ConfigDataObject();

                /*GETTING Insta Cards Request Data*/
                DataTable DtInstaCards = ModuleDAL.getInstaCardsRequestData(IssuerNo);
                    if (DtInstaCards.Rows.Count < 1)
                    {
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "No data found for Insta Caeds", "**************************No data found for Insta Cards Generation !**************************", IssuerNo.ToString(), 1);
                        ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "**************************APPLICATION ENDED!**************************", IssuerNo.ToString(), 1);
                        return;
                    }
                  ModuleDAL.FunInsertIntoErrorLog("Process", "", "", "", "total request data for Insta cards: DtInstaCards.Rows.Count : " + DtInstaCards.Rows.Count.ToString(), IssuerNo.ToString(), 1);
                  foreach (DataRow item in DtInstaCards.Rows)
                    {
                        try
                        {
                           ObjData.id = item["id"].ToString();
                           /*CONVERT THE DATATABLE TO CLASS OBJECT*/
                           /*STEP 1*/
                            ModuleDAL.FunInsertIntoErrorLog("Process",item["id"].ToString(), "", "", "Conversion From DataTable To Class Objected Started", ObjData.IssuerNo, 1);
                            ObjData = BindDatatableToClass<ConfigDataObject>(item, DtInstaCards);
                            ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", "", "Conversion From DataTable To Class Objected ENDED", ObjData.IssuerNo, 1);
                           
                            /*STEP 2*/
                            string[] ArrResult = GenrerateFileData(ObjData);

                            if (ArrResult != null)
                            {
                                bool Result = ModuleDAL.FunCreateUploadInstaCIF(ObjData, ArrResult);
                                ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", "", "FunCreateUploadInstaCIF ended Result : " + Result.ToString(), ObjData.IssuerNo, 1);
                            }
                         
                        }
                        catch (Exception ex)
                        {
                            ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", ex.ToString(),"Exception 2", IssuerNo.ToString(), 0);
                            ObjData.Filestatus = "FAIL";
                            ObjData.Reason = "Eror while generating insta card file";

                        }

                    ModuleDAL.UpdateFileUploadStatus(ObjData);
                }

                }
                catch (Exception ex)
                {
                ModuleDAL.FunInsertIntoErrorLog("Process", "", "", ex.ToString(), "Exception 3", IssuerNo.ToString(), 0);
                }

            ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", "", "**************************APPLICATION END**************************", IssuerNo.ToString(), 1);

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public String[] GenrerateFileData(ConfigDataObject ObjData)
        {
            try {
                //string LastAccNo = "";
                //string LastCust = "";
                //string LastInstaReferenceNo = "";
                string[] ArrResult = new string[Convert.ToInt32(ObjData.NoOfCards)];
                ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", "", "Required Fileheader: " + ObjData.FileHeader, ObjData.IssuerNo, 1);
                ModuleDAL.FunInsertIntoErrorLog("Process", ObjData.id, "", "", "Generate CIF for NO Of Cards: " + ObjData.NoOfCards, ObjData.IssuerNo, 1);

                if (!string.IsNullOrEmpty(ObjData.FileHeader))
                {

                    try
                    {
                        //string LastCustomerID = ObjData.LastCustomerID;
                        //string LastAccountNo = ObjData.LastAccountNo;
                        //string LastCustPrefix = "";
                        //string LastAccPrefix = "";
                        //string InstaReferencePrefix = "";
                        //Int64 IntLastCustomerID = GetNumber(LastCustomerID, ref LastCustPrefix);
                        //Int64 IntLastAccountNo = GetNumber(LastAccountNo, ref LastAccPrefix);
                        //Int64 IntInsta_Reference_No = GetNumber(ObjData.Insta_Reference_No,ref InstaReferencePrefix);



                        for (int i = 0; i < Convert.ToInt32(ObjData.NoOfCards); i++)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb = sb.Append(ObjData.FileHeader);

                            //IntLastCustomerID = IntLastCustomerID + 1;
                            //IntLastAccountNo = IntLastAccountNo + 1;
                            //LastCust = LastCustPrefix + IntLastCustomerID.ToString();
                            //LastAccNo = LastAccPrefix + IntLastAccountNo.ToString();
                            //LastInstaReferenceNo = InstaReferencePrefix + IntInsta_Reference_No.ToString();

                            if (ObjData.FileHeader.Contains("#CIF_ID#"))
                            {
                                sb.Replace("#CIF_ID#", ObjData.LastCustomerID);
                            }
                            if (ObjData.FileHeader.Contains("#AccountNo#"))
                            {
                                sb.Replace("#AccountNo#", ObjData.LastAccountNo);

                            }
                            if (ObjData.FileHeader.Contains("#CardPrefix#"))
                            {
                                sb.Replace("#CardPrefix#", ObjData.Cardprefix);

                            }
                            //if (ObjData.FileHeader.Contains("#Insta_Reference_No#"))
                            //{
                            //    sb.Replace("#Insta_Reference_No#", LastInstaReferenceNo);

                            //}

                            ArrResult[i] = sb.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModuleDAL.FunInsertIntoErrorLog("GenrerateFileData", ObjData.id, "", ex.ToString(), "Exception 1 ", ObjData.IssuerNo, 0);
                        ObjData.Filestatus = "FAIL";
                        ObjData.Reason = "Error while generating file data";
                        return null;


                    }

                    //ObjData.LastCustomerID = LastCust;
                    //ObjData.LastAccountNo = LastAccNo;
                    //ObjData.Insta_Reference_No = LastInstaReferenceNo;

                    return ArrResult;
                }

                else
                {
                    ModuleDAL.FunInsertIntoErrorLog("GenrerateFileData", ObjData.id, "", "", "CIFFormat Not Configured in tblbin Table column name PrepaidCIFFormat ", ObjData.IssuerNo, 1);
                    ObjData.Filestatus = "FAIL";
                    ObjData.Reason = "Insta Card File Format Not Configured";
                    return null;
                }
            }
            catch (Exception ex)
            {
                ModuleDAL.FunInsertIntoErrorLog("GenrerateFileData", ObjData.id, "", ex.ToString(), "Exception 2", ObjData.IssuerNo, 0);
                ObjData.Filestatus = "FAIL";
                ObjData.Reason = "Exeception while generating file data";
                return null;


            }

        }

        private Int64 GetNumber(string str, ref string Prefix)
        {
            string number = "";
            foreach (char item in str)
            {
                if (char.IsDigit(item))
                {
                    number = number + item.ToString();
                }
                else
                {
                    number = "";
                }
            }
            int indexOf = str.IndexOf(number.ToString());
            Prefix = str.Substring(0, indexOf);
            return Convert.ToInt64(number);
        }
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
                    string Name = propertyInfo.Name;
                    if (columns.Select(x => x.Equals(Name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                    {
                        // Fill the data into the property
                        System.Reflection.PropertyInfo pI = ob.GetType().GetProperty(propertyInfo.Name);

                        Type t = Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType;
                        try
                        {
                            object safeValue = dr[propertyInfo.Name] == DBNull.Value ? null : Convert.ChangeType(dr[propertyInfo.Name], t);
                            propertyInfo.SetValue(ob, safeValue, null);
                        }
                        catch (Exception ex)
                        {

                        }



                        // propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ob;
        }

    }
}
