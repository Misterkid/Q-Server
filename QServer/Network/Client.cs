using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
namespace QServer.Network
{
    class Client
    {
        public string ID;//client ID
        public IPEndPoint ipEndpoint;//Clients IP
        public bool isLoggedIn;//IS the client logged in?
        public string userName;//The username
        Socket sSocket;//Da socket

        public Client(Socket acceptedSocket)
        {
            sSocket = acceptedSocket;//The clients socked is ofcourse the one that got accepted.
            ID = Guid.NewGuid().ToString();//Random!
            isLoggedIn = false;//We have to login first
            userName = "";//we don't know this.
            ipEndpoint = (IPEndPoint)sSocket.RemoteEndPoint;// The IP

            //the socket can start recieving. in a asynchronous way.
            //recieveCallback is the function!
            sSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, recieveCallback, null);
        }
        //The Data we get!
        void recieveCallback(IAsyncResult ar)
        {
            try
            {
                sSocket.EndReceive(ar);//Lets end recieving. 
                byte[] buffer = new byte[1024];//the buffer. only 1024 bytes long? That is not much.
                int recCount = sSocket.Receive(buffer, buffer.Length, 0);//The amount of recieved bytes.

                //recCount = recCount - 2;//A test client added 2 bytes....
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
                //Is there someone listening to our recieved event?
                if (recieved != null)
                {
                    //Then we send this.
                    recieved(this, buffer);
                }
                //Start receiving again.
                //Hey! its a loop.
                sSocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, recieveCallback, null);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Recieving:{0} StackTrace:{1} ",ex.Message,ex.StackTrace);
                Close();
            }
        }
        //the string data we are going to send.
        public void Send(string dataToSend)
        {
            //We us UTF8 characterss so lets convert the bytes into utf8 string!
            dataToSend = QEncryption.Encrypt(dataToSend);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
            //And then send it to our client.
            sSocket.Send(dataBytes,SocketFlags.None);
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
            }
        }
        //Client recieve event function. We need to know who and what data we get!
        public delegate void ClientRecievedHandler(Client sender,byte[] data);
        //Disconnect event function! we only need to know who dissconnected.
        public delegate void ClientDisconnectedHandler(Client sender);
        //Event to listen to.
        public event ClientRecievedHandler recieved;
        public event ClientDisconnectedHandler disconnected;
    }
}
