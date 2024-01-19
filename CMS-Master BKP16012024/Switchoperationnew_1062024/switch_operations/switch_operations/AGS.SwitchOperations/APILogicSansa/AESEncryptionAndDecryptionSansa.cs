using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AGS.SwitchOperations
{
    class AESEncryptionAndDecryptionSansa
    {
        public static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            //var keyBytes = new byte[16];
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);

            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));

            return new RijndaelManaged

            {

                Mode = CipherMode.CBC,

                Padding = PaddingMode.PKCS7,

                KeySize = 128,

                BlockSize = 128,

                Key = keyBytes,

                IV = keyBytes

            };

        }





        public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {

            byte[] enData = null;

            try
            {

                enData = rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            }

            catch (Exception ex)
            {

                enData = null;

            }

            return enData;

        }



        public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {

            byte[] deData = null;

            try
            {

                deData = rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            }

            catch (Exception ex)
            {

                deData = null;

            }

            return deData;

        }



        public static String Encrypt(String plainText, String key)
        {

            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            string enData = "";

            try
            {

                enData = Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));

            }

            catch (Exception ex)
            {

                enData = null;

            }

            return enData;

        }



        public static String Decrypt(String encryptedText, String key)
        {

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            string deData = "";

            try
            {

                deData = Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));

            }

            catch (Exception ex)
            {

                deData = null;

            }

            return deData;

        }
    }
}
