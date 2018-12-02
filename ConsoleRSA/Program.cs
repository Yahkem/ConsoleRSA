using System;
using System.Text;
using System.Numerics;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            //double
            //    p = 3, 
            //    q = 7;

            //double n = p * q; // modulus for public key and the private keys

            //// length of common factors (totient)
            //double phi = (q - 1) * (p - 1);

            //// for encryption key:
            //// it has to be between <1;l>, and coprime

            //// find coprime
            //double e = 2;
            //while (e < phi)
            //{
            //    if (Gcd((int)e, (int)phi) == 1)
            //        break;
            //    else
            //        ++e;
            //}
            //Console.WriteLine($"Coprime: {e}");

            //// compute d to satisfy the congruence relation
            //// d = (1+ k * totient) / coprime | for some integer k
            //int k = 2;
            //double d = (1 + (k * phi)) / e;

            //Console.WriteLine($"1+{k}*{phi}/{e}={d}");

            //Console.WriteLine($"Public key: (n={n}, e={e})");
            //Console.WriteLine($"Private key: (n={n}, d={d})");
            // The public key is made of the modulus n
            // and the public (or encryption) exponent e. 
            // The private key is made of the modulus n
            // and the private (or decryption) exponent d
            // which must be kept secret.

            // encrypting
            // message M to a number m smaller than n
            // c = m^e % n

            // decrypting
            // m = c^d % n

            //double msg = 20;
            //double c = Math.Pow(msg, e) % n;
            //Console.WriteLine($"Encrypted data: {c}");

            //double m = Math.Pow(c, d) % n;
            //Console.WriteLine($"Decrypted data: {m}");
            bool продолжать = true;

            while (продолжать)
            {
                Console.Write("Write your message: ");
                string message = Console.ReadLine();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                foreach (var b in messageBytes)
                {
                    BigInteger msgInInteger = new BigInteger(b);

                    var res = Test1(msgInInteger);

                    byte[] backBytes = res.msgBack.ToByteArray();
                    string backMessage = Encoding.UTF8.GetString(backBytes);

                    //Console.WriteLine(messageBytes);
                    Console.WriteLine(msgInInteger);
                    //Console.WriteLine(backBytes);
                    Console.WriteLine(backMessage);
                }

                Console.WriteLine("Continue (Y/N)?");
                продолжать = Console.ReadKey().Key != ConsoleKey.N;

                Console.WriteLine($"\n\n{new string('=', 100)}\n");
            }

            //Test1(20);
            //Console.ReadKey();
        }
        
        public static (BigInteger ciphertext, BigInteger msgBack) Test1(BigInteger msg)
        {
            BigInteger
                p = 17,
                q = 53;
            var generator = new RsaKeyGenerator(p, q);

            var (publicKey, privateKey) = generator.GenerateKeys();

            Console.WriteLine($"Public Key: {publicKey.ToString()}");
            Console.WriteLine($"Private Key: {privateKey.ToString()}");

            // c = m^e % n
            BigInteger ciphertext = BigInteger.Pow(msg, (int)publicKey.E);
            ciphertext %= publicKey.N;
            // m = c^d % n
            BigInteger msgBack = BigInteger.Pow(ciphertext, (int)privateKey.D);
            msgBack %= privateKey.N;


            Console.WriteLine($"Message: {msg}");
            Console.WriteLine($"Ciphertext: {ciphertext}, {Encoding.UTF8.GetString(ciphertext.ToByteArray())}");
            Console.WriteLine($"Message back: {msgBack}");

            return (ciphertext, msgBack);
        }

        //public static bool AreCoprime(BigInteger x, BigInteger y)
        //{
        //    while (x != 0 && y != 0)
        //    {
        //        if (x > y)
        //            x %= y;
        //        else
        //            y %= x;
        //    }

        //    BigInteger gcd = x > y ? x : y;
        //    //BigInteger gcd = GreatestCommonDivisor(x, y);// x > y ? x : y;

        //    return gcd == 1;
        //}

        //public static int Gcd(int a, int b)
        //{
        //    int tmp;

        //    while(true)
        //    {
        //        tmp = a % b;
        //        if (tmp == 0)
        //            return b;

        //        a = b;
        //        b = tmp;
        //    }
        //}

        //public static BigInteger GreatestCommonDivisor(BigInteger m, BigInteger n)
        //{
        //    BigInteger tmp = 0;

        //    if (m < n)
        //    {
        //        tmp = m;
        //        m = n;
        //        n = tmp;
        //    }

        //    while (n != 0)
        //    {
        //        tmp = m % n;
        //        m = n;
        //        n = tmp;
        //    }

        //    return m;
        //}

        //public static (BigInteger m2, BigInteger n2) 
        //    Reduce(BigInteger m, BigInteger n)
        //{
        //    var gcd = GreatestCommonDivisor(m, n);
        //    m /= gcd;
        //    n /= gcd;

        //    return (m, n);
        //}
    }
}
