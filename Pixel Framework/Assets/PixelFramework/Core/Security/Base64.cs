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
    using System.Text;
    using System.Security.Cryptography;
    
    /// <summary>
    /// Base64 Encryption Class
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Encode to Base64
        /// </summary>
        /// <param name="decodedText"></param>
        /// <returns></returns>
        public static string Encode(string decodedText)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes (decodedText);
            string encodedText = Convert.ToBase64String (bytesToEncode);
            return encodedText;
        }
        
        /// <summary>
        /// Decode from Base64
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        public static string Decode(string encodedText)
        {
            byte[] decodedBytes = Convert.FromBase64String (encodedText);
            string decodedText = Encoding.UTF8.GetString (decodedBytes);
            return decodedText;
        }
    }
}