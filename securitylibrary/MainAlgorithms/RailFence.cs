using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {

        public void get_possiple_key(string plainText, string cipherText, int[] pK)
        {
            int i = 0;
            while (i < plainText.Length)
            {
                if (plainText[i] == cipherText[1]) pK[i] = i;
                i++;
            }


        }
        public int complement(string plainText, string cipherText, int[] pK)
        {

            int start = 0;
            while (start < pK.Length)
            {
                string s = Encrypt(plainText, pK[start]).ToUpper();
                if (String.Equals(cipherText, s))
                {
                    return pK[start];
                }
                start++;
            }
            return -1;

        }
        public int Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToUpper();
            plainText = plainText.ToUpper();
            int[] pK = new int[plainText.Length];
            get_possiple_key(plainText, cipherText, pK);
            int res = complement(plainText, cipherText, pK);
            return res;

        }

        public string Decrypt(string cipherText, int key)
        {
            double valu = (double)cipherText.Length / key;
            int PTL = (int)Math.Ceiling(valu);
            return Encrypt(cipherText, PTL).ToUpper();
        }
        public string inside_lo(int start_inside, char[] mytext, int key, string my_output)
        {
            while (start_inside < mytext.Length)
            {
                my_output += mytext[start_inside];
                start_inside += key;
            }
            return my_output;
        }
        public string Encrypt(string plainText, int key)
        {
            String my_output = "";
            char[] mytext = plainText.ToUpper().ToCharArray();
            int start = 0;
            while (start < key)
            {
                int start_inside = start;
                my_output = inside_lo(start_inside, mytext, key, my_output);
                start++;
            }
            return my_output;

        }
    }
}
