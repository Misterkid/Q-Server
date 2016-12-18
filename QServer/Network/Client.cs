using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
namespace QServer.Network
{
    class Client
    {
        private const int DATA_LENGTH = 512;

        public string ID;//client ID
        public IPEndPoint ipEndpoint;//Clients IP
        public bool isLoggedIn;//IS the client logged in?
        public string userName;//The username
        private Socket socket;//Da socket
        public bool isClosed = false;
        public Client(Socket acceptedSocket)
        {
            socket = acceptedSocket;//The clients socked is ofcourse the one that got accepted.
            ID = Guid.NewGuid().ToString();//Random!
            isLoggedIn = false;//We have to login first
            userName = "";//we don't know this.
            ipEndpoint = (IPEndPoint)socket.RemoteEndPoint;// The IP

            //the socket can start receiving. in a asynchronous way.
            //receiveCallback is the function!
            //Faster,better
            // Create the state object.
            StateObject state = new StateObject();
            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,new AsyncCallback(ReceiveCallback), state);
            //Slower
           // StartReceive();
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                if(isClosed || socket == null || !socket.Connected || socket.Available != 0 || socket.Poll(0,SelectMode.SelectRead))
                {
                    Close();
                    return;
                }
                // Read data from the client socket. 
                int bytesRead = socket.EndReceive(ar);
                //More data
                if (bytesRead > 0)
                {
                    state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    string packetData = state.stringBuilder.ToString();
                    state = new StateObject();//reset
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    OnReceive(packetData);
                }
            }
            catch(Exception e)
            {
                Eutils.FileError(String.Format("{0} {1} \n", e.Message, e.StackTrace), "client_error.txt");
                Eutils.WriteLine("Error receiving:{0} {1}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
                Close();
            }
        }
        private void StartReceive()
        {
            new Thread(delegate()
            {
                byte[] buffer = new byte[DATA_LENGTH];
                while (true)
                {
                    try
                    {
                        if (isClosed || !socket.Connected)
                        {
                            //Close();
                            return;
                        }
                        buffer = new byte[socket.Available];
                        int length = socket.Receive(buffer, buffer.Length, SocketFlags.None);

                        if (length <= 0)
                        {
                            continue;
                        }
                        string packetString = Encoding.Default.GetString(buffer);
                        OnReceive(packetString);
                        //Thread.Sleep(1);
                    }
                    catch(Exception e)
                    {
                        Eutils.FileError(String.Format("{0} {1} \n", e.Message, e.StackTrace), "client_error.txt");
                        Eutils.WriteLine("Error Receiving! Message:{0} Stacktrace: {1}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
                        Close();
                        break;
                    }
                }
            }).Start();
        }

        private void OnReceive(string packetString)
        {
            //Then we send this.
            if (packetString[0].ToString() != PacketDatas.PACKET_HEADER)
            {
                Close();
                return;
            }
            string[] packets = packetString.Split(new string[] { PacketDatas.PACKET_HEADER }, StringSplitOptions.None);
            for (int i = 1; i < packets.Length; i++)
            {
                //Eutils.WriteLine("received data:{0} at Time:{1} Length:{2}", Eutils.MESSSAGE_TYPE.NORMAL, packets[i], DateTime.Now, packets[i].Length);
                packets[i] = QEncryption.Decrypt(packets[i]);
                Eutils.WriteLine("decrypted data:{0} at Time:{1} Length:{2}", Eutils.MESSSAGE_TYPE.NORMAL, packets[i], DateTime.Now, packets[i].Length);
                if (packets[i] == "NULL")
                {
                    Eutils.FileError(String.Format("{0} \n", "Error decrypting: packets[i] == NULL"), "client_error.txt");
                    Eutils.WriteLine("Error decrypting", Eutils.MESSSAGE_TYPE.ERROR);
                    //Close();
                    break;
                }
                //Split the packet by our split character.
                string[] packetStrings = packets[i].Split(new string[] { PacketDatas.PACKET_SPLIT }, StringSplitOptions.None);
                received(this, packetStrings);
            }
        }

        //the string data we are going to send.
        public void SendWEncrypt(string encryptedData)
        {
            if (isClosed)
                return;
            if (socket == null || !socket.Connected)
            {
                Close();
                return;
            }

            //We us UTF8 characterss so lets convert the bytes into utf8 string!
           // encryptedData = PacketDatas.PACKET_HEADER + encryptedData;
            string dataToSend = PacketDatas.PACKET_HEADER + encryptedData;
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            //And then send it to our client.
            try
            {
                socket.Send(dataBytes, SocketFlags.None);
            }
            catch (Exception e)
            {
                Eutils.FileError(String.Format("{0} {1} \n", e.Message, e.StackTrace), "client_error.txt");
                Eutils.WriteLine("Error SendWEncrypt! Message:{0}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
                Close();
            }
        }
        public void Send(string dataToSend)
        {
            if (isClosed)
                return;
            if (socket == null || !socket.Connected)
            {
                Close();
                return;
            }
            
            dataToSend = QEncryption.Encrypt(dataToSend);
            dataToSend = PacketDatas.PACKET_HEADER + dataToSend;
            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            //And then send it to our client.
            try
            {
                socket.Send(dataBytes, SocketFlags.None);
            }
            catch(Exception e)
            {
                Eutils.FileError(String.Format("{0} {1} \n", e.Message, e.StackTrace), "client_error.txt");
                Eutils.WriteLine("Error Sending! Message:{0}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
                Close();
            }
        }
        //Lets clossssssssse
        public void Close()
        {
            if (isClosed)
            {
                //Eutils.WriteLine("Error socket is already closed:{0}", Eutils.MESSSAGE_TYPE.ERROR, ID);
                return;
            }
            ClientDisconnectedHandler disEvent = disconnected;
            if (disEvent != null)
            {
                disEvent(this);
            }
            if (socket != null)
            {
                socket.Close();//Close!
            }

            isClosed = true;
        }
        //Client receive event function. We need to know who and what data we get!
        public delegate void ClientReceivedHandler(Client sender, string[] data);
        //Disconnect event function! we only need to know who dissconnected.
        public delegate void ClientDisconnectedHandler(Client sender);
        //Event to listen to.
        public event ClientReceivedHandler received;
        public event ClientDisconnectedHandler disconnected;
    }

    // State object for reading client data asynchronously

    public class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder stringBuilder = new StringBuilder();
    }
}
