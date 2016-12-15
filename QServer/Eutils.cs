using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace QServer
{
    public static class Eutils
    {
        public enum MESSSAGE_TYPE
        {
            NORMAL,
            WARNING,
            ERROR,
            DANGER
        }
        public static void FileError(string error,string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(error);
            writer.Close();
        }
        public static void WriteLine(string main, MESSSAGE_TYPE error, object arg1 = null, object arg2= null, object arg3= null)
        {
            switch(error)
            {
                case MESSSAGE_TYPE.NORMAL:
                         Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case MESSSAGE_TYPE.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case MESSSAGE_TYPE.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case MESSSAGE_TYPE.DANGER:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(main, arg1, arg2, arg3);
        }
    }
}
