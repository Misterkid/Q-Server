using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace QServer.Network
{
    public static class QEncryption
    {
        public static String Encrypt(String text)
        {
            String newText = Cryptography.Encrypt(text,"megapass");
            return newText;
        }
        public static String Decrypt(String text)
        {
            String newText = Cryptography.Decrypt(text, "megapass");//text;
            return newText;
        }
    }
}
