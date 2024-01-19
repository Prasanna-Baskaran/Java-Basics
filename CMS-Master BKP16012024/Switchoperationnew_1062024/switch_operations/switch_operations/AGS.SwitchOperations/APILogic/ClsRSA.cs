using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
namespace AGS.SwitchOperations
{
    internal class RSAModel
    {
        public Dictionary<string, long> PublicKey { get; set; }
        public Dictionary<string, long> PrivateKey { get; set; }
    }
    internal class RSAModelProfile
    {
        public static RSAModel CurrentRSAModel
        {
            get
            {
                if (HttpContext.Current.Session["RSAContext"] != null)
                    return (RSAModel)HttpContext.Current.Session["RSAContext"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["RSAContext"] = value;
            }
        }

        public static Dictionary<string, long> PublicKey
        {
            //get;
            //set;
            get
            {
                if (HttpContext.Current.Session["RSAContext"] != null)
                    return ((RSAModel)HttpContext.Current.Session["RSAContext"]).PublicKey;
                else
                    return new Dictionary<string, long> { { "n", 0 }, { "ek", 0 } };
            }
        }
        public static Dictionary<string, long> PrivateKey
        {
            //get;
            //set;
            get
            {
                if (HttpContext.Current.Session["RSAContext"] != null)
                    return ((RSAModel)HttpContext.Current.Session["RSAContext"]).PrivateKey;
                else
                    return new Dictionary<string, long> { { "n", 0 }, { "d", 0 } };
            }
        }
    }
    internal sealed class ClsRSA
    {

        private static int[] PrimeNumbers = { 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 
                                 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 
                                 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 
                                 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 
                                 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 
                                 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 
                                 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997 };
        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static Dictionary<string, long> GetRSAKeys() //publik key is (n,ek) AND private key is (n,d)
        {
            int p = GetRandomPrimeNumber();
            int q = GetRandomPrimeNumber();
            while (p == q)
            {
                q = GetRandomPrimeNumber();
            }

            long n = p * q;
            long totient = (p - 1) * (q - 1);
            // let Choose e as 1 < e < totient that is coprime to totient. Choosing a prime number for e leaves us only to check that e is not a divisor of totient.
            long ek = random.Next(2, (((p - 1) * (q - 1)) - 1));
            while (gcd(totient, ek) != 1)
            {
                ek = random.Next(2, (((p - 1) * (q - 1)) - 1));
            }
            long d = modInverse(ek, totient);

            return new Dictionary<string, long> { { "n", n }, { "ek", ek }, { "d", d } };
        }

        private static long modInverse(long a, long n)
        {
            long i = n, v = 0, d = 1;
            while (a > 0)
            {
                long t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        private static int GetRandomPrimeNumber()
        {
            int index = random.Next(PrimeNumbers.Length);
            return PrimeNumbers[index];
            //Array.Clear(PrimeNumbers, index, 1);
        }

        private static long gcd(long n1, long n2)
        {
            long rem = 0;
            while (n2 > 0)
            {
                rem = n1 % n2;
                if (rem == 0)
                    return n2;
                n1 = n2;
                n2 = rem;
            }
            return n1;
        }

        public static string EncryptRSA(string Text)
        {
            string EncData = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    long n, ek;
                    RSAModelProfile.PublicKey.TryGetValue("n", out n);
                    RSAModelProfile.PublicKey.TryGetValue("ek", out ek);

                    foreach (char c in Text)
                    {
                        string EncChar = Convert.ToString(EncDycChar(Convert.ToInt64(c), ek, n));
                        if (EncChar.Length < 12)
                        {
                            string strAppend = "000000000000";
                            EncChar = strAppend.Substring(0, (12 - EncChar.Length)) + EncChar;
                        }
                        List<string> ChunkEncChar = Chunk(EncChar, 3).ToList();
                        string EncString = string.Empty;
                        for (int i = 0; i < ChunkEncChar.Count; i++)
                        {
                            ChunkEncChar[i] = Convert.ToString(Math.Floor(Convert.ToDecimal(random.Next(1, 9)))) + ChunkEncChar[i];
                            EncString = EncString + (char)Convert.ToInt32(ChunkEncChar[i]);
                        }
                        EncData = EncData + EncString;
                    }
                    EncData = Merge(EncData);
                }
            }
            catch (Exception ex)
            {
                EncData = string.Empty;
            }
            return EncData;
        }

        public static string DecryptRSA(string Text)
        {
            string DecData = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    string NoPadText = string.Empty;
                    long n, d;
                    RSAModelProfile.PrivateKey.TryGetValue("n", out n);
                    RSAModelProfile.PrivateKey.TryGetValue("d", out d);
                    foreach (char c in Text)
                    {
                        if (Convert.ToInt64(c) < 10000)
                        {
                            NoPadText = NoPadText + c;
                        }
                    }
                    Text = NoPadText;
                    String[] EncCh = Chunk(Text, 4).ToArray();
                    for (int j = 0; j < EncCh.Length; j++)
                    {
                        string EncChar = string.Empty;
                        foreach (char c in EncCh[j])
                        {
                            string Char = Convert.ToString((int)c).Substring(1, 3);
                            EncChar = EncChar + Char;
                        }
                        char DycChar = (char)EncDycChar(Convert.ToInt64(EncChar), d, n);
                        DecData = DecData + DycChar;
                    }
                }
            }
            catch (Exception ex)
            {

                DecData = string.Empty;
            }
            return DecData;
        }

        public static string DecryptRSA(string Text, string Keys)
        {
            string DecData = string.Empty;
            if (!string.IsNullOrEmpty(Text))
            {
                string NoPadText = string.Empty;
                Int64 n, d;
                n = Convert.ToInt64(Keys.Split(';')[1]);
                d = Convert.ToInt64(Keys.Split(';')[0]);
                //UserProfile.PrivateKey.TryGetValue("n", out n);
                //UserProfile.PrivateKey.TryGetValue("d", out d);
                foreach (char c in Text)
                {
                    if (Convert.ToInt64(c) < 10000)
                    {
                        NoPadText = NoPadText + c;
                    }
                }
                Text = NoPadText;
                String[] EncCh = Chunk(Text, 4).ToArray();
                for (int j = 0; j < EncCh.Length; j++)
                {
                    string EncChar = string.Empty;
                    foreach (char c in EncCh[j])
                    {
                        string Char = Convert.ToString((int)c).Substring(1, 3);
                        EncChar = EncChar + Char;
                    }
                    char DycChar = (char)EncDycChar(Convert.ToInt64(EncChar), d, n);
                    DecData = DecData + DycChar;
                }
            }
            return DecData;
        }

        private static long EncDycChar(long val, long root, long mod)
        {
            long Rem = root % 2;
            long Quotient = (root - Rem) / 2;
            long value = 0;
            long intb = 1;
            for (long k = 0; k < Quotient; k++)
            {
                long inta = (val * val) % mod;
                intb = (intb * inta) % mod;
            }
            if (Rem == 1)
            {
                intb = (intb * val) % mod;
            }

            value = intb;

            return value;
        }

        private static IEnumerable<string> Chunk(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private static string Merge(string strChar)
        {
            string strMerge = string.Empty;
            if (!string.IsNullOrEmpty(strChar))
            {
                string[] CharArray = strChar.Select(c => c.ToString()).ToArray();
                for (int i = 0; i < CharArray.Length; i++)
                {
                    int b = Convert.ToInt32(Math.Floor(random.NextDouble() * 2));
                    strMerge = strMerge + RandomString(b) + CharArray[i];
                    if (i == (CharArray.Length - 1))
                    {
                        strMerge = strMerge + RandomString(Convert.ToInt32(Math.Floor(random.NextDouble() * 2)));
                    }
                }
            }
            return strMerge;
        }

        private static string RandomString(int size)
        {
            string text = string.Empty;
            //string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_=[]{}";

            for (var i = 0; i < size; i++)
            {
                text += (char)random.Next(10000, 20000);
            }
            return text;
        }

        public static string GetPublicKey()
        {
            long n, ek;
            RSAModelProfile.PublicKey.TryGetValue("n", out n);
            RSAModelProfile.PublicKey.TryGetValue("ek", out ek);
            return Convert.ToString(n) + ";" + Convert.ToString(ek);
        }
    }

    public class RSABuilder
    {
        public void DisposeRSA()
        {
            RSAModelProfile.CurrentRSAModel = null;
        }
        public string InitiateRSA()
        {
            RSAModel objRSA = new RSAModel();
            Dictionary<string, long> Keys = ClsRSA.GetRSAKeys();
            long n, ek, d;
            Keys.TryGetValue("n", out n);
            Keys.TryGetValue("ek", out ek);
            Keys.TryGetValue("d", out d);
            objRSA.PublicKey = new Dictionary<string, long> { { "n", n }, { "ek", ek } };
            objRSA.PrivateKey = new Dictionary<string, long> { { "n", n }, { "d", d } };
            RSAModelProfile.CurrentRSAModel = objRSA;
            return ClsRSA.GetPublicKey();
        }
        public string Decrypt(string Str)
        {
            return ClsRSA.DecryptRSA(Str);
        }
    }
}