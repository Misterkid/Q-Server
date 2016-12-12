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
        private Game game;
        public string gameRoomName = "";
        public GameRoom(Client owner,Lobby lLobby,string name = "null")
        {
            lobby = lLobby;
            gameroomOwner = owner;
            gameRoomName = name;
            Eutils.WriteLine("Created Gameroom {0}", name);
        }

        public void AddClient(Client visitor)
        {
            clients.Add(visitor);
            visitor.received += new Client.ClientReceivedHandler(client_received);
            visitor.disconnected += new Client.ClientDisconnectedHandler(visitor_disconnected);
        }

        void visitor_disconnected(Client sender)
        {
            RemoveClient(sender);
            sender.disconnected -= new Client.ClientDisconnectedHandler(visitor_disconnected);
        }
        public void RemoveClient(Client visitor)
        {
            visitor.received -= new Client.ClientReceivedHandler(client_received);
            clients.Remove(visitor);
            visitor = null;
        }
        private void client_received(Client sender, string[] packetStrings)
        {
            switch (packetStrings[1])
            {
                case PacketDatas.PACKET_CHAT:
                    DoChat(sender, packetStrings);
                    break;

                case PacketDatas.PACKET_GAME_START:
                    DoStartGame(sender, packetStrings);
                    break;
                default:
                    Eutils.WriteLine("[Gameroom {1}] Error wrong packet! {0}", packetStrings[1], gameRoomName);
                    //sender.Close();
                    break;
            }
        }
        private void DoStartGame(Client sender, String[] packetStrings)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                game = new Game(this);
                for(int i = 0; i < clients.Count; i++)
                {
                    game.AddClient(clients[i]);
                    //RemoveClient(clients[i]);

                }
                Console.WriteLine("Game Started");
                RemoveAllClients();
            }
        }
        public void DCAll()
        {
            if(game != null)
                game.DCAll();

            RemoveAllClients();
        }
        private void RemoveAllClients()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                RemoveClient(clients[i]);
                i--;
            }
        }
        private void DoDestroyRoom(Client sender, String[] packetStrings)
        {
            Console.WriteLine("Game Destroyed");
            for (int i = 0; i < clients.Count; i++)
            {
                lobby.AddClient(clients[i]);
                this.RemoveClient(clients[i]);
                clients[i].received -= client_received;
            }
            lobby.AddClient(gameroomOwner);
            gameroomOwner.received -= client_received;

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
