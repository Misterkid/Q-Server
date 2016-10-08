using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QServer.Network;

namespace QServer
{
    class GameRoom
    {
        private List<Client> clients = new List<Client>();
        private Client gameroomOwner;
        private bool gameStarted = false;
        private Lobby lobby;
        public string gameRoomName = "";
        public GameRoom(Client owner,Lobby lLobby,string name = "null")
        {
            lobby = lLobby;
            gameroomOwner = owner;
            gameRoomName = name;
            Console.WriteLine("Created Gameroom {0}", name);
        }

        public void AddClient(Client visitor)
        {
            visitor.recieved += new Client.ClientRecievedHandler(client_recieved);
            clients.Add(visitor);
        }
        public void RemoveClient(Client visitor)
        {
            if (visitor.isLoggedIn)
            {
                List<Client> newClientList = new List<Client>();
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i] != visitor)
                    {
                        newClientList.Add(clients[i]);
                    }
                }
                clients.Clear();
                clients = null;
                clients = newClientList;
            }
            visitor = null;
        }
        private void client_recieved(Client sender, byte[] data)
        {
            string packetString = Encoding.Default.GetString(data);
            Console.WriteLine("Recieved data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);

            string[] packetStrings = packetString.Split(PacketDatas.PACKET_SPLIT[0]);
            if (packetStrings[0] == PacketDatas.PACKET_HEADER)
            {
                if (packetStrings[1] != PacketDatas.LOGIN_PACKET && sender.isLoggedIn == false)
                {
                    sender.Close();
                    return;
                }
                switch (packetStrings[1])
                {
                    case PacketDatas.PACKET_CHAT:
                        DoChat(sender, packetStrings);
                        break;

                    case PacketDatas.PACKET_GAME_START:
                        DoStartGame(sender, packetStrings);
                        break;
                    default:
                        Console.WriteLine("Error wrong packet! {0}", packetStrings[1]);
                        //sender.Close();
                        break;
                }
            }
            else
            {
                sender.Close();
                return;
            }
        }
        private void DoStartGame(Client sender, String[] packetStrings)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                Console.WriteLine("Game Started");
            }
        }
        private void DoDestroyRoom(Client sender, String[] packetStrings)
        {
            Console.WriteLine("Game Destroyed");
            for (int i = 0; i < clients.Count; i++)
            {
                lobby.AddClient(clients[i]);
                this.RemoveClient(clients[i]);
                clients[i].recieved -= client_recieved;
            }
            lobby.AddClient(gameroomOwner);
            gameroomOwner.recieved -= client_recieved;

            lobby.DestroyGameRoom(this);
        }
        private void DoEndGame(Client sender, String[] packetStrings)
        {
            if (gameStarted)
            {
                gameStarted = false;
                Console.WriteLine("Game Ended");
            }
        }
        private void DoChat(Client sender, String[] packetStrings)
        {
            for (int i = 0; i < packetStrings.Length; i++)
            {
                if (i > 2)
                {
                    packetStrings[2] += PacketDatas.PACKET_SPLIT;
                    packetStrings[2] += packetStrings[i];
                }
            }
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[2]);
            }
        }
    }
}
