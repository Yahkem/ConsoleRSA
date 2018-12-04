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
            var originalConsoleTextColor = Console.ForegroundColor;
            int p, q;

            while (continueLoop)
            {
                do
                {
                    Console.Write("Input P - must be prime number: ");
                    int.TryParse(Console.ReadLine(), out p);
                    Console.WriteLine();
                } while (!IsPrime(p));

                do
                {
                    Console.Write("Input Q - must be prime number and different from P: ");
                    int.TryParse(Console.ReadLine(), out q);
                    Console.WriteLine();
                } while (!IsPrime(q) || q == p);

                var generator = new RsaKeyGenerator(new BigInteger(p), new BigInteger(q));

                var (publicKey, privateKey) = generator.GenerateKeys();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Public Key: {publicKey.ToString()}");
                Console.WriteLine($"Private Key: {privateKey.ToString()}\n");
                Console.ForegroundColor = originalConsoleTextColor;

                Console.Write("Write your message: ");
                string message = Console.ReadLine(),
                    decryptedMessage = "",
                    fullCiphertext = "";

                foreach (var letter in message)
                {
                    Console.WriteLine($"Letter: {letter}");
                    var bytes = Encoding.UTF8.GetBytes(letter.ToString());
                    BigInteger msgInInteger = new BigInteger(bytes);

                    var res = PerformRsa(msgInInteger, publicKey, privateKey);

                    byte[] decryptedBytes = res.decryptedText.ToByteArray();
                    string decryptedLetter = Encoding.UTF8.GetString(decryptedBytes);

                    decryptedMessage += decryptedLetter;
                    fullCiphertext += res.ciphertext.ToString();

                    Console.WriteLine($"Ciphertext: {res.ciphertext}");
                    Console.WriteLine($"Decrypted letter: {decryptedLetter}\n");
                }
                
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Full ciphertext: {fullCiphertext}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Decrypted message: {decryptedMessage}");
                Console.ForegroundColor = originalConsoleTextColor;
                Console.Write("\nContinue (Y/N)? ");
                continueLoop = Console.ReadKey().Key != ConsoleKey.N;

                Console.WriteLine($"\n\n{new string('=', 100)}");
            }

            Console.WriteLine("Press any key to end the program...");
            Console.ReadKey();
        }
        
        public static (BigInteger ciphertext, BigInteger decryptedText) 
            PerformRsa(BigInteger msg, PublicKey publicKey, PrivateKey privateKey)
        {
            // c = m^e % n
            BigInteger ciphertext = BigInteger.Pow(msg, (int)publicKey.E);
            ciphertext %= publicKey.N;
            // m = c^d % n
            BigInteger decryptedText = BigInteger.Pow(ciphertext, (int)privateKey.D);
            decryptedText %= privateKey.N;

            return (ciphertext, decryptedText);
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
