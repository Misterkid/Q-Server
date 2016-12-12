using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QServer.Network;

namespace QServer
{
    class Game
    {
        public List<Client> clients = new List<Client>();
        private GameRoom gameRoom;
        public Game(GameRoom gameRoom)
        {
            this.gameRoom = gameRoom;
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
                case PacketDatas.PACKET_GAME_STOP:
                    GameStop(sender, packetStrings);
                    break;
                default:
                    Console.WriteLine("[Gameroom InGame {1}] Error wrong packet! {0}", packetStrings[1], gameRoom.gameRoomName);
                    //sender.Close();
                    break;
            }
        }
        private void GameStop(Client sender, String[] packetStrings)
        {
            for(int i = 0; i < clients.Count; i++)
            {
                gameRoom.AddClient(clients[i]);
            }
            RemoveAllClients();
        }
        public void DCAll()
        {
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
