using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            DES des = new DES();
            string B = des.Decrypt(cipherText, key[0]);
            string A = des.Encrypt(B, key[1]);
            string cipher = des.Decrypt(A, key[0]);
            return cipher;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            DES des = new DES();
            string A = des.Encrypt(plainText, key[0]);
            string B = des.Decrypt(A, key[1]);
            string cipher = des.Encrypt(B, key[0]);
            return cipher;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}
