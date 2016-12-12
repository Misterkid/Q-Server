using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace QServer
{
    public static class Eutils
    {
        public static void WriteLine(string main, object arg1 = null, object arg2= null, object arg3= null)
        {
            Console.WriteLine(main, arg1, arg2, arg3);
            /*
            new Thread(delegate()
            {
                Console.WriteLine(main, arg1, arg2,arg3);

            }).Start();*/
        }
    }
}
