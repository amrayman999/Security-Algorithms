using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
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
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            int ya = (big_power(alpha, xa , q)) % q;
            int yb = (big_power(alpha, xb , q)) % q;
            int k1 = (big_power(yb, xa , q)) % q;
            int k2 = (big_power(ya, xb, q)) % q;
            List<int> keys = new List<int>() { k1, k2 };
            return keys;
        }
    }
}
