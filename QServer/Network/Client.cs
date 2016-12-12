using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private Socket sSocket;//Da socket
        public Client(Socket acceptedSocket)
        {
            sSocket = acceptedSocket;//The clients socked is ofcourse the one that got accepted.
            ID = Guid.NewGuid().ToString();//Random!
            isLoggedIn = false;//We have to login first
            userName = "";//we don't know this.
            ipEndpoint = (IPEndPoint)sSocket.RemoteEndPoint;// The IP

            //the socket can start receiving. in a asynchronous way.
            //receiveCallback is the function!
            //sSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, receiveCallback, null);

            StartReceive();
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
                        int length = sSocket.Receive(buffer, buffer.Length,SocketFlags.None);
                        if (length <= 0)
                        {
                            //Close();
                            //break;
                            continue;
                        }
                        if (length < buffer.Length)
                        {
                            Array.Resize<byte>(ref buffer, length);
                        }
                        OnReceive(buffer);
                    }
                    catch(Exception e)
                    {
                        Eutils.WriteLine(e.Message);
                        break;
                    }
                }
            }).Start();
        }
        private void OnReceive(byte[] buffer)
        {
            //Then we send this.
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
            received(this, packetStrings);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                sSocket.EndReceive(ar);//Lets end receiving. 
                byte[] buffer = new byte[1024];//the buffer. only 1024 bytes long? That is not much.
                int recCount = sSocket.Receive(buffer, buffer.Length, 0);//The amount of received bytes.
                if (recCount <= 0)
                {
                    //if we get nothing then why? DC!
                    Close();
                    return;
                }
                //Lets resize the buffer to a good ammount. aslong as it isn't over the length
                if (recCount < buffer.Length)
                {
                    Array.Resize<byte>(ref buffer, recCount);
                }
                //Is there someone listening to our received event?
                if (received != null)
                {
                    OnReceive(buffer);
                }
                //Start receiving again.
                //Hey! its a loop.
                sSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Eutils.WriteLine("Error receiving:{0} StackTrace:{1} ", ex.Message, ex.StackTrace);
                Close();
            }
        }
        //the string data we are going to send.
        public void SendWEncrypt(string encryptedData)
        {
            if (sSocket == null || !sSocket.Connected)
                return;

            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            byte[] dataBytes = Encoding.UTF8.GetBytes(encryptedData);
            //And then send it to our client.
            try
            {
                sSocket.Send(dataBytes, SocketFlags.None);
            }
            catch (Exception e)
            {
                Eutils.WriteLine(e.Message);
            }
        }
        public void Send(string dataToSend)
        {
            if (sSocket == null || !sSocket.Connected)
                return;

            dataToSend = QEncryption.Encrypt(dataToSend);
            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            //And then send it to our client.
            try
            {
                sSocket.Send(dataBytes, SocketFlags.None);
            }
            catch(Exception e)
            {
                Eutils.WriteLine(e.Message);
            }
        }
        //Lets clossssssssse
        public void Close()
        {
            if (sSocket != null)//There is no socket connection? How can we even close.
            {
                sSocket.Close();//Close!
                sSocket.Dispose();//Dispose!

                if (disconnected != null)//is someone listening to our DC event?
                {
                    //If so we send this to our listener!
                    disconnected(this);
                }
                sSocket = null;
            }
        }
        //Client receive event function. We need to know who and what data we get!
        public delegate void ClientReceivedHandler(Client sender, string[] data);
        //Disconnect event function! we only need to know who dissconnected.
        public delegate void ClientDisconnectedHandler(Client sender);
        //Event to listen to.
        public event ClientReceivedHandler received;
        public event ClientDisconnectedHandler disconnected;
    }
}
