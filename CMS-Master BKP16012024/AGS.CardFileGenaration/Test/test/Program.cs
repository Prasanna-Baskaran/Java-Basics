using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AGS.CardFileGenaration;
using System.Management;
//using System.Management.Instrumentation;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Net;
using System.Net.Security;
//Created by sai
namespace Test
{
    class Program
    {
        public static void InitiateSSLTrust()
        {
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {
                //ActivityLog.InsertSyncActivity(ex);
            }
        }
        public static void CallAPI(string JsonData, out string Status, out string Msg)
        {
            Status = string.Empty;
            Msg = string.Empty;
            try
            {
                string str = "https://103.70.91.11:4430/login";
                InitiateSSLTrust();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(str);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // RespCode = string.Empty;
                    //ResponsMsg = "Unable to set FX rate";
                    var result = streamReader.ReadToEnd();
                    //string APIResponse = result.ReadToEnd();
                    // ClsAPIResponseBO objResponse = JsonConvert.DeserializeObject<ClsAPIResponseBO>(result);
                    //Status = objResponse.Status;
                    //  Msg = objResponse.Msg;
                }
            }
            catch (Exception ex)
            {
                //(new ClsCommonBAL()).FunInsertIntoErrorLog("CS, PIN RESET ,  " + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, BankId);
                //LblMessage.Text = ex.Message;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "javascript", "FunShowMsg()", true);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //string a = "";
                //string b = "";
                //CallAPI("", out a, out b);

                //---------- PRE File generation
                int IssuerNo = 7; //for uat RDB bank
               // int IssuerNo = 5; //for Prod RDB bank
                //(new AGS.PREFileGeneration.PREFile()).Process(IssuerNo,"1212","abc","11");

                //(new HotListCardReissue.ClsHotListCardReissue()).Process();

                //----------- Card Automation

                //Get issuer for
                //int IssuerNo =2;
                //(new AGS.PREFileGenerationwithOutFileID.PREFile()).Process(IssuerNo);
                //(new AGS.RBLCardAutomationISO.CardAutomation()).Process(IssuerNo);



                //Insta card file generation
                Console.WriteLine("InstaCardsFileGeneration Started !");
                (new InstaCardsFileGeneration.ClsInstaCardsFileGen()).Process(IssuerNo);

                Console.WriteLine("InstaCardsFileGeneration Completed !");
                Console.WriteLine("Press Any Key to start Processing of Insta Cards !");
               // Console.ReadKey();

                //Insta card file Processing
                Console.WriteLine("CardAutomationISO Started !");
                (new AGS.CardAutomationISO.CardAutomation()).Process(IssuerNo);
                Console.WriteLine("CardAutomationISO Ended !");

              // Console.ReadKey();



                //(new AGS.PREFileGenerationwithOutFileID.PREFile()).Process(IssuerNo);
                //(new AGS.PREFileGenerationRBL.PREFile()).Process(IssuerNo, "", "", "3");


                //(new AGS.RBLCardAutomationISO.CardAutomation()).Process(IssuerNo);
                //---------------- RBL CardAutomation
                //(new AGS.RBLCardAutomationISO.CardAutomation()).Process(IssuerNo);
                //(new AGS.PREFileGenerationRBL.PREFile()).Process(1, "230546", "asdadaddad.txt", "6");

                //                (new AGS.RBLCardAutomation.RBLCardAuto()).Process(IssuerNo);


                //  int IssuerNo = 23;
                // new AGS.CardReissue.CardReissue().Process(IssuerNo);
                //For Reissue
                //(new AGS.CardReissue.CardReissue()).Process(IssuerNo);

                //FunExecuteProcess();




                //------------- Bank API testing -----------------//

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.72.204.18:8080/novasclient/NCNICDetails");
                ////var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.72.204.18:8080/novasclient/NCAccountDetails");
                //httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //httpWebRequest.Accept= "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,application/json";
                //httpWebRequest.UserAgent= "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
                ////string postjsonString = "[{\"key\":\"NIC\",\"value\":\"853132322V\",\"description\":\"ffftf\",\"type\":\"text\",\"enabled\":true},{\"key\":\"PARA\",\"value\":\"BASIC\",\"description\":\"rgf\",\"type\":\"text\",\"enabled\":true},{\"key\":\"USER\",\"value\":\"IBUSER\",\"description\":\"dff\",\"type\":\"text\",\"enabled\":true},{\"key\":\"TOKEN\",\"value\":\"BMVvOips1WnZha99wZ3IsA==\",\"description\":\"dfff\",\"type\":\"text\",\"enabled\":true}]";

                ////httpWebRequest.AuthenticationLevel = AuthenticationLevel.Default;
                //httpWebRequest.Headers.Remove("Authorization");

                //httpWebRequest.Method = "POST";
                //string postjsonString = "[{'key':'NIC','value':'853132322V','description':'','type':'text','enabled':'true'},{'key':'PARA','value':'BASIC','description':'','type':'text','enabled':'true'},{'key':'USER','value':'IBUSER','description':'','type':'text','enabled':'true'},{'key':'TOKEN','value':'BMVvOips1WnZha99wZ3IsA==','description':'','type':'text','enabled':'true'}]";
                ////string postjsonString = "[{'key':'NIC','value':'853132322V','description':'yg','type':'text','enabled':true} ]"; //,{'key':'PARA','value':'BASIC','description':'','type':'text','enabled':true},{'key':'USER','value':'IBUSER','description':'','type':'text','enabled':true},{'key':'TOKEN','value':'BMVvOips1WnZha99wZ3IsA==','description':'','type':'text','enabled':true}]";
                ////string postjsonString = "[{ 'key':'CIF','value':'0000809161','description':'','type':'text','enabled':true},;{ 'key':'PARA','value':'SAVING','description':'','type':'text','enabled':true},{ 'key':'USER','value':'IBUSER','description':'','type':'text','enabled':true},{ 'key':'TOKEN','value':'BMVvOips1WnZha99wZ3IsA==','description':'','type':'text','enabled':true}]";
                //byte[] buffer = Encoding.Default.GetBytes(postjsonString);
                //if(buffer != null)
                //{
                //    httpWebRequest.ContentLength = buffer.Length;
                //    httpWebRequest.GetRequestStream().Write(buffer, 0, buffer.Length);
                //}
                ////using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                ////{
                ////    streamWriter.Write(postjsonString);
                ////    streamWriter.Flush();
                ////    streamWriter.Close();
                ////}
                //Console.WriteLine(postjsonString);

                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                //Console.WriteLine("REsponse: " +httpResponse.ToString());

                //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    var responsedata = streamReader.ReadToEnd();

                //    Console.WriteLine(responsedata.ToString());
                //}
                //Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine("err" + ex.ToString());
                Console.ReadKey();
            }
        }


        public static void FunExecuteProcess()
        {
            try
            {
                string StrPriPath = @"D:\Test_DLL\AGS.CardAutomation.dll";
                string StrPriClass = "AGS.CardAutomation.CardAutomation";
                string StrPriIssuernum = "18";

                string StrLogIdentity = string.Empty;
                string StrReturnMsg = string.Empty;
                //  StrLogIdentity = ClsCommon.insertAPIRqstRspDetail("IntMassModuleCode=" + IntMassModuleCode.ToString() + "|$|" + "StrPriPath=" + StrPriPath + "|$|" + "StrPriClass=" + StrPriClass.ToString() + "|$|" + "StrPriIssuernum=" + StrPriIssuernum);
                //  ClsCommon.LogFileWrite("IntMassModuleCode=" + IntMassModuleCode.ToString() + "|$|" + "StrPriPath=" + StrPriPath + "|$|" + "StrPriClass=" + StrPriClass.ToString() + "|$|" + "StrPriIssuernum=" + StrPriIssuernum);
                //ClsReturnStatusBO ObjReturnStatus = new ClsReturnStatusBO();
                try
                {
                    //   ObjReturnStatus = ClsCommon.FunSetModuleStatus(IntMassModuleCode.ToString(), "2");

                    // Use the file name to load the assembly into the current
                    Assembly ObjAssemblyLoad = Assembly.LoadFile(StrPriPath.ToString());
                    // Get the type to use.
                    Type ObjTypCls = ObjAssemblyLoad.GetType(StrPriClass.ToString());
                    // Get the method to call.
                    //     MethodInfo myMethod = ObjTypCls.GetMethod("Process");
                    //use reflection or dynamic to call the method ann Create an instance.
                    //dynamic ObjDynmic = Activator.CreateInstance(ObjTypCls);
                    //ObjDynmic.Process();
                    //   ClsCommon.LogFileWrite("1 FunExecuteProcess() ObjAssemblyLoad" + ObjAssemblyLoad.FullName.ToString());
                    if (ObjTypCls != null)
                    {
                        MethodInfo methodInfo = ObjTypCls.GetMethod("Process");
                        if (methodInfo != null)
                        {
                            object result = null;
                            ParameterInfo[] parameters = methodInfo.GetParameters();
                            // ClsCommon.LogFileWrite("2 FunExecuteProcess() methodInfo" + methodInfo.Attributes.ToString());
                            object classInstance = Activator.CreateInstance(ObjTypCls, null);
                            //  ClsCommon.LogFileWrite("3 FunExecuteProcess() parameters" + parameters.Count().ToString());
                            if (parameters.Length == 0)
                            {


                                // This works fine
                                // ClsCommon.LogFileWrite("5.1 FunExecuteProcess() result" + result.ToString());
                                result = methodInfo.Invoke(classInstance, null);


                                //  ClsCommon.LogFileWrite("5.2 FunExecuteProcess() result" + result.ToString());
                            }
                            else
                            {
                                //  ClsCommon.LogFileWrite("4 FunExecuteProcess()parameters" + parameters.Count().ToString());
                                Int32 IntIssuerNum = Convert.ToInt32(StrPriIssuernum);
                                object[] parametersArray = new object[] { IntIssuerNum };

                                // The invoke does NOT work;
                                // it throws "Object does not match target type" 
                                try
                                {
                                    // ClsCommon.LogFileWrite("5.1 FunExecuteProcess() result" + result.ToString());
                                    result = methodInfo.Invoke(classInstance, parametersArray);
                                    // ClsCommon.LogFileWrite("5.2 FunExecuteProcess() result" + result.ToString());

                                }
                                catch (Exception Ex)
                                {
                                    // ClsCommon.LogFileWrite("Error FunExecuteProcess() " + Ex.Message.ToString());
                                }
                            }
                            // ClsCommon.LogFileWrite("5 FunExecuteProcess() result" + result.ToString());
                        }
                    }
                    //  ClsCommon.LogFileWrite("6 FunExecuteProcess() MassModule" + IntMassModuleCode.ToString());
                    //  ObjReturnStatus = ClsCommon.FunSetModuleStatus(IntMassModuleCode.ToString(), "1");
                    StrReturnMsg = "1";
                }
                catch (Exception Ex)
                {
                    // Console.WriteLine("Error : " + Ex.Message.ToString());
                    //   ObjReturnStatus = ClsCommon.FunSetModuleStatus(IntMassModuleCode.ToString(), "3");
                    StrReturnMsg = "3";
                    // ClsCommon.LogFileWrite("IntMassModuleCode=" + IntMassModuleCode.ToString() + "|$|" + "StrPriPath=" + StrPriPath + "|$|" + "StrPriClass=" + StrPriClass.ToString() + "|$|" + "StrPriIssuernum=" + StrPriIssuernum + "|$$|" + "Error : " + Ex.Message);

                }
                if (StrLogIdentity != "")
                {
                    // ClsCommon.UpdateAPILogRspDetail("Status=" + Convert.ToString(ObjReturnStatus.Code) + ",MSG=" + ObjReturnStatus.Description + ",Task Status=" + StrReturnMsg, Convert.ToDouble(StrLogIdentity));
                }
                //  ClsCommon.LogFileWrite("Status=" + Convert.ToString(ObjReturnStatus.Code) + ",MSG=" + ObjReturnStatus.Description + ",Task Status=" + StrReturnMsg);
            }
            catch (Exception Ex)
            {

                //  ClsCommon.LogFileWrite("FunExecuteProcess " + Ex.Message);
            }
        }

    }
}
