using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class LogFetchingService
    {
    }
}


//using System;
//using System.Net;
//using System.Net.NetworkInformation;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;

//public class Program
//{
//    const string SUBSCRIBE_REQUEST = "hello, will you produce logs?";
//    const string SUBSCRIBE_RESPONSE = "yes, I will."; // with pid, pname, logfile and logcount info appended
//    const string UNSUBSCRIBE_MSG = "goodbye!";
//    const string CLOSED_MSG = "UDP server is closed.";
//    const string HEART_BEAT = "HEART_BEAT";
//    public static void Main(string[] args)
//    {
//        try
//        {
//            byte[] requestData = Encoding.UTF8.GetBytes(SUBSCRIBE_REQUEST);
//            UdpClient[] udpClients = Enumerable.Range(0, 20)
//                .Select(x => new UdpClient())
//                .ToArray();
//            Task.Run(async () =>
//            {
//                while (true)
//                {
//                    for (int i = 0; i < udpClients.Length; i++)
//                    {
//                        IPAddress ipAddress = IPAddress.Loopback;
//                        int port = 2020 + i;
//                        var serverEP = new IPEndPoint(ipAddress, port);
//                        var udpClient = udpClients[i];
//                        if (udpClient.Client.Connected)
//                        {
//                            continue;
//                        }
//                        var sendBufferLength = await udpClient.SendAsync(requestData, requestData.Length, serverEP);
//                        try
//                        {
//                            var responseData = await udpClient.ReceiveAsync();
//                            string responseMessage = Encoding.UTF8.GetString(responseData.Buffer);
//                            if (responseMessage.StartsWith(SUBSCRIBE_RESPONSE))
//                            {
//                                try
//                                {
//                                    Console.WriteLine("Received from server: " + responseMessage);
//                                    udpClient.Connect(serverEP);
//                                    udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), udpClient);
//                                }
//                                catch (Exception ex)
//                                {

//                                }
//                            }
//                        }
//                        catch
//                        {

//                        }
//                    }
//                }
//            });
//            Console.ReadLine();

//        }
//        catch (Exception e)
//        {
//            Console.WriteLine("Exception: " + e.ToString());
//        }
//    }

//    private static void ReceiveCallback(IAsyncResult iar)
//    {
//        try
//        {
//            var udpClient = iar.AsyncState as UdpClient;
//            if (udpClient != null && udpClient.Client.Connected)
//            {
//                IPEndPoint? remoteEP = null;
//                byte[] data = udpClient.EndReceive(iar, ref remoteEP);
//                string responseMessage = Encoding.UTF8.GetString(data);
//                if (responseMessage.StartsWith(CLOSED_MSG))
//                {
//                }
//                if (responseMessage.StartsWith(HEART_BEAT))
//                {
//                }
//                Console.WriteLine("Received from server: " + responseMessage);
//                udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), udpClient);
//            }
//            else
//            {

//            }
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine("Exception: " + e.ToString());
//        }
//    }
//}