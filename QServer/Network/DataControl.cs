using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using System.Windows.Forms;

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
            Console.WriteLine("Client Connected: {0}", client.ID);//Client.ID is random, every client is unique
            //Listen to recieved. The data we get from the client.
            client.recieved += new Client.ClientRecievedHandler(client_recieved);
            //Listen if the client got disconnected.
            client.disconnected += new Client.ClientDisconnectedHandler(client_disconnected);
        }
        //This function removes a client. We need to know what client we will remove.
        private void RemoveClient(Client removedClient)
        {
            lobby.RemoveClient(removedClient);//The lobby doesn't need the client anymore.
            if (removedClient.isLoggedIn)//If logged in. (We don't need this o.O)
            {
                List<Client> newClientList = new List<Client>();//New client list.
                //Perhabs I should find a better way...
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i] != removedClient)
                    {
                        newClientList.Add(clients[i]);//add the clients into here. But not the one that will be deleted.
                    }
                }
                clients.Clear();//Remove the list
                clients = null;//Just to be sure.
                clients = newClientList;// it is now updated.
            }
            removedClient = null;//Just to be sure. We don't like memory leaks.
        }
        //The DC function
        private void client_disconnected(Client sender)
        {
            //PRINT!
            Console.WriteLine("Client Dissconnected: {0}", sender.ID);

            RemoveClient(sender);//Remove the client
            winformsThings.counter = "Count:" + GetOnlineCount();//counter text!
            if (countChanged != null)//if it doesn't excist we won't edit the counter
            {
                countChanged(winformsThings);///Edit the counter.
            }
        }
        //The amount of clients that is connected. Uhm...
        private int GetOnlineCount()
        {
            return clients.Count;
        }
        //The big recieved function.
        //I do want to find another way to do this but there might not be one ...
        private void client_recieved(Client sender, byte[] data)
        {
            string packetString = Encoding.Default.GetString(data);//The packet
            Console.WriteLine("Recieved data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
            //Split the packet by our split character.
            string[] packetStrings = packetString.Split(PacketDatas.PACKET_SPLIT[0]);

            if (packetStrings[0] == PacketDatas.PACKET_HEADER)//Is the packet header correct?
            {
                /*
                //Is the sender doing something else then logging in ?
                if (packetStrings[1] != PacketDatas.LOGIN_PACKET && sender.isLoggedIn == false)
                {
                    //Must be a wrong client. Or a hacker.
                    sender.Close();
                    return;
                }
                 */ 
                //So what kind of packet is it?
                switch (packetStrings[1])
                {
                    case PacketDatas.LOGIN_PACKET://Login packet! we log in.
                        DoLogin(sender, packetStrings);//Do this function.
                        break;
                    default:
                        //Wrong packet type. We close the connection. Could be a wrong client or a hacker or a bug.
                        sender.Close();
                        break;
                }
            }
            else
            {
                //Same as the last comment
                sender.Close();
                return;
            }
        }
        //The login function.
        private void DoLogin(Client sender, String[] packetStrings)
        {
            //Todo
            //Database connection for logging in.
            //Detect sql injections
            //Detect Invalid character

            //My failed attempt :
            //Regex objAlphaPattern = new Regex(@"^[a-zA-Z0-9_@.-]*$");
            //bool userCorrect = objAlphaPattern.IsMatch(packetStrings[2]);
            //bool passCorrect = objAlphaPattern.IsMatch(packetStrings[3]);
            //Both on true. :D always correct.
            bool userCorrect = true;//Correct
            bool passCorrect = true;//correct
            //Everything needs to be right. If something is wrong then goodbye.
            if (!userCorrect || !passCorrect || packetStrings[2].Length < 1|| packetStrings[3].Length < 1)
            {
                //<3 the first packed you see!
                //We send a error packet.
                sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_ERROR + PacketDatas.PACKET_SPLIT + "Invalid username or password");
                
                Console.WriteLine("Login in failed by ip: {0}",sender.ipEndpoint.Address);
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
            sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.LOGIN_PACKET);
            Console.WriteLine("Login in:{0} with password:{1}", packetStrings[2], packetStrings[3]);
            //We remove the recieve event to so the lobby can handle the recieving now.
           // sender.recieved -= client_recieved;
            sender.recieved -= new Client.ClientRecievedHandler(client_recieved);
            //Add client to the lobby.
            lobby.AddClient(sender);
            //Remove client from here?
            //RemoveClient(sender);
            //Lets not do that. (Got to fix this!)
        }
        //DC all sockets!
        public void CloseAllSockets()
        {
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].Close();
            }
        }
        //Our countChanged event.
        public delegate void CountChanged(WinformsThings e);//The "Function" 
        public event CountChanged countChanged;//The event itself. We use countChanged += new CountChanged(FUNCTION NAME);
    }
}
