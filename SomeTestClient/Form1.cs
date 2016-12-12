using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SomeTestClient.Network;
using QServer.Network;
using System.Threading;

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
            TestSpam(1);

        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            
            client = new Client();
            client.InitSocket("127.0.0.1", 9650);
            //client.InitSocket("141.252.230.10", 9650);
            
            client.receive += Client_receive;
            //Login packet
            
            client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT +
                PacketDatas.PACKET_LOGIN + PacketDatas.PACKET_SPLIT +
                userTextBox.Text + PacketDatas.PACKET_SPLIT +
                passTextBox.Text
                );
        }
        private void spamButton_Click(object sender, EventArgs e)
        {
            TestSpam(50);
        }
        private void TestSpam(int count)
        {
            for (int i = 0; i < count; i++)
            {
                client = new Client();
                client.InitSocket("127.0.0.1", 9650);
                //client.InitSocket("141.252.230.10", 9650);

                client.receive += Client_receive;
                //Login packet

                client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT +
                    PacketDatas.PACKET_LOGIN + PacketDatas.PACKET_SPLIT +
                    "spam" + i + PacketDatas.PACKET_SPLIT +
                    "pass"
                    );
                Thread.Sleep(50);
            }
        }
        void Reset()
        {
            roomListBox.Items.Clear();
        }
        void Client_receive(Client sender, string[] packetStrings)
        {
            switch (packetStrings[1])
            {
                case PacketDatas.PACKET_LOGIN:
                    isLoggedIn = true;
                    labelLoggedIn.Text = "Logged in";
                    client.Send(PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GET_GAME_ROOM);
                    break;
                case PacketDatas.PACKET_CHAT:
                    chatRichTextBox.AppendText("Chat: " + packetStrings[2] + "\n");
                    break;
                case PacketDatas.PACKET_GET_GAME_ROOM:
                    string[] rooms = packetStrings[2].Split(',');
                    for (int r = 0; r < rooms.Length; r ++ )
                    {
                        if (rooms[r] != "" || rooms[r] != string.Empty)
                        {
                            for (int i = 0; i < roomListBox.Items.Count; i++)
                            {
                                if (roomListBox.Items[i].ToString() == rooms[r])
                                {
                                    HandleError(rooms[r] + " already in the list");
                                    return;
                                }
                            }
                            roomListBox.Items.Add(rooms[r]);
                        }
                        else
                        {
                            HandleError(packetStrings[2]);
                        }
                    }
                    break;
                case PacketDatas.PACKET_FORCE_DC:
                    Application.Exit();
                    break;
                    /*
                case PacketDatas.PACKET_GAME_START:

                    break;
                case PacketDatas.PACKET_GAME_STOP:

                    break;*/
                //Get error
                case PacketDatas.PACKET_ERROR:
                    HandleError(packetStrings[2] + " : " + packetStrings);
                    break;
                default:
                    unusedPacketsRichTextBox.AppendText("Unhandled: " + packetStrings + "\n");
                    break;
            }
        }
        private void HandleError(string errorText)
        {
            /*
            client.receive -= Client_receive;
            client.Close();
            client = null;
            MessageBox.Show(errorText + "\nYou have been disconnected!", "Whoops something went wrong");
             */
            //MessageBox.Show(errorText, "Whoops something went wrong");
            unusedPacketsRichTextBox.AppendText("Error: " + errorText + "\n");
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
            String Package = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_CREATE + PacketDatas.PACKET_SPLIT + roomTextBox.Text;
            client.Send(Package);
        }

        private void roomListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Package = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_SEL + PacketDatas.PACKET_SPLIT + roomListBox.SelectedItem.ToString();
            client.Send(Package);
        }

        private void chatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && chatTextBox.Text != "")
            {
                String Package = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + chatTextBox.Text;
                client.Send(Package);
                
                chatTextBox.Text = "";
            }
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            //startGameButton.
            String Package = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_START;
            client.Send(Package);
        }

        private void stopGameButton_Click(object sender, EventArgs e)
        {
            String Package = PacketDatas.PACKET_HEADER + PacketDatas.PACKET_SPLIT + PacketDatas.PACKET_GAME_STOP;
            client.Send(Package);
        }

    }
}
