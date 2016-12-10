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
            visitor.recieved += new Client.ClientRecievedHandler(client_recieved);
        }
        public void RemoveClient(Client visitor)
        {
            visitor.recieved -= new Client.ClientRecievedHandler(client_recieved);
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
            Console.WriteLine("Recieved encrypted data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
            packetString = QEncryption.Decrypt(packetString);
            Console.WriteLine("[Gameroom InGame {3}] Recieved data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length, gameRoom.gameRoomName);
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
                    case PacketDatas.PACKET_GAME_STOP:
                        GameStop(sender, packetStrings);
                        break;
                    default:
                        Console.WriteLine("[Gameroom InGame {1}] Error wrong packet! {0}", packetStrings[1], gameRoom.gameRoomName);
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
        private void GameStop(Client sender, String[] packetStrings)
        {
            for(int i = 0; i < clients.Count; i++)
            {
                gameRoom.AddClient(clients[i]);
            }
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
