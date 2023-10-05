using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public int big_power(int a, int b, int c)
        {
            int res = 1;
            for (int i = 0; i < b; i++)
            {
                res = (res * a) % c;
            }
            return res;
        }
        public int MultiplicativeInverse(int num, int baseNum)
        {
            for (int i = 1; i < baseNum; i++)
            {
                if ((num * i) % baseNum == 1)
                {
                    return i;
                }
            }

            throw new Exception("Multiplicative inverse does not exist.");
        }
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            int K = big_power(y, k, q) % q;
            long c1 = big_power(alpha,k,q) % q;
            long c2 = (K * m) % q;
            List<long> res = new List<long>() { c1, c2 };
            return res;



        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            int key = big_power(c1,x,q) % q;
            int k_inv = MultiplicativeInverse(key, q) % q;
            int M = (c2* k_inv) % q;
            return M;

        }
    }
}
