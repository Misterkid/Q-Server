using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/* Author: Eddy Meivogel
 * Website: www.eddymeivogel.com
 */
namespace QServer.Network
{
    public struct PacketDatas
    {
        public const string PACKET_HEADER = "\u0001";
        public const string PACKET_SPLIT = "\u0003";
        public const string PACKET_CHAT = "Chat";
        public const string PACKET_LOGIN = "Login";
        public const string PACKET_FORCE_DC = "ForceDC";
        public const string PACKET_ERROR = "Error";
        public const string PACKET_GAME_CREATE = "GaCreate";
        public const string PACKET_GAME_SEL = "GaSel";
        public const string PACKET_GAME_START = "GaStart";
        public const string PACKET_GAME_STOP = "GaStop";
        public const string PACKET_GAME_POS = "GaPOS";
        public const string PACKET_GAME_IMAGE_START = "GaIMGStart";
        public const string PACKET_GAME_IMAGE = "GaIMG";
        public const string PACKET_GAME_IMAGE_END = "GaIMGEND";
        public const string PACKET_GAME_DESTROY = "GaKill";
        public const string PACKET_GET_GAME_ROOM = "GaGetRoom";
    }
}
