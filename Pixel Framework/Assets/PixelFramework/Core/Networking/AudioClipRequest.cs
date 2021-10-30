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
namespace PixelFramework.Core.Networking
{
    using System;
    using System.Text;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using PixelFramework.Utils;
    using PixelFramework.Core.Security;
    using PixelFramework.Core.ContentManagement;
    
    /// <summary>
    /// Audio Clip Request
    /// </summary>
    public class AudioClipRequest : INetRequest
    {
        // Private Params
        private AudioClipRequestConfig _config;
        
        // Private Events
        private Action _onRequestDispose;
        
        /// <summary>
        /// AudioClip Request Constructor
        /// </summary>
        /// <param name="requestData"></param>
        public AudioClipRequest(AudioClipRequestConfig requestData)
        {
            _config = requestData;
        }
        
        /// <summary>
        /// Send Request
        /// </summary>
        /// <returns></returns>
        public INetRequest SendRequest()
        {
            CoroutineProvider.Start(Send());
            return this;
        }
        
        /// <summary>
        /// Cancel Request
        /// </summary>
        /// <returns></returns>
        public INetRequest CancelRequest()
        {
            CoroutineProvider.Stop(Send());
            return this;
        }
        
        /// <summary>
        /// Request Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator Send()
        {
            // Load Request Cache
            if (_config.CacheRequest)
            {
                AudioClip requestCache = GetRequestCache(_config.Url);
                if (requestCache != null)
                {
                    if (_config.OnComplete != null) 
                        _config.OnComplete.Invoke(requestCache);
                    if(_onRequestDispose!=null) 
                        _onRequestDispose.Invoke();
                }
                
                yield return null;
            }
            
            // Detect Method
            UnityWebRequest multimedia = UnityWebRequestMultimedia.GetAudioClip(_config.Url, _config.AudioType);

            // Send Request
            yield return multimedia.SendWebRequest();
            if (multimedia.result == UnityWebRequest.Result.Success)
            {
                if (_config.OnComplete != null) 
                    _config.OnComplete.Invoke(DownloadHandlerAudioClip.GetContent(multimedia));
                if (_config.CacheRequest) SaveRequestCache(_config.Url, multimedia.downloadHandler.data);
            }
            else
            {
                if (_config.OnError != null) _config.OnError(multimedia.error);
            }
            
            
            if(_onRequestDispose!=null) 
                _onRequestDispose.Invoke();
            multimedia.Dispose();
        }
        
        /// <summary>
        /// Get Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private AudioClip GetRequestCache(string url)
        {
            string cacheFilePath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.contentcache";
            string cacheStampPath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.contentcachestamp";

            byte[] cacheFile = FileReader.LoadBinary(cacheFilePath);
            string cacheStamp = FileReader.LoadText(cacheStampPath, Encoding.UTF8);

            // Has Cache
            if (cacheFile != null && cacheStamp != null)
            {
                int cacheCreationTime = Int32.Parse(cacheStamp);
                if (UnixTime.SecondsElapsed(cacheCreationTime) > _config.CacheLifetime)
                {
                    cacheFile = null;
                    cacheStamp = null;
                    FileReader.DeleteFile(cacheFilePath);
                    FileReader.DeleteFile(cacheStampPath);
                    return null;
                }

                AudioClip audioClip = null;
                using (Stream s = new MemoryStream(cacheFile))
                {
                    audioClip = AudioClip.Create(url, cacheFile.Length, 1, 48000, false);
                    float[] f = ConvertByteToFloat(cacheFile);
                    audioClip.SetData(f, 0);
                }
                audioClip.LoadAudioData();
                return audioClip;
            }

            return null;
        }
        
        /// <summary>
        /// Save Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        private void SaveRequestCache(string url, byte[] contentData)
        {
            string cacheFilePath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.contentcache";
            string cacheStampPath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.contentcachestamp";
            FileReader.SaveBinary(cacheFilePath, contentData);
            FileReader.SaveText(cacheStampPath, UnixTime.Current().ToString(), Encoding.UTF8);
        }
        
        /// <summary>
        /// On Dispose Action
        /// </summary>
        /// <param name="onDispose"></param>
        /// <returns></returns>
        public INetRequest OnDispose(Action onDispose)
        {
            _onRequestDispose = onDispose;
            return this;
        }
        
        /// <summary>
        /// Convert Byte to Float
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private float[] ConvertByteToFloat(byte[] array)
        {
            float[] floatArr = new float[array.Length / 4];
            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(array, i * 4, 4);
                }
                floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
            }
            return floatArr;
        }
    }
}