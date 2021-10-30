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
    /// RSA Encryption Class
    /// </summary>
    public class RSA
    {
        /// <summary>
        /// <para>Generate Public And Private KeyPair</para>
        /// <para>【argument1】keySize</para>
        /// <para>【return】Public key and private key KeyValuePair</para>
        /// </summary>
        public static KeyValuePair<string, string> GenrateKeyPair(int keySize){
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }
        
        /// <summary>
        /// <para>Standard Rijndael(AES) encrypt</para>
        /// <para>【argument1】plane text</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Encrypted and converted to Base64 string</para>
        /// </summary>
        public static string Encrypt(string plane, string publicKey)
        {
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plane), publicKey);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// <para>Standard Rijndael(AES) encrypt</para>
        /// <para>【argument1】plane binary</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Encrypted binary</para>
        /// </summary>
        public static byte[] Encrypt(byte[] src, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] encrypted = rsa.Encrypt(src, false);
                return encrypted;
            }
        }

        /// <summary>
        /// <para>Standard Rijndael(AES) decrypt</para>
        /// <para>【argument1】encrypted string</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Decrypted string</para>
        /// </summary>
        public static string Decrypt(string encrtpted, string privateKey)
        {
            byte[] decripted = Decrypt(Convert.FromBase64String(encrtpted), privateKey);
            return Encoding.UTF8.GetString(decripted);
        }

        /// <summary>
        /// <para>Standard Rijndael(AES) decrypt</para>
        /// <para>【argument1】encrypted binary</para>
        /// <para>【argument2】password</para>
        /// <para>【return】Decrypted binary</para>
        /// </summary>
        public static byte[] Decrypt(byte[] src, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] decrypted = rsa.Decrypt(src, false);
                return decrypted;
            }
        }
    }
}