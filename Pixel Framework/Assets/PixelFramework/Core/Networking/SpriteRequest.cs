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
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using PixelFramework.Utils;
    using PixelFramework.Core.Security;
    using PixelFramework.Core.ContentManagement;
    
    /// <summary>
    /// Sprite Request Class
    /// </summary>
    public class SpriteRequest : INetRequest
    {
        // Private Params
        private SpriteRequestConfig _config;
        
        // Private Events
        private Action _onRequestDispose;
        
        /// <summary>
        /// Sprite Request Constructor
        /// </summary>
        /// <param name="requestData"></param>
        public SpriteRequest(SpriteRequestConfig requestData)
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
                Texture2D requestCache = GetRequestCache(_config.Url);
                if (requestCache != null)
                {
                    if (_config.OnComplete != null) 
                        _config.OnComplete.Invoke(SpriteFromTexture2D(requestCache));
                    if(_onRequestDispose!=null) 
                        _onRequestDispose.Invoke();
                }
                
                yield return null;
            }
            
            // Detect Method
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(_config.Url);

            // Send Request
            yield return textureRequest.SendWebRequest();
            if (textureRequest.result == UnityWebRequest.Result.Success)
            {
                if (_config.OnComplete != null) 
                    _config.OnComplete.Invoke(SpriteFromTexture2D(DownloadHandlerTexture.GetContent(textureRequest)));
                if (_config.CacheRequest) SaveRequestCache(_config.Url, textureRequest.downloadHandler.data);
            }
            else
            {
                if (_config.OnError != null) _config.OnError(textureRequest.error);
            }
            
            
            if(_onRequestDispose!=null) 
                _onRequestDispose.Invoke();
            textureRequest.Dispose();
        }
        
        /// <summary>
        /// Get Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Texture2D GetRequestCache(string url)
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

                Texture2D texture = null;
                texture = new Texture2D(1, 1);
                texture.LoadImage(cacheFile, true);
                return texture;
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
        /// Get Sprite From Texture2D
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        private Sprite SpriteFromTexture2D(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}