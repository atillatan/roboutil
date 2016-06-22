using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace RoboUtil.utils
{
    public class CryptographyUtil
    {

        private const string _secretEntropy = @"&&&////// ANKARA ////// PROJECT ENTROPY IDENTITY! &&&";
        private const string _rootKeyPath = @"Software\Usis\ConnectionStrings";
        private const string ENCRYPTION_KEY = "key";

        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());

        /// <summary>
        /// Encrypts any string using the Rijndael algorithm.
        /// </summary>
        /// <param name="inputText">The string to encrypt.</param>
        /// <returns>A Base64 encrypted string.</returns>
        public static string Encrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);
            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a previously encrypted Rijndael algorithm string.
        /// </summary>
        /// <param name="inputText">The encrypted string to decrypt.</param>
        /// <returns>A decrypted string.</returns>
        public static string Decrypt(string inputText)
        {
            inputText = inputText.Replace("%3d", "=").Replace("%2f", "/").Replace(" ", "+");
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            byte[] encryptedData = Convert.FromBase64String(inputText);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        }

        /// <summary>
        /// RSACryptoServiceProvider
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetDecrypted(string str)
        {
            string keyContainerName = "SAFAKSDKE4R32W43242J342K34@2016";
            CspParameters paramCSP = new CspParameters();
            paramCSP.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(paramCSP);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToDecrypt = ByteConverter.GetBytes(str);
            byte[] decriptedData = rsaCSP.Decrypt(dataToDecrypt, false);
            return ByteConverter.GetString(decriptedData);
        }

        /// <summary>
        /// RSACryptoServiceProvider
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetEncripted(string str)
        {
            string keyContainerName = "SAFAKSDKE4R32W43242J342K34@2016";
            CspParameters paramCSP = new CspParameters();
            paramCSP.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(paramCSP);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(str);
            byte[] encryptedData = rsaCSP.Encrypt(dataToEncrypt, false);
            return ByteConverter.GetString(encryptedData);
        }

        /// <summary>
        /// Encrypts a string using the MD5 hash encryption algorithm.
        /// Message Digest is 128-bit and is commonly used to verify data, by checking
        /// the 'MD5 checksum' of the data. Information on MD5 can be found at:
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the MD5 hash.</returns>
        public static string MD5Hash(string Data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Computes an MD5 hash for the provided file.
        /// </summary>
        /// <param name="filename">The full path to the file</param>
        /// <returns>A hexadecimal encoded MD5 hash for the file.</returns>
        public static string MD5FileHash(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    byte[] hash = md5.ComputeHash(stream);

                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        builder.AppendFormat("{0:x2}", b);
                    }
                    return builder.ToString();
                }
            }
        }

        /// <summary>
        /// Encrypts a string using the SHA256 (Secure Hash Algorithm) algorithm.
        /// Details:
        /// This works in the same manner as MD5, providing however 256bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA256 algorithm.</returns>
        public static string SHA256Hash(string Data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA384(Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 384bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA384 algorithm.</returns>
        public static string SHA384Hash(string Data)
        {
            SHA384 sha = new SHA384Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA512 (Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 512bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA512 algorithm.</returns>
        public static string SHA512Hash(string Data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public static string DecryptWithRSA(string strEncryptPassword)
        {
            string keyContainerName = "sokaktasayamamgibi";
            CspParameters paramCSP = new CspParameters();
            paramCSP.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(paramCSP);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToDecrypt = Convert.FromBase64String(strEncryptPassword);
            byte[] decriptedData = rsaCSP.Decrypt(dataToDecrypt, false);
            return ByteConverter.GetString(decriptedData);
        }

        //-----------------------------------------------------------------------
        public static string EncriptWithRSA(string strPassword)
        {
            string keyContainerName = "sokaktasayamamgibi";
            CspParameters paramCSP = new CspParameters();
            paramCSP.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider(paramCSP);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(strPassword);
            byte[] encryptedData = rsaCSP.Encrypt(dataToEncrypt, false);
            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptWithDES(string encryptedString)
        {
            string key = "de/.1s*q";
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Key = Encoding.ASCII.GetBytes(key);
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedString)))
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public static string EncryptWithDES(string decryptedString)
        {
            string key = "de/.1s*q";
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Key = Encoding.ASCII.GetBytes(key);
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] data = Encoding.Default.GetBytes(decryptedString);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

        public static string GetConnectinStringMetadata()
        {

            return "res://*/Model.UsisDataModel.csdl|res://*/Model.UsisDataModel.ssdl|res://*/Model.UsisDataModel.msl";

            //var rootKey = Registry.LocalMachine.OpenSubKey(_rootKeyPath);
            //if (rootKey == null) throw new ConfigurationErrorsException("Connection strings are not present in the registry");
            //return Encoding.ASCII.GetString(System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String((string)rootKey.GetValue("MetaData")), Encoding.ASCII.GetBytes(_secretEntropy), DataProtectionScope.LocalMachine));
        }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["UsisConnectionString"].ConnectionString;
 
        }

        public static string GetLogConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["UsisLogConnectionString"].ConnectionString;
 
        }
    }
}
