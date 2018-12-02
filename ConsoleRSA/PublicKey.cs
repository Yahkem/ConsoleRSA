using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleRSA
{
    class PublicKey
    {
        public BigInteger N { get; set; }
        public BigInteger E { get; set; }
        
        public override string ToString() => $"(N = {N}, E = {E})";
    }
}
