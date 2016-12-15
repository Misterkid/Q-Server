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
namespace QServer.Servers
{
    class Lobby:Server
    {
        private List<GameRoom> gameRooms;//All game rooms that got created.
        public Lobby():base()
        {
            //clients = new List<Client>();
            gameRooms = new List<GameRoom>();
        }

        protected override void client_received(Client sender, string[] packetStrings)
        {
            switch (packetStrings[0])
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
                    Eutils.WriteLine("Lobby Error wrong packet! {0}", Eutils.MESSSAGE_TYPE.WARNING, packetStrings[1]);
                    break;
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


            string gameRoomPackage = PacketDatas.PACKET_GET_GAME_ROOM + PacketDatas.PACKET_SPLIT + gameRoomNames;
           // gameRoomPackage = QEncryption.Encrypt(gameRoomPackage);
            SendToAll(gameRoomPackage);
        }
        private void SelectGameRoom(Client sender, String[] packetStrings)
        {
            //AddClientToGameRoom
            GameRoom gameRoom = GetGameRoomByName(packetStrings[1]);
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
            client.Send(PacketDatas.PACKET_GAME_SEL + PacketDatas.PACKET_SPLIT + "OK!");
            gameRoom.AddClient(client);
            RemoveClient(client);
        }
        private void CreateGameRoom(Client sender, String[] packetStrings)
        {

            if (GetGameRoomByName(packetStrings[1]) != null || packetStrings[1].Length <= 2)
            {
                sender.Send(PacketDatas.PACKET_ERROR + PacketDatas.PACKET_SPLIT + "There is already a gameroom with the same name or need more then 2 characters");
                return;
            }

            GameRoom gameRoom = new GameRoom(sender, this, packetStrings[1]);
            gameRooms.Add(gameRoom);
            sender.Send(PacketDatas.PACKET_GAME_CREATE + PacketDatas.PACKET_SPLIT + "OK!");
            UpdateGameRooms();
            AddClientToGameRoom(sender, gameRoom);
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
            string chatPackage = PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + sender.userName + ": " + packetStrings[1];
            SendToAll(chatPackage);
            /*
            chatPackage = QEncryption.Encrypt(chatPackage);
            for (int c = 0; c < clients.Count; c++)
            {
                clients[c].SendWEncrypt(chatPackage);
            }*/
        }
    }
}
