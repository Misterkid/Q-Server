using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using QServer.Network;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
namespace QServer
{
    class Lobby
    {
        public List<Client> clients;//All connected clients
        private List<GameRoom> gameRooms;//All game rooms that got created.
        public Lobby()
        {
            clients = new List<Client>();
            gameRooms = new List<GameRoom>();
        }
        public void AddClient(Client visitor)
        {
            visitor.received += new Client.ClientReceivedHandler(client_received);
            visitor.disconnected += new Client.ClientDisconnectedHandler(visitor_disconnected);
            clients.Add(visitor);
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
            new Thread(delegate() 
            { 
                switch (packetStrings[1])
                {
                    case PacketDatas.PACKET_CHAT:
                        DoChat(sender, packetStrings);
                        break;
                    case PacketDatas.PACKET_GAME_CREATE:
                        CreateGameRoom(sender, packetStrings);
                        break;

                    case PacketDatas.PACKET_GAME_SEL:
                        SelectGameRoom(sender, packetStrings);
                        break;

                    case PacketDatas.PACKET_GET_GAME_ROOM:
                        GameRoomsUpdate(sender, packetStrings);
                        break;

                    default:
                        sender.Close();
                        break;
                }
            }).Start();
        }
        public void DCAll()
        {
            for(int i = 0; i < gameRooms.Count; i++)
            {
                gameRooms[i].DCAll();
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
        private void GameRoomsUpdate(Client sender, String[] packetStrings)
        {
           // PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[2]);
            UpdateGameRooms(sender);
        }
        private void UpdateGameRooms(Client client = null)
        {
            if(gameRooms.Count <= 0 )
                return;//No game rooms... nothing to send!

            string gameRoomNames = "";
            for (int i = 0; i < gameRooms.Count; i++)
            {
                gameRoomNames += gameRooms[i].gameRoomName;

                if (i != gameRooms.Count - 1)
                    gameRoomNames += ",";
            }
            if(client != null)
            {
                client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GET_GAME_ROOM + PacketDatas.PACKET_SPLIT + gameRoomNames);
                return;
            }
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GET_GAME_ROOM + PacketDatas.PACKET_SPLIT + gameRoomNames);

            }
        }
        private void SelectGameRoom(Client sender, String[] packetStrings)
        {
            //AddClientToGameRoom
            GameRoom gameRoom = GetGameRoomByName(packetStrings[2]);
            if (gameRoom != null)
            {
                AddClientToGameRoom(sender, gameRoom);
            }
        }
        private GameRoom GetGameRoomByName(string name)
        {
            for(int i = 0; i < gameRooms.Count; i ++)
            {
                if(gameRooms[i].gameRoomName == name)
                {
                    return gameRooms[i];
                }
            }
            return null;
        }
        private void AddClientToGameRoom(Client client,GameRoom gameRoom)
        {
            client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_SEL + PacketDatas.PACKET_SPLIT + "OK!");
            gameRoom.AddClient(client);
            RemoveClient(client);
        }
        private void CreateGameRoom(Client sender, String[] packetStrings)
        {

            if (GetGameRoomByName(packetStrings[2]) != null || packetStrings[2].Length <= 2)
            {
                sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_ERROR + PacketDatas.PACKET_SPLIT + "There is already a gameroom with the same name or need more then 2 characters");
                return;
            }

            GameRoom gameRoom = new GameRoom(sender, this, packetStrings[2]);
            gameRooms.Add(gameRoom);
            sender.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_CREATE + PacketDatas.PACKET_SPLIT + "OK!");
            AddClientToGameRoom(sender, gameRoom);
            UpdateGameRooms();
        }

        public void DestroyGameRoom(GameRoom GgameRoom)
        {
            for (int i = 0; i < gameRooms.Count; i++)
            {
                if (gameRooms[i] == GgameRoom)
                {
                    gameRooms.Remove(GgameRoom);
                    GgameRoom = null;
                }
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
            string chatPackage = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[2];
            chatPackage = QEncryption.Encrypt(chatPackage);
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].SendWEncrypt(chatPackage);
            }
        }
    }
}
