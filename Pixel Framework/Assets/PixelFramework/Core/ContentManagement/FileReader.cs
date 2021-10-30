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
namespace PixelFramework.Core.ContentManagement
{
    using System;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;
    using UnityEngine;
    using PixelFramework.Core.Security;
    using System.Runtime.Serialization.Formatters.Binary;
    
    /// <summary>
    /// File Reader Class
    /// </summary>
    public class FileReader
    {
        // Private Params
        private static string _encryptionKey = "A&fv2hAD9jgkdf89^ASD2q89zsjdA"; // Default Data Encryption Key
        
        /// <summary>
        /// Set Encryption Key
        /// </summary>
        /// <param name="key"></param>
        public static void SetEncryptionKey(string key)
        {
            _encryptionKey = key;
        }

        /// <summary>
        /// Get Encryption Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEnryptionKey(string key)
        {
            return _encryptionKey;
        }

        #region Read Files
        /// <summary>
        /// Read Text File from Path
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string LoadText(string pathToFile, Encoding encoding = null)
        {
            string path = Application.persistentDataPath + pathToFile;
            if(File.Exists(path))
            {
                if (encoding != null)
                {
                    return File.ReadAllText(path, encoding);
                }
                else
                {
                    return File.ReadAllText(path);
                }
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Read Binary File from Path
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static byte[] LoadBinary(string pathToFile)
        {
            string path = Application.persistentDataPath + pathToFile;
            if(File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Read Text file from Resources
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static string LoadTextFromResources(string pathToFile)
        {
            TextAsset output = Resources.Load<TextAsset>(pathToFile);
            return output.text;
        }

        /// <summary>
        /// Read Binary file from Resources
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static byte[] LoadBinaryFromResources(string pathToFile)
        {
            TextAsset output = Resources.Load<TextAsset>(pathToFile);
            return output.bytes;
        }

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="pathToFile"></param>
        public static void DeleteFile(string pathToFile)
        {
            if(File.Exists(pathToFile)) File.Delete(pathToFile);
        }
        #endregion

        #region Save Files
        /// <summary>
        /// Save Text File to Path
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void SaveText(string pathToFile, string content, Encoding encoding = null)
        {
            string path = Application.persistentDataPath + pathToFile;
            if (encoding != null)
            {
                File.WriteAllText(path, content, encoding);
            }
            else
            {
                File.WriteAllText(path, content);
            }
        }

        /// <summary>
        /// Save Binary File to Path
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="data"></param>
        public static void SaveBinary(string pathToFile, byte[] data)
        {
            string path = Application.persistentDataPath + pathToFile;
            File.WriteAllBytes(path, data);
        }
        #endregion

        #region Read Objects
        /// <summary>
        /// Read Object from file without type
        /// </summary>
        /// <param name="referenceObject"></param>
        /// <param name="pathToFile"></param>
        /// <param name="serializationType"></param>
        /// <returns></returns>
        public static void ReadObjectFromFile(object referenceObject, string pathToFile,
            SerializationType serializationType = SerializationType.JSON)
        {
            string path = Application.persistentDataPath + pathToFile;
            if (File.Exists(path))
            {
                if (serializationType == SerializationType.JSON || serializationType == SerializationType.EncryptedJSON)
                {
                    string serializedData = LoadText(pathToFile, Encoding.UTF8);
                    if(serializationType == SerializationType.EncryptedJSON) serializedData = AES.Decrypt(serializedData, _encryptionKey);
                    if (serializedData != null)
                    {
                        JsonUtility.FromJsonOverwrite(serializedData, referenceObject);
                    }
                }else if (serializationType == SerializationType.Binary ||
                          serializationType == SerializationType.EncryptedBinary)
                {
                    BinaryFormatter converter = new BinaryFormatter();
                    FileStream inputStream = new FileStream(pathToFile, FileMode.Open);
                    referenceObject = converter.Deserialize(inputStream);
                    inputStream.Close();
                }else if (serializationType == SerializationType.XML)
                {
                    throw new Exception(
                        "Failed to Deserialize Object. XML Deserialization cannot be work with abstract objects");
                }
            }
        }
        
        /// <summary>
        /// Read Object from File
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="serializationType"></param>
        /// <returns></returns>
        public static T ReadObjectFromFile<T>(string pathToFile, SerializationType serializationType = SerializationType.JSON) where T : new()
        {
            string path = Application.persistentDataPath + pathToFile;
            T newObject = new T();
            if (File.Exists(path))
            {
                if (serializationType == SerializationType.JSON || serializationType == SerializationType.EncryptedJSON)
                {
                    string serializedData = LoadText(pathToFile, Encoding.UTF8);
                    if(serializationType == SerializationType.EncryptedJSON) serializedData = AES.Decrypt(serializedData, _encryptionKey);
                    if (serializedData != null)
                    {
                        newObject = JsonUtility.FromJson<T>(serializedData);
                    }
                }else if (serializationType == SerializationType.Binary ||
                          serializationType == SerializationType.EncryptedBinary)
                {
                    BinaryFormatter converter = new BinaryFormatter();
                    FileStream inputStream = new FileStream(pathToFile, FileMode.Open);
                    newObject = (T)converter.Deserialize(inputStream);
                    inputStream.Close();
                }else if (serializationType == SerializationType.XML)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    StreamReader reader = new StreamReader(path);
                    newObject = (T)serializer.Deserialize(reader.BaseStream);
                    reader.Close();
                }
                
                return newObject;
            }
            else
            {
                return newObject;
            }
        }
        #endregion

        #region Save Objects
        /// <summary>
        /// Save Object to File
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="serializationObject"></param>
        /// <param name="serializationType"></param>
        public static void SaveObjectToFile<T>(string pathToFile, T serializationObject,
            SerializationType serializationType = SerializationType.JSON)
        {
            string path = Application.persistentDataPath + pathToFile;
            if (serializationType == SerializationType.JSON || serializationType == SerializationType.EncryptedJSON)
            {
                string serializedData = JsonUtility.ToJson(serializationObject);
                if (serializationType == SerializationType.EncryptedJSON)
                    serializedData = AES.Encrypt(serializedData, _encryptionKey);
                SaveText(pathToFile, serializedData, Encoding.UTF8);
            }else if (serializationType == SerializationType.Binary ||
                      serializationType == SerializationType.EncryptedBinary)
            {
                BinaryFormatter converter = new BinaryFormatter();
                FileStream outputStream = new FileStream(pathToFile, FileMode.Create);
                converter.Serialize(outputStream, serializationObject);
                outputStream.Close();
            }else if (serializationType == SerializationType.XML)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(pathToFile);
                serializer.Serialize(writer.BaseStream, serializationObject);
                writer.Close();
            }
        }
        #endregion
    }
}