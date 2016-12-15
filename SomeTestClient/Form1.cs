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

        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            
            client = new Client();
            client.InitSocket(ipTextBox.Text, 9650);
            //client.InitSocket("141.252.230.10", 9650);
            
            client.receive += Client_receive;
            //Login packet
            
            client.Send(
                PacketDatas.PACKET_LOGIN + PacketDatas.PACKET_SPLIT +
                userTextBox.Text + PacketDatas.PACKET_SPLIT +
                passTextBox.Text
                );
        }
        private void spamButton_Click(object sender, EventArgs e)
        {
            TestSpam(10000);
        }
        private void TestSpam(int count)
        {
            for (int i = 0; i < count; i++)
            {
                client = new Client();
                client.InitSocket(ipTextBox.Text, 9650);
                //client.InitSocket("141.252.230.10", 9650);

                client.receive += Client_receive;
                //Login packet

                client.Send(
                    PacketDatas.PACKET_LOGIN + PacketDatas.PACKET_SPLIT +
                    "spam" + i + PacketDatas.PACKET_SPLIT +
                    "pass"
                    );
                Thread.Sleep(25);
            }

        }
        void Reset()
        {
            roomListBox.Items.Clear();
        }
        void Client_receive(Client sender, string[] packetStrings)
        {
            switch (packetStrings[0])
            {
                case PacketDatas.PACKET_LOGIN:
                    isLoggedIn = true;
                    labelLoggedIn.Text = "Logged in";
                    client.Send(PacketDatas.PACKET_GET_GAME_ROOM);
                    break;
                case PacketDatas.PACKET_CHAT:
                    try
                    {
                        chatRichTextBox.AppendText("Chat: " + packetStrings[1] + "\n");
                    }
                    catch(Exception e)
                    {
                        
                    }
                    break;
                case PacketDatas.PACKET_GET_GAME_ROOM:
                    string[] rooms = packetStrings[1].Split(',');
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
                            HandleError(packetStrings[1]);
                        }
                    }
                    break;
                case PacketDatas.PACKET_GAME_IMAGE:
                    PutMemeInBox(packetStrings[1]);
                    break;
                case PacketDatas.PACKET_GAME_POS:
                        //PutMemeInBox(packetStrings[1]);
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
                    HandleError(packetStrings[1] + " : " + packetStrings);
                    break;
                default:
                    unusedPacketsRichTextBox.AppendText("Unhandled: " + packetStrings + "\n");
                    break;
            }
        }

        private void PutMemeInBox(string packet)
        {
            String[] bytesString = packet.Split(' ');
            byte[] bytes = new byte[bytesString.Length];
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = Byte.Parse(bytesString[i]);//Byte.ParseByte(bytesString[i]);
            }

            memePictureBox1.Image = GetImageFromByteArray(bytes);
            //throw new NotImplementedException();
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
                client.Send(
                    PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT +
                    chatTextBox.Text
                    );
                chatTextBox.Text = "";
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            String Package = PacketDatas.PACKET_GAME_CREATE + PacketDatas.PACKET_SPLIT + roomTextBox.Text;
            client.Send(Package);
        }

        private void roomListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Package = PacketDatas.PACKET_GAME_SEL + PacketDatas.PACKET_SPLIT + roomListBox.SelectedItem.ToString();
            client.Send(Package);
        }

        private void chatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && chatTextBox.Text != "")
            {
                String Package = PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT + chatTextBox.Text;
                client.Send(Package);
                
                chatTextBox.Text = "";
            }
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            //startGameButton.
            String Package = PacketDatas.PACKET_GAME_START;
            client.Send(Package);
        }

        private void stopGameButton_Click(object sender, EventArgs e)
        {
            String Package = PacketDatas.PACKET_GAME_STOP;
            client.Send(Package);
        }

        private void spamSendButton_Click(object sender, EventArgs e)
        {
            if (chatTextBox.Text != "")
            {
                for(int i = 0; i < 500; i++)
                {
                    client.Send(
                        PacketDatas.PACKET_CHAT + PacketDatas.PACKET_SPLIT +
                        chatTextBox.Text
                        );
                     Thread.Sleep(100);
                }
            }
            chatTextBox.Text = "";
        }

        private void sendMemeBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dialog.InitialDirectory = @"C:\";
            dialog.Title = "Please select an image file to send.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //dialog.
                byte[] imgdata = System.IO.File.ReadAllBytes(dialog.FileName);
                string imgString = "";
                int length = imgdata.Length;
                for (int i = 0; i < imgdata.Length; i ++ )
                {
                    imgString += imgdata[i] + " ";
                }
                length = imgString.Length;
                client.Send(PacketDatas.PACKET_GAME_IMAGE + PacketDatas.PACKET_SPLIT + imgString);
            }
        }
        public Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            ImageConverter imageConverter = new ImageConverter();
            Bitmap bm = (Bitmap)imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting 
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }
            return bm;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                client.Send(PacketDatas.PACKET_GAME_POS + PacketDatas.PACKET_SPLIT + e.X);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
