using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {

        public void my_frist_loop(double h, int outside_loop, int C, double text_s, string p_T, string[,] plain_arr)
        {
            int i = 0;
            while (i < h)
            {
                int j = 0;
                while (j < outside_loop)
                {
                    if (text_s > C)
                    {
                        plain_arr[i, j] = p_T[C].ToString();
                        C++;
                    }
                    else
                    {
                        plain_arr[i, j] = "";

                    }
                    j++;
                }

                i++;
            }
        }
        public List<string> make_words(List<string> my_list, double h, string[,] plain_arr, int out_loop)
        {
            int i = 0;
            while (i < out_loop)
            {
                string MY_conat_Word = "";
                int K = 0;
                while (K < h)
                {
                    MY_conat_Word += plain_arr[K, i];
                    K++;
                }
                my_list.Add(MY_conat_Word);
                i++;

            }

            return my_list;
        }

        public List<int> OUT_P(SortedDictionary<int, int> MY_sorted_dictionary)
        {

            List<int> data = new List<int>();
            Dictionary<int, int> n_D = new Dictionary<int, int>();

            int a = 0;
            int j = 1;
            while (a < MY_sorted_dictionary.Count)
            {
                n_D.Add(MY_sorted_dictionary.ElementAt(a).Value, a + 1);
                a++;
            }


            while (j < n_D.Count + 1)
            {
                data.Add(n_D[j]);
                j++;
            }

            return data;
        }

        public List<int> Analyse(string plainText, string cipherText)
        {
            SortedDictionary<int, int> MY_sorted_dictionary = new SortedDictionary<int, int>();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            double P_T_Z = plainText.Length;
            double Max = Int32.MaxValue;
            int Start = 1;
            while (Start < Max)
            {
                bool ck = true;
                string ciph_cop = (string)cipherText.Clone();
                int VAL = 0;
                double w = Start;
                double h = Math.Ceiling(P_T_Z / Start);
                string[,] PLAIN_TEXT = new string[(int)h, (int)w];
                List<string> mylist = new List<string>();

                my_frist_loop(h, Start, VAL, P_T_Z, plainText, PLAIN_TEXT);


                mylist = make_words(mylist, h, PLAIN_TEXT, Start);

                MY_sorted_dictionary = new SortedDictionary<int, int>();
                int i = 0;
                while (i < mylist.Count)
                {

                    int x = ciph_cop.IndexOf(mylist[i]);
                    if (x != -1)
                    {
                        MY_sorted_dictionary.Add(x, i + 1);
                        ciph_cop.Replace(mylist[i], "#");

                    }
                    else
                    {
                        ck = false;

                    }
                    i++;
                }

                if (ck == true)
                {
                    break;
                }
                Start++;
            }

            return OUT_P(MY_sorted_dictionary);
        }

        public void fill_diriction(Dictionary<int, int> kD, List<int> Key)
        {
            int start = 0;
            while (start < Key.Count)
            {
                kD.Add(Key[start] - 1, start);
                start++;
            }

        }
        int number_of_fulling_coulmn(string cipherText, List<int> MY_key)
        {
            int n = cipherText.Length % MY_key.Count;
            return n;

        }

        public void MY_ciulmn_loop(int N, List<int> MY_key, double h, Dictionary<int, int> keyDictionary, string cipherText, int cnt, char[,] arr)
        {
            for (int i = 0; i < MY_key.Count; i++)
            {
                // int x = key[i];

                for (int k = 0; k < h; k++)
                {
                    if (N != 0 && k == h - 1 && keyDictionary[i] >= N) continue;

                    arr[k, keyDictionary[i]] = cipherText[cnt];
                    cnt++;


                }

            }



        }
        public string filling_array(char[,] arr, double h, double w)
        {
            StringBuilder builder = new StringBuilder();
            int start = 0;
            while (start < h)
            {
                int start2 = 0;
                while (start2 < w)
                {
                    builder.Append(arr[start, start2]);
                    start2++;
                }
                start++;
            }
            string res = builder.ToString();
            return res.ToUpper();

        }
        public string Decrypt(string cipherText, List<int> key)
        {
            double C_T_Z = cipherText.Length;
            double w = key.Count;
            double h = Math.Ceiling(C_T_Z / w);
            int cnt = 0;

            char[,] arr = new char[(int)h, (int)w];


            // TRIAL filling the dictionary
            Dictionary<int, int> keyDictionary = new Dictionary<int, int>();

            fill_diriction(keyDictionary, key);
            int N = number_of_fulling_coulmn(cipherText, key);



            MY_ciulmn_loop(N, key, h, keyDictionary, cipherText, cnt, arr);

            string r = filling_array(arr, h, w);


            return r;

        }
        public void for_rounding(double plainTxtSize, double height, double width, char[,] pl, string plainText)
        {
            int C = 0;
            int i = 0;
            while (i < height)
            {
                int j = 0;
                while (j < width)
                {
                    if (C >= plainTxtSize)
                    {
                        pl[i, j] = 'x';
                    }
                    else
                    {
                        pl[i, j] = plainText[C];

                        C++;
                    }
                    j++;

                }
                i++;
            }


        }
        public string my_conca(double height, List<int> key, char[,] pl, Dictionary<int, int> keyDictionary)
        {


            string myC_T = "";
            int i = 0;
            while (i < key.Count)
            {
                int j = 0;
                while (j < height)
                {
                    //0 based
                    myC_T += pl[j, keyDictionary[i]];
                    j++;
                }
                i++;
            }
            return myC_T;


        }



        public string Encrypt(string plainText, List<int> key)
        {
            double P_L_S = plainText.Length;
            double w = key.Count;
            double h = P_L_S / w;
            double x = P_L_S / w;

            //for rounding
            h = Math.Ceiling(x);

            char[,] plain_text = new char[(int)h, (int)w];

            for_rounding(P_L_S, h, w, plain_text, plainText);



            //filling the dictionary
            Dictionary<int, int> kD = new Dictionary<int, int>();
            fill_diriction(kD, key);
            string out_puttt = my_conca(h, key, plain_text, kD);

            return out_puttt.ToUpper();
        }
    }
}
