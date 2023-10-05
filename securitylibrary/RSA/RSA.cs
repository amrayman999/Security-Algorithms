using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {

        public int big_power(int a, int b, int c)
        {
            int res = 1;
            for (int i = 0; i < b; i++)
            {
                res = (res * a) % c;
            }
            return res;
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int c = big_power(M, e, n) % n;
            return c;

        }
        public  int MultiplicativeInverse(int num, int baseNum)
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
        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int Q_n = (p-1)*(q-1);
            int e_inv = MultiplicativeInverse(e,Q_n);
            int d = e_inv % (Q_n);
            int M = big_power(C,d,n)%n;
            return M;

        }
    }
}
