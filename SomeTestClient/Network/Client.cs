using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
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
            socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, recieveCallback, null);
        }
        private void recieveCallback(IAsyncResult ar)
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
                if (recieve != null)
                {
                    recieve(socket, buffer);
                }
                socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, recieveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Recieving:{0} StackTrace:{1} ", ex.Message, ex.StackTrace);
                Close();
            }
        }
        public void Send(string dataToSend)
        {
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
        public delegate void EventRecieve(Socket sender, byte[] data);
        public event EventRecieve recieve;
    }
}
