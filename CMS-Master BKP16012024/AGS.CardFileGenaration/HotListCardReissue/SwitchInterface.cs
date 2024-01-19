using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SwitchInterface
/// </summary>

public class SwitchInterface
{
    string ip = "";
    int port;
    DateTime emptyTime;
    TcpClient TcpSwitch = new TcpClient();
    Boolean processTran = true;
    byte[] response = null;
    int responseSize = 0;

	public SwitchInterface(string ip, int port)
	{
        this.ip = ip;
        this.port = port;
	}

    public byte[] sendDataToSwitch(byte[] request)
    {
        string returndata = string.Empty;
        
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            TcpSwitch.Connect(serverEndPoint);
        }
        catch (Exception e)
        {
            processTran = false;
            response = null;
            new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + e.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(DateTime.Now + ">> Unable to connect to the remote server. " + e.Message);
        }

        if (processTran)
        {
            try
            {
               
                if (request != null)
                {
                    TcpSwitch.GetStream().Write(request, 0, request.Length);
                    emptyTime = DateTime.Now;

                    while (true)
                    {
                        responseSize = TcpSwitch.Available;

                        if (responseSize > 0)
                        {
                            response = new byte[responseSize];
                            TcpSwitch.GetStream().Read(response, 0, responseSize);
                            break;
                        }
                        else
                        {

                            if ((DateTime.Now - emptyTime).Seconds >= 45)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    TcpSwitch.Close();
                    return null;
                }
            }
            catch (Exception xObj)
            {
                TcpSwitch.Close();
                response = null;
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + xObj.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Console.WriteLine(DateTime.Now + ">> " + xObj.Message);
            }
            TcpSwitch.Close();
            return response;
        }
        else
        {
            TcpSwitch.Close();
            return response;
        }
    }

    public byte[] sendDataToHsm(byte[] request)
    {
        string returndata = string.Empty;

        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            TcpSwitch.Connect(serverEndPoint);
        }
        catch (Exception e)
        {
            processTran = false;
            response = null;
            new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + e.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine(DateTime.Now + ">> Unable to connect to the remote server. " + e.Message);
        }

        if (processTran)
        {
            try
            {

                if (request != null)
                {
                    TcpSwitch.GetStream().Write(request, 0, request.Length);
                    emptyTime = DateTime.Now;

                    while (true)
                    {
                        responseSize = TcpSwitch.Available;

                        if (responseSize > 0)
                        {
                            response = new byte[responseSize];
                            TcpSwitch.GetStream().Read(response, 0, responseSize);
                            break;
                        }
                        else
                        {

                            if ((DateTime.Now - emptyTime).Seconds >= 45)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    TcpSwitch.Close();
                    return null;
                }
            }
            catch (Exception xObj)
            {
                TcpSwitch.Close();
                response = null;
                new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + xObj.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Console.WriteLine(DateTime.Now + ">> " + xObj.Message);
            }
            TcpSwitch.Close();
            return response;
        }
        else
        {
            TcpSwitch.Close();
            return response;
        }
    }

    public static byte[] StringToByteArray(string hex)
    {
        try
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        catch (Exception e)
        {
            Console.Write(DateTime.Now + ">> String to Byte array conversion error " + e.Message);
            new CardReissueDAL().InsertLog(DateTime.Now.ToString() + ">> Message # :  " + e.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            return null;
        }
    }
}