using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleRSA
{
    class PrivateKey
    {
        public BigInteger N { get; set; }
        public BigInteger D { get; set; }

        public override string ToString() => $"(N = {N}, D = {D})";
    }
}
