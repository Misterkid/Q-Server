using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
            else
            {
                sender.Close();
                return;
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
                AddClientToGameRoom(sender, gameRoom);
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
            client.recieved -= new Client.ClientRecievedHandler(client_recieved);
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
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[2]);
            }
        }
    }
}
