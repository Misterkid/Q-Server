using QServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QServer.Servers
{
    class Login:Server
    {
        private Lobby lobby;//The lobby.
        public Login():base()
        {
            lobby = new Lobby();
        }
        protected override void client_received(Client sender, string[] packetStrings)
        {
            //So what kind of packet is it?
            switch (packetStrings[0])
            {
                case PacketDatas.PACKET_LOGIN://Login packet! we log in.
                    DoLogin(sender, packetStrings);//Do this function.
                    break;
                default:
                    Eutils.WriteLine("Login Error wrong packet! {0}", Eutils.MESSSAGE_TYPE.WARNING, packetStrings[1]);
                    break;
            }
        }
        //The login function.
        private void DoLogin(Client sender, String[] packetStrings)
        {
            bool userCorrect = true;//Correct
            bool passCorrect = true;//correct
            //Everything needs to be right. If something is wrong then goodbye.
            if (!userCorrect || !passCorrect || packetStrings[1].Length < 1 || packetStrings[2].Length < 1)
            {
                //<3 the first packed you see!
                //We send a error packet.
                sender.Send(PacketDatas.PACKET_ERROR + PacketDatas.PACKET_SPLIT + "Invalid username or password");

                Eutils.WriteLine("Login in failed by ip: {0}", Eutils.MESSSAGE_TYPE.WARNING, sender.ipEndpoint.Address);
                sender.Close();//DC
                return;
            }
            //Set the username
            sender.userName = packetStrings[1];
            sender.isLoggedIn = true;
            //Login succes! We just send a login packet back
            sender.Send(PacketDatas.PACKET_LOGIN);
            Eutils.WriteLine("Login in:{0} with password:{1} ip: {2}", Eutils.MESSSAGE_TYPE.NORMAL, packetStrings[1], packetStrings[2], sender.ipEndpoint.Address);
            //We remove the receive event to so the lobby can handle the receiving now.
            RemoveClient(sender);
            //Add client to the lobby.
            lobby.AddClient(sender);
        }
    }
}
