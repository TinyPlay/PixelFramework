/*
 * Pixel Framework
 * 
 * This framework allows you to develop games for mobile devices even faster than you normally do.
 * It includes a lot of useful classes, resources, and examples to get your project started.
 * The framework will be especially useful for developers of hyper-casual games.
 *
 * This framework was developed specifically for Pixel Incubator members with the support of TinyPlay.
 * You can read more about Incubator at the official website:
 * https://pixinc.club/
 *
 * @developer       Ilya Rastorguev
 * @autor           TinyPlay, Inc.
 * @version         1.0
 * @url             https://pixinc.club/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */
namespace PixelFramework.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;
    
    /// <summary>
    /// AES Encryption Library
    /// </summary>
    public class AES
    {
        // Private Params
        private static int _bufferKeySize = 32;
        private static int _blockSize = 256;
        private static int _keySize = 256;
        
        /// <summary>
        /// <para>If you want to update the settings, you can update the settings.</para>
        /// <para>【argument1】buffer key size</para>
        /// <para>【argument2】block size</para>
        /// <para>【argument3】key size</para>
        /// </summary>
        public static void UpdateEncryptionKeySize(int bufferKeySize = 32, int blockSize = 256, int keySize = 256)
        {
            _bufferKeySize = bufferKeySize;
            _blockSize = blockSize;
            _keySize = keySize;
        }
        
        /// <summary>
        /// <para>Standard Rijndael(AES) encrypt</para>
        /// <para>【argument1】plane text</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Encrypted and converted to Base64 string</para>
        /// </summary>
        public static string Encrypt(string plane, string password)
        {
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plane), password);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// <para>Standard Rijndael(AES) encrypt</para>
        /// <para>【argument1】plane binary</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Encrypted binary</para>
        /// </summary>
        public static byte[] Encrypt(byte[] src, string password)
        {
            RijndaelManaged rij = SetupRijndaelManaged;

            // A pseudorandom number is newly generated based on the inputted password
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, _bufferKeySize);
            // The missing parts are specified in advance to fill in 0 length
            byte[] salt = new byte[_bufferKeySize];
            // Rfc2898DeriveBytes gets an internally generated salt
            salt = deriveBytes.Salt;
            // The 32-byte data extracted from the generated pseudorandom number is used as a password
            byte[] bufferKey = deriveBytes.GetBytes(_bufferKeySize);

            rij.Key = bufferKey;
            rij.GenerateIV();

            using (ICryptoTransform encrypt = rij.CreateEncryptor(rij.Key, rij.IV))
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                // first 32 bytes of salt and second 32 bytes of IV for the first 64 bytes
                List<byte> compile = new List<byte>(salt);
                compile.AddRange(rij.IV);
                compile.AddRange(dest);
                return compile.ToArray();
            }
        }
        
        /// <summary>
        /// <para>Standard Rijndael(AES) decrypt</para>
        /// <para>【argument1】encrypted string</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Decrypted string</para>
        /// </summary>
        public static string Decrypt(string encrtpted, string password)
        {
            byte[] decripted = Decrypt(Convert.FromBase64String(encrtpted), password);
            return Encoding.UTF8.GetString(decripted);
        }

        /// <summary>
        /// <para>Standard Rijndael(AES) decrypt</para>
        /// <para>【argument1】encrypted binary</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Decrypted binary</para>
        /// </summary>
        public static byte[] Decrypt(byte[] src, string password)
        {
            RijndaelManaged rij = SetupRijndaelManaged;

            List<byte> compile = new List<byte>(src);

            // First 32 bytes are salt.
            List<byte> salt = compile.GetRange(0, _bufferKeySize);
            // Second 32 bytes are IV.
            List<byte> iv = compile.GetRange(_bufferKeySize, _bufferKeySize);
            rij.IV = iv.ToArray();

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt.ToArray());
            byte[] bufferKey = deriveBytes.GetBytes(_bufferKeySize);    // Convert 32 bytes of salt to password
            rij.Key = bufferKey;

            byte[] plain = compile.GetRange(_bufferKeySize * 2, compile.Count - (_bufferKeySize * 2)).ToArray();

            using (ICryptoTransform decrypt = rij.CreateDecryptor(rij.Key, rij.IV))
            {
                byte[] dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                return dest;
            }
        }
        

        private static RijndaelManaged SetupRijndaelManaged
        {
            get
            {
                RijndaelManaged rij = new RijndaelManaged();
                rij.BlockSize = _blockSize;
                rij.KeySize = _keySize;
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                return rij;
            }
        }
    }
}