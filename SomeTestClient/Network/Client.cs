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
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipAddr, port);
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, receiveCallback, null);
                socket.NoDelay = true;
            }
            catch(Exception e)
            {
                
            }
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
                    OnReceive(buffer);
                    /*
                    string packetString = Encoding.Default.GetString(buffer);
                    packetString = QEncryption.Decrypt(packetString);
                    Console.WriteLine("received data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
                    string[] packetStrings = packetString.Split(PacketDatas.PACKET_SPLIT[0]);
                    if (packetStrings[0] != PacketDatas.PACKET_HEADER)//Is the packet header correct?
                    {
                        Close();
                        return;
                    }
                    receive(this, packetStrings);*/
                }
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, receiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving:{0} StackTrace:{1} ", ex.Message, ex.StackTrace);
                Close();
            }
        }
        private void OnReceive(byte[] buffer)
        {
            //Then we send this.

            string packetString = Encoding.Default.GetString(buffer);
            if (packetString[0].ToString() != PacketDatas.PACKET_HEADER)
            {
                Close();
                return;
            }
            string[] packets = packetString.Split(new string[] { PacketDatas.PACKET_HEADER }, StringSplitOptions.None);
            for (int i = 1; i < packets.Length; i++)
            {
                packets[i] = QEncryption.Decrypt(packets[i]);
                //Split the packet by our split character.
                string[] packetStrings = packets[i].Split(new string[] { PacketDatas.PACKET_SPLIT }, StringSplitOptions.None);
                receive(this, packetStrings);
            }
            /*
            
            string packetString = Encoding.Default.GetString(buffer);//The packet
            //packetString = packetString.Trim('\0');
            Eutils.WriteLine("received data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
            packetString = QEncryption.Decrypt(packetString);
            Eutils.WriteLine("received data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
            //Split the packet by our split character.
            string[] packetStrings = packetString.Split(new string[] { PacketDatas.PACKET_SPLIT }, StringSplitOptions.None);
            if (packetStrings[0] != PacketDatas.PACKET_HEADER)//Is the packet header correct?
            {
                Close();
                return;
            }
            received(this, packetStrings);*/

        }
        public void SendWEncrypt(string encryptedData)
        {
            if (socket == null || !socket.Connected)
                return;

            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            encryptedData = PacketDatas.PACKET_HEADER + encryptedData;
            byte[] dataBytes = Encoding.UTF8.GetBytes(encryptedData);
            //And then send it to our client.
            try
            {
                socket.Send(dataBytes, SocketFlags.None);
            }
            catch (Exception e)
            {
                Close();
            }
        }
        public void Send(string dataToSend)
        {
            if (socket == null || !socket.Connected)
                return;

            dataToSend = QEncryption.Encrypt(dataToSend);
            dataToSend = PacketDatas.PACKET_HEADER + dataToSend;
            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            //And then send it to our client.
            try
            {
                socket.Send(dataBytes, SocketFlags.None);
            }
            catch (Exception e)
            {
                Close();
            }
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
