using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AGS.RBLCardAutomationISO
{
    class checkException
    {
        ModuleDAL ModuleDAL = new ModuleDAL();
        public bool startprocessing(ConfigDataObject ObjData)
        {
            
            #region CHECK PREVIOUS FILE STATUS
            /*CHECK PREVIOUS FILE STATUS*/
            /*START*/
            try
            {
                bool status=ModuleDAL.CheckPreviousfilestatus(ObjData);
                if(!status)
                {
                   DataTable dtexceptionrecords = ModuleDAL.usp_getExceptionRecordForProcessing(ObjData);
                   if(dtexceptionrecords.Rows.Count>0)
                   {
                       ExceptionISOProcessing(dtexceptionrecords, ObjData);
                       Standard4Call(ObjData);
                       new CardAutomation().FunInsertTextLog("PRE Processing Started FOR !" + ObjData.FileID, Convert.ToInt32(ObjData.IssuerNo), ObjData.ErrorLogPath);
                       ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "PRE PROCESSING STARTED FOR:" + ObjData.FileName, ObjData.IssuerNo.ToString(), 1);
                       PREFileGenerationRBL.PREFile ObjPRE = new PREFileGenerationRBL.PREFile();
                       ObjPRE.Process(Convert.ToInt32(ObjData.IssuerNo), ObjData.FileID, ObjData.FileName, ObjData.ProcessId);
                       ModuleDAL.FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "", "PRE PROCESSING ENDED FOR:" + ObjData.FileName, ObjData.IssuerNo.ToString(), 1);
                   }
                }
            }   
            catch(Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID.ToString(), ObjData.ProcessId, ex.ToString(), "", ObjData.IssuerNo.ToString(), 0);
                return false;
            }
            return true;
            /*END*/
            #endregion
        }
        public void ExceptionISOProcessing(DataTable PendingISOrecords, ConfigDataObject objData)
        {
            try
            {
                Parallel.ForEach(PendingISOrecords.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                                        drow =>
                                        {
                                            new GenerateCardAPIRequest().CallCardAPIService(drow,PendingISOrecords, objData);
                                        });

                /*UPDATE THE SWITCH RSP*/
                /*START*/
                ModuleDAL.UpdateSwitchRspStatus(objData);
                /*END*/
                /*CHECK ANY RECORD FAILED WITH RESP CODE 42 FOR LINKING*/
                DataTable dtAccountLinikingRecord = ModuleDAL.GETAccountlinkingRecords(objData);
                if (dtAccountLinikingRecord.Rows.Count > 0)
                {

                    objData.APIRequestType = "AccountLinkingDelinking";
                    objData.IsNewCardGenerate = false;
                    Parallel.ForEach(dtAccountLinikingRecord.AsEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CardAuomationISOThreadCount"].ToString()) },
                    drow =>
                    {
                        new GenerateCardAPIRequest().CallCardAPIService(drow, dtAccountLinikingRecord, objData);
                    });
                }
                /*UPDATE THE SWITCH RSP*/
                /*START*/
                ModuleDAL.UpdateSwitchRspStatus(objData);
                /*END*/
               ModuleDAL.UpdateFileStatus(objData, 7);
               
            }
            catch(Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
            }

        }
        public void Standard4Call(ConfigDataObject objData)
        {
            try
            {
                ModuleDAL.RunSTD4(objData);
                ModuleDAL.UpdateFileStatus(objData, 8);
            }
            catch (Exception ex)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, objData.FileID.ToString(), objData.ProcessId, ex.ToString(), "", objData.IssuerNo.ToString(), 0);
            }

        }
    }
}
