using System;
using System.Text;
using System.Numerics;
using System.Linq;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueLoop = true;

            int p, q;

            do
            {
                Console.WriteLine("Write P:");
                int.TryParse(Console.ReadLine(), out p);
            } while (!IsPrime(p));

            do
            {
                Console.WriteLine("Write Q:");
                int.TryParse(Console.ReadLine(), out q);
            } while (!IsPrime(q));


            while (continueLoop)
            {
                Console.Write("Write your message: ");
                string message = Console.ReadLine();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                foreach (var b in messageBytes)
                {
                    BigInteger msgInInteger = new BigInteger(b);

                    var res = PerformRsa(msgInInteger, p, q);

                    byte[] backBytes = res.msgBack.ToByteArray();
                    string backMessage = Encoding.UTF8.GetString(backBytes);

                    //Console.WriteLine(messageBytes);
                    Console.WriteLine(msgInInteger);
                    //Console.WriteLine(backBytes);
                    Console.WriteLine(backMessage);
                }

                Console.WriteLine("Continue (Y/N)?");
                continueLoop = Console.ReadKey().Key != ConsoleKey.N;

                Console.WriteLine($"\n\n{new string('=', 100)}\n");
            }

            //PerformRsa(20);
            //Console.ReadKey();
        }
        
        public static (BigInteger ciphertext, BigInteger msgBack) PerformRsa(BigInteger msg, int _p, int _q)
        {
            BigInteger
                p = new BigInteger(_p),
                q = new BigInteger(_q);
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
            Console.WriteLine($"Ciphertext: {ciphertext}");
            Console.WriteLine($"Message back: {msgBack}");

            return (ciphertext, msgBack);
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
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
