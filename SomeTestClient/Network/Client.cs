using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using QServer.Network;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
namespace SomeTestClient.Network
{
    public class Client
    {
        public int BUFFER_SIZE = 2049;
        private Socket socket;
        public void InitSocket(string ipAddr,int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipAddr, port);
            socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, receiveCallback, null);
        }
        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndReceive(ar);
                byte[] buffer = new byte[BUFFER_SIZE];
                int recCount = socket.Receive(buffer, buffer.Length, 0);

                if (recCount <= 0)
                {
                    Close();
                    return;
                }

                if (recCount < buffer.Length)
                {
                    Array.Resize<byte>(ref buffer, recCount);
                }
                if (receive != null)
                {
                    string packetString = Encoding.Default.GetString(buffer);
                    packetString = QEncryption.Decrypt(packetString);
                    Console.WriteLine("received data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
                    string[] packetStrings = packetString.Split(PacketDatas.PACKET_SPLIT[0]);
                    if (packetStrings[0] != PacketDatas.PACKET_HEADER)//Is the packet header correct?
                    {
                        Close();
                        return;
                    }
                    receive(this, packetStrings);
                }
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, receiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving:{0} StackTrace:{1} ", ex.Message, ex.StackTrace);
                Close();
            }
        }
        public void Send(string dataToSend)
        {
            dataToSend = QEncryption.Encrypt(dataToSend);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            socket.Send(dataBytes, SocketFlags.None);
        }
        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }
        public delegate void Eventreceive(Client sender, string[] data);
        public event Eventreceive receive;
    }
}
