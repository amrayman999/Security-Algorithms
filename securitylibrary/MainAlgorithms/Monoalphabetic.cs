using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string alphabetics = "abcdefghijklmnopqrstuvwxyz";

            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            Dictionary<char, bool> Txt_alphabetic = new Dictionary<char, bool>();
            SortedDictionary<char, char> Permutations_key = new SortedDictionary<char, char>();

            int P_Len = plainText.Length;

            for (int i = 0; i < P_Len; i++)
            {
                if (!Permutations_key.ContainsKey(plainText[i]))
                {
                    Permutations_key.Add(plainText[i], cipherText[i]);
                    Txt_alphabetic.Add(cipherText[i], true);
                }
            }
            if (Permutations_key.Count == 26)
            {
                string key = "";
                foreach (var item in Permutations_key)
                {
                    key += item.Value;
                }
                return key;
            }
            else
            {
                for (int i = 0; i < 26; i++)
                {
                    if (!Permutations_key.ContainsKey(alphabetics[i]))
                    {
                        for (int j = 0; j < 26; j++)
                        {
                            if (!Txt_alphabetic.ContainsKey(alphabetics[j]))
                            {
                                Permutations_key.Add(alphabetics[i], alphabetics[j]);
                                Txt_alphabetic.Add(alphabetics[j], true);
                                j = 26;
                            }
                        }
                    }
                }
                string key = "";
                foreach (var item in Permutations_key)
                {
                    key += item.Value;
                }
                return key;
            }
        }
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            char[] chars = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] == ' ')
                {
                    chars[i] = ' ';
                }
                else
                {
                    int j = key.IndexOf(cipherText[i]) + 97;
                    chars[i] = (char)j;
                }
            }
            return new string(chars);
        }

        public string Encrypt(string plainText, string key)
        {
            char[] chars = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == ' ')
                {
                    chars[i] = ' ';
                }

                else
                {
                    int j = plainText[i] - 97;
                    chars[i] = key[j];
                }
            }

            return new string(chars);
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            
            string alpha_freqs = "etaoinsrhldcumfpgwybvkxjqz";
            cipher = cipher.ToLower();
            Dictionary<char,int> cipher_freqs = new Dictionary<char, int>();
            for(int i = 0; i < cipher.Length; i++)
            {
                if(cipher_freqs.ContainsKey(cipher[i]))
                {
                    cipher_freqs[cipher[i]]++;
                }
                else
                {
                    cipher_freqs.Add(cipher[i], 1);
                }
            }
            var sortedDict = from entry in cipher_freqs orderby entry.Value descending select entry;
            Dictionary<char, char> char_mapping = new Dictionary<char, char>();
            int count = 0;
            foreach(var i in sortedDict)
            {
                char_mapping.Add(i.Key, alpha_freqs[count]);
                count++;
            }
            string key_out = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                key_out += char_mapping[cipher[i]];
            }
            return key_out;



        }
    }
}
