using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//I dislike putting comments! So I did this afterwards.
namespace QServer
{
    class Program
    {
        static void Main(string[] args)//It all starts here.
        {
            //It is auto generated, we keep it.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());//Main! no not the function the class! winforms! woohoo
        }
    }
}