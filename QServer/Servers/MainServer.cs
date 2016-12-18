using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Timers;
using QServer.Network;
//using System.Windows.Forms;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
/* Done
 * A lobby with chat
 * A base gameroom
 * Login system without database connections
 * Multi client
 * Log into a gameroom
 * See game rooms
 * 
 * Todo
 * Create game rooms
 * Connect to a database
 * Show connected users
 * Show game rooms
 * Join game rooms
 * Fix gameroom chat. Make the gameroom work!
 * Create a base room class. (Lobby and gameroom seem to have the same things!)
 */
namespace QServer.Servers
{
    //A struct that will contain all kind of things that winforms need.
    //Not sure why. I had things in my mind here!
    public struct WinformsThings
    {
        public string counter;//Amount of users connected
    }
    class MainServer:Server
    {
        private Listener listener;//Listener. Our server has to listen to a port for connections
        private WinformsThings winformsThings;//well just what it say's 
        private Login login;
        private int maxCount = 10000;
        private int disCount = 0;
        public MainServer():base()
        {
            //Init!
            winformsThings = new WinformsThings();
            login = new Login();
            listener = new Listener(9650);//Listen to port 9650,  because we can.
            //Listen to the event "socketAccepted" and handle it.
            listener.socketAccepted += new Listener.SocketAcceptedHandler(listener_SocketAccepted);
            //Start listening.
            listener.Start();
            /*
            System.Timers.Timer sweepTimer = new System.Timers.Timer(5000);
            sweepTimer.AutoReset = true;
            sweepTimer.Elapsed += sweepTimer_Elapsed;
            sweepTimer.Start();*/
        }

        private void sweepTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Sweep();
        }
        private void Sweep()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i] == null)
                {
                    clients.RemoveAt(i);
                    i--;
                }
                else if (clients[i].isClosed)
                {
                    clients.RemoveAt(i);
                    i--;
                }
            }
            Eutils.WriteLine("Clients Sweeped", Eutils.MESSSAGE_TYPE.NORMAL);
            UpdateStats();
        }
        private void UpdateStats()
        {
            winformsThings.counter = "Count:" + clients.Count + ":" + disCount;
            if (countChanged != null)//if it doesn't excist we won't edit the counter
            {
                countChanged(winformsThings);///Edit the counter.
            }
        }
        protected override void visitor_disconnected(Client sender)
        {
            disCount++;
            base.visitor_disconnected(sender);
            Eutils.WriteLine("Client Disconnected: {0}", Eutils.MESSSAGE_TYPE.NORMAL, sender.ID);
            UpdateStats();
            Sweep();
        }
        //A client got connected! Aka socket accepted!
        private void listener_SocketAccepted(Socket e)
        {
            Client client = new Client(e);//it is a client.
            Eutils.WriteLine("Client Connected: {0}", Eutils.MESSSAGE_TYPE.NORMAL, client.ID);//Client.ID is random, every client is unique
            AddClient(client);
            if (clients.Count <= maxCount)
            {
                login.AddClient(client);
                client.received -= new Client.ClientReceivedHandler(client_received);
                UpdateStats();
            }
            else
            {
                client.Close();
            }
        }
        //The big received function.
        protected override void client_received(Client sender, string[] packetStrings)
        {
            //So what kind of packet is it?
            switch (packetStrings[0])
            {
                default:
                    Eutils.WriteLine("main server Error wrong packet! {0}", Eutils.MESSSAGE_TYPE.WARNING);
                    break;
            }
        }
        //DC all sockets!
        public void CloseAllSockets()
        {
            string dcAllPackage = PacketDatas.PACKET_FORCE_DC + PacketDatas.PACKET_SPLIT + "DC all command";
            dcAllPackage = QEncryption.Encrypt(dcAllPackage);
            while(clients.Count != 0)
            {
                //clients[0].Close();
                
                if (clients[0] == null || clients[0].isClosed)
                {
                    clients.RemoveAt(0);
                }
                else
                {
                    clients[0].Close();
                }
            }
        }
        //Our countChanged event.
        public delegate void CountChanged(WinformsThings e);//The "Function" 
        public event CountChanged countChanged;//The event itself. We use countChanged += new CountChanged(FUNCTION NAME);
    }
}
