using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QServer.Network;

namespace QServer.Servers
{
    class GameRoom:Server
    {
        private Client gameroomOwner;
        private bool gameStarted = false;
        private Lobby lobby;
        private Game game;
        public string gameRoomName = "";
        public GameRoom(Client owner,Lobby lLobby,string name = "null"):base()
        {
            lobby = lLobby;
            gameroomOwner = owner;
            gameRoomName = name;
            Eutils.WriteLine("Created Gameroom {0}",Eutils.MESSSAGE_TYPE.NORMAL, name);
        }
        protected override void client_received(Client sender, string[] packetStrings)
        {
            switch (packetStrings[0])
            {
                case PacketDatas.PACKET_CHAT:
                    DoChat(sender, packetStrings);
                    break;

                case PacketDatas.PACKET_GAME_START:
                    DoStartGame(sender, packetStrings);
                    break;
                default:
                    Eutils.WriteLine("[Gameroom {1}] Error wrong packet! {0}", Eutils.MESSSAGE_TYPE.WARNING, packetStrings[1], gameRoomName);
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
                }
                Console.WriteLine("Game Started");
                RemoveAllClients();
            }
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
            string chatPackage = PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[1];
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].Send(chatPackage);
            }
        }
    }
}
