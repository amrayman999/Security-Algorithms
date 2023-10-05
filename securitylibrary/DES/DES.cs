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
    public class DES : CryptographicTechnique
    {
        public string Inverse_permutation(string text)
        {
            string permutated = "";
            List<int> indices = new List<int>() {39,7,47,15,55,23,63,31,
                                                 38,6,46,14,54,22,62,30,
                                                 37,5,45,13,53,21,61,29,
                                                 36,4,44,12,52,20,60,28,
                                                 35,3,43,11,51,19,59,27,
                                                 34,2,42,10,50,18,58,26,
                                                 33,1,41,9,49,17,57,25,
                                                 32,0,40,8,48,16,56,24};
            for (int i = 0; i < text.Length; i++)
            {
                permutated += text[indices[i]];
            }

            return permutated;
        }
        public string Rounds_function(string right, string key)
        {
            string expanded_R = Expansion_function(right);
            string XOR_r_key = Get_XOR(expanded_R, key);
            string s_boxes_out = S_box_substitution(XOR_r_key);
            string final_value = Permutation(s_boxes_out);
            return final_value;
        }
        public string Permutation(string text)
        {
            string permutated = "";
            List<int> indices = new List<int>() { 15,6,19,20,28,11,27,16,0,14,22,25,
                                                  4,17,30,9,1,7,23,13,31,26,2,8,18,12,29,5,21,10,3,24};
            for (int i = 0; i < text.Length; i++)
            {
                permutated += text[indices[i]];
            }

            return permutated;
        }
        public string S_box_substitution(string text)
        {
            string[] b_s = {text.Substring(0, 6), text.Substring(6, 6), text.Substring(12, 6),
                            text.Substring(18, 6),text.Substring(24, 6),text.Substring(30, 6),
                            text.Substring(36, 6),text.Substring(42, 6) };
            int[,] s1 = { {14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7 },
                          {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8 },
                          {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0 },
                          {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13 } };
            int[,] s2 = { { 15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10},
                          {3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5 },
                          {0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15 },
                          {13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9 } };
            int[,] s3 = { {10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8 },
                          {13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1 },
                          {13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7 },
                          {1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12 } };
            int[,] s4 = { {7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15 },
                          {13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9 },
                          {10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4 },
                          {3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14 } };
            int[,] s5 = { {2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9 },
                          {14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6},
                          {4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14},
                          {11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3} };
            int[,] s6 = { {12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11 },
                          {10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8 },
                          {9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6 },
                          {4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13} };
            int[,] s7 = { {4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1 },
                          {13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6 },
                          {1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2 },
                          {6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12 } };
            int[,] s8 = { {13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7 },
                          {1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2 },
                          {7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8},
                          {2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 } };
            List<int[,]> s_s = new List<int[,]>() { s1, s2, s3, s4, s5, s6, s7, s8 };
            string s_box_out = "";
            for (int i = 0; i < 8; i++)
            {

                int row = int.Parse(Convert.ToString(Convert.ToInt32(Char.ToString(b_s[i][0]) + Char.ToString(b_s[i][5]), 2), 10));
                int column = int.Parse(Convert.ToString(Convert.ToInt32(b_s[i].Substring(1, 4), 2), 10));
                s_box_out += Convert.ToString(Convert.ToInt32(Convert.ToString(s_s[i][row, column]), 10), 2).PadLeft(4, '0');

            }

            return s_box_out;
        }
        public string Expansion_function(string text)
        {
            string expanded_text = "";
            List<int> indices = new List<int>() {31,0,1,2,3,4,
                                                  3,4,5,6,7,8,
                                                  7,8,9,10,11,12,
                                                  11,12,13,14,15,16,
                                                  15,16,17,18,19,20,
                                                  19,20,21,22,23,24,
                                                  23,24,25,26,27,28,
                                                  27,28,29,30,31,0};
            for (int i = 0; i < indices.Count; i++)
            {
                expanded_text += text[indices[i]];
            }

            return expanded_text;
        }
        public string Get_XOR(string A, string B)
        {
            string Xor = "";
            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] == B[i])
                {
                    Xor += "0";
                }
                else
                {
                    Xor += "1";
                }
            }
            return Xor;
        }
        public string left_circular_shift(string bin_text, int round)
        {
            Dictionary<int, int> R_No_vs_shift = new Dictionary<int, int>();
            string shifted = bin_text;
            R_No_vs_shift.Add(1, 1);
            R_No_vs_shift.Add(2, 1);
            R_No_vs_shift.Add(3, 2);
            R_No_vs_shift.Add(4, 2);
            R_No_vs_shift.Add(5, 2);
            R_No_vs_shift.Add(6, 2);
            R_No_vs_shift.Add(7, 2);
            R_No_vs_shift.Add(8, 2);
            R_No_vs_shift.Add(9, 1);
            R_No_vs_shift.Add(10, 2);
            R_No_vs_shift.Add(11, 2);
            R_No_vs_shift.Add(12, 2);
            R_No_vs_shift.Add(13, 2);
            R_No_vs_shift.Add(14, 2);
            R_No_vs_shift.Add(15, 2);
            R_No_vs_shift.Add(16, 1);

            for (int i = 0; i < R_No_vs_shift[round]; i++)
            {
                char first = shifted[0];
                shifted = shifted.Remove(0, 1);
                shifted += first;
            }

            return shifted;
        }
        public string Initial_permutation(string bin_text)
        {
            string permutated = "";
            List<int> indices = new List<int>() { 57, 49, 41, 33, 25,
                17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37,
                29,21, 13, 5, 63, 55, 47, 39 ,31, 23, 15, 7, 56, 48, 40,
                32, 24, 16, 8, 0, 58, 50 , 42, 34, 26, 18, 10, 2, 60 ,
                52, 44, 36, 28, 20, 12, 4 , 62 , 54,46,38,30,22,14,6};
            for (int i = 0; i < bin_text.Length; i++)
            {
                permutated += bin_text[indices[i]];
            }

            return permutated;
        }
        public string[] Permuted_choice_1(string bin_text)
        {
            string perm_key = "";
            string[] permuted = new string[2];
            List<int> indices = new List<int>() { 56,48,40,32,24,16,8,
                                                    0,57,49,41,33,25,17,
                                                    9,1,58,50,42,34,26,
                                                    18,10,2,59,51,43,35,
                                                    62,54,46,38,30,22,14,
                                                    6,61,53,45,37,29,21,
                                                    13,5,60,52,44,36,28,
                                                   20,12,4,27,19,11,3};
            for (int i = 0; i < indices.Count; i++)
            {
                perm_key += bin_text[indices[i]];
            }
            permuted[0] = perm_key.Substring(0, 28);
            permuted[1] = perm_key.Substring(28, 28);

            return permuted;
        }
        public string Permuted_choice_2(string bin_text)
        {
            string permuted = "";

            List<int> indices = new List<int>() {13,16,10,23,0,4,2,27,14,5,20,9,22,18,11,3,25,7,15,
                                                 6,26,19,12,1,40,51,30,36,46,54,29,39,50,44,32,47,
                                                 43,48,38,55,33,52,45,41,49,35,28,31};

            for (int i = 0; i < indices.Count; i++)
            {
                permuted += bin_text[indices[i]];
            }

            return permuted;
        }
        public string Hex_Binary_representation(string Representation)
        {
            Dictionary<char, string> hex_to_bin = new Dictionary<char, string>();
            hex_to_bin.Add('0', "0000");
            hex_to_bin.Add('1', "0001");
            hex_to_bin.Add('2', "0010");
            hex_to_bin.Add('3', "0011");
            hex_to_bin.Add('4', "0100");
            hex_to_bin.Add('5', "0101");
            hex_to_bin.Add('6', "0110");
            hex_to_bin.Add('7', "0111");
            hex_to_bin.Add('8', "1000");
            hex_to_bin.Add('9', "1001");
            hex_to_bin.Add('A', "1010");
            hex_to_bin.Add('B', "1011");
            hex_to_bin.Add('C', "1100");
            hex_to_bin.Add('D', "1101");
            hex_to_bin.Add('E', "1110");
            hex_to_bin.Add('F', "1111");
            string bin_out = "";
            for (int i = 2; i < Representation.Length; i++)
            {
                bin_out += hex_to_bin[Representation[i]];
            }
            return bin_out;

        }
        public override string Decrypt(string cipherText, string key)
        {
            // convert the cipher text and key to binary 64 bit representation : 
            string binary_cipher = Hex_Binary_representation(cipherText);
            string binary_key = Hex_Binary_representation(key);
            //-----------------------------------------------------------------
            // step(1) : apply initial permutation to the cipher text
            // and  permuted choice 1  to key : 
            string initial_permuted_cipher = Initial_permutation(binary_cipher);
            string[] permuted_choice_one = Permuted_choice_1(binary_key);
            string C0 = permuted_choice_one[0];
            string D0 = permuted_choice_one[1];
            // step(2) : divide initial permuted cipher text into 2 halves
            string l0 = initial_permuted_cipher.Substring(0, 32);
            string r0 = initial_permuted_cipher.Substring(32, 32);
            // step(3) : get the 16 subkeys 
            string ci = C0;
            string di = D0;
            string[] subkeys = new string[16];
            for (int i = 1; i <= 16; i++)
            {
                ci = left_circular_shift(ci, i);
                di = left_circular_shift(di, i);
                subkeys[i - 1] = Permuted_choice_2(ci + di);
            }
            // step(4) : apply all 16 rounds of des reversely
            string li = l0;
            string ri = r0;
            for (int i = 15; i >= 0; i--)
            {
                string temp_ri = Get_XOR(li, Rounds_function(ri, subkeys[i]));
                string temp_li = ri;
                li = temp_li;
                ri = temp_ri;
            }
            // step(5) : reverxe the right and left blocks
            string bin_res = ri + li;
            // step(6) : apply inverse permutation to the last RL
            bin_res = Inverse_permutation(bin_res);
            // step(7) : convert to hex decimal 
            string hex_out = "0x";
            hex_out += Convert.ToInt64(bin_res, 2).ToString("X").PadLeft(16,'0');


            return hex_out;
        }

        public override string Encrypt(string plainText, string key)
        {
            // convert the plain text and key to binary 64 bit representation : 
            string binary_plain = Hex_Binary_representation(plainText);
            string binary_key = Hex_Binary_representation(key);
            //-----------------------------------------------------------------
            // step(1) : apply initial permutation to the plain text
            // and  permuted choice 1  to key : 
            string initial_permuted_plain = Initial_permutation(binary_plain);
            string[] permuted_choice_one = Permuted_choice_1(binary_key);
            string C0 = permuted_choice_one[0];
            string D0 = permuted_choice_one[1];
            // step(2) : divide initial permuted plain text into 2 halves
            string l0 = initial_permuted_plain.Substring(0, 32);
            string r0 = initial_permuted_plain.Substring(32, 32);
            // step(3) : get the 16 subkeys 
            string ci = C0;
            string di = D0;
            string[] subkeys = new string[16];
            for (int i = 1; i <= 16; i++)
            {
                ci = left_circular_shift(ci, i);
                di = left_circular_shift(di, i);
                subkeys[i - 1] = Permuted_choice_2(ci + di);
            }
            // step(4) : apply all 16 rounds of des
            string li = l0;
            string ri = r0;
            for (int i = 0; i < 16; i++)
            {
                string temp_ri = Get_XOR(li, Rounds_function(ri, subkeys[i]));
                string temp_li = ri;
                li = temp_li;
                ri = temp_ri;
            }
            // step(5) : reverxe the right and left blocks
            string bin_res = ri + li;
            // step(6) : apply inverse permutation to the last RL
            bin_res = Inverse_permutation(bin_res);
            // step(7) : convert to hex decimal 
            string hex_out = "0x";
            hex_out += Convert.ToInt64(bin_res, 2).ToString("X").PadLeft(16, '0');


            return hex_out;

        }
    }
}
