
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AGS.RBLCardAutomationISO
{
    public class TCPCommunicator
    {

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        byte[] response = null;
        int Flag = 0;
        //public void SendRequest(DataObjects ObjData)
        //{
        //    TcpClient tclSocket = new TcpClient();
        //    NetworkStream ns = null;
        //    Byte[] byResponse = null;
        //    String sResponse = "";
        //    String sUniqueID = "";
        //    String sResponseUniqueiID = "";
        //    bool isEmpty = false;
        //    DateTime emptyTime = DateTime.Now;

        //    // Read Server IP & Port
        //    String sServerIP = ObjData.HostIP;
        //    int iPort = Convert.ToInt32(ObjData.HostPort); // Convert.ToInt16(ConfigurationSettings.AppSettings["ServerPort"].ToString());

        //    try
        //    {
        //        Log.debug("Host communication Message", "TCP Connection started", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
        //        int Wait = Convert.ToInt32(AppData.LstMasParamModel.Where(rec => rec.ParamName.ToLower() == "hostsessiontimeout").First().ParamValue);
        //        Log.debug("Host communication Message", "Wait for :" + Wait.ToString(), ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
        //        Byte[] byRequest = ISGenerixLib.stringToByteArray(ObjData.HostReqMessage);
        //        // Connect to server
        //        tclSocket.Connect(sServerIP, iPort);
        //        ns = tclSocket.GetStream();

        //        // Send Data to Server
        //        ns.Write(byRequest, 0, byRequest.Length);
        //       // sUniqueID = Guid.NewGuid().ToString();

        //        //string req = ;
        //        //string[] HostReq = req.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //        //foreach (var item in HostReq)
        //        //{
        //            //sUniqueID = item;

        //            //System.Text.Encoding.ASCII.GetBytes(sUniqueID);
        //            //Byte[] byRequest = Guid.NewGuid().ToByteArray();
        //            //sUniqueID = System.Text.Encoding.ASCII.GetString(byRequest);

        //            //Console.WriteLine(DateTime.Now + " : Request Sent Unique ID: " + sUniqueID);
        //            //log.write(DateTime.Now + " : Request Sent Unique ID: " + sUniqueID);
        //        //}
        //        // Read response from Server
        //        while (tclSocket.Connected)
        //        {
        //            if (ns.DataAvailable)
        //            {
        //                byResponse = new Byte[tclSocket.Available];
        //                ns.Read(byResponse, 0, byResponse.Length);
        //                sResponse = ISGenerixLib.byteToHex(byResponse);
        //                // Console.WriteLine(DateTime.Now + " : Response Received for Unquie ID : " + sUniqueID + " : is : " + sResponse);
        //                //  log.write(DateTime.Now + " : Response Received for Unquie ID : " + sUniqueID + " : is : " + sResponse);
        //                //Log.debug("Host communication Message", "Received incorrect Response for Unique ID" + sResponse, ObjData.RequestId, ObjData.SourceId, ObjData.TranType);

        //                // if (sResponse.Contains("|"))
        //                //{
        //                //    sResponseUniqueiID = sResponse.Split('|')[1].ToString();
        //                //    if (sResponseUniqueiID.Equals(sUniqueID))
        //                //    {
        //                ObjData.HostResMessage = sResponse;
        //                tclSocket.Close();
        //                Log.debug("Host communication Message: Response set", ObjData.HostResMessage, ObjData.RequestId, ObjData.SourceId, ObjData.TranType);

        //                // }
        //                //else
        //                //{
        //                //    Log.debug("Host communication Message", "Received incorrect Response for Unique ID", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);

        //                //    // Console.WriteLine(DateTime.Now + " : Received incorrect Response for Unique ID : " + sUniqueID);
        //                //    //   log.write(DateTime.Now + " : Received incorrect Response for Unique ID : " + sUniqueID);
        //                //}


        //                //    }
        //            }
        //            else
        //            {
        //                if (isEmpty)
        //                {


        //                    if ((DateTime.Now - emptyTime).Seconds >= Wait)
        //                    {
        //                        tclSocket.Close();

        //                        Log.debug("Host communication Message", "Disconnecting due to timeout Unique ID", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);

        //                        //Console.WriteLine(DateTime.Now + " : Disconnecting due to timeout Unique ID: " + sUniqueID);
        //                        //log.write(DateTime.Now + " : Disconnecting due to timeout Unique ID: " + sUniqueID);
        //                    }
        //                }
        //                else
        //                {
        //                    isEmpty = true;
        //                    emptyTime = DateTime.Now;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Console.WriteLine(DateTime.Now + " : Exception: " + e.ToString());
        //        //log.write(DateTime.Now + " : Exception: " + e.ToString());
        //        if (tclSocket != null)
        //            if (tclSocket.Connected)
        //                tclSocket.Close();
        //        ObjData.Status = "107";
        //        ObjData.Description = "HostCommunicator  failed";
        //        Log.error(e, "Host communication Message: TCP connection closed", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);

        //    }

        //}

        public void SendRequest(APIMessage ObjAPIRequest, APIResponseObject DataAPIRspObject, ConfigDataObject ObjData)
        {
            Socket client = null;
            bool isEmpty = false;
            DateTime emptyTime = DateTime.Now;

            // Read Server IP & Port


            try
            {
                //Log.debug("Host communication Message", "TCP Connection started", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + Convert.ToString(ObjAPIRequest.Code) + " | TCP Connection started ", ObjData.IssuerNo.ToString(), 1);

                IPAddress ipAddress = IPAddress.Parse(Convert.ToString(ObjAPIRequest.ServerIP));
                IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(ObjAPIRequest.Port));
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Connect to the remote endpoint.  
                client.BeginConnect(serverEndPoint, new AsyncCallback(ConnectCallback), client);
                int Wait = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RBLCardGeneration_Wait"].ToString()) * 1000;

                //int wait = Convert.ToInt32(AppData.LstMasParamModel.Where(rec => rec.ParamName.ToLower() == "hostsessiontimeout").First().ParamValue);
                connectDone.WaitOne(Wait);
                if (ObjAPIRequest.RequestMessage != null)
                {
                    Byte[] byRequest = ISGenerixLib.stringToByteArray(ObjAPIRequest.RequestMessage);

                    new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + Convert.ToString(ObjAPIRequest.Code) + " | TCP Connection started to send request", ObjData.IssuerNo.ToString(), 1);

                    Send(client, byRequest);
                    sendDone.WaitOne(Wait);


                    Receive(client);
                    receiveDone.WaitOne(Wait);

                    //if (client != null)
                    //    if (client.Connected)
                    //        client.Close();


                    if (Flag == 0)
                    {
                        DataAPIRspObject.ResponseMessage = null;
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + Convert.ToString(ObjAPIRequest.Code) + " | No TCP ResponseMessage", ObjData.IssuerNo.ToString(), 1);

                        //Log.debug("Host communication Message: Response set", ObjData.HostResMessage, ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
                    }
                    else
                    {

                        DataAPIRspObject.ResponseMessage = ISGenerixLib.byteToHex(response);
                        new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId, "", " Record_Id: " + Convert.ToString(ObjAPIRequest.Code) + " | TCP ResponseMessage:" + DataAPIRspObject.ResponseMessage, ObjData.IssuerNo.ToString(), 1);

                        //Log.debug("Host communication Message: Response set", ObjData.HostResMessage, ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
                    }
                }
            }
            catch (Exception e)
            {

                if (client != null)
                    if (client.Connected)
                        client.Close();
                ObjData.StepStatus = true;
                ObjData.ErrorDesc = "ISO Call failed";
                DataAPIRspObject.ResponseMessage = null;
                //Log.error(e, "Host communication Message: TCP connection closed", ObjData.RequestId, ObjData.SourceId, ObjData.TranType);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, ObjData.FileID, ObjData.ProcessId.ToString(), "TCP connection closed, Exception: " + e.ToString(), " Record_Id: " + Convert.ToString(ObjAPIRequest.Code), ObjData.IssuerNo.ToString(), 0);

            }
            finally
            {
                if (client != null)
                    if (client.Connected)
                        client.Close();
            }

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //Log.debug("Host communication Message", "ConnectCallback Started", "", "", "");
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "ConnectCallback Started", "1", 1);


                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                //Console.WriteLine("Socket connected to {0}",
                // client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                //Log.error(e, " Error while ConnectCallback", "", "", "");

                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Exception: " + e.ToString(), "", "1", 0);

            }
        }
        private void Send(Socket client, byte[] request)
        {
            // Convert the string data to byte data using ASCII encoding.  
            //byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Console.WriteLine("Sent Time." + DateTime.Now);
            // Begin sending the data to the remote device.  
            try
            {
                client.BeginSend(request, 0, request.Length, 0,
                    new AsyncCallback(SendCallback), client);
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "BeginSend request", "1", 1);
            }
            catch (Exception e)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Error while BeginSend request", "", "1", 0);
            }
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {

                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "SendCallback Started", "1", 1);
                //Log.debug("Host communication Message", "SendCallback Started", "", "", "");
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                // Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                //Log.error(e, " Error while SendCallback", "", "", "");
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Error while SendCallback", "", "1", 0);
            }
        }
        private void Receive(Socket client)
        {
            try
            {
                //Log.debug("Host communication Message", "Receive Started", "", "", "");
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "Receive Started", "1", 1);
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;
                AsyncCallback callBack = new AsyncCallback(ReceiveCallback);

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);

            }
            catch (Exception e)
            {
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Error while Receive", "", "1", 0);
                //Log.error(e, " Error while Receive", "", "", "");
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "", "ReceiveCallback Started", "1", 1);



                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    string strEncoding = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    //state.sb.Append(ISGenerixLib.byteToHex(state.buffer));

                    // Console.WriteLine("Rec Time." + DateTime.Now);
                    // Get the rest of the data.  
                    //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReceiveCallback), state);


                    Flag = 1;
                    response = state.buffer;
                    // Signal that all bytes have been received.  

                    receiveDone.Set();
                }
                //else
                //{

                //Flag = 1;
                //response = state.buffer;
                // Signal that all bytes have been received.  

                //receiveDone.Set();
                //}
            }
            catch (Exception ex)
            {
                //Log.error(e, " Error while ReceiveCallback", "", "", "");
                new ModuleDAL().FunInsertIntoErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "Error while ReceiveCallback:" + ex.ToString(), "", "1", 0);
            }
        }

    }
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
}
