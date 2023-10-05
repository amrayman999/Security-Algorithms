using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            int M = Convert.ToInt32(Math.Sqrt((cipherText.Count)));
            List<int> prop_Key = new List<int>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            prop_Key = new List<int>(new[] { i, j, k, l });
                            List<int> ciph = Encrypt(plainText, prop_Key);
                            if (ciph.SequenceEqual(cipherText))
                            {
                                return prop_Key;
                            }

                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }
        public int[,] get_transpose(int[,] inp)
        {
            int[,] matrix_out = new int[inp.GetLength(1), inp.GetLength(0)];
            for (int r = 0; r < inp.GetLength(1); r++)
            {
                for (int j = 0; j < inp.GetLength(0); j++)
                {
                    matrix_out[r, j] = inp[j, r];
                }
            }
            return matrix_out;
        }
        public int get_determinant_3x3_mod26(int[,] inp)
        {
            int det = 0;
            int matA = (inp[1, 1] * inp[2, 2]) - (inp[1, 2] * inp[2, 1]);
            int matB = (inp[1, 0] * inp[2, 2]) - (inp[1, 2] * inp[2, 0]);
            int matC = (inp[1, 0] * inp[2, 1]) - (inp[1, 1] * inp[2, 0]);
            det = (inp[0, 0] * matA) - (inp[0, 1] * matB) + (inp[0, 2] * matC);

            return det;
        }
        public int get_determinant_2x2_mod26(int[,] inp)
        {
            int det = 0;
            det += ((inp[0, 0] * inp[1, 1]) - (inp[0, 1] * inp[1, 0]));
            //det = det % 26;
            return det;
        }
        public int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public int[,] ModMinorCofactor(int[,] M, int A)
        {
            int[,] resMat = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = i == 0 ? 1 : 0, y = j == 0 ? 1 : 0, x1 = i == 2 ? 1 : 2, y1 = j == 2 ? 1 : 2;
                    int r = ((M[x, y] * M[x1, y1] - M[x, y1] * M[x1, y]) * (int)Math.Pow(-1, i + j) * A) % 26;
                    resMat[i, j] = r >= 0 ? r : r + 26;
                }
            }
            return resMat;
        }
        public Boolean hasCommonFactor(int x, int y)
        {
            int min = Math.Min(x, y);
            int max = Math.Max(x, y);
            for (int i = 2; i <= min; i++)
            {
                if (min % i == 0 && max % i == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int M = Convert.ToInt32(Math.Sqrt((key.Count)));
            int[,] cipher_matrix = new int[M, cipherText.Count() / M];
            int[,] key_matrix = new int[M, M];
            // filling cipher matrix with its values
            int counter = 0;
            for (int i = 0; i < (cipherText.Count() / M); i++)
            {
                for (int j = 0; j < M; j++)
                {
                    cipher_matrix[j, i] = cipherText[counter];
                    counter++;
                }
            }
            //---------------------------------------------------------------
            // filling key matrix with its values
            int index = 0;
            for (int i = 0; i < M; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    key_matrix[i, k] = key[index];
                    index++;
                }
            }
            //---------------------------------------------------------------
            int det = 0;
            int[,] key_matrix_out = new int[M, M];
            // condition of invalid key flags
            int flag2 = 0;
            int flag = 0;
            Boolean flag1 = true;
            // if the key was 3x3 matrix
            if (M == 3)
            {
                //getting determinant
                det = get_determinant_3x3_mod26(key_matrix);
                if (det < 0)
                {
                    det = mod(det, 26);
                }
                else
                {
                    det = det % 26;
                }
                // calculating b and c 
                decimal mul = 26 - det;
                decimal c = 1;
                for (decimal i = 1; i < 1000000; i += 26)
                {
                    c = i / mul;
                    if ((c % 1) == 0)
                    {
                        flag2 = 1;
                        break;
                    }
                }
                int b = 26 - (int)c;
                // calculating 
                int[,] k_matrix = new int[M, M];
                k_matrix[0, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(0)) * (key_matrix[1, 1] * key_matrix[2, 2] - key_matrix[1, 2] * key_matrix[2, 1])), 26);
                k_matrix[0, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(1)) * (key_matrix[1, 0] * key_matrix[2, 2] - key_matrix[1, 2] * key_matrix[2, 0])), 26);
                k_matrix[0, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (key_matrix[1, 0] * key_matrix[2, 1] - key_matrix[1, 1] * key_matrix[2, 0])), 26);
                k_matrix[1, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(1)) * (key_matrix[0, 1] * key_matrix[2, 2] - key_matrix[0, 2] * key_matrix[2, 1])), 26);
                k_matrix[1, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (key_matrix[0, 0] * key_matrix[2, 2] - key_matrix[0, 2] * key_matrix[2, 0])), 26);
                k_matrix[1, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(3)) * (key_matrix[0, 0] * key_matrix[2, 1] - key_matrix[0, 1] * key_matrix[2, 0])), 26);
                k_matrix[2, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (key_matrix[0, 1] * key_matrix[1, 2] - key_matrix[0, 2] * key_matrix[1, 1])), 26);
                k_matrix[2, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(3)) * (key_matrix[0, 0] * key_matrix[1, 2] - key_matrix[0, 2] * key_matrix[1, 0])), 26);
                k_matrix[2, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(4)) * (key_matrix[0, 0] * key_matrix[1, 1] - key_matrix[0, 1] * key_matrix[1, 0])), 26);
                key_matrix_out = get_transpose(k_matrix);
            }
            else
            {
                flag2 = 1;
                // getting determinant
                det = get_determinant_2x2_mod26(key_matrix);
                int[,] key_transpose = new int[M, M];
                // getting transpose of 2x2 key matrix
                key_transpose[0, 0] = key_matrix[1, 1];
                key_transpose[0, 1] = key_matrix[0, 1] * (-1);
                key_transpose[1, 0] = key_matrix[1, 0] * (-1);
                key_transpose[1, 1] = key_matrix[0, 0];
                int[,] inv_key = new int[M, M];
                decimal k = (decimal)1 / det;
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        inv_key[i, j] = (int)(k * (decimal)key_transpose[i, j]);
                    }
                }
                key_matrix_out = inv_key;
            }
            // checking conditions of valid key 
            for (int i = 0; i < key.Count(); i++)
            {
                if (key[i] < 0)
                {
                    flag = 1;
                    break;
                }
            }
            flag1 = hasCommonFactor(26, det);
            // if the key is invalid
            if (flag > 0 || flag1 == true || flag2 == 0 || det == 0)
            {
                throw new Exception();
            }
            //-------------------------------------------------------
            int[,] plain_matrix = new int[M, cipherText.Count() / M];
            for (int i = 0; i < M; i++)
            {
                for (int l = 0; l < cipherText.Count() / M; l++)
                {
                    plain_matrix[i, l] = 0;
                    for (int k = 0; k < M; k++)
                    {
                        plain_matrix[i, l] += key_matrix_out[i, k] * cipher_matrix[k, l];
                    }
                }
            }
            List<int> plain_out = new List<int>();
            for (int i = 0; i < cipherText.Count() / M; i++)
            {
                for (int a = 0; a < M; a++)
                {
                    plain_out.Add(mod(plain_matrix[a, i], 26));
                }
            }
            return plain_out;
        }
        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int M = Convert.ToInt32(Math.Sqrt(key.Count()));
            int[,] Key = new int[M, M];
            //filling key array with its values
            int index = 0;
            for (int i = 0; i < M; i++)
            {
                for (int k = 0; k < M; k++)
                {
                    Key[i, k] = key[index];
                    index++;
                }
            }
            int[,] Plain = new int[M, plainText.Count() / M];
            //filling plain array with its values
            int counter = 0;
            for (int i = 0; i < (plainText.Count() / M); i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Plain[j, i] = plainText[counter];
                    counter++;
                }
            }
            int[,] cipher = new int[M, plainText.Count() / M];
            // applying matrix multiplication and mod operation 
            for (int i = 0; i < M; i++)
            {
                for (int l = 0; l < plainText.Count() / M; l++)
                {
                    cipher[i, l] = 0;
                    for (int k = 0; k < M; k++)
                    {
                        cipher[i, l] += Key[i, k] * Plain[k, l];
                    }
                }
            }
            // squeezing cipher array to list 
            List<int> cipher_out = new List<int>();
            for (int i = 0; i < plainText.Count() / M; i++)
            {
                for (int a = 0; a < M; a++)
                {
                    cipher_out.Add((cipher[a, i] % 26));
                }
            }
            return cipher_out;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int M = Convert.ToInt32(Math.Sqrt((cipherText.Count)));
            int[,] Plain = new int[M, plainText.Count() / M];
            int[,] cipher_matrix = new int[M, cipherText.Count() / M];
            List<int> mayBeKey = new List<int>();
            int[,] key_matrix = new int[3, 3];
            int[,] prop_key = new int[3, 3];
            // filling cipher matrix with its values
            int counter = 0;
            for (int i = 0; i < (cipherText.Count() / M); i++)
            {
                for (int j = 0; j < M; j++)
                {
                    cipher_matrix[j, i] = cipherText[counter];
                    counter++;
                }
            }
            //filling plain array with its values
            int counter1 = 0;
            for (int i = 0; i < (plainText.Count() / M); i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Plain[j, i] = plainText[counter1];
                    counter1++;
                }
            }
            int det = get_determinant_3x3_mod26(Plain);
            if (det < 0)
            {
                det = mod(det, 26);
            }
            else
            {
                det = det % 26;
            }
            decimal mul = 26 - det;
            decimal c = 1;
            for (decimal i = 1; i < 1000000; i += 26)
            {
                c = i / mul;
                if ((c % 1) == 0)
                {
                    break;
                }
            }
            int b = 26 - (int)c;
            // calculating 
            int[,] k_matrix = new int[M, M];
            k_matrix[0, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(0)) * (Plain[1, 1] * Plain[2, 2] - Plain[1, 2] * Plain[2, 1])), 26);
            k_matrix[0, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(1)) * (Plain[1, 0] * Plain[2, 2] - Plain[1, 2] * Plain[2, 0])), 26);
            k_matrix[0, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (Plain[1, 0] * Plain[2, 1] - Plain[1, 1] * Plain[2, 0])), 26);
            k_matrix[1, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(1)) * (Plain[0, 1] * Plain[2, 2] - Plain[0, 2] * Plain[2, 1])), 26);
            k_matrix[1, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (Plain[0, 0] * Plain[2, 2] - Plain[0, 2] * Plain[2, 0])), 26);
            k_matrix[1, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(3)) * (Plain[0, 0] * Plain[2, 1] - Plain[0, 1] * Plain[2, 0])), 26);
            k_matrix[2, 0] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(2)) * (Plain[0, 1] * Plain[1, 2] - Plain[0, 2] * Plain[1, 1])), 26);
            k_matrix[2, 1] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(3)) * (Plain[0, 0] * Plain[1, 2] - Plain[0, 2] * Plain[1, 0])), 26);
            k_matrix[2, 2] = mod((int)(b * (int)Math.Pow((double)(-1), (double)(4)) * (Plain[0, 0] * Plain[1, 1] - Plain[0, 1] * Plain[1, 0])), 26);
            Plain = get_transpose(k_matrix);
            foreach (var item in Plain)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("\n");
            for (int i = 0; i < M; i++)
            {
                for (int l = 0; l < M; l++)
                {
                    key_matrix[i, l] = 0;
                    for (int k = 0; k < M; k++)
                    {
                        key_matrix[i, l] += cipher_matrix[i, k] * Plain[k, l];
                    }
                }
            }
            prop_key = get_transpose(key_matrix);
            List<int> key_out = new List<int>();
            for (int i = 0; i < plainText.Count() / M; i++)
            {
                for (int a = 0; a < M; a++)
                {
                    key_out.Add((prop_key[a, i] % 26));
                }
            }
            return key_out;

        }

    }
}
