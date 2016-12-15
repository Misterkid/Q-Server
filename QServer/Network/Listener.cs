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

    class Listener
    {
        private Socket sSocket;//Socket!

        public bool bListening;//Are we listening?
        public int nPort;//The port.

        public Listener(int lPort)
        {
            nPort = lPort;//Port is the port we got.
            //Lets create our socket
            sSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            sSocket.NoDelay = true;

        }

        public void Start()
        {
            //Are we already listening?
            if(bListening)
                return;//then we shouldn't listen again

            //Lets bind! we are going to bind on a port!
            sSocket.Bind(new IPEndPoint(0,nPort));
            //Now we are going to listen.
            sSocket.Listen(0);
            //asynchronous accepting clients.
            StartListening();
            //sSocket.BeginAccept(beginAcceptCallback,null);
            bListening = true;//We are ready and listening.

            Eutils.WriteLine("Server is Listening on port {0}", Eutils.MESSSAGE_TYPE.NORMAL, nPort);
        }
        private void StartListening()
        {
            new Thread(delegate()
            {
                while (true)
                {
                    try
                    {
                        Socket socket = sSocket.Accept();
                        socket.NoDelay = true;
                        socketAccepted(socket);
                    }
                    catch (Exception e)
                    {
                        Eutils.WriteLine("Error Accepting! Message:{0}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
                        continue;
                    }
                }

            }).Start();
        }
        //Stop listening
        public void Stop()
        {
            if(!bListening)
                return;//we already stopped so no need for this.

            sSocket.Close();//Close the socket.
            sSocket.Dispose();//Dispose it.
            //Make a new one.
            sSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        }
        //when a client got accepted this happends.
        void beginAcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = sSocket.EndAccept(ar);//Lets get the clients socket!
                if(socketAccepted != null)//if no one is listening then we can't send it.
                {
                    //Lets send this event with the clients socket
                    socketAccepted(s);
                }
                //And we start over again. 
                //A loop!
                sSocket.BeginAccept(beginAcceptCallback, null);
            }
            catch(Exception e)
            {
                Eutils.WriteLine("Error accepting! Message:{0}", Eutils.MESSSAGE_TYPE.ERROR, e.Message, e.StackTrace);
            }
        }
        //Socket accepted event!
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler socketAccepted;
    }
}
