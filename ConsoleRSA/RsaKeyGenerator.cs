using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleRSA
{
    class RsaKeyGenerator
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        public RsaKeyGenerator(BigInteger p, BigInteger q)
        {
            P = p;
            Q = q;
        }

        public (PublicKey publicKey, PrivateKey privateKey) GenerateKeys()
        {
            BigInteger n = ComputeN();
            BigInteger phi = ComputePhi();
            BigInteger e = ComputeE(phi);
            BigInteger d = ComputeD(e, phi);

            PublicKey publicKey = new PublicKey { N = n, E = e };
            PrivateKey privateKey = new PrivateKey { N = n, D = d };

            return (publicKey, privateKey);
        }

        private BigInteger ComputeN() => P * Q;

        private BigInteger ComputePhi() => (P - 1) * (Q - 1);

        private BigInteger ComputeE(BigInteger phi)
        {
            // if only integer, that divides both a,b is 1, then a,b are coprimes
            // equivalent of Gcd==1

            // Choose any number 1 < e < Phi that is co-prime to Phi
            BigInteger e = new BigInteger(2);

            while (e < phi)
            {
                if (BigInteger.GreatestCommonDivisor(e, phi) == 1)
                {
                    return e;
                }

                ++e;
            }

            throw new InvalidOperationException("Cannot find e (part of the public key)");
        }

        private BigInteger ComputeD(BigInteger e, BigInteger phi)
        {
            BigInteger d = 2;

            // Compute a value for d such that
            // (d * e) % Phi = 1
            while(true)
            {
                if (((d * e) % phi) == 1)
                {
                    return d;
                }

                ++d;
            }
        }
    }
}
