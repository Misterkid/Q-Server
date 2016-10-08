using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SomeTestClient.Network;

namespace SomeTestClient
{
    public partial class Form1 : Form
    {
        private bool isLoggedIn = false;
        private Client client;
        public Form1()
        {
            InitializeComponent();
            //client = new Client();

        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            client = new Client();
            client.InitSocket("127.0.0.1", 9650);
            client.recieve += Client_recieve;
            //Login packet
            client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT +
                PacketDatas.LOGIN_PACKET + PacketDatas.PACKET_SPLIT +
                userTextBox.Text + PacketDatas.PACKET_SPLIT +
                passTextBox.Text
                );

        }
        void Client_recieve(System.Net.Sockets.Socket sender, byte[] data)
        {
            string packetString = Encoding.Default.GetString(data);
            Console.WriteLine("Recieved data:{0} at Time:{1} Length:{2}", packetString, DateTime.Now, packetString.Length);
            string[] packetStrings = packetString.Split(PacketDatas.PACKET_SPLIT[0]);
            if (packetStrings[0] == PacketDatas.PACKET_HEADER)
            {
                switch (packetStrings[1])
                {
                    case PacketDatas.LOGIN_PACKET:
                        isLoggedIn = true;
                        labelLoggedIn.Text = "Logged in";
                        client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GET_GAME_ROOM);
                        break;
                    case PacketDatas.PACKET_CHAT:
                        chatRichTextBox.AppendText(packetStrings[2] + "\n");
                        break;
                    case PacketDatas.PACKET_GET_GAME_ROOM:
                        string[] rooms = packetStrings[2].Split(',');
                        for (int r = 0; r < rooms.Length; r ++ )
                        {
                            roomListBox.Items.Add(rooms[r]);
                        }
                        break;
                    //Get error
                    case PacketDatas.PACKET_ERROR:
                        HandleError(packetStrings[2]);
                        break;
                    default:
                        unusedPacketsRichTextBox.AppendText(packetString + "\n");
                        break;
                }
            }
            else
            {
                HandleError("Not packet header");
                return;
            }
        }
        private void HandleError(string errorText)
        {
            client.recieve -= Client_recieve;
            client.Close();
            client = null;
            MessageBox.Show(errorText + "\nYou have been disconnected!", "Whoops something went wrong");
        }
        private void messageButton_Click(object sender, EventArgs e)
        {
            //Chat packet
            if (chatTextBox.Text != "")
            {
                client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT +
                    PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT +
                    chatTextBox.Text
                    );
                chatTextBox.Text = "";
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_CREATE + PacketDatas.PACKET_SPLIT + roomTextBox.Text);
        }

        private void roomListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_SEL + PacketDatas.PACKET_SPLIT + roomListBox.SelectedItem.ToString());
        }

        private void chatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && chatTextBox.Text != "")
            {
                //Chat packet
                client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT +
                    PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT +
                    chatTextBox.Text
                    );
                chatTextBox.Text = "";
            }
        }
    }
}
