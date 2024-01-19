using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;


namespace AGS.SwitchOperations
{
    class CallCardAPISansa
    {
        ModuleDALSansa ModuleDAL = new ModuleDALSansa();
        ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();

        public void Process(CustSearchFilter ObjFilter, APIResponseObjectSansa ObjAPIResponseObject)
        {
            string sResponse = String.Empty;
            var json = sResponse;
            try
            {
                ClsCommonBAL _ClsCommonBAL = new ClsCommonBAL();
                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallSansaAPIService Process START", "", ObjFilter.LoginId);
                DataTable ObjDTOutPut = new DataTable();
                DataTable DtRequest = ModuleDAL.GetCardAPIRequest(ObjFilter.tranType, ObjFilter);

                try
                {
                    string postData = String.Empty;
                    foreach (DataRow dr in DtRequest.Rows)
                    {
                        postData = postData + dr["ParamsName"].ToString() + "=";
                        object currentobj = ObjFilter.GetType().GetProperty(Convert.ToString(dr["ParamsName"]), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(ObjFilter);
                        postData = postData + Convert.ToString(currentobj) + "&";
                    }
                    if (!String.IsNullOrEmpty(postData))

                    {
                        postData = postData.TrimEnd('&');
                        // postData = "NIC=" + ObjFilter.NIC.ToString() + "&PARA=" + ObjFilter.PARA.ToString() + "&USER=" + ObjFilter.USER.ToString() + "&TOKEN=" + ObjFilter.TOKEN.ToString();
                        System.Net.WebClient webClient = new System.Net.WebClient();
                        webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        sResponse = webClient.UploadString(ObjFilter.SDBAPIURL, postData);

                        //string sResponse = webClient.UploadString("http://10.72.204.18:8080/novasclient/NCNICDetails", postData);

                        if (sResponse.IndexOf("ERROR") != -1)
                        {
                            ObjAPIResponseObject.Status = "100";
                        }
                        else
                        {

                            ObjAPIResponseObject.Status = "000";
                        }
                    }
                    else
                    {
                        sResponse = "postData is empty";
                        ObjAPIResponseObject.Status = "104";
                    }


                }
                catch (Exception ex)
                {
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallCardAPISansa.Process() >> Error", ex.ToString(), ObjFilter.LoginId);

                    ObjAPIResponseObject.Status = "102";
                    sResponse = ex.Message.ToString();
                }



                //APIResponseDataObjectSansa DataAPIRspObject = new APIResponseDataObjectSansa();
                //if (ObjFilter.tranType == "SDBNICDETAILS" || ObjFilter.tranType == "SDBACCDETAILS") //SDB NIC based ACC DETAILS
                //{
                //    DataAPIRspObject.Msg = "[{'CFPRNM':'Staff Security Savings Account','MEMOBAL':'15435.08','DMDOPN':'2012-06-11','CURBAL':'15935.08','DMACCT':'993454','DMBRCH':'56','DMTYPE':'113'},{'CFPRNM':'Staff Savings Account','MEMOBAL':'107911.65','DMDOPN':'2012-06-14','CURBAL':'108661.65','DMACCT':'0002380796','DMBRCH':'56','DMTYPE':'118'}]";
                //}
                //else if (ObjFilter.tranType == "SDBCUSTDETAILS") // SDB CUST based DETAILS 
                //{
                //    DataAPIRspObject.Msg = "[{'CIF':'0000809161','DrivingLicExpDT':'','BusinessName':'Mr P A C Kumara','NewNICNo':'853132322V','CustomerOpeningDate':'31122018','ShortName':'KUMARA PAC','City':'Kamburupitiya','Gender':'F','CustomerType':'P','OldNICNo':'','Nationality':'S','PassportExpDt':'','CustomerClassification':'K','MarketSeqment':'EOP','DOB':'1985312','PassportNo':'','BusinessRegNo':'853132322V','FullOrDispName':'Mr.P.A.C.Kumara','BranchCode':'56','Age':'','DrivingLicNo':''}]";
                //}
                //ObjAPIResponseObject.Status = "000";
                //json = DataAPIRspObject.Msg;
                //ObjAPIResponseObject.dtdetails = JsonConvert.DeserializeObject<DataTable>(json);
                //return;

            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallCardAPISansa.Process() >> Error", ex.ToString(), ObjFilter.LoginId);
                ObjAPIResponseObject.Status = "101";
                sResponse = ex.Message.ToString();
            }
            try
            {
                if (ObjAPIResponseObject.Status != "000")
                {
                    ObjAPIResponseObject.dtdetails = null;
                }
                else
                {
                    json = sResponse;
                    var resultObjects = AllChildren(JObject.Parse(json))
                    .Last(c => c.Type == JTokenType.Array && c.Path.Contains(((ObjFilter.tranType == "SDBCUSTDETAILS" || ObjFilter.tranType == "SDBNICDETAILS") ? "DETAILS": "SAVING")))
                    .Children<JObject>();
                    //foreach (var item in resultObjects)
                    //{
                        json = "[" + string.Join(",", resultObjects) + "]";
                    //}

                    ObjAPIResponseObject.dtdetails = JsonConvert.DeserializeObject<DataTable>(json);
                    
                    _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "",json, null, ObjFilter.LoginId);
                }
            }
            catch (Exception ex)
            {
                _ClsCommonBAL.FunInsertPortalISOLog(ObjFilter.tranType, "", "", "", "CallCardAPISansa.Process() >> Error", ex.ToString(), ObjFilter.LoginId);
                ObjAPIResponseObject.Status = "105";
                ObjAPIResponseObject.dtdetails = null;
            }
        }
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }

}
