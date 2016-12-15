using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QServer.Network;
using QServer.Servers;

namespace QServer
{
    public partial class Main : Form
    {
        private MainServer dataControl;//Data control. Here is where the network crap happends
        public Main()
        {
            InitializeComponent();
            Load += new EventHandler(Main_Load);//My winforms(body) is ready.
        }
        void Main_Load(object sender, EventArgs e)
        {
            dataControl = new MainServer();//Now we can assign this.
            //listen to the event whenever a user is logged in.
            //going to function count_Changed
            dataControl.countChanged += new MainServer.CountChanged(count_Changed);
        }
        void count_Changed(WinformsThings e)
        {
            //We need this to edit our text.
            //since winforms is running on another thread. Well atleast I think.
            this.Invoke((MethodInvoker)delegate
            {
                onlineCountLabel.Text = e.counter;
            });
        }
        //exit button click..............
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();//Yup... exit
        }
        //DC all button click
        private void dcBtn_Click(object sender, EventArgs e)
        {
            dataControl.CloseAllSockets();
            //DC all socket connections.
        }

        private void sendPackageBtn_Click(object sender, EventArgs e)
        {
            dataControl.SendToAll(packageText.Text);
        }

        private void splitBtn_Click(object sender, EventArgs e)
        {
            packageText.Text += "\u0003";
        }
    }
}
