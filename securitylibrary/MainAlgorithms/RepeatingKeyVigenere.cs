using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        // Helping Function
        public int get_char_index(char charact)
        {
            char[] characters = new char[] {'A','B','C','D','E','F','G','H','I','J','K','L','M','N'
                                            ,'O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            char c = Char.ToUpper(charact);
            int ind = Array.IndexOf(characters, c);

            return ind;

        }
        // Helping Function
        public char get_char_by_index(int index)
        {
            char[] characters = new char[] {'A','B','C','D','E','F','G','H','I','J','K','L','M','N'
                                            ,'O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            return Char.ToLower(characters[index]);
        }
        public string Analyse(string plainText, string cipherText)
        {
            int cipher = cipherText.Length;
            int plain = plainText.Length;
            string key_stream = string.Empty;
            for (int i = 0; i < cipher; i++)
            {
                char c_char = cipherText[i];
                char p_char = plainText[i];
                int i_c = get_char_index(c_char);
                int i_p = get_char_index(p_char);
                int i_k;
                if (i_c >= i_p)
                {
                    i_k = Math.Abs(i_c - i_p);
                }
                else
                {
                    i_k = Math.Abs(26 + (i_c - i_p));
                }
                key_stream += get_char_by_index(i_k);
            }
            string key_out = string.Empty;
            key_out += key_stream[0];
            key_out += key_stream[1];
            for (int i = 2; i < cipher; i++)
            {
                char c = key_stream[i];
                char c1 = key_stream[i + 1];
                if (c == key_stream[0] && c1 == key_stream[1])
                {
                    break;
                }
                key_out += key_stream[i];
            }
            return key_out.ToLower();
        }

        public string Decrypt(string cipherText, string key)
        {
            int cipher = cipherText.Length;
            int Key = key.Length;
            int diff = cipher - Key;
            string plain_text = string.Empty;
            string key_stream = key;
            while(key_stream.Length != cipher)
            {
                for(int i =0;i < Key;i++)
                {
                    if(key_stream.Length == cipher)
                    {
                        break;
                    }
                    key_stream += key[i];
                }
            }
            for (int i = 0; i < cipher; i++)
            {
                char c_char = cipherText[i];
                char k_char = key_stream[i];
                int i_c = get_char_index(c_char);
                int i_k = get_char_index(k_char);
                int p_p;
                if (i_c >= i_k)
                {
                    p_p = Math.Abs(i_c - i_k);
                }
                else
                {
                    p_p = Math.Abs(26 + (i_c - i_k));
                }
                plain_text += get_char_by_index(p_p);
            }
            return plain_text.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            int plain = plainText.Length;
            int Key = key.Length;
            string key_stream = key;
            if (plain != Key)
            {
                int counter = 0;
                for (int i = Key; i <= plain; i++)
                {
                    key_stream += key_stream[counter];
                    counter++;
                }
                key = key_stream;
            }
            string cipher = string.Empty;
            for (int i = 0; i < plain; i++)
            {
                char p_char = plainText[i];
                char k_char = key[i];
                int i_p = get_char_index(p_char);
                int i_k = get_char_index(k_char);
                int c_p = i_p + i_k;
                cipher += get_char_by_index(c_p % 26);
            }
            return cipher.ToLower();
        }
    }
}