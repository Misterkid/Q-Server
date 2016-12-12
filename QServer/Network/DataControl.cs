using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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
namespace QServer.Network//NAME SPACEEEEEEEEEEEEEEE!!!
{
    //A struct that will contain all kind of things that winforms need.
    //Not sure why. I had things in my mind here!
    public struct WinformsThings
    {
        public string counter;//Amount of users connected
    }
    class DataControl//Finally!
    {
        private Listener listener;//Listener. Our server has to listen to a port for connections
        private List<Client> clients;//All CONNECTED! Clients.

        private Lobby lobby;//The lobby.
        WinformsThings winformsThings;//well just what it say's 
        public DataControl()
        {
            listener = new Listener(9650);//Listen to port 9650,  because we can.
            //Listen to the event "socketAccepted" and handle it.
            listener.socketAccepted += new Listener.SocketAcceptedHandler(listener_SocketAccepted);
            //Start listening.
            listener.Start();
            //Init!
            clients = new List<Client>();
            winformsThings = new WinformsThings();
            lobby = new Lobby();
        }
        //A client got connected! Aka socket accepted!
        private void listener_SocketAccepted(Socket e)
        {
            Client client = new Client(e);//it is a client.
            Eutils.WriteLine("Client Connected: {0}", client.ID);//Client.ID is random, every client is unique
            //Listen to received. The data we get from the client.
            client.received += new Client.ClientReceivedHandler(client_received);
            //Listen if the client got disconnected.
            client.disconnected += new Client.ClientDisconnectedHandler(client_disconnected);
        }
        //This function removes a client. We need to know what client we will remove.
        private void RemoveClient(Client removedClient)
        {
            clients.Remove(removedClient);
            removedClient = null;
            winformsThings.counter = "Count:" + GetOnlineCount();//counter text!
            if (countChanged != null)//if it doesn't excist we won't edit the counter
            {
                countChanged(winformsThings);///Edit the counter.
            }
        }
        //The DC function
        private void client_disconnected(Client sender)
        {
            //PRINT!
            Eutils.WriteLine("Client Disconnected: {0}", sender.ID);
            RemoveClient(sender);//Remove the client
            sender.disconnected -= new Client.ClientDisconnectedHandler(client_disconnected);
        }
        //The amount of clients that is connected. Uhm...
        private int GetOnlineCount()
        {
            return clients.Count;
        }
        //The big received function.
        private void client_received(Client sender, string[] packetStrings)
        {
            //So what kind of packet is it?
            switch (packetStrings[1])
            {
                case PacketDatas.PACKET_LOGIN://Login packet! we log in.
                    DoLogin(sender, packetStrings);//Do this function.
                    break;
                default:
                    //Wrong packet type. We close the connection. Could be a wrong client or a hacker or a bug.
                    sender.Close();
                    break;
            }
        }
        //The login function.
        private void DoLogin(Client sender, String[] packetStrings)
        {
            //Todo
            //Database connection for logging in.
            //Detect sql injections
            //Detect Invalid character
            //Both on true. :D always correct.
            bool userCorrect = true;//Correct
            bool passCorrect = true;//correct
            //Everything needs to be right. If something is wrong then goodbye.
            if (!userCorrect || !passCorrect || packetStrings[2].Length < 1|| packetStrings[3].Length < 1)
            {
                //<3 the first packed you see!
                //We send a error packet.
                sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_ERROR + PacketDatas.PACKET_SPLIT + "Invalid username or password");

                Eutils.WriteLine("Login in failed by ip: {0}", sender.ipEndpoint.Address);
                sender.Close();//DC
                return;
            }
            //Add the client to clients.
            clients.Add(sender);

            //This is repeated! I need a new function for this.
            winformsThings.counter = "Count:" + GetOnlineCount();//counter text!
            //Update the counter if not null.
            if (countChanged != null)
            {
                countChanged(winformsThings);
            }
            //Set the username
            sender.userName = packetStrings[2];
            //We are logged in
            sender.isLoggedIn = true;
            //Login succes! We just send a login packet back with nothing but a type.
            sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_LOGIN);
            Eutils.WriteLine("Login in:{0} with password:{1} ip: {2}", packetStrings[2], packetStrings[3], sender.ipEndpoint.Address);
            //We remove the receive event to so the lobby can handle the receiving now.
           // sender.received -= client_received;
            sender.received -= new Client.ClientReceivedHandler(client_received);
            //Add client to the lobby.
            lobby.AddClient(sender);
            //Remove client from here?
            //RemoveClient(sender);
            //Lets not do that. (Got to fix this!)
        }
        //DC all sockets!
        public void CloseAllSockets()
        {
            lobby.DCAll();
            while(clients.Count != 0)
            {
                if (clients[0] != null)
                {
                    clients[0].Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_FORCE_DC + PacketDatas.PACKET_SPLIT + "DC all command");
                    clients[0].Close();
                }
            }
        }
        //Our countChanged event.
        public delegate void CountChanged(WinformsThings e);//The "Function" 
        public event CountChanged countChanged;//The event itself. We use countChanged += new CountChanged(FUNCTION NAME);
    }
}
