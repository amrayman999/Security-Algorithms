using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            //convert the whole message into capital letters .. because  the matrix is capital letters
            StringBuilder tmp_str = new StringBuilder(cipherText.ToUpper());

            StringBuilder tmp_key = new StringBuilder(key.ToUpper());


            for (int count1 = 0; count1 < tmp_key.Length; count1 += 1)
            {

                if (tmp_key[count1] == 'J')
                {
                    tmp_key[count1] = 'I';
                }

            }

            //remove all the repeated characters from the keyword
            for (int count1 = 0; count1 < tmp_key.Length; count1++)
            {

                //loop for the previous characters to see if the current character existed before
                for (int count2 = 0; count2 < count1; count2++)
                {

                    if (tmp_key[count1] == tmp_key[count2])
                    {
                        tmp_key = tmp_key.Remove(count1, 1);
                        count1--;
                        break;
                    }

                }

            }

            int keyword_stored_flag = 0;
            int exists_in_keyord = 0;

            char[,] matrix = new char[5, 5];

            //this loop stores the keyword and the rest of the alphabets in the matrix
            for (int row_count = 0, alphabet_counter = 0; row_count < 5; row_count++)
            {

                for (int col_count = 0; col_count < 5; col_count++)
                {

                    if ((((row_count * 5) + col_count) < tmp_key.Length) && (keyword_stored_flag == 0))
                    {
                        matrix[row_count, col_count] = tmp_key[(row_count * 5) + col_count];
                    }
                    else
                    {
                        keyword_stored_flag = 1;
                        exists_in_keyord = 0;

                        for (int count1 = 0; count1 < tmp_key.Length; count1++)
                        {

                            if ('A' + alphabet_counter == tmp_key[count1]) //if the current alphabet exists in the keyword
                            {
                                exists_in_keyord = 1;
                                break;
                            }

                        }

                        if ((exists_in_keyord == 0) && (('A' + alphabet_counter) != 'J'))
                        {
                            matrix[row_count, col_count] = (char)((int)'A' + alphabet_counter);//store it in the matrix
                        }
                        else
                        {
                            col_count--;
                        }

                        alphabet_counter++;
                    }

                }

            }

            //perform the decryption
            int letter1_row = 0, letter1_col = 0, letter2_row = 0, letter2_col = 0;

            for (int m_count = 0; m_count < tmp_str.Length; m_count += 2)
            {

                get_index(matrix, tmp_str[m_count], ref letter1_row, ref letter1_col);
                get_index(matrix, tmp_str[m_count + 1], ref letter2_row, ref letter2_col);

                if (letter1_row == letter2_row)
                {
                    tmp_str[m_count] = matrix[letter1_row, (letter1_col + 4) % 5];
                    tmp_str[m_count + 1] = matrix[letter2_row, (letter2_col + 4) % 5];
                }
                else if (letter1_col == letter2_col)
                {
                    tmp_str[m_count] = matrix[(letter1_row + 4) % 5, letter1_col];
                    tmp_str[m_count + 1] = matrix[(letter2_row + 4) % 5, letter2_col];
                }
                else
                {
                    tmp_str[m_count] = matrix[letter1_row, letter2_col];
                    tmp_str[m_count + 1] = matrix[letter2_row, letter1_col];
                }


            }

            for (int i = tmp_str.Length - 1; i >= 0; i--)
            {
                if (tmp_str[i] == 'X')
                {

                    if (i > 0)
                    {
                        if (i == (tmp_str.Length - 1)) //if it's the X on the last string
                        {
                            tmp_str.Remove(i, 1); // remove th X
                        }
                        else if (tmp_str[i - 1] == tmp_str[i + 1]) //if it's the X that separates the 2 same chars
                        {
                            if (tmp_str[i - 2] != 'R' && tmp_str[i + 2] != 'R')
                            {
                                tmp_str.Remove(i, 1); // remove the X
                            }

                        }
                    }

                }
            }
            for (int i = tmp_str.Length - 1; i >= 0; i--)
            {
                if (tmp_str[i] == 'X')
                {

                    if (i > 0)
                    {
                        if (tmp_str[i - 1] == tmp_str[i + 1]) //if it's the X that separates the 2 same chars
                        {
                            if (tmp_str[i - 2] == 'R' && tmp_str[i + 2] != 'R')//if it's the X that its previous char is R and the next char is any char
                            {
                                tmp_str.Remove(i, 1); // remove the X
                            }

                        }
                    }

                }
            }

            string str = tmp_str.ToString();

            if (char.IsUpper(cipherText[0]))
            {
                return str.ToLower();
            }
            return str;
        }

        public string Encrypt(string plainText, string key)
        {

            StringBuilder tmp_str = new StringBuilder(plainText.ToUpper());

            for (int count1 = 0; count1 < tmp_str.Length; count1 += 1)
            {

                if (tmp_str[count1] == 'j')
                {
                    tmp_str[count1] = 'i';
                }

                if (tmp_str[count1] == 'J')
                {
                    tmp_str[count1] = 'I';
                }

            }

            for (int count1 = 0; ((count1 < tmp_str.Length) && ((count1 + 1) < tmp_str.Length)); count1 += 2)
            {

                if (tmp_str[count1] == tmp_str[count1 + 1])
                {
                    tmp_str.Insert(count1 + 1, "x");
                }

            }

            //if the size of the message is odd .. then add x at the end.
            if ((tmp_str.Length % 2) == 1)
            {
                tmp_str.Append("x");
            }

            for (int count1 = 0; count1 < tmp_str.Length; count1++)
            {

                if (tmp_str[count1] >= 'a' && tmp_str[count1] <= 'z')
                {
                    tmp_str[count1] -= (char)((int)'a' - (int)'A');
                }

            }

            StringBuilder tmp_key = new StringBuilder(key);

            for (int count1 = 0; count1 < tmp_key.Length; count1 += 1)
            {

                if (tmp_key[count1] == 'j')
                {
                    tmp_key[count1] = 'i';
                }

                if (tmp_key[count1] == 'J')
                {
                    tmp_key[count1] = 'I';
                }

            }

            for (int count1 = 0; count1 < tmp_key.Length; count1++)
            {
                for (int count2 = 0; count2 < count1; count2++)
                {

                    if (tmp_key[count1] == tmp_key[count2]) //if a character existed before
                    {
                        tmp_key = tmp_key.Remove(count1, 1); //erase this repeated element
                        count1--;
                        break; // exit the loop
                    }

                }

            }

            for (int count1 = 0; count1 < tmp_key.Length; count1++)
            {

                if (tmp_key[count1] >= 'a' && tmp_key[count1] <= 'z') //if the letter is small letter
                {
                    tmp_key[count1] -= (char)((int)'a' - (int)'A'); //convert it into capital letter
                }

            }

            int keyword_stored_flag = 0;
            int exists_in_keyord = 0;

            char[,] matrix = new char[5, 5];

            //loop stores the keyword and the rest of the alphabets in the matrix
            for (int row_count = 0, alphabet_counter = 0; row_count < 5; row_count++)
            {

                for (int col_count = 0; col_count < 5; col_count++)
                {
                    if ((((row_count * 5) + col_count) < tmp_key.Length) && (keyword_stored_flag == 0))
                    {
                        matrix[row_count, col_count] = tmp_key[(row_count * 5) + col_count];
                    }
                    else
                    {
                        keyword_stored_flag = 1;
                        exists_in_keyord = 0;
                        for (int count1 = 0; count1 < tmp_key.Length; count1++)
                        {

                            if ('A' + alphabet_counter == tmp_key[count1])
                            {
                                exists_in_keyord = 1;
                                break;
                            }

                        }

                        if ((exists_in_keyord == 0) && (('A' + alphabet_counter) != 'J'))
                        {
                            matrix[row_count, col_count] = (char)((int)'A' + alphabet_counter);//store it in the matrix
                        }
                        else
                        {
                            col_count--;
                        }

                        alphabet_counter++;
                    }

                }

            }


            //perform the cipher
            int letter1_row = 0, letter1_col = 0, letter2_row = 0, letter2_col = 0;

            for (int m_count = 0; m_count < tmp_str.Length; m_count += 2)
            {

                get_index(matrix, tmp_str[m_count], ref letter1_row, ref letter1_col);
                get_index(matrix, tmp_str[m_count + 1], ref letter2_row, ref letter2_col);

                if (letter1_row == letter2_row)
                {
                    tmp_str[m_count] = matrix[letter1_row, (letter1_col + 1) % 5];
                    tmp_str[m_count + 1] = matrix[letter2_row, (letter2_col + 1) % 5];
                }
                else if (letter1_col == letter2_col)
                {
                    tmp_str[m_count] = matrix[(letter1_row + 1) % 5, letter1_col];
                    tmp_str[m_count + 1] = matrix[(letter2_row + 1) % 5, letter2_col];
                }
                else
                {
                    tmp_str[m_count] = matrix[letter1_row, letter2_col];
                    tmp_str[m_count + 1] = matrix[letter2_row, letter1_col];
                }

            }

            string str = tmp_str.ToString();

            return str;


        }
        void get_index(char[,] matrix, char chr, ref int row, ref int col)
        {

            //keep looping in  the matrix until you find the character and then return its coordinates
            for (int row_count = 0, char_match_flag = 0; char_match_flag == 0; row_count++)
            {

                for (int col_count = 0; col_count < 5; col_count++)
                {

                    if (matrix[row_count, col_count] == chr)
                    {
                        char_match_flag = 1;
                        col = col_count;
                        row = row_count;
                        break;
                    }

                }

            }

        }
    }

}
