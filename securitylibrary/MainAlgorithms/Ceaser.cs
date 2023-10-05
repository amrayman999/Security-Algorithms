using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                if (char.IsUpper(plainText[i]))
                {
                    char ch = (char)(((int)plainText[i] +
                                    key - 65) % 26 + 65);
                    result.Append(ch);
                }
                else
                {
                    char ch = (char)(((int)plainText[i] +
                                    key - 97) % 26 + 97);
                    result.Append(ch);
                }
            }
            return result.ToString();
        }

        public string Decrypt(string cipherText, int key)
        {
            return Encrypt(cipherText, 26 - key);
        }

        public int Analyse(string plainText, string cipherText)
        {
            int key;
            if (plainText[0] == cipherText[0])
            {
                key = 0;
            }
            else
            {
                char p_ch = plainText[0];
                char c_ch = cipherText[0];
                // Uses the uppercase character unicode code point.'A' = 65, 'Z' = 90
                int p_index = char.ToUpper(p_ch) - 64;
                int c_index = char.ToUpper(c_ch) - 64;

                key = c_index - p_index;
                if (key < 0)
                {
                    key = key + 26;
                }
            }
            return key;

        }
    }
}
